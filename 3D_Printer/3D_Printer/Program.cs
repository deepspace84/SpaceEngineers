using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game;
using Sandbox.Game.EntityComponents;
using VRageMath;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

private string pwcompare = "";
private string channel = "CH01";
public float pistonOffset = 0.18f;
public float pistonDefaultSpeed = 0.3f;
public float rotorDefaultSpeed = 0.3f;

IMyBroadcastListener listener;
/*
Most commands may take a password as second parameter. It can be omitted if password is "";
NAMES can be a list of block-names. Separated by ",". 
    Groups may be used. If a name starts with "G:" the following name is asumed a group name.
Use "help;help;functions" as argument for help.

Commands
action              -   Perform Predefines Actions.
broadcastlcd    -   Copy LCD content via Antenna.
channel             -   Set channel for antenna broadcasts.
check               -   Check Positions for Pistons,Rotors or Hinges. Start Timers depending on result.
checkpw         -   Check if your password is correct
float                   -   Set float Values for Block Properties.
getchannel          -   Echo current channel name.
help                    -   Display help.
hinge                   -   Set Hinge Positions&Velocities
offset                  -   Set Rotor Displacement
piston                  -   Set Piston Positions&Velocities
resetpw (debug) -   Reset password to ""
rotor                   -   Set Rotor Positions&Velocities
run                     -   Run Programmable Block with given arguments
send                    -   Broadcast text message
setpw               -   Set password
settext                 -   Set LCD text
string                  -   Set string value for blocks.
*/

public Program()
{
    string test = Me.CustomData;
    Echo(test);

    if (Storage.Length > 0)
    {
        string[] stor = Storage.Split(';');
        if (null == stor) { stor = new string[1]; stor[0] = Storage; }

        pwcompare = stor[0];
        if (stor.Count() > 1)
            channel = stor[1];
    }
    listener = IGC.RegisterBroadcastListener(channel);
    listener.SetMessageCallback("incoming");

    helptext = new Dictionary<string, string>();
    helptext.Add("", help_general);
    helptext.Add("action", help_action);
    helptext.Add("broadcastlcd", help_broadcastlcd);
    helptext.Add("channel", help_channel);
    helptext.Add("check", help_check);
    helptext.Add("checkpw", help_checkpw);
    helptext.Add("float", help_float);
    helptext.Add("functions", help_functions);
    helptext.Add("getchannel", help_getchannel);
    helptext.Add("help", help_help);
    helptext.Add("hinge", help_hinge);
    helptext.Add("names", help_names);
    helptext.Add("offset", help_offset);
    helptext.Add("piston", help_piston);
    helptext.Add("resetpw", help_resetpw);
    helptext.Add("rotor", help_rotor);
    helptext.Add("run", help_run);
    helptext.Add("send", help_send);
    helptext.Add("setpw", help_setpw);
    helptext.Add("settext", help_settext);
    helptext.Add("string", help_string);
}
public void Save()
{ Storage = pwcompare + ";" + channel; }

public void Main(string argument, UpdateType updateSource)
{
    Echo("Run...");
    string farg = argument;
    if ("incoming" == argument && listener.HasPendingMessage)
    {
        Echo("Incoming Message...");
        MyIGCMessage msg = listener.AcceptMessage();
        farg = msg.Data as string;
        Echo(farg);
    }

    if ("" == argument || "help" == argument)
        farg = "help;";

    if (!(farg.Contains(";")))
    {
        Echo("Argument Error!");
        return;
    }
    int pwstart = 0;

    string[] args = farg.Split(';');
    if (null == args) { args = new string[1]; args[0] = farg; }

    //Help
    if (args[0] == "help")
    {
        if (args.Count() < 2)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: help;command");
            return;
        }
        for (int i = 1; i < args.Count(); i++)
        {
            string s;
            if (!helptext.TryGetValue(args[i], out s))
            {
                Echo("Wrong argument!");
                Echo(help_functions);
                return;
            }
            Echo("");
            Echo(s);
            Echo("");
        }
        return;
    }

    //Password hard reset
    if (args[0] == "resetpw")
    {
        if (args.Count() != 2)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: resetpw");
            return;
        }
        pwcompare = "";
        Save();
        Echo("Password reset!");
        return;
    }

    //Set Password
    if (args[0] == "setpw")
    {
        Echo("Set PW...");
        if (args.Count() != 3)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: setpw;pwold;pwnew");
            return;
        }
        setpw(args[1], args[2]);
        return;
    }

    //Check Password
    if (args[0] == "checkpw")
    {
        Echo("Check PW...");
        if (args.Count() != 2)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: checkpw;pw");
            return;
        }
        if (checkpw(args[1]))
            Echo("Password OK");
        else
            Echo("Wrong Password");
        return;
    }

    //Check wether PW is set
    if (pwcompare == "")
    {
        Echo("No Password set.");
        if ("" == args[1])
        {
            pwstart++;
            Echo("Assuming empty password");
        }
    }
    else
    {
        Echo("There is a Password!");
        Echo("ENC: " + pwcompare);
        pwstart++;
        if (!checkpw(args[1]))
        {
            Echo("Wrong PW!");
            return;
        }
    }


    //Execute Programmable Block
    if (args[0] == "run")
    {
        Echo("Running Programmable Block...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: run;PB-Name;args");
            return;
        }
        IMyProgrammableBlock pb = (IMyProgrammableBlock)GridTerminalSystem.GetBlockWithName(args[pwstart + 1]);
        if (null == pb)
        {
            Echo("Block not found: " + args[pwstart + 1]);
            return;
        }
        var passarg = new StringBuilder();
        if (args.Count() > 2 + pwstart)
            passarg.Append(args[pwstart + 2]);
        if (args.Count() > 3 + pwstart)
        {
            for (int i = 3 + pwstart; i < args.Count(); i++)
            {
                passarg.Append(";");
                passarg.Append(args[i]);
            }
        }
        if (pb.TryRun(passarg.ToString()))
            Echo("Executed!");
        else
            Echo("Run failed!");
        return;
    }

    //Change Broadcast Channel
    if (args[0] == "channel")
    {
        Echo("Sending Message...");
        if (args.Count() != 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: channel;string");
            return;
        }
        channel = args[1 + pwstart];
        Save();
    }

    if (args[0] == "getchannel")
    {
        Echo("Current Channel: " + channel);
        return;
    }

    //Transmit Message
    if (args[0] == "send")
    {
        Echo("Sending Message...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: send;args");
            return;
        }
        var passarg = new StringBuilder();
        if (args.Count() > 1 + pwstart)
            passarg.Append(args[pwstart + 1]);
        if (args.Count() > 2 + pwstart)
        {
            for (int i = 2 + pwstart; i < args.Count(); i++)
            {
                passarg.Append(";");
                passarg.Append(args[i]);
            }
        }
        IGC.SendBroadcastMessage<string>(channel, passarg.ToString(), TransmissionDistance.TransmissionDistanceMax);
        return;
    }

    Echo("Block interaction...");
    if (args.Count() < 2 + pwstart)
    {
        Echo("Wrong Number of \";\" ! (not allowed in Password)");
        Echo("Example: command;<pw>;Name,Name,G:Name;args");
        return;
    }

    //Get Blocks
    List<IMyTerminalBlock> blocks = parseNames(args[1 + pwstart]);
    if (null == blocks)
        return;

    Echo("" + blocks.Count + " Blocks.");

    //Apply Action
    if (args[0] == "action")
    {
        Echo("Doing Block Action...");
        if (args.Count() != 3 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: action;<pw>;Name,Name,G:Name;action");
            return;
        }
        foreach (IMyTerminalBlock tb in blocks)
            tb.ApplyAction(args[pwstart + 2]);
    }

    //Set Float Value
    if (args[0] == "float")
    {
        Echo("Setting Block value...");
        if (args.Count() != 4 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: float;<pw>;Name,Name,G:Name;varname;value");
            return;
        }
        float val = 0.0f;
        if (!float.TryParse(args[pwstart + 3], out val))
        {
            Echo("Ivalid Float Value: " + args[pwstart + 3]);
            return;
        }
        foreach (IMyTerminalBlock tb in blocks)
            tb.SetValueFloat(args[pwstart + 2], val);
    }

    //Set String Value
    if (args[0] == "string")
    {
        Echo("Setting Block string...");
        if (args.Count() != 4 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: string;<pw>;Name,Name,G:Name;varname;value");
            return;
        }
        foreach (IMyTerminalBlock tb in blocks)
            tb.SetValue(args[pwstart + 2], args[pwstart + 3]);
    }

    float pos = -1.0f;
    float v = 0.0f;
    bool delta = false;

    //Piston Position & Velocity
    if (args[0] == "piston")
    {
        Echo("Piston Control...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: piston;<pw>;Name,Name,G:Name;<P=position>;<V=velocity>;<delta>");
            return;
        }
        if (!parsePVdelta(args, 2 + pwstart, out pos, out v, out delta))
            return;
        setPistons(blocks, pos, v, delta);
        Echo("Delta: " + delta);
    }

    //Rotor Position & Velocity
    if (args[0] == "rotor")
    {
        Echo("Rotor Control...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: rotor;<pw>;Name,Name,G:Name;<P=position>;<V=velocity>;<delta>");
            return;
        }
        if (!parsePVdelta(args, 2 + pwstart, out pos, out v, out delta))
            return;
        setRotors(blocks, pos, v, delta);
    }

    //Hinge Position & Velocity
    if (args[0] == "hinge")
    {
        Echo("Hinge Control...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: hinge;<pw>;Name,Name,G:Name;<P=position>;<V=velocity>;<delta>");
            return;
        }
        if (!parsePVdelta(args, 2 + pwstart, out pos, out v, out delta))
            return;
        setHinges(blocks, pos, v, delta);
    }

    //Rotor Offset
    if (args[0] == "offset")
    {
        Echo("Rotor Offset...");
        if (args.Count() < 2 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: hinge;<pw>;Name,Name,G:Name;<P=position>;<V=velocity>;<delta>");
            return;
        }
        if (!parsePVdelta(args, 2 + pwstart, out pos, out v, out delta))
            return;
        setOffset(blocks, pos, delta);
    }

    //Position Check
    if (args[0] == "check")
    {
        Echo("Position check...");
        if (args.Count() < 5 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: check;<pw>;Name,Name,G:Name;position;tolerance;TimerPositive;<TimerNegative>");
            return;
        }
        if (!float.TryParse(args[2 + pwstart], out pos))
        {
            Echo("Could not parse: " + args[2 + pwstart]);
            return;
        }
        float tol = 0.05f;
        if (!float.TryParse(args[3 + pwstart], out tol))
        {
            Echo("Could not parse: " + args[3 + pwstart]);
            return;
        }
        List<IMyTerminalBlock> tp = parseNames(args[4 + pwstart]);
        List<IMyTerminalBlock> tn = null;
        if (args.Count() == 6 + pwstart)
            tn = parseNames(args[4 + pwstart]);
        positionCheck(blocks, pos, tol, tp, tn);
    }

    //LCD Setup
    if (args[0] == "settext")
    {
        Echo("Setting LCD-Text...");
        if (args.Count() < 3 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: settext;<pw>;Name,Name,G:Name;text");
            return;
        }
        var passarg = new StringBuilder();
        if (args.Count() > 2 + pwstart)
            passarg.Append(args[pwstart + 2]);
        if (args.Count() > 3 + pwstart)
        {
            for (int i = 3 + pwstart; i < args.Count(); i++)
            {
                passarg.Append(";");
                passarg.Append(args[i]);
            }
        }
        foreach (IMyTerminalBlock block in blocks)
        {
            if (block is IMyTextPanel)
            {
                IMyTextPanel lcd = (IMyTextPanel)block;
                lcd.WritePublicText(passarg.ToString());
            }
        }
    }

    //LCD Setup
    if (args[0] == "broadcastlcd")
    {
        Echo("Broadcasting LCD-Text via Antenna...");
        if (args.Count() < 3 + pwstart)
        {
            Echo("Wrong Number of \";\" ! (not allowed in Password)");
            Echo("Example: broadcastlcd;<pw>;Name;TargetName");
            return;
        }
        string text = "ERROR!";
        foreach (IMyTerminalBlock block in blocks)
        {
            if (block is IMyTextPanel)
            {
                IMyTextPanel lcd = (IMyTextPanel)block;
                text = lcd.GetPublicText();
                break;
            }
        }
        var builder = new StringBuilder();
        builder.Append("settext");
        if (pwstart > 0)
        {
            builder.Append(";");
            builder.Append(args[1]);
        }
        builder.Append(";");
        builder.Append(args[2 + pwstart]);
        builder.Append(";");
        builder.Append(text);
        IGC.SendBroadcastMessage<string>(channel, builder.ToString(), TransmissionDistance.TransmissionDistanceMax);
        return;
    }
}

public List<IMyTerminalBlock> parseNames(string s)
{
    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
    List<IMyTerminalBlock> acc = new List<IMyTerminalBlock>();

    string[] names = s.Split(',');
    if (null == names) { names = new string[1]; names[0] = s; }

    foreach (string name in names)
    {
        if (name.StartsWith("G:"))
        {
            IMyBlockGroup g = GridTerminalSystem.GetBlockGroupWithName(name);
            g.GetBlocks(acc);
        }
        else
            GridTerminalSystem.SearchBlocksOfName(name, acc);
        foreach (IMyTerminalBlock tb in acc)
        {
            if (!blocks.Contains(tb))
                blocks.Add(tb);
        }
        acc.Clear();
    }

    if (blocks.Count == 0)
    {
        Echo("No Blocks found!");
        return null;
    }
    return blocks;
}

//Parse (for Pistons,Rotors and Hinges)
public bool parsePVdelta(string[] args, int pstart, out float pos, out float v, out bool delta)
{
    float defp = -1000f;
    float defv = 0.3f;
    bool defdelta = false;
    for (int i = pstart; i < args.Count(); i++)
    {
        string param = args[i];
        if (param.StartsWith("P="))
        {
            if (!float.TryParse(param.Substring(2, param.Length - 2), out defp))
            {
                Echo("Could not parse: " + param);
                pos = defp;
                v = defv;
                delta = defdelta;
                return false;
            }
        }
        else if (param.StartsWith("V="))
        {
            if (!float.TryParse(param.Substring(2, param.Length - 2), out defv))
            {
                Echo("Could not parse: " + param);
                pos = defp;
                v = defv;
                delta = defdelta;
                return false;
            }
        }
        else if ("delta" == param)
        {
            defdelta = true;
            delta = true;
        }
    }
    pos = defp;
    v = defv;
    delta = defdelta;
    return true;
}

//Set Position&Velocity for Pistons
public void setPistons(List<IMyTerminalBlock> blocks, float posv, float v, bool delta)
{
    float pos = posv;
    foreach (IMyTerminalBlock block in blocks)
    {
        if (block is IMyPistonBase)
        {
            IMyPistonBase piston = (IMyPistonBase)block;
            float cpos = piston.CurrentPosition;
            if (delta)
            {
                pos = posv + cpos;
                if (pos > 35.0f)
                    pos = 35.0f;
                else if (pos < 0.0f)
                    pos = 0.0f;
            }
            if (pos >= 0.0f && pos <= 35.0f)
            {
                float vabs = v;
                if (v == 0.0f)
                    vabs = pistonDefaultSpeed;
                if (pos > cpos)
                {
                    piston.MinLimit = 0.0f;
                    piston.MaxLimit = pos;
                    if (vabs < 0)
                        piston.Velocity = -vabs;
                    else
                        piston.Velocity = vabs;
                }
                else if (pos < cpos)
                {
                    piston.MinLimit = pos;
                    piston.MaxLimit = 35.0f;
                    if (vabs < 0)
                        piston.Velocity = vabs;
                    else
                        piston.Velocity = -vabs;
                }
                else
                {
                    piston.Velocity = 0.0f;
                    piston.MinLimit = 0.0f;
                    piston.MaxLimit = 35.0f;
                }
            }
            else
            {
                piston.MinLimit = 0.0f;
                piston.MaxLimit = 0.0f;
                piston.Velocity = v;
            }
        }
    }
}

//Set Position&Velocity for Rotors
public void setRotors(List<IMyTerminalBlock> blocks, float posv, float v, bool delta)
{
    float pos = posv;
    foreach (IMyTerminalBlock block in blocks)
    {
        if (block is IMyMotorStator)
        {
            IMyMotorStator rot = (IMyMotorStator)block;
            float cpos = 180.0f * rot.Angle / 3.14159265f;
            bool negflag = false;
            if (cpos < 0.0f)
            {
                cpos += 360.0f;
                negflag = true;
            }
            if (delta)
            {
                pos = posv + cpos;
                if (pos < 0.0f)
                    pos += 360.0f;
                while (pos > 360.0f)
                    pos -= 360.0f;
            }
            float dclock = pos - cpos;
            if (dclock < 0.0f)
                dclock = pos + 360.0f - cpos;
            float dcounter = cpos - pos;
            if (dcounter < 0.0f)
                dcounter = cpos + 360.0f - pos;

            if (pos > 0.0f && pos < 360.0f)
            {
                float vabs = v;
                if (v == 0.0f)
                    vabs = rotorDefaultSpeed;
                if (dclock < dcounter)
                {
                    rot.SetValueFloat("LowerLimit", -361.0f);
                    if (negflag)
                        pos = cpos + dclock - 360.0f;
                    rot.SetValueFloat("UpperLimit", pos);
                    if (vabs < 0)
                        rot.SetValueFloat("Velocity", -vabs);
                    else
                        rot.SetValueFloat("Velocity", vabs);
                }
                else
                {
                    if (negflag)
                    {
                        pos = cpos - dcounter - 360.0f;
                        if (pos < -360.0f)
                            pos += 720.0f;
                    }
                    rot.SetValueFloat("LowerLimit", pos);
                    rot.SetValueFloat("UpperLimit", 361.0f);
                    if (vabs < 0)
                        rot.SetValueFloat("Velocity", vabs);
                    else
                        rot.SetValueFloat("Velocity", -vabs);
                }
            }
            else
            {
                rot.SetValueFloat("LowerLimit", -361.0f);
                rot.SetValueFloat("UpperLimit", 361.0f);
                rot.SetValueFloat("Velocity", v);
            }
        }
    }
}

//Set Position & Velocity for Hinges
public void setHinges(List<IMyTerminalBlock> blocks, float posv, float v, bool delta)
{
    float pos = posv;
    foreach (IMyTerminalBlock block in blocks)
    {
        if (block is IMyMotorStator)
        {
            IMyMotorStator rot = (IMyMotorStator)block;
            float cpos = 180.0f * rot.Angle / 3.14159265f;
            if (delta)
            {
                pos = posv + cpos;
                if (pos < -90.0f)
                    pos = -90.0f;
                else if (pos > 90.0f)
                    pos = 90.0f;
            }
            if (pos >= -90.0f && pos <= 90.0f)
            {
                float vabs = v;
                if (v == 0.0f)
                    vabs = rotorDefaultSpeed;
                if (pos > cpos)
                {
                    rot.SetValueFloat("LowerLimit", -90.0f);
                    rot.SetValueFloat("UpperLimit", pos);
                    if (vabs < 0)
                        rot.SetValueFloat("Velocity", -vabs);
                    else
                        rot.SetValueFloat("Velocity", vabs);
                }
                else
                {
                    rot.SetValueFloat("LowerLimit", pos);
                    rot.SetValueFloat("UpperLimit", 90.0f);
                    if (vabs < 0)
                        rot.SetValueFloat("Velocity", vabs);
                    else
                        rot.SetValueFloat("Velocity", -vabs);
                }
            }
            else
            {
                rot.SetValueFloat("LowerLimit", -90.0f);
                rot.SetValueFloat("UpperLimit", 90.0f);
                rot.SetValueFloat("Velocity", v);
            }
        }
    }
}

//Set Rotor-Head-Offset
public void setOffset(List<IMyTerminalBlock> blocks, float posv, bool delta)
{
    float pos = posv;
    foreach (IMyTerminalBlock block in blocks)
    {
        if (block is IMyMotorStator)
        {
            IMyMotorStator rot = (IMyMotorStator)block;
            if (delta)
            {
                float cpos = rot.Displacement;
                pos = posv + cpos;
                if (pos > 0.2f)
                    pos = 0.2f;
                else if (pos < -0.4f)
                    pos = -0.4f;
            }
            rot.SetValueFloat("Displacement", pos);
        }
    }
}

//Check Position and run Timers depending on Result
public void positionCheck(List<IMyTerminalBlock> blocks, float posv, float tolerance, List<IMyTerminalBlock> tPos, List<IMyTerminalBlock> tNeg)
{
    float pos = posv;
    bool positive = false;
    foreach (IMyTerminalBlock block in blocks)
    {
        float cpos = 0;
        if (block is IMyMotorStator)
        {
            IMyMotorStator rot = (IMyMotorStator)block;
            cpos = 180.0f * rot.Angle / 3.14159265f;
            if (cpos < 0.0f)
                cpos = 360.0f + cpos;
            if (pos < 0)
                pos = 360.0f + pos;
        }
        else if (block is IMyPistonBase)
        {
            IMyPistonBase piston = (IMyPistonBase)block;
            cpos = piston.CurrentPosition;
        }
        float delta = pos - cpos;
        if (delta < 0.0f)
            delta = -delta;
        if (delta < tolerance)
        {
            positive = true;
            break;
        }
    }
    List<IMyTerminalBlock> runTimers = null;
    if (positive)
        runTimers = tPos;
    else
        runTimers = tNeg;
    if (null == runTimers)
        return;
    foreach (IMyTimerBlock timer in runTimers)
    {
        if (!timer.Enabled)
        {
            timer.ApplyAction("OnOff_On");
        }
        if (!timer.IsCountingDown)
        {
            timer.ApplyAction("Start");
        }
    }
}

public void setpw(string pwold, string pwnew)
{
    if (!checkpw(pwold))
    {
        Echo("Wrong Password!");
        return;
    }
    pwcompare = encrypt(pwnew);
    Save();
    Echo("Password set!");
}

public string encrypt(string s)
{
    char[] characters = System.Text.Encoding.ASCII.GetChars(new byte[] { 2 });
    int olen = s.Length;
    string ret = s + "._" + s + "!%?" + s;
    ret = ret.Substring(olen / 3 + olen % 5, olen + 5);
    var builder = new StringBuilder();
    for (int i = 0; i < ret.Length; i++)
    {
        char c = ret[i];
        char c2 = ret[(7 * i + 11) % ret.Length];
        int iaccp = (int)c + (int)c2;
        int iaccm = (int)c * (int)c2;
        int iacc = iaccm - iaccp;
        if (iacc < 0)
            iacc = -iacc + 13 + i;
        iacc = iacc % 127;
        if (iacc < 32)
            iacc = (iacc + 17 + i) % 95 + 32;
        char[] cr = System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)iacc });
        builder.Append(cr[0]);
    }
    ret = builder.ToString().Replace(";", "-");
    return ret;
}

public bool checkpw(string pw)
{
    if ("" == pwcompare && "" == pw)
        return true;
    if (null == pw)
        return false;
    return encrypt(pw) == pwcompare;
}

Dictionary<String, String> helptext;
string help_general = "Usage: command;<password>;arguments\npassword can be skipped, if not set.\nCan be activated via Antenna.\nTry help;help;functions";
string help_help = "help;keyword\nDisplays help text for a given function.\nhelp;functions will list all functions.";
string help_functions = "action;broadcastlcd;channel;check;checkpw;float;getchannel;help;hinge;names;offset;piston;rotor;run;send;setpw;settext;string";
string help_resetpw = "resetpw; \nwill disable/reset password for debug reasons.";
string help_setpw = "setpw;oldpassword;newpassword \nWill set a new password.\n\";\" is not an allowed character.";
string help_checkpw = "checkpw;testword \nWill echo wether your Password is correct.(debug)";
string help_run = "run;<password>;PBName;arguments \nWill run a given Programmable Block with given arguments.";
string help_channel = "channel;<password>;channelname \nWill set the broadcast-channel for this PB.";
string help_getchannel = "getchannel;<password> \nWill display current channel name.";
string help_send = "send;<password>;message \nWill broadcast given message on current channel.";
string help_float = "float;<password>;namelist;varname;value \nWill set a float value for given Blocks.";
string help_string = "string;<password>;namelist;varname;value \nWill set a value for given Blocks.";
string help_action = "action;<password>;namelist;actionName\nWill apply given action to given Blocks.";
string help_piston = "piston;<password>;namelist;<P=position>;<V=velocity>;<delta>\nWill move Pistons to a given Position.(Optional)\nWill move Pistons with given velocity.(Optional)\nif(\"delta\" is a parameter, position value will be relative to current.";
string help_rotor = "rotor;<password>;namelist;<P=position>;<V=velocity>;<delta>\nWill move Rotors to a given Position.(Optional)\nWill move Rotors with given velocity.(Optional)\nif(\"delta\" is a parameter, position value will be relative to current.";
string help_hinge = "hinge;<password>;namelist;<P=position>;<V=velocity>;<delta>\nWill move Hinges to a given Position.(Optional)\nWill move Hinges with given velocity.(Optional)\nif(\"delta\" is a parameter, position value will be relative to current.";
string help_offset = "offset;<password>;namelist;position;<delta>\nWill set Rotor Offset to a given Position.\nif(\"delta\" is a parameter, position value will be relative to current.";
string help_check = "check;<password>;testname;position;tolerance;timerlist(positive Test);timerlist(negativeTest)\nWill check wether given block is at given position.\nWill activate given Timers depending on result.";
string help_settext = "settext;<password>;namelist;Text\nWill set LCD-Text for given LCD.";
string help_broadcastlcd = "broadcastlcd;<password>;name;targetNames\nWill send given LCDs text via Antenna to target LCDs. Assuming same Password for target Grid.\nPW-Broadcast currently unencrypted.";
string help_names = "Multiple names can be given separated by \",\".\nBlock Groups can be used indicated by \"G:\" directly before name.";
