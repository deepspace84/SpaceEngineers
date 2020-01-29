using System;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;


namespace Klime.CaptureTheFlag
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class CaptureTheFlag : MySessionComponentBase
    {
        private IMyCubeGrid reuse_grid;
        private bool initial_flags_spawned = false;
        private int _session_timer = 0;
        private bool setup_complete = false;
        private List<IMySlimBlock> reuse_blocks = new List<IMySlimBlock>();
        private List<Flag> flags_in_play = new List<Flag>();
        private List<IMyPlayer> all_players = new List<IMyPlayer>();
        private MatrixD reuse_matrix = MatrixD.Identity;
        private bool game_over = false;

        private string team_one_faction_tag = "RED";
        private string team_two_faction_tag = "BLU";

        private int team_one_score = 0;
        private int team_two_score = 0;
        private int max_score = 3;

        private int max_wait = 600;

        private Flag reuse_flag_one;
        private Flag reuse_flag_two;
        private const ushort ctf_net_id = 20132;
        private List<byte> ctf_byte_list = new List<byte>();

        public class Flag
        {
            public IMyCubeGrid flag_grid { get; set; }
            public IMyPlayer carrier { get; set; }
            public int state { get; set; }
            public string faction_owner { get; set; }
            public MatrixD home_position { get; set; }
            public MatrixD dropped_position { get; set; }
            public int current_time_on_ground { get; set; }

            public Flag(IMyCubeGrid incoming_grid, string incoming_owner, MatrixD incoming_home)
            {
                flag_grid = incoming_grid;
                carrier = null;
                state = 0;
                faction_owner = incoming_owner;
                home_position = incoming_home;
                dropped_position = MatrixD.Identity;
                current_time_on_ground = 0;
            }

        }

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                MyVisualScriptLogicProvider.PrefabSpawnedDetailed += CTF_PrefabSpawned;
                MyVisualScriptLogicProvider.PlayerDied += CTF_PlayerDied;
                MyVisualScriptLogicProvider.PlayerEnteredCockpit += CTF_CockpitSwitch;
            }
            MyAPIGateway.Multiplayer.RegisterMessageHandler(ctf_net_id, ctf_handler);
        }

        public override void LoadData()
        {
            if (!MyAPIGateway.Utilities.GetVariable("team_one_faction_tag", out team_one_faction_tag))
            {
                team_one_faction_tag = "RED";
                MyAPIGateway.Utilities.SetVariable("team_one_faction_tag", "RED");
            }
            if (!MyAPIGateway.Utilities.GetVariable("team_two_faction_tag", out team_two_faction_tag))
            {
                team_two_faction_tag = "BLU";
                MyAPIGateway.Utilities.SetVariable("team_two_faction_tag", "BLU");
            }
            if (!MyAPIGateway.Utilities.GetVariable<int>("max_score", out max_score))
            {
                max_score = 3;
                MyAPIGateway.Utilities.SetVariable<int>("max_score", 3);
            }
        }

        private void ctf_handler(byte[] obj)
        {
            team_one_score = BitConverter.ToInt32(obj, 0);
            team_two_score = BitConverter.ToInt32(obj, 4);
        }

        private void CTF_CockpitSwitch(string entityName, long playerId, string gridName)
        {
            foreach (var flag in flags_in_play)
            {
                if (flag.state == 1)
                {
                    if (flag.carrier.IdentityId == playerId)
                    {
                        flag.state = 2;
                        MyPlanet closest_planet = MyGamePruningStructure.GetClosestPlanet(flag.flag_grid.WorldMatrix.Translation);
                        if (closest_planet != null)
                        {
                            Vector3D closest_pos = closest_planet.GetClosestSurfacePointGlobal(flag.flag_grid.WorldMatrix.Translation);
                            Vector3D up = Vector3D.Normalize(closest_pos - closest_planet.PositionComp.GetPosition());
                            Vector3D final_pos = closest_pos + up;
                            reuse_matrix = flag.flag_grid.WorldMatrix;
                            reuse_matrix.Translation = final_pos;
                            flag.dropped_position = reuse_matrix;
                            flag.current_time_on_ground = 0;
                        }
                        else
                        {
                            flag.dropped_position = flag.flag_grid.WorldMatrix;
                            flag.current_time_on_ground = 0;
                        }
                        flag.flag_grid.WorldMatrix = flag.dropped_position;
                        MyVisualScriptLogicProvider.SendChatMessage(flag.faction_owner + " flag dropped!");
                    }
                }
            }
        }

        private void CTF_PlayerDied(long playerId)
        {
            foreach (var flag in flags_in_play)
            {
                if (flag.state == 1)
                {
                    if (flag.carrier.IdentityId == playerId)
                    {
                        flag.state = 2;
                        MyPlanet closest_planet = MyGamePruningStructure.GetClosestPlanet(flag.flag_grid.WorldMatrix.Translation);
                        if (closest_planet != null)
                        {
                            Vector3D closest_pos = closest_planet.GetClosestSurfacePointGlobal(flag.flag_grid.WorldMatrix.Translation);
                            Vector3D up = Vector3D.Normalize(closest_pos - closest_planet.PositionComp.GetPosition());
                            Vector3D final_pos = closest_pos + up;
                            reuse_matrix = flag.flag_grid.WorldMatrix;
                            reuse_matrix.Translation = final_pos;
                            flag.dropped_position = reuse_matrix;
                            flag.current_time_on_ground = 0;
                        }
                        else
                        {
                            flag.dropped_position = flag.flag_grid.WorldMatrix;
                            flag.current_time_on_ground = 0;
                        }
                        flag.flag_grid.WorldMatrix = flag.dropped_position;
                        MyVisualScriptLogicProvider.SendChatMessage(flag.faction_owner + " flag dropped!");
                    }
                }
            }
        }

        private void CTF_PrefabSpawned(long entityId, string prefabName)
        {
            if (!setup_complete)
            {
                var grid = MyAPIGateway.Entities.GetEntityById(entityId) as IMyCubeGrid;
                grid.Save = false;
                string owning_fac = "";
                if (prefabName == "RED_CTFFLAG")
                {
                    owning_fac = team_one_faction_tag;

                    reuse_blocks.Clear();
                    grid.GetBlocks(reuse_blocks);
                    foreach (var block in reuse_blocks)
                    {
                        if (block.FatBlock != null & block.FatBlock is IMyBeacon)
                        {
                            IMyBeacon beacon = block.FatBlock as IMyBeacon;
                            beacon.CustomName = owning_fac + " FLAG";
                        }
                    }
                }
                else
                {
                    if (prefabName == "BLU_CTFFLAG")
                    {
                        owning_fac = team_two_faction_tag;

                        reuse_blocks.Clear();
                        grid.GetBlocks(reuse_blocks);
                        foreach (var block in reuse_blocks)
                        {
                            if (block.FatBlock != null & block.FatBlock is IMyBeacon)
                            {
                                IMyBeacon beacon = block.FatBlock as IMyBeacon;
                                beacon.CustomName = owning_fac + " FLAG";
                            }
                        }
                    }
                }
                if (owning_fac != "")
                {
                    Flag the_flag = new Flag(grid, owning_fac, grid.WorldMatrix);
                    flags_in_play.Add(the_flag);
                }

                if (flags_in_play.Count == 2)
                {
                    setup_complete = true;
                }
            }
        }

        public override void UpdateAfterSimulation()
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                _session_timer += 1;
                if (_session_timer == 60)
                {
                    FirstSpawn();
                }
                if (_session_timer % 60 == 0)
                {
                    all_players.Clear();
                    MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);
                }

                if (setup_complete)
                {
                    foreach (var working_flag in flags_in_play)
                    {
                        if (!working_flag.flag_grid.MarkedForClose)
                        {
                            if (working_flag.state == 0)
                            {
                                working_flag.flag_grid.Physics.Enabled = false;
                                if (!game_over)
                                {
                                    foreach (var player in all_players)
                                    {
                                        if ((player.GetPosition() - working_flag.flag_grid.GetPosition()).Length() <= 2f)
                                        {
                                            if (player.Character != null && player.Controller?.ControlledEntity?.Entity is IMyCharacter)
                                            {
                                                if (!player.Character.IsDead)
                                                {
                                                    if (MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId) != working_flag.faction_owner &&
                                                        MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId) != "")
                                                    {
                                                        working_flag.carrier = player;
                                                        working_flag.state = 1;
                                                        MyVisualScriptLogicProvider.SendChatMessage(working_flag.faction_owner + " flag grabbed!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (working_flag.state == 1)
                            {
                                working_flag.flag_grid.Physics.Enabled = true;
                                if (working_flag.carrier.Character != null)
                                {
                                    reuse_matrix = working_flag.carrier.Character.WorldMatrix;
                                    reuse_matrix.Translation = reuse_matrix.Translation + reuse_matrix.Backward * 1.5 + reuse_matrix.Up * 1.5;
                                    working_flag.flag_grid.WorldMatrix = reuse_matrix;
                                }
                            }

                            if (working_flag.state == 2)
                            {
                                working_flag.flag_grid.Physics.Enabled = true;
                                working_flag.current_time_on_ground += 1;
                                if (working_flag.current_time_on_ground <= max_wait)
                                {
                                    working_flag.flag_grid.WorldMatrix = working_flag.dropped_position;
                                    foreach (var player in all_players)
                                    {
                                        if ((player.GetPosition() - working_flag.flag_grid.GetPosition()).Length() <= 2f)
                                        {
                                            if (player.Character != null && player.Controller?.ControlledEntity?.Entity is IMyCharacter)
                                            {
                                                if (!player.Character.IsDead)
                                                {
                                                    if (MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId) != working_flag.faction_owner &&
                                                        MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId) != "")
                                                    {
                                                        working_flag.carrier = player;
                                                        working_flag.state = 1;
                                                        working_flag.current_time_on_ground = 0;
                                                        MyVisualScriptLogicProvider.SendChatMessage(working_flag.faction_owner + " flag grabbed!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    working_flag.flag_grid.WorldMatrix = working_flag.home_position;
                                    working_flag.current_time_on_ground = 0;
                                    working_flag.state = 0;
                                    MyVisualScriptLogicProvider.SendChatMessage(working_flag.faction_owner + " flag reset!");
                                }
                            }
                        }
                    }
                    if (flags_in_play.Count == 2)
                    {
                        reuse_flag_one = flags_in_play[0];
                        reuse_flag_two = flags_in_play[1];

                        if (reuse_flag_one.state == 1 && reuse_flag_two.state == 0 && !reuse_flag_one.flag_grid.MarkedForClose && !reuse_flag_two.flag_grid.MarkedForClose)
                        {
                            if ((reuse_flag_one.flag_grid.GetPosition() - reuse_flag_two.flag_grid.GetPosition()).Length() <= 2f)
                            {
                                CaptureEvent(reuse_flag_two.faction_owner);
                            }
                        }
                        if (reuse_flag_one.state == 0 && reuse_flag_two.state == 1 && !reuse_flag_one.flag_grid.MarkedForClose && !reuse_flag_two.flag_grid.MarkedForClose)
                        {
                            if ((reuse_flag_one.flag_grid.GetPosition() - reuse_flag_two.flag_grid.GetPosition()).Length() <= 2f)
                            {
                                CaptureEvent(reuse_flag_one.faction_owner);
                            }
                        }
                    }
                }
            }

            if (MyAPIGateway.Session != null && !MyAPIGateway.Utilities.IsDedicated)
            {
                MyAPIGateway.Utilities.ShowNotification("                                                                         " +
                                        team_one_faction_tag + ": " + team_one_score.ToString() + "        " + team_two_faction_tag + ": " + team_two_score, 1, "Clear");
            }
        }

        private void CaptureEvent(string faction_owner)
        {
            if (team_one_score < max_score && team_two_score < max_score && !game_over)
            {
                if (faction_owner == team_one_faction_tag)
                {
                    team_one_score += 1;
                    ctf_byte_list.Clear();
                    ctf_byte_list.AddRange(BitConverter.GetBytes(team_one_score));
                    ctf_byte_list.AddRange(BitConverter.GetBytes(team_two_score));
                    MyAPIGateway.Multiplayer.SendMessageToOthers(ctf_net_id, ctf_byte_list.ToArray());
                }
                if (faction_owner == team_two_faction_tag)
                {
                    team_two_score += 1;
                    ctf_byte_list.Clear();
                    ctf_byte_list.AddRange(BitConverter.GetBytes(team_one_score));
                    ctf_byte_list.AddRange(BitConverter.GetBytes(team_two_score));
                    MyAPIGateway.Multiplayer.SendMessageToOthers(ctf_net_id, ctf_byte_list.ToArray());
                }

                if (team_one_score == max_score || team_two_score == max_score)
                {
                    game_over = true;
                    foreach (var flag in flags_in_play)
                    {
                        flag.state = 0;
                        flag.flag_grid.WorldMatrix = flag.home_position;
                    }
                    MyVisualScriptLogicProvider.SendChatMessage("Game Over! Final score: " + team_one_faction_tag + ": " + team_one_score.ToString() + " " + team_two_faction_tag + ": " + team_two_score.ToString());
                }
                else
                {
                    MyVisualScriptLogicProvider.SendChatMessage(faction_owner + " captured a flag!");
                    foreach (var flag in flags_in_play)
                    {
                        flag.state = 0;
                        flag.flag_grid.WorldMatrix = flag.home_position;
                    }
                }
            }
        }

        private void FirstSpawn()
        {
            HashSet<IMyEntity> all_ents = new HashSet<IMyEntity>();
            List<IMyTimerBlock> initial_spawns = new List<IMyTimerBlock>();

            MyAPIGateway.Entities.GetEntities(all_ents);
            foreach (var ent in all_ents)
            {
                if (ent is IMyCubeGrid)
                {
                    reuse_grid = ent as IMyCubeGrid;
                    reuse_blocks.Clear();
                    reuse_grid.GetBlocks(reuse_blocks);
                    foreach (var block in reuse_blocks)
                    {
                        if (block.FatBlock != null && block.FatBlock is IMyTimerBlock)
                        {
                            if (block.FatBlock.BlockDefinition.SubtypeName.Contains("FlagSpawn"))
                            {
                                IMyTimerBlock test_timer = block.FatBlock as IMyTimerBlock;
                                if (!initial_spawns.Contains(test_timer))
                                {
                                    initial_spawns.Add(test_timer);
                                }
                            }
                        }
                    }
                }
            }

            if (initial_spawns.Count == 2)
            {
                foreach (var timer in initial_spawns)
                {
                    Vector3D pos = timer.WorldMatrix.Translation + timer.WorldMatrix.Up * 3;
                    Vector3D forw = timer.WorldMatrix.Forward;
                    Vector3D up = timer.WorldMatrix.Up;
                    string name_to_spawn = "";
                    if (timer.CustomName.Contains("RED"))
                    {
                        name_to_spawn = "RED_CTFFLAG";
                    }
                    else
                    {
                        name_to_spawn = "BLU_CTFFLAG";
                    }
                    MyVisualScriptLogicProvider.SpawnPrefab(name_to_spawn, pos, forw, up);
                }
            }
        }

        protected override void UnloadData()
        {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= CTF_PrefabSpawned;
            MyVisualScriptLogicProvider.PlayerDied -= CTF_PlayerDied;
            MyVisualScriptLogicProvider.PlayerEnteredCockpit -= CTF_CockpitSwitch;
            MyAPIGateway.Multiplayer.UnregisterMessageHandler(ctf_net_id, ctf_handler);
        }
    }
}