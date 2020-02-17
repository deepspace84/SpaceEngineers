
// Isy's Inventory Manager
// ===================
// Version: 2.7.0
// Date: 2019-11-17

//  =======================================================================================
//                                                                            --- Configuration ---
//  =======================================================================================

// --- Sorting ---
// =======================================================================================

// Define the keyword a cargo container has to contain in order to be recognized as a container of the given type.
const string oreContainerKeyword = "Ores";
const string ingotContainerKeyword = "Ingots";
const string componentContainerKeyword = "Components";
const string toolContainerKeyword = "Tools";
const string ammoContainerKeyword = "Ammo";
const string bottleContainerKeyword = "Bottles";

// Keyword an inventory has to contain to be skipped by the sorting (= no items will be taken out)
const string lockedContainerKeyword = "Locked";

// Keyword an inventory has to contain to be excluded from item counting (used by autocrafting and inventory panels)
const string hiddenContainerKeyword = "Hidden";

// Keyword for connectors to disable sorting of a grid, that is docked to that connector.
// This also disables the usage of refineries, arc furnaces and assemblers on that grid.
// Special containers, reactors and O2/H2 generators will still be filled.
string noSortingKeyword = "[No Sorting]";

// Keyword for connectors to disable IIM completely for a ship, that is docked to that connector.
string noIIMKeyword = "[No IIM]";

// Balance items between containers of the same type? This will result in an equal amount of all items in all containers of that type.
bool balanceTypeContainers = false;

// Show a fill level in the container's name?
bool showFillLevel = true;

// Fill bottles before storing them in the bottle container?
bool fillBottles = true;


// --- Automated container assignment ---
// =======================================================================================

// Master switch. If this is set to false, automated container un-/assignment is disabled entirely.
bool autoContainerAssignment = true;

// Assign new containers if a type is full or not present?
bool assignNewContainers = true;

// Unassign empty type containers that aren't needed anymore (at least one of each type always remains).
// This doesn't touch containers with manual priority tokens, like [P1].
bool unassignEmptyContainers = true;

// Assign ores and ingots containers as one?
bool oresIngotsInOne = true;

// Assign tool, ammo and bottle containers as one?
bool toolsAmmoBottlesInOne = true;


// --- Autocrafting ---
// =======================================================================================

// Enable autocrafting or autodisassembling (disassembling will disassemble everything above the wanted amounts)
// All assemblers will be used. To use one manually, add the manualMachineKeyword to it (by default: "!manual")
bool enableAutocrafting = true;
bool enableAutodisassembling = false;

// A LCD with the keyword "Autocrafting" is required where you can set the wanted amount!
// This has multi LCD support. Just append numbers after the keyword, like: "LCD Autocrafting 1", "LCD Autocrafting 2", ..
string autocraftingKeyword = "Autocrafting";

// If you want an assembler to only assemble or only disassemble, use the following keywords in its name.
// A assembler without a keyword will do both tasks
string assembleKeyword = "!assemble-only";
string disassembleKeyword = "!disassemble-only";

// You can teach the script new crafting recipes, by adding one of the following tags to an assembler's name.
// This is needed if the autocrafting screen shows [NoBP] for an item. There are two tag options to teach new blueprints:
// !learn will learn one item and then remove the tag so that the assembler is part of the autocrafting again.
// !learnMany will learn everything you queue in it and will never be part of the autorafting again until you remove the tag.
// To learn an item, queue it up about 100 times (Shift+Klick) and wait until the script removes it from the queue.
string learnKeyword = "!learn";
string learnManyKeyword = "!learnMany";

// Margins for assembling or disassembling items in percent based on the wanted amount (default: 0 = exact value).
// Examples:
// assembleMargin = 5 with a wanted amount of 100 items will only produce new items, if less than 95 are available.
// disassembleMargin = 10 with a wanted amount of 1000 items will only disassemble items if more than 1100 are available.
double assembleMargin = 0;
double disassembleMargin = 0;

// Add the header to every screen when using multiple autocrafting LCDs?
bool headerOnEveryScreen = false;

// To hide certain items from the LCD, simply set their wanted amount to a negative value (e.g.: -1 or -500). These items will be moved
// to the custom data of the first autocrafting LCD. To let them reappear on the LCD again, remove the entry from the custom data.

// Sort the assembler queue based on the most needed components?
bool sortAssemblerQueue = true;

// Autocraft ingots from stone in survival kits until you have proper refineries?
bool enableBasicIngotCrafting = true;

// Disable autocrafting in survival kits when you have regular assemblers?
bool disableBasicAutocrafting = true;


// --- Special Loadout Containers ---
// =======================================================================================

// Keyword an inventory has to contain to be filled with a special loadout (see in it's custom data after you renamed it!)
// Special containers will be filled with your wanted amount of items and never be drained by the auto sorting!
const string specialContainerKeyword = "Special";

// Are special containers allowed to 'steal' items from other special containers with a lower priority?
bool allowSpecialSteal = true;


// --- Refinery handling ---
// =======================================================================================

// By enabling ore balancing, the script will balance the ores between all refinieres so that every refinery has the same amount of ore in it.
// To still use a refinery manually, add the manualMachineKeyword to it (by default: "!manual")
bool enableOreBalancing = true;

// Enable script assisted refinery filling? This will move in the most needed ore and will make room, if the refinery is already full
// Also, the script puts as many ores into the refinery as possible and will pull ores even from other refineries if needed.
bool enableScriptRefineryFilling = true;

// Sort the refinery queue based on the most needed ingots?
bool sortRefiningQueue = true;

// If you want an ore to always be refined first, simply remove the two // in front of the ore name to enable it.
// Enabled ores are refined in order from top to bottom so if you removed several // you can change the order by
// copying and pasting them inside the list. Just be careful to keep the syntax correct: "OreName",
// By default stone is enabled and will always be refined first.
List<String> fixedRefiningList = new List<string> {
    "Stone",
	//"Iron",
	//"Nickel",
	//"Cobalt",
	//"Silicon",
	//"Uranium",
	//"Silver",
	//"Gold",
	//"Platinum",
	//"Magnesium",
	//"Scrap",
};


// --- O2/H2 generator handling ---
// =======================================================================================

// Enable balancing of ice in O2/H2 generators?
// All O2/H2 generators will be used. To use one manually, add the manualMachineKeyword to it (by default: "!manual")
bool enableIceBalancing = true;

// Put ice into O2/H2 generators that are turned off? (default: false)
bool fillOfflineGenerators = false;

// Ice fill level in percent in order to be able to fill bottles? (default: 90)
// Note: O2/H2 generators will pull more ice automatically if value is below 60%
double iceFillLevelPercentage = 90;


// --- Reactor handling ---
// =======================================================================================

// Enable balancing of uranium in reactors? (Note: conveyors of reactors are turned off to stop them from pulling more)
// All reactors will be used. To use one manually, add the manualMachineKeyword to it (by default: "!manual")
bool enableUraniumBalancing = true;

// Put uranium into reactors that are turned off? (default: false)
bool fillOfflineReactors = false;

// Amount of uranium in each reactor? (default: 100 for large grid reactors, 25 for small grid reactors)
double uraniumAmountLargeGrid = 100;
double uraniumAmountSmallGrid = 25;


// --- Assembler Cleanup ---
// =======================================================================================

// This cleans up assemblers, if they have no queue and puts the contents back into a cargo container.
bool enableAssemblerCleanup = true;


// --- Internal item sorting ---
// =======================================================================================

// Sort the items inside all containers?
// Note, that this could cause inventory desync issues in multiplayer, so that items are invisible
// or can't be taken out. Use at your own risk!
bool enableInternalSorting = false;

// Internal sorting pattern. Always combine one of each category, e.g.: 'Ad' for descending item amount (from highest to lowest)
// 1. Quantifier:
// A = amount
// N = name
// T = type (alphabetical)
// X = type (number of items)

// 2. Direction:
// a = ascending
// d = descending

string sortingPattern = "Na";

// Internal sorting can also be set per inventory. Just use '(sort:PATTERN)' in the block's name.
// Example: Small Cargo Container 3 (sort:Ad)
// Note: Using this method, internal sorting will always be activated for this container, even if the main switch is turned off!


// --- LCD panels ---
// =======================================================================================

// To display the main script informations, add the following keyword to any LCD name (default: !IIM-main).
// You can enable or disable specific informations on the LCD by editing its custom data.
string mainLCDKeyword = "!IIM-main";

// To display current item amounts of different types, add the following keyword to any LCD name
// and follow the on screen instructions.
string inventoryLCDKeyword = "!IIM-inventory";

// To display all current warnings and problems, add the following keyword to any LCD name (default: IIM-warnings).
string warningsLCDKeyword = "!IIM-warnings";

// To display the script performance (PB terminal output), add the following keyword to any LCD name (default: !IIM-performance).
string performanceLCDKeyword = "!IIM-performance";

// Default screen font and fontsize, when a screen is first initialized. Fonts: "Debug" or "Monospace"
string defaultFont = "Debug";
float defaultFontSize = 0.6f;


// --- Settings for enthusiasts ---
// =======================================================================================

// Extra breaks between script methods in ticks (1 tick = 16.6ms).
double extraScriptTicks = 0;

// Exclude welders or grinders from sorting? Set this to true, if you have huge welder or grinder walls!
bool excludeWelders = false;
bool excludeGrinders = false;

// Enable connection check for inventories (needed for [No Conveyor] info)?
bool connectionCheck = false;

// Tag inventories, that have no access to the main type containers with [No Conveyor]?
// This only works if the above setting connectionCheck is set to true!
bool showNoConveyorTag = true;

// Script mode: "ship", "station" or blank for autodetect
string scriptMode = "";

// Protect type containers when docking to another grid running the script?
bool protectTypeContainers = true;

// If you want to use a machine manually, append the keyword to it.
// This works for assemblers, refineries, reactors and O2/H2 generators
string manualMachineKeyword = "!manual";


//  =======================================================================================
//                                                                      --- End of Configuration ---
//                                                        Don't change anything beyond this point!
//  =======================================================================================


List<IMyTerminalBlock> Ƀ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ɂ = new List<IMyTerminalBlock>(); List<
        IMyTerminalBlock> Ɂ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ɀ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ȿ = new List<
                IMyTerminalBlock>(); List<IMyTerminalBlock> Ƚ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> Ʉ = new List<IMyTerminalBlock>(); List<
                         IMyTerminalBlock> Ʌ = new List<IMyTerminalBlock>(); List<IMyShipConnector> ɦ = new List<IMyShipConnector>(); List<IMyRefinery> ɧ = new List<
                                 IMyRefinery>(); List<IMyRefinery> ɨ = new List<IMyRefinery>(); List<IMyRefinery> ɩ = new List<IMyRefinery>(); List<IMyRefinery> ɪ = new List<
                                          IMyRefinery>(); List<IMyAssembler> ɫ = new List<IMyAssembler>(); List<IMyAssembler> ɬ = new List<IMyAssembler>(); List<IMyAssembler> ɭ = new
                                                   List<IMyAssembler>(); List<IMyAssembler> ɮ = new List<IMyAssembler>(); List<IMyGasGenerator> ɯ = new List<IMyGasGenerator>(); List<
                                                            IMyReactor> ɰ = new List<IMyReactor>(); List<IMyTextPanel> ɱ = new List<IMyTextPanel>(); List<string> ɲ = new List<string>(); HashSet<
                                                                        IMyCubeGrid> ɳ = new HashSet<IMyCubeGrid>(); HashSet<IMyCubeGrid> ɴ = new HashSet<IMyCubeGrid>(); List<IMyTerminalBlock> ɵ = new List<
                                                                                IMyTerminalBlock>(); List<IMyTerminalBlock> ɶ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ʇ = new List<IMyTerminalBlock>(); List<
                                                                                         IMyTerminalBlock> ʅ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ʄ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ʃ = new List<
                                                                                                 IMyTerminalBlock>(); List<IMyTerminalBlock> ʂ = new List<IMyTerminalBlock>(); string[] ʁ ={oreContainerKeyword,ingotContainerKeyword,
componentContainerKeyword,toolContainerKeyword,ammoContainerKeyword,bottleContainerKeyword}; string ʀ = ""; IMyTerminalBlock ʆ; IMyInventory ɿ;
IMyTerminalBlock ɽ; bool ɼ = false; int ɻ = 0; int ɺ = 0; int ɹ = 0; int ɸ = 0; int ɷ = 0; int ɾ = 0; int ɥ = 0; int ɛ = 0; int Ɇ = 0; int ɇ = 0; int Ɉ = 0; int ɉ = 0; int Ɋ = 0;
int ɋ = 0; string Ɍ = ""; string[] ɍ = { "/", "-", "\\", "|" }; int Ɏ = 0; List<IMyTerminalBlock> ɏ = new List<IMyTerminalBlock>(); List<
                      IMyTerminalBlock> ɐ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ɑ = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> ɒ = new List<
                              IMyTerminalBlock>(); string[] ũ ={"showHeading=true","showWarnings=true","showContainerStats=true","showManagedBlocks=true",
"showLastAction=true","scrollTextIfNeeded=true"}; string[] ɓ = { "showHeading=true", "scrollTextIfNeeded=true" }; string Ľ; int ɤ = 0; string ɢ = ""; bool ɡ
               = false; bool ɠ = false; HashSet<string> ɟ = new HashSet<string>(); HashSet<string> ɞ = new HashSet<string>(); int ɝ = 0; int ɣ = 0; bool ɜ =
                                  true; bool ɚ = false; int ə = 0; string ɘ = "itemID;blueprintID"; Dictionary<string, string> ɗ = new Dictionary<string, string>(){{
"oreContainer",oreContainerKeyword},{"ingotContainer",ingotContainerKeyword},{"componentContainer",componentContainerKeyword},{
"toolContainer",toolContainerKeyword},{"ammoContainer",ammoContainerKeyword},{"bottleContainer",bottleContainerKeyword},{
"lockedContainer",lockedContainerKeyword},{"specialContainer",specialContainerKeyword},{"oreBalancing","true"},{"iceBalancing","true"},{
"uraniumBalancing","true"}}; string ɖ = "Isy's Autocrafting"; string ɕ = "Remove a line to show this item on the LCD again!"; char[] ɔ ={'=','>',
'<'}; IMyAssembler ȼ; string ǰ = ""; MyDefinitionId Ȓ; List<String> Ǳ = new List<string>{"Uranium","Silicon","Silver","Gold",
"Platinum","Magnesium","Iron","Nickel","Cobalt","Stone","Scrap"}; List<MyItemType> ǲ = new List<MyItemType>(); List<MyItemType> ǳ = new
     List<MyItemType>(); Dictionary<string, double> Ǵ = new Dictionary<string, double>(){{"Cobalt",0.3},{"Gold",0.01},{"Iron",0.7},{
"Magnesium",0.007},{"Nickel",0.4},{"Platinum",0.005},{"Silicon",0.7},{"Silver",0.1},{"Stone",0.014},{"Uranium",0.01}}; const string
 ǵ = "MyObjectBuilder_"; const string Ƕ = "Ore"; const string Ƿ = "Ingot"; const string Ǹ = "Component"; const string ǹ = "AmmoMagazine"
             ; const string Ǻ = "OxygenContainerObject"; const string ǻ = "GasContainerObject"; const string Ǽ = "PhysicalGunObject"; const
                       string ǽ = "PhysicalObject"; const string Ǿ = "ConsumableItem"; const string ǿ = "Datapad"; const string Ȁ = ǵ + "BlueprintDefinition/";
SortedSet<MyDefinitionId> ȑ = new SortedSet<MyDefinitionId>(new ř()); SortedSet<string> ȏ = new SortedSet<string>(); SortedSet<string> Ȏ =
        new SortedSet<string>(); SortedSet<string> ȍ = new SortedSet<string>(); SortedSet<string> Ȍ = new SortedSet<string>(); SortedSet<
                 string> ȋ = new SortedSet<string>(); SortedSet<string> Ȋ = new SortedSet<string>(); SortedSet<string> Ȑ = new SortedSet<string>();
SortedSet<string> ȉ = new SortedSet<string>(); SortedSet<string> ȇ = new SortedSet<string>(); SortedSet<string> Ȇ = new SortedSet<string>();
Dictionary<MyDefinitionId, double> ȅ = new Dictionary<MyDefinitionId, double>(); Dictionary<MyDefinitionId, double> Ȅ = new Dictionary<
      MyDefinitionId, double>(); Dictionary<MyDefinitionId, double> ȃ = new Dictionary<MyDefinitionId, double>(); Dictionary<MyDefinitionId,
              MyDefinitionId> Ȃ = new Dictionary<MyDefinitionId, MyDefinitionId>(); Dictionary<MyDefinitionId, MyDefinitionId> ȁ = new Dictionary<
                   MyDefinitionId, MyDefinitionId>(); Dictionary<string, MyDefinitionId> ǯ = new Dictionary<string, MyDefinitionId>(); Dictionary<string, string> Ǚ
                           = new Dictionary<string, string>(); bool ǣ = false; string ǚ = "station_mode;\n"; string Ǜ = "ship_mode;\n"; string ǜ = "[PROTECTED] ";
string ǝ = ""; string Ǟ = ""; string ǟ = ""; DateTime Ǡ; string[] ǡ ={"Get inventory blocks","Find new items","Create item lists",
"Name correction","Assign containers","Fill special containers","Sort items","Container balancing","Internal sorting",
"Add fill level to names","Get global item amount","Get assembler queue","Autocrafting","Sort assembler queue","Clean up assemblers",
"Learn unknown blueprints","Fill refineries","Ore balancing","Ice balancing","Uranium balancing"}; Program()
{
    Echo("Script ready to be launched..\n"
); assembleMargin /= 100; disassembleMargin /= 100; Runtime.UpdateFrequency = UpdateFrequency.Update100;
}
void Main(string Ǣ)
{
    if (ɤ >=
10)
    {
        throw new Exception("Too many errors in script step " + ɣ + ":\n" + ǡ[ɣ] +
    "\n\nPlease recompile!\nScript stoppped!\n\nLast error:\n" + ɢ + "\n");
    }
    try
    {
        ǁ("", true); if (ɜ)
        {
            if (ɣ > 0) Echo("Initializing script.. (" + (ɣ + 1) + "/10) \n"); if (ɣ >= 2)
            {
                Echo(
"Getting inventory blocks.."); if (ɣ == 2) ȱ(); if (ɼ) return;
            }
            if (ɣ >= 3)
            {
                Echo("Loading saved items.."); if (ɣ == 3)
                {
                    if (!Ô())
                    {
                        Echo("-> No assembler found!"); Echo(
"-> Can't check saved blueprints.."); Echo("\nScript stopped!"); Me.Enabled = false; return;
                    }
                }
            }
            if (ɣ >= 4)
            {
                Echo("Clearing assembler queues.."); if (ɣ == 4 && (
enableAutocrafting || enableAutodisassembling))
                {
                    GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(ɱ, µ => µ.IsSameConstructAs(Me) && µ.CustomName.
Contains(autocraftingKeyword)); if (ɱ.Count > 0)
                    {
                        foreach (var Ɯ in ɫ)
                        {
                            Ɯ.Mode = MyAssemblerMode.Disassembly; Ɯ.ClearQueue(); Ɯ.Mode =
MyAssemblerMode.Assembly; Ɯ.ClearQueue();
                        }
                    }
                }
            }
            if (ɣ >= 5) { Echo("Checking blueprints.."); if (ɣ == 5) { foreach (var D in ȑ) { Ƣ(D); } } }
            if (ɣ >= 6)
            {
                Echo(
"Checking type containers.."); if (ɣ == 6) ȴ();
            }
            if (ɣ >= 7)
            {
                if (scriptMode == "station") { ǣ = true; } else if (Me.CubeGrid.IsStatic && scriptMode != "ship") { ǣ = true; }
                Echo
("Setting script mode to: " + (ǣ ? "station.." : "ship..")); if (ɣ == 7) Me.CustomData = (ǣ ? ǚ : Ǜ) + Me.CustomData.Replace(ǚ, "").Replace(Ǜ
, "");
            }
            if (ɣ >= 8) { Echo("Starting script.."); }
            if (ɣ >= 9) { ɣ = 0; ɜ = false; return; }
            ɣ++; return;
        }
        if (Ǣ != "")
        {
            ǟ = Ǣ; ɣ = 1; Ǟ = ""; Ǡ = DateTime.Now;
        }
        if (ɝ < extraScriptTicks) { Runtime.UpdateFrequency = UpdateFrequency.Update1; ɝ++; return; }
        if (ɚ)
        {
            if (ə == 0) Ġ(); if (ə == 1) ĺ(); if (ə == 2
) ľ(); if (ə == 3) ķ(); if (ə > 3) ə = 0; ɚ = false; return;
        }
        if (ɣ == 0 || ɡ || ɼ)
        {
            if (!ɠ) ȱ(); if (ɼ) return; ɡ = false; ɠ = false; if (!ƻ(30))
            {
                ɏ = Ǝ(
mainLCDKeyword, ũ); ɐ = Ǝ(warningsLCDKeyword, ɓ); ɑ = Ǝ(performanceLCDKeyword, ɓ); ɒ = Ǝ(inventoryLCDKeyword);
            }
            else { ɡ = true; ɠ = true; }
            if (ɣ == 0)
            {
                ǁ(ǡ[ɣ]
); Ç(); ɣ++;
            }
            return;
        }
        if (!ǣ) ǩ(); if (Ǯ(ǟ)) return; ɝ = 0; Runtime.UpdateFrequency = UpdateFrequency.Update10; ɚ = true; if (ɣ == 1) { Ï(); }
        if (
ɣ == 2) { Î(); }
        if (ɣ == 3) { ȥ(); }
        if (ɣ == 4)
        {
            if (autoContainerAssignment) { if (unassignEmptyContainers) Ȝ(); if (assignNewContainers) ȗ(); }
        }
        if (ɣ == 5) { if (ʂ.Count != 0) ˎ(); }
        if (ɣ == 6) { Ⱦ(); }
        if (ɣ == 7) { if (balanceTypeContainers) ʜ(); }
        if (ɣ == 8) { ͺ(); }
        if (ɣ == 9) { Ή(Ƚ); Ή(ʂ); }
        if (ɣ
== 10) { Ɛ(); }
        if (ɣ == 11) { if (enableAutocrafting || enableAutodisassembling) Ɲ(); }
        if (ɣ == 12)
        {
            if (enableAutocrafting ||
enableAutodisassembling) Α();
        }
        if (ɣ == 13) { if (sortAssemblerQueue) ʿ(); }
        if (ɣ == 14)
        {
            if (enableAssemblerCleanup) ʾ(); if (enableBasicIngotCrafting)
            {
                if (ɧ.
Count > 0) { enableBasicIngotCrafting = false; }
                else { ʘ(); }
            }
        }
        if (ɣ == 15) { Ơ(); }
        if (ɣ == 16) { ʥ(); }
        if (ɣ == 17)
        {
            if (enableOreBalancing) ʣ(); if (
sortRefiningQueue) { ʢ(ɩ, ǲ); ʢ(ɪ, ǳ); }
        }
        if (ɣ == 18) { if (enableIceBalancing) ʱ(); }
        if (ɣ == 19)
        {
            if (enableUraniumBalancing)
            {
                ë("uraniumBalancing", "true")
; Ĕ();
            }
            else if (!enableUraniumBalancing && â("uraniumBalancing") == "true")
            {
                ë("uraniumBalancing", "false"); foreach (IMyReactor ď
in ɰ) { ď.UseConveyorSystem = true; }
            }
        }
        ǁ(ǡ[ɣ]); Ç(); if (ɣ >= 19)
        {
            ɣ = 0; ɟ = new HashSet<string>(ɞ); ɞ.Clear(); if (ɤ > 0) ɤ--; if (ɟ.Count == 0) Ľ =
null;
        }
        else { ɣ++; }
        Ɏ = Ɏ >= 3 ? 0 : Ɏ + 1;
    }
    catch (NullReferenceException e)
    {
        ɤ++; ɡ = true; ɼ = false; ɢ = e.ToString(); Ʀ(
"Execution of script step aborted:\n" + ǡ[ɣ] + " (ID: " + ɣ + ")\n\nCached block not available..");
    }
    catch (Exception e)
    {
        ɤ++; ɡ = true; ɼ = false; ɢ = e.ToString(); Ʀ(
"Critical error in script step:\n" + ǡ[ɣ] + " (ID: " + ɣ + ")\n\n" + e);
    }
}
bool Ǯ(string Ǣ)
{
    if (Ǣ.Contains("pauseThisPB"))
    {
        Echo("Script execution paused!\n"); var Ǥ = Ǣ.
Split(';'); if (Ǥ.Length == 3)
        {
            Echo("Found:"); Echo("'" + Ǥ[1] + "'"); Echo("on grid:"); Echo("'" + Ǥ[2] + "'"); Echo(
"also running the script.\n"); Echo("Type container protection: " + (protectTypeContainers ? "ON" : "OFF") + "\n"); Echo(
       "Everything else is managed by the other script.");
        }
        return true;
    }
    bool ǥ = true; bool Ǧ = true; bool ǧ = false; if (Ǣ != "reset" && Ǣ != "msg")
    {
        if (!Ǣ.Contains(" on") && !Ǣ.Contains(" off")
&& !Ǣ.Contains(" toggle")) return false; if (Ǣ.Contains(" off")) Ǧ = false; if (Ǣ.Contains(" toggle")) ǧ = true;
    }
    if (Ǣ == "reset")
    {
        ƥ();
        return true;
    }
    else if (Ǣ == "msg") { }
    else if (Ǣ.StartsWith("balanceTypeContainers"))
    {
        ǝ = "Balance type containers"; if (ǧ) Ǧ = !
balanceTypeContainers; balanceTypeContainers = Ǧ;
    }
    else if (Ǣ.StartsWith("showFillLevel"))
    {
        ǝ = "Show fill level"; if (ǧ) Ǧ = !showFillLevel; showFillLevel
= Ǧ;
    }
    else if (Ǣ.StartsWith("autoContainerAssignment"))
    {
        ǝ = "Auto assign containers"; if (ǧ) Ǧ = !autoContainerAssignment;
        autoContainerAssignment = Ǧ;
    }
    else if (Ǣ.StartsWith("assignNewContainers"))
    {
        ǝ = "Assign new containers"; if (ǧ) Ǧ = !assignNewContainers;
        assignNewContainers = Ǧ;
    }
    else if (Ǣ.StartsWith("unassignEmptyContainers"))
    {
        ǝ = "Unassign empty containers"; if (ǧ) Ǧ = !unassignEmptyContainers;
        unassignEmptyContainers = Ǧ;
    }
    else if (Ǣ.StartsWith("oresIngotsInOne"))
    {
        ǝ = "Assign ores and ingots as one"; if (ǧ) Ǧ = !oresIngotsInOne; oresIngotsInOne = Ǧ
;
    }
    else if (Ǣ.StartsWith("toolsAmmoBottlesInOne"))
    {
        ǝ = "Assign tools, ammo and bottles as one"; if (ǧ) Ǧ = !toolsAmmoBottlesInOne;
        toolsAmmoBottlesInOne = Ǧ;
    }
    else if (Ǣ.StartsWith("fillBottles")) { ǝ = "Fill bottles"; if (ǧ) Ǧ = !fillBottles; fillBottles = Ǧ; }
    else if (Ǣ.StartsWith(
"enableAutocrafting")) { ǝ = "Autocrafting"; if (ǧ) Ǧ = !enableAutocrafting; enableAutocrafting = Ǧ; }
    else if (Ǣ.StartsWith("enableAutodisassembling"))
    {
        ǝ =
"Autodisassembling"; if (ǧ) Ǧ = !enableAutodisassembling; enableAutodisassembling = Ǧ;
    }
    else if (Ǣ.StartsWith("headerOnEveryScreen"))
    {
        ǝ =
"Show header on every autocrafting screen"; if (ǧ) Ǧ = !headerOnEveryScreen; headerOnEveryScreen = Ǧ;
    }
    else if (Ǣ.StartsWith("sortAssemblerQueue"))
    {
        ǝ = "Sort assembler queue"
; if (ǧ) Ǧ = !sortAssemblerQueue; sortAssemblerQueue = Ǧ;
    }
    else if (Ǣ.StartsWith("enableBasicIngotCrafting"))
    {
        ǝ =
"Basic ingot crafting"; if (ǧ) Ǧ = !enableBasicIngotCrafting; enableBasicIngotCrafting = Ǧ;
    }
    else if (Ǣ.StartsWith("disableBasicAutocrafting"))
    {
        ǝ =
"Disable autocrafting in survival kits"; if (ǧ) Ǧ = !disableBasicAutocrafting; disableBasicAutocrafting = Ǧ;
    }
    else if (Ǣ.StartsWith("allowSpecialSteal"))
    {
        ǝ =
"Allow special container steal"; if (ǧ) Ǧ = !allowSpecialSteal; allowSpecialSteal = Ǧ;
    }
    else if (Ǣ.StartsWith("enableOreBalancing"))
    {
        ǝ = "Ore balancing"; if (ǧ) Ǧ = !
enableOreBalancing; enableOreBalancing = Ǧ;
    }
    else if (Ǣ.StartsWith("enableScriptRefineryFilling"))
    {
        ǝ = "Script assisted refinery filling"; if (ǧ) Ǧ =
!enableScriptRefineryFilling; enableScriptRefineryFilling = Ǧ;
    }
    else if (Ǣ.StartsWith("sortRefiningQueue"))
    {
        ǝ =
"Sort refinery queue"; if (ǧ) Ǧ = !sortRefiningQueue; sortRefiningQueue = Ǧ;
    }
    else if (Ǣ.StartsWith("enableIceBalancing"))
    {
        ǝ = "Ice balancing"; if (ǧ) Ǧ = !
enableIceBalancing; enableIceBalancing = Ǧ;
    }
    else if (Ǣ.StartsWith("fillOfflineGenerators"))
    {
        ǝ = "Fill offline O2/H2 generators"; if (ǧ) Ǧ = !
fillOfflineGenerators; fillOfflineGenerators = Ǧ;
    }
    else if (Ǣ.StartsWith("enableUraniumBalancing"))
    {
        ǝ = "Uranium balancing"; if (ǧ) Ǧ = !
enableUraniumBalancing; enableUraniumBalancing = Ǧ;
    }
    else if (Ǣ.StartsWith("fillOfflineReactors"))
    {
        ǝ = "Fill offline reactors"; if (ǧ) Ǧ = !
fillOfflineReactors; fillOfflineReactors = Ǧ;
    }
    else if (Ǣ.StartsWith("enableAssemblerCleanup"))
    {
        ǝ = "Assembler cleanup"; if (ǧ) Ǧ = !
enableAssemblerCleanup; enableAssemblerCleanup = Ǧ;
    }
    else if (Ǣ.StartsWith("enableInternalSorting"))
    {
        ǝ = "Internal sorting"; if (ǧ) Ǧ = !
enableInternalSorting; enableInternalSorting = Ǧ;
    }
    else if (Ǣ.StartsWith("excludeWelders"))
    {
        ǝ = "Exclude welders"; if (ǧ) Ǧ = !excludeWelders;
        excludeWelders = Ǧ;
    }
    else if (Ǣ.StartsWith("excludeGrinders")) { ǝ = "Exclude grinders"; if (ǧ) Ǧ = !excludeGrinders; excludeGrinders = Ǧ; }
    else if (Ǣ.
StartsWith("connectionCheck")) { ǝ = "Connection check"; if (ǧ) Ǧ = !connectionCheck; connectionCheck = Ǧ; }
    else if (Ǣ.StartsWith(
"showNoConveyorTag")) { ǝ = "Show no conveyor access"; if (ǧ) Ǧ = !showNoConveyorTag; showNoConveyorTag = Ǧ; }
    else if (Ǣ.StartsWith(
"protectTypeContainers")) { ǝ = "Protect type containers"; if (ǧ) Ǧ = !protectTypeContainers; protectTypeContainers = Ǧ; }
    else { ǥ = false; }
    if (ǥ)
    {
        TimeSpan Ǩ =
DateTime.Now - Ǡ; if (Ǟ == "") Ǟ = ǝ + " temporarily " + (Ǧ ? "enabled" : "disabled") + "!\n"; Echo(Ǟ); Echo("Continuing in " + Math.Ceiling(3 - Ǩ.
TotalSeconds) + " seconds.."); ǟ = "msg"; if (Ǩ.TotalSeconds >= 3) { ǝ = ""; Ǟ = ""; ǟ = ""; }
    }
    return ǥ;
}
void ǩ()
{
    List<IMyProgrammableBlock> Ǫ = new List<
IMyProgrammableBlock>(); GridTerminalSystem.GetBlocksOfType(Ǫ, ǫ => ǫ != Me); if (ǟ.StartsWith("pauseThisPB") || ǟ == "")
    {
        ǟ = ""; foreach (var Ǭ in Ǫ)
        {
            if (Ǭ.
CustomData.Contains(ǚ) || (Ǭ.CustomData.Contains(Ǜ) && Ú(Ǭ) < Ú(Me)))
            {
                ǟ = "pauseThisPB;" + Ǭ.CustomName + ";" + Ǭ.CubeGrid.CustomName; foreach (
var V in Ƚ) { if (protectTypeContainers && !V.CustomName.Contains(ǜ) && V.IsSameConstructAs(Me)) V.CustomName = ǜ + V.CustomName; }
                return;
            }
        }
        if (ǟ == "") { foreach (var V in Ʉ) { V.CustomName = V.CustomName.Replace(ǜ, ""); } }
    }
}
void ǭ()
{
    ɳ.Clear(); GridTerminalSystem.
GetBlocksOfType<IMyShipConnector>(ɦ, ŝ => ŝ.IsSameConstructAs(Me) && ŝ.CustomName.Contains(noSortingKeyword)); foreach (var Ȉ in ɦ)
    {
        if (Ȉ.
Status != MyShipConnectorStatus.Connected) continue; ɳ.Add(Ȉ.OtherConnector.CubeGrid);
    }
    ɴ.Clear(); GridTerminalSystem.
GetBlocksOfType<IMyShipConnector>(ɦ, ŝ => ŝ.IsSameConstructAs(Me) && ŝ.CustomName.Contains(noIIMKeyword)); foreach (var Ȉ in ɦ)
    {
        if (Ȉ.Status !=
MyShipConnectorStatus.Connected) continue; ɴ.Add(Ȉ.OtherConnector.CubeGrid);
    }
}
void Ȩ()
{
    if (ɿ != null) { try { ɿ = ʆ.GetInventory(0); } catch { ɿ = null; } }
    if (ɿ
== null)
    {
        try
        {
            foreach (var V in Ƚ)
            {
                foreach (var Q in ɂ)
                {
                    if (V == Q) continue; if (V.GetInventory(0).IsConnectedTo(Q.GetInventory(0))
) { ʆ = Ƚ[0]; ɿ = ʆ.GetInventory(0); return; }
                }
            }
        }
        catch { ɿ = null; }
    }
}
void ȩ(IMyTerminalBlock Q)
{
    foreach (var Æ in ɴ)
    {
        if (Q.CubeGrid.
IsSameConstructAs(Æ)) return;
    }
    if (Q.BlockDefinition.SubtypeId.Contains("Locker") || Q.BlockDefinition.SubtypeId == "VendingMachine") return; if (Q
is IMyShipWelder && excludeWelders) return; if (Q is IMyShipGrinder && excludeGrinders) return; string ȣ = Q.CustomName; if (ȣ.Contains
(ǜ)) { Ʉ.Add(Q); return; }
    bool Ȫ = ȣ.Contains(specialContainerKeyword), ȫ = ȣ.Contains(lockedContainerKeyword), Ȭ = ȣ.Contains(
manualMachineKeyword), ȭ = ȣ.Contains(hiddenContainerKeyword), Ȯ = ȣ.Contains(learnKeyword) || ȣ.Contains(learnManyKeyword), ȯ = true, Ȱ = false; if (!Ȫ && !(
Q is IMyReactor) && !(Q is IMyGasGenerator)) { foreach (var Æ in ɳ) { if (Q.CubeGrid.IsSameConstructAs(Æ)) return; } }
    if (!ȭ) ɂ.Add(Q)
; if (connectionCheck)
    {
        if (ɿ != null) { if (!Q.GetInventory(0).IsConnectedTo(ɿ)) { ȯ = false; } }
        if (!ȯ)
        {
            if (showNoConveyorTag) ȷ(Q,
"[No Conveyor]"); return;
        }
        else { ȷ(Q, "[No Conveyor]", false); }
    }
    if (ȣ.Contains(oreContainerKeyword)) { ɵ.Add(Q); Ȱ = true; }
    if (ȣ.Contains(
ingotContainerKeyword)) { ɶ.Add(Q); Ȱ = true; }
    if (ȣ.Contains(componentContainerKeyword)) { ʇ.Add(Q); Ȱ = true; }
    if (ȣ.Contains(toolContainerKeyword))
    {
        ʅ.
Add(Q); Ȱ = true;
    }
    if (ȣ.Contains(ammoContainerKeyword)) { ʄ.Add(Q); Ȱ = true; }
    if (ȣ.Contains(bottleContainerKeyword))
    {
        ʃ.Add(Q); Ȱ = true
;
    }
    if (Ȫ) { ʂ.Add(Q); if (Q.CustomData.Length < 200) è(Q); }
    if (Ȱ) Ƚ.Add(Q); if (Q.GetType().ToString().Contains("Weapon")) return; if (Q
is IMyRefinery)
    {
        if (Q.IsSameConstructAs(Me) && !Ȫ && !Ȭ && Q.IsWorking)
        {
            (Q as IMyRefinery).UseConveyorSystem = true; ɧ.Add(Q as
IMyRefinery); if (Q.BlockDefinition.SubtypeId == "Blast Furnace") { ɪ.Add(Q as IMyRefinery); } else { ɩ.Add(Q as IMyRefinery); }
        }
        if (!ȫ && Q.
GetInventory(1).ItemCount > 0) ɨ.Add(Q as IMyRefinery);
    }
    else if (Q is IMyAssembler)
    {
        if (Q.IsSameConstructAs(Me) && !Ȭ && !Ȯ && Q.IsWorking)
        {
            ɫ.
Add(Q as IMyAssembler); if (Q.BlockDefinition.SubtypeId.Contains("Survival")) ɮ.Add(Q as IMyAssembler);
        }
        if (!ȫ && !Ȯ && Q.
GetInventory(1).ItemCount > 0) ɬ.Add(Q as IMyAssembler); if (Ȯ) ɭ.Add(Q as IMyAssembler);
    }
    else if (Q is IMyGasGenerator)
    {
        if (!Ȫ && !Ȭ && Q.
IsFunctional)
        {
            if (fillOfflineGenerators && !(Q as IMyGasGenerator).Enabled) { ɯ.Add(Q as IMyGasGenerator); }
            else if ((Q as IMyGasGenerator)
.Enabled) { ɯ.Add(Q as IMyGasGenerator); }
        }
    }
    else if (Q is IMyReactor)
    {
        if (!Ȫ && !Ȭ && Q.IsFunctional)
        {
            if (fillOfflineReactors && !(Q
as IMyReactor).Enabled) { ɰ.Add(Q as IMyReactor); }
            else if ((Q as IMyReactor).Enabled) { ɰ.Add(Q as IMyReactor); }
        }
    }
    else if (Q is
IMyCargoContainer) { if (Q.IsSameConstructAs(Me) && !Ȱ && !ȫ && !Ȫ) Ʌ.Add(Q); }
    if (Q.InventoryCount == 1 && !Ȫ && !ȫ && !(Q is IMyReactor))
    {
        if (Q.GetInventory
(0).ItemCount > 0) ɀ.Add(Q); if (!Q.BlockDefinition.TypeIdString.Contains("Oxygen"))
        {
            if (Q.IsSameConstructAs(Me))
            {
                ȿ.Insert(0, Q)
;
            }
            else { ȿ.Add(Q); }
        }
    }
}
void ȱ()
{
    if (!ɼ)
    {
        ǭ(); if (connectionCheck) Ȩ(); try
        {
            for (int J = 0; J < ʂ.Count; J++)
            {
                if (!ʂ[J].CustomName.
Contains(specialContainerKeyword)) ʂ[J].CustomData = "";
            }
        }
        catch { }
        Ƚ.Clear(); ɵ.Clear(); ɶ.Clear(); ʇ.Clear(); ʅ.Clear(); ʄ.Clear(); ʃ.
Clear(); ʂ.Clear(); Ʌ.Clear(); Ʉ.Clear(); ɂ.Clear(); ɀ.Clear(); ȿ.Clear(); ɧ.Clear(); ɩ.Clear(); ɪ.Clear(); ɨ.Clear(); ɫ.Clear(); ɮ.Clear
(); ɬ.Clear(); ɯ.Clear(); ɰ.Clear(); ɻ = 0; GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(Ƀ, ŕ => ŕ.HasInventory);
    }
    Runtime.
UpdateFrequency = UpdateFrequency.Update1; for (int J = ɻ; J < Ƀ.Count; J++) { ȩ(Ƀ[J]); ɻ++; if (J % 200 == 0) { ɼ = true; return; } }
    if (ɺ == 0) Ⱥ(ɵ); if (ɺ == 1) Ⱥ(ɶ);
    if (ɺ == 2) Ⱥ(ʇ); if (ɺ == 3) Ⱥ(ʅ); if (ɺ == 4) Ⱥ(ʄ); if (ɺ == 5) Ⱥ(ʂ); if (ɺ == 6) Ⱥ(ʃ); ɺ++; if (ɺ > 6) { ɺ = 0; } else { ɼ = true; return; }
    if (
disableBasicAutocrafting && ɫ.Count != ɮ.Count) ɫ.RemoveAll(ī => ī.BlockDefinition.SubtypeId.Contains("Survival")); if (fillBottles)
    {
        ɀ.Sort((Ȼ, ŕ) => ŕ.
BlockDefinition.TypeIdString.Contains("Oxygen").CompareTo(Ȼ.BlockDefinition.TypeIdString.Contains("Oxygen")));
    }
    ɼ = false; Runtime.
UpdateFrequency = UpdateFrequency.Update10;
}
void Ⱥ(List<IMyTerminalBlock> ȹ)
{
    if (ȹ.Count >= 2 && ȹ.Count <= 500) ȹ.Sort((Ȼ, ŕ) => Ú(Ȼ).CompareTo(Ú(ŕ)
)); if (!ƻ()) ɺ++;
}
void ȷ(IMyTerminalBlock Q, string ȶ, bool ȵ = true)
{
    if (ȵ)
    {
        if (Q.CustomName.Contains(ȶ)) return; Q.CustomName +=
" " + ȶ;
    }
    else { if (!Q.CustomName.Contains(ȶ)) return; Q.CustomName = Q.CustomName.Replace(" " + ȶ, "").Replace(ȶ, "").TrimEnd(' '); }
}
void ȴ()
{
    bool ȳ = false; string Ȳ = â("oreContainer"); string ȸ = â("ingotContainer"); string ȧ = â("componentContainer"); string Ȥ = â(
       "toolContainer"); string ȓ = â("ammoContainer"); string Ȕ = â("bottleContainer"); string ȕ = â("lockedContainer"); string Ȗ = â("specialContainer")
                 ; if (oreContainerKeyword != Ȳ) { ȳ = true; }
    else if (ingotContainerKeyword != ȸ) { ȳ = true; }
    else if (componentContainerKeyword != ȧ)
    {
        ȳ =
true;
    }
    else if (toolContainerKeyword != Ȥ) { ȳ = true; }
    else if (ammoContainerKeyword != ȓ) { ȳ = true; }
    else if (bottleContainerKeyword != Ȕ)
    {
        ȳ
= true;
    }
    else if (lockedContainerKeyword != ȕ) { ȳ = true; } else if (specialContainerKeyword != Ȗ) { ȳ = true; }
    if (ȳ)
    {
        for (int J = 0; J < ɂ.Count
; J++)
        {
            if (ɂ[J].CustomName.Contains(Ȳ)) { ɂ[J].CustomName = ɂ[J].CustomName.Replace(Ȳ, oreContainerKeyword); }
            if (ɂ[J].CustomName.
Contains(ȸ)) { ɂ[J].CustomName = ɂ[J].CustomName.Replace(ȸ, ingotContainerKeyword); }
            if (ɂ[J].CustomName.Contains(ȧ))
            {
                ɂ[J].CustomName = ɂ
[J].CustomName.Replace(ȧ, componentContainerKeyword);
            }
            if (ɂ[J].CustomName.Contains(Ȥ))
            {
                ɂ[J].CustomName = ɂ[J].CustomName.
Replace(Ȥ, toolContainerKeyword);
            }
            if (ɂ[J].CustomName.Contains(ȓ))
            {
                ɂ[J].CustomName = ɂ[J].CustomName.Replace(ȓ, ammoContainerKeyword
);
            }
            if (ɂ[J].CustomName.Contains(Ȕ)) { ɂ[J].CustomName = ɂ[J].CustomName.Replace(Ȕ, bottleContainerKeyword); }
            if (ɂ[J].CustomName.
Contains(ȕ)) { ɂ[J].CustomName = ɂ[J].CustomName.Replace(ȕ, lockedContainerKeyword); }
            if (ɂ[J].CustomName.Contains(Ȗ))
            {
                ɂ[J].CustomName =
ɂ[J].CustomName.Replace(Ȗ, specialContainerKeyword);
            }
        }
        ë("oreContainer", oreContainerKeyword); ë("ingotContainer",
ingotContainerKeyword); ë("componentContainer", componentContainerKeyword); ë("toolContainer", toolContainerKeyword); ë("ammoContainer",
ammoContainerKeyword); ë("bottleContainer", bottleContainerKeyword); ë("lockedContainer", lockedContainerKeyword); ë("specialContainer",
specialContainerKeyword);
    }
}
void ȗ()
{
    for (int J = 0; J < Ʌ.Count; J++)
    {
        bool Ș = false; bool ș = false; string Ț = Ʌ[J].CustomName; string ț = ""; if (ɵ.Count == 0 || ʀ
== "Ore") { if (oresIngotsInOne) { ș = true; } else { Ʌ[J].CustomName += " " + oreContainerKeyword; ɵ.Add(Ʌ[J]); ț = "Ores"; } }
        else if (ɶ.Count
== 0 || ʀ == "Ingot") { if (oresIngotsInOne) { ș = true; } else { Ʌ[J].CustomName += " " + ingotContainerKeyword; ɶ.Add(Ʌ[J]); ț = "Ingots"; } }
        else
if (ʇ.Count == 0 || ʀ == "Component") { Ʌ[J].CustomName += " " + componentContainerKeyword; ʇ.Add(Ʌ[J]); ț = "Components"; }
        else if (ʅ.Count
== 0 || ʀ == "PhysicalGunObject")
        {
            if (toolsAmmoBottlesInOne) { Ș = true; }
            else
            {
                Ʌ[J].CustomName += " " + toolContainerKeyword; ʅ.Add(Ʌ[J]);
                ț = "Tools";
            }
        }
        else if (ʄ.Count == 0 || ʀ == "AmmoMagazine")
        {
            if (toolsAmmoBottlesInOne) { Ș = true; }
            else
            {
                Ʌ[J].CustomName += " " +
ammoContainerKeyword; ʄ.Add(Ʌ[J]); ț = "Ammo";
            }
        }
        else if (ʃ.Count == 0 || ʀ == "OxygenContainerObject" || ʀ == "GasContainerObject")
        {
            if (
toolsAmmoBottlesInOne) { Ș = true; }
            else { Ʌ[J].CustomName += " " + bottleContainerKeyword; ʃ.Add(Ʌ[J]); ț = "Bottles"; }
        }
        if (ș)
        {
            Ʌ[J].CustomName += " " +
oreContainerKeyword + " " + ingotContainerKeyword; ɵ.Add(Ʌ[J]); ɶ.Add(Ʌ[J]); ț = "Ores and Ingots";
        }
        if (Ș)
        {
            Ʌ[J].CustomName += " " + toolContainerKeyword +
" " + ammoContainerKeyword + " " + bottleContainerKeyword; ʅ.Add(Ʌ[J]); ʄ.Add(Ʌ[J]); ʃ.Add(Ʌ[J]); ț = "Tools, Ammo and Bottles";
        }
        if (ț !=
"") { Ɍ = "Assigned '" + Ț + "' as a new container for type '" + ț + "'."; }
        ʀ = "";
    }
}
void Ȝ()
{
    ȝ(ɵ, oreContainerKeyword); ȝ(ɶ,
ingotContainerKeyword); ȝ(ʇ, componentContainerKeyword); ȝ(ʅ, toolContainerKeyword); ȝ(ʄ, ammoContainerKeyword); ȝ(ʃ, bottleContainerKeyword);
}
void ȝ
(List<IMyTerminalBlock> ă, string Ȟ)
{
    if (ă.Count > 1)
    {
        bool ȟ = false; foreach (var V in ă)
        {
            if (V.CustomName.Contains("[P")) continue
; if (V.GetInventory(0).ItemCount == 0)
            {
                if (ȟ) continue; V.CustomName = V.CustomName.Replace(Ȟ, Ȟ + "!"); ȟ = true; if (V.CustomName.
Contains(Ȟ + "!!!"))
                {
                    string Ȧ = System.Text.RegularExpressions.Regex.Replace(V.CustomName, @"(" + Ȟ + @")(!+)", ""); Ȧ = System.Text.
          RegularExpressions.Regex.Replace(Ȧ, @"\(\d+\.?\d*\%\)", ""); Ȧ = Ȧ.Replace("  ", " "); V.CustomName = Ȧ.TrimEnd(' '); Ƚ.Remove(V); Ɍ = "Unassigned '" + Ȧ
                     + "' from being a container for type '" + Ȟ + "'.";
                }
            }
            else
            {
                if (V.CustomName.Contains(Ȟ + "!"))
                {
                    string Ȧ = System.Text.
RegularExpressions.Regex.Replace(V.CustomName, @"(" + Ȟ + @")(!+)", Ȟ); Ȧ = Ȧ.Replace("  ", " "); V.CustomName = Ȧ.TrimEnd(' ');
                }
            }
        }
    }
}
void ȥ()
{
    for (int J
= 0; J < ɂ.Count; J++)
    {
        string ȣ = ɂ[J].CustomName; string Ȣ = ȣ.ToLower(); List<string> ȡ = new List<string>(); if (Ȣ.Contains(
oreContainerKeyword.ToLower()) && !ȣ.Contains(oreContainerKeyword)) ȡ.Add(oreContainerKeyword); if (Ȣ.Contains(ingotContainerKeyword.ToLower())
    && !ȣ.Contains(ingotContainerKeyword)) ȡ.Add(ingotContainerKeyword); if (Ȣ.Contains(componentContainerKeyword.ToLower()) && !ȣ.
       Contains(componentContainerKeyword)) ȡ.Add(componentContainerKeyword); if (Ȣ.Contains(toolContainerKeyword.ToLower()) && !ȣ.Contains(
         toolContainerKeyword)) ȡ.Add(toolContainerKeyword); if (Ȣ.Contains(ammoContainerKeyword.ToLower()) && !ȣ.Contains(ammoContainerKeyword)) ȡ.Add(
               ammoContainerKeyword); if (Ȣ.Contains(bottleContainerKeyword.ToLower()) && !ȣ.Contains(bottleContainerKeyword)) ȡ.Add(bottleContainerKeyword); if (
                     Ȣ.Contains(lockedContainerKeyword.ToLower()) && !ȣ.Contains(lockedContainerKeyword)) ȡ.Add(lockedContainerKeyword); if (Ȣ.
                         Contains(hiddenContainerKeyword.ToLower()) && !ȣ.Contains(hiddenContainerKeyword)) ȡ.Add(hiddenContainerKeyword); if (Ȣ.Contains(
                             specialContainerKeyword.ToLower()) && !ȣ.Contains(specialContainerKeyword)) ȡ.Add(specialContainerKeyword); if (Ȣ.Contains(noSortingKeyword.ToLower(
                                 )) && !ȣ.Contains(noSortingKeyword)) ȡ.Add(noSortingKeyword); if (Ȣ.Contains(manualMachineKeyword.ToLower()) && !ȣ.Contains(
                                     manualMachineKeyword)) ȡ.Add(manualMachineKeyword); if (Ȣ.Contains(autocraftingKeyword.ToLower()) && !ȣ.Contains(autocraftingKeyword)) ȡ.Add(
                                           autocraftingKeyword); if (Ȣ.Contains(assembleKeyword.ToLower()) && !ȣ.Contains(assembleKeyword)) ȡ.Add(assembleKeyword); if (Ȣ.Contains(
                                                 disassembleKeyword.ToLower()) && !ȣ.Contains(disassembleKeyword)) ȡ.Add(disassembleKeyword); if (Ȣ.Contains(learnKeyword.ToLower()) && !ȣ.
                                                     Contains(learnKeyword)) ȡ.Add(learnKeyword); if (Ȣ.Contains(learnManyKeyword.ToLower()) && !ȣ.Contains(learnManyKeyword)) ȡ.Add(
                                                           learnManyKeyword); if (Ȣ.Contains(inventoryLCDKeyword.ToLower()) && !ȣ.Contains(inventoryLCDKeyword)) ȡ.Add(inventoryLCDKeyword); if (Ȣ.
                                                                 Contains(mainLCDKeyword.ToLower()) && !ȣ.Contains(mainLCDKeyword)) ȡ.Add(mainLCDKeyword); if (Ȣ.Contains(warningsLCDKeyword.ToLower()
                                                                     ) && !ȣ.Contains(warningsLCDKeyword)) ȡ.Add(warningsLCDKeyword); if (Ȣ.Contains(performanceLCDKeyword.ToLower()) && !ȣ.Contains(
                                                                         performanceLCDKeyword)) ȡ.Add(performanceLCDKeyword); if (Ȣ.Contains("[p") && !ȣ.Contains("[P")) ȡ.Add("[P"); if (Ȣ.Contains("[pmax]") && !ȣ.Contains(
                                                                                "[PMax]")) ȡ.Add("[PMax]"); if (Ȣ.Contains("[pmin]") && !ȣ.Contains("[PMin]")) ȡ.Add("[PMin]"); foreach (var É in ȡ)
        {
            ɂ[J].CustomName =
System.Text.RegularExpressions.Regex.Replace(ɂ[J].CustomName, É, É, System.Text.RegularExpressions.RegexOptions.IgnoreCase); Ɍ =
"Corrected name\nof: '" + ȣ + "'\nto: '" + ɂ[J].CustomName + "'";
        }
    }
    List<IMyTextPanel> Ŕ = new List<IMyTextPanel>(); GridTerminalSystem.GetBlocksOfType(Ŕ);
    for (int J = 0; J < Ŕ.Count; J++)
    {
        string ȣ = Ŕ[J].CustomName; string Ȣ = ȣ.ToLower(); List<string> ȡ = new List<string>(); if (Ȣ.Contains(
mainLCDKeyword.ToLower()) && !ȣ.Contains(mainLCDKeyword)) ȡ.Add(mainLCDKeyword); if (Ȣ.Contains(inventoryLCDKeyword.ToLower()) && !ȣ.Contains
(inventoryLCDKeyword)) ȡ.Add(inventoryLCDKeyword); if (Ȣ.Contains(warningsLCDKeyword.ToLower()) && !ȣ.Contains(
warningsLCDKeyword)) ȡ.Add(warningsLCDKeyword); if (Ȣ.Contains(performanceLCDKeyword.ToLower()) && !ȣ.Contains(performanceLCDKeyword)) ȡ.Add(
   performanceLCDKeyword); foreach (var É in ȡ)
        {
            Ŕ[J].CustomName = System.Text.RegularExpressions.Regex.Replace(Ŕ[J].CustomName, É, É, System.Text.
RegularExpressions.RegexOptions.IgnoreCase); Ɍ = "Corrected name\nof: '" + ȣ + "'\nto: '" + Ŕ[J].CustomName + "'";
        }
    }
}
void Ⱦ()
{
    if (ɹ == 0) ʈ(Ƕ, ɵ,
oreContainerKeyword); if (ɹ == 1) ʈ(Ƿ, ɶ, ingotContainerKeyword); if (ɹ == 2) ʈ(Ǹ, ʇ, componentContainerKeyword); if (ɹ == 3) ʈ(Ǽ, ʅ, toolContainerKeyword); if (ɹ
== 4) ʈ(ǹ, ʄ, ammoContainerKeyword); if (ɹ == 5) ʈ(Ǻ, ʃ, bottleContainerKeyword); if (ɹ == 6) ʈ(ǻ, ʃ, bottleContainerKeyword); if (ɹ == 7) ʈ(ǽ, ʅ,
toolContainerKeyword); if (ɹ == 8) ʈ(Ǿ, ʅ, toolContainerKeyword); if (ɹ == 9) ʈ(ǿ, ʅ, toolContainerKeyword); ɹ++; if (ɹ > 9) ɹ = 0;
}
void ʈ(string ˊ, List<
IMyTerminalBlock> ˠ, string ˡ)
{
    if (ˠ.Count == 0)
    {
        Ʀ("There are no containers for type '" + ˡ +
"'!\nBuild new ones or add the tag to existing ones!"); ʀ = ˊ; return;
    }
    IMyTerminalBlock Â = null; int ˢ = int.MaxValue; string ˣ =
"\nhas a different owner/faction!\nCan't move items from there!"; for (int J = 0; J < ˠ.Count; J++)
    {
        if (ˊ == Ǻ && ˠ[J].BlockDefinition.TypeIdString.Contains("OxygenTank") && ˠ[J].BlockDefinition.
SubtypeId.Contains("Hydrogen")) { continue; }
        else if (ˊ == ǻ && ˠ[J].BlockDefinition.TypeIdString.Contains("OxygenTank") && !ˠ[J].
BlockDefinition.SubtypeId.Contains("Hydrogen")) { continue; }
        var Ù = ˠ[J].GetInventory(0); if ((float)Ù.CurrentVolume < (float)Ù.MaxVolume * 0.98f
) { Â = ˠ[J]; ˢ = Ú(ˠ[J]); break; }
    }
    if (Â == null)
    {
        Ʀ("All containers for type '" + ˡ +
"' are full!\nYou should build new cargo containers!"); ʀ = ˊ; return;
    }
    IMyTerminalBlock ˤ = null; if (fillBottles && (ˊ == Ǻ || ˊ == ǻ)) { ˤ = ˬ(ˊ); }
    for (int J = 0; J < ɀ.Count; J++)
    {
        if (ɀ[J] == Â || (ɀ[J]
.CustomName.Contains(ˡ) && Ú(ɀ[J]) <= ˢ) || (ˊ == "Ore" && ɀ[J].GetType().ToString().Contains("MyGasGenerator"))) { continue; }
        if (ɀ[J]
.CustomName.Contains(ˡ) && balanceTypeContainers && !ɀ[J].BlockDefinition.TypeIdString.Contains("OxygenGenerator") && !ɀ[J].
BlockDefinition.TypeIdString.Contains("OxygenTank")) continue; if (ɀ[J].GetOwnerFactionTag() != Me.GetOwnerFactionTag())
        {
            Ʀ("'" + ɀ[J].
CustomName + "'" + ˣ); continue;
        }
        if (ˤ != null) { if (!ɀ[J].BlockDefinition.TypeIdString.Contains("Oxygen")) { Å(ˊ, ɀ[J], 0, ˤ, 0); continue; } }
        Å(ˊ, ɀ
[J], 0, Â, 0);
    }
    for (int J = 0; J < ɨ.Count; J++)
    {
        if (ɨ[J] == Â || (ɨ[J].CustomName.Contains(ˡ) && Ú(ɨ[J]) <= ˢ)) { continue; }
        if (ɨ[J].
GetOwnerFactionTag() != Me.GetOwnerFactionTag()) { Ʀ("'" + ɨ[J].CustomName + "'" + ˣ); continue; }
        Å(ˊ, ɨ[J], 1, Â, 0);
    }
    for (int J = 0; J < ɬ.Count; J++)
    {
        if (ɬ[J].
Mode == MyAssemblerMode.Disassembly || ɬ[J] == Â || (ɬ[J].CustomName.Contains(ˡ) && Ú(ɬ[J]) <= ˢ)) { continue; }
        if (ɬ[J].GetOwnerFactionTag(
) != Me.GetOwnerFactionTag()) { Ʀ("'" + ɬ[J].CustomName + "'" + ˣ); continue; }
        if (ˤ != null) { Å(ˊ, ɬ[J], 1, ˤ, 0); continue; }
        Å(ˊ, ɬ[J], 1, Â, 0);
    }
    if (!ƻ()) ɹ++;
}
IMyTerminalBlock ˬ(string ˊ)
{
    List<IMyGasTank> ˮ = new List<IMyGasTank>(); GridTerminalSystem.GetBlocksOfType<
IMyGasTank>(ˮ, Ͱ => Ͱ.IsSameConstructAs(Me) && !Ͱ.CustomName.Contains(specialContainerKeyword) && !Ͱ.CustomName.Contains(
lockedContainerKeyword) && Ͱ.IsWorking); if (ˊ == Ǻ) ˮ.RemoveAll(Ͱ => Ͱ.BlockDefinition.SubtypeId.Contains("Hydrogen")); if (ˊ == ǻ) ˮ.RemoveAll(Ͱ => !Ͱ.
BlockDefinition.SubtypeId.Contains("Hydrogen")); foreach (var Έ in ˮ) { if (Έ.FilledRatio > 0) { Έ.AutoRefillBottles = true; return Έ; } }
    List<
IMyGasGenerator> ͽ = ɯ.Where(ͼ => ͼ.IsSameConstructAs(Me) && ͼ.Enabled == true).ToList(); MyDefinitionId Ƞ = MyDefinitionId.Parse(ǵ + Ƕ + "/Ice");
    foreach (var ͻ in ͽ) { if (e(Ƞ, ͻ) > 0) { ͻ.AutoRefill = true; return ͻ; } }
    return null;
}
void ͺ()
{
    char ͷ = '0'; char Ά = '0'; char[] Ͷ ={'A','N','T',
'X'}; char[] ͳ = { 'a', 'd' }; if (sortingPattern.Length == 2) { ͷ = sortingPattern[0]; Ά = sortingPattern[1]; }
    Ɂ = new List<IMyTerminalBlock>(ɀ
); Ɂ.AddRange(ʂ); if (enableInternalSorting)
    {
        if (ͷ.ToString().IndexOfAny(Ͷ) < 0 || Ά.ToString().IndexOfAny(ͳ) < 0)
        {
            Ʀ(
"You provided the invalid sorting pattern '" + sortingPattern + "'!\nCan't sort the inventories!"); return;
        }
    }
    else
    {
        Ɂ = Ɂ.FindAll(J => J.CustomName.ToLower().Contains("(sort:"
));
    }
    for (var ƍ = ɸ; ƍ < Ɂ.Count; ƍ++)
    {
        if (ƻ()) return; if (ɸ >= Ɂ.Count - 1) { ɸ = 0; } else { ɸ++; }
        var Ù = Ɂ[ƍ].GetInventory(0); var B = new List<
MyInventoryItem>(); Ù.GetItems(B); if (B.Count > 200) continue; char Ͳ = ͷ; char ͱ = Ά; string ʹ = System.Text.RegularExpressions.Regex.Match(Ɂ[ƍ].
CustomName, @"(\(sort:)(.{2})", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups[2].Value; if (ʹ.Length == 2)
        {
            ͷ = ʹ[0]; Ά = ʹ[1
]; if (ͷ.ToString().IndexOfAny(Ͷ) < 0 || Ά.ToString().IndexOfAny(ͳ) < 0)
            {
                Ʀ("You provided an invalid sorting pattern in\n'" + Ɂ[ƍ].
CustomName + "'!\nUsing global pattern!"); ͷ = Ͳ; Ά = ͱ;
            }
        }
        var ˑ = new List<MyInventoryItem>(); Ù.GetItems(ˑ); if (ͷ == 'A')
        {
            if (Ά == 'd')
            {
                ˑ.Sort((Ȼ,
ŕ) => ŕ.Amount.ToIntSafe().CompareTo(Ȼ.Amount.ToIntSafe()));
            }
            else
            {
                ˑ.Sort((Ȼ, ŕ) => Ȼ.Amount.ToIntSafe().CompareTo(ŕ.Amount.
ToIntSafe()));
            }
        }
        else if (ͷ == 'N')
        {
            if (Ά == 'd') { ˑ.Sort((Ȼ, ŕ) => ŕ.Type.SubtypeId.ToString().CompareTo(Ȼ.Type.SubtypeId.ToString())); }
            else { ˑ.Sort((Ȼ, ŕ) => Ȼ.Type.SubtypeId.ToString().CompareTo(ŕ.Type.SubtypeId.ToString())); }
        }
        else if (ͷ == 'T')
        {
            if (Ά == 'd')
            {
                ˑ.Sort((
Ȼ, ŕ) => ŕ.Type.ToString().CompareTo(Ȼ.Type.ToString()));
            }
            else
            {
                ˑ.Sort((Ȼ, ŕ) => Ȼ.Type.ToString().CompareTo(ŕ.Type.ToString()))
;
            }
        }
        else if (ͷ == 'X')
        {
            if (Ά == 'd')
            {
                ˑ.Sort((Ȼ, ŕ) => (ŕ.Type.TypeId.ToString() + ŕ.Amount.ToIntSafe().ToString(@"000000000")).
CompareTo((Ȼ.Type.TypeId.ToString() + Ȼ.Amount.ToIntSafe().ToString(@"000000000"))));
            }
            else
            {
                ˑ.Sort((Ȼ, ŕ) => (Ȼ.Type.TypeId.ToString() +
Ȼ.Amount.ToIntSafe().ToString(@"000000000")).CompareTo((ŕ.Type.TypeId.ToString() + ŕ.Amount.ToIntSafe().ToString(
@"000000000"))));
            }
        }
        if (ˑ.SequenceEqual(B, new Ŝ())) continue; foreach (var É in ˑ)
        {
            string ː = É.ToString(); for (int J = 0; J < B.Count; J++)
            {
                if (B[
J].ToString() == ː) { Ù.TransferItemTo(Ù, J, B.Count, false); B.Clear(); Ù.GetItems(B); break; }
            }
        }
        ͷ = Ͳ; Ά = ͱ;
    }
}
void ˎ()
{
    for (int ƍ = ɷ; ƍ < ʂ
.Count; ƍ++)
    {
        if (ƻ()) return; ɷ++; è(ʂ[ƍ]); int R = 0; if (ʂ[ƍ].BlockDefinition.SubtypeId.Contains("Assembler"))
        {
            IMyAssembler Ɯ = ʂ[ƍ
] as IMyAssembler; if (Ɯ.Mode == MyAssemblerMode.Disassembly) R = 1;
        }
        var Ó = ʂ[ƍ].CustomData.Split('\n'); List<string> ˏ = new List<
string>(); foreach (var o in Ó)
        {
            if (!o.Contains("=")) continue; MyDefinitionId D; double ˍ = 0; var ˌ = o.Split('='); if (ˌ.Length >= 2)
            {
                if (!
MyDefinitionId.TryParse(ǵ + ˌ[0], out D)) continue; double.TryParse(ˌ[1], out ˍ); if (ˌ[1].ToLower().Contains("all")) { ˍ = int.MaxValue; }
            }
            else
            {
                continue;
            }
            double ˋ = e(D, ʂ[ƍ], R); double ʦ = 0; if (ˍ >= 0) { ʦ = ˍ - ˋ; } else { ʦ = Math.Abs(ˍ) - ˋ; }
            if (ʦ >= 1 && ˍ >= 0)
            {
                var Ù = ʂ[ƍ].GetInventory(R); if ((
float)Ù.CurrentVolume > (float)Ù.MaxVolume * 0.98f) continue; if (ʦ > e(D) && ˍ != int.MaxValue) { ˏ.Add(ʦ - e(D) + " " + D.SubtypeName); }
                IMyTerminalBlock U = null; if (allowSpecialSteal) { U = S(D, true, ʂ[ƍ]); } else { U = S(D); }
                if (U != null) { Å(D.ToString(), U, 0, ʂ[ƍ], R, ʦ, true); }
            }
            else if (ʦ < 0
) { IMyTerminalBlock Â = W(ʂ[ƍ], Ʌ); if (Â != null) Å(D.ToString(), ʂ[ƍ], R, Â, 0, Math.Abs(ʦ), true); }
        }
        if (ˏ.Count > 0)
        {
            Ʀ(ʂ[ƍ].CustomName +
"\nis missing the following items to match its quota:\n" + String.Join(", ", ˏ));
        }
    }
    ɷ = 0;
}
void Ή(List<IMyTerminalBlock> ă)
{
    foreach (var V in ă)
    {
        string Κ = V.CustomName; string Ȧ = ""; var Λ
= System.Text.RegularExpressions.Regex.Match(Κ, @"\(\d+\.?\d*\%\)").Value; if (Λ != "") { Ȧ = Κ.Replace(Λ, "").TrimEnd(' '); }
        else
        {
            Ȧ =
Κ;
        }
        var Ù = V.GetInventory(0); string ǐ = ((float)Ù.CurrentVolume).ŵ((float)Ù.MaxVolume); if (showFillLevel)
        {
            Ȧ += " (" + ǐ + ")"; Ȧ = Ȧ.
Replace("  ", " ");
        }
        if (Ȧ != Κ) V.CustomName = Ȧ;
    }
}
StringBuilder Μ()
{
    if (ɱ.Count > 1)
    {
        string Ν = @"(" + autocraftingKeyword + @" *)(\d*)"; ɱ.
Sort((Ȼ, ŕ) => System.Text.RegularExpressions.Regex.Match(Ȼ.CustomName, Ν).Groups[2].Value.CompareTo(System.Text.
RegularExpressions.Regex.Match(ŕ.CustomName, Ν).Groups[2].Value));
    }
    StringBuilder ĸ = new StringBuilder(); if (!ɱ[0].GetText().Contains(
"Isy's Autocrafting")) { ɱ[0].Font = defaultFont; ɱ[0].FontSize = defaultFontSize; }
    foreach (var µ in ɱ)
    {
        ĸ.Append(µ.GetText() + "\n"); µ.
WritePublicTitle("Craft item manually once to show up here"); µ.FontSize = ɱ[0].FontSize; µ.Font = ɱ[0].Font; µ.Alignment = VRage.Game.GUI.
TextPanel.TextAlignment.LEFT; µ.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
    }
    List<string> Ξ = ĸ.ToString().Split(
'\n').ToList(); List<string> Ó = ɱ[0].CustomData.Split('\n').ToList(); Ξ.RemoveAll(o => o.IndexOfAny(ɔ) <= 0); foreach (var G in ɲ)
    {
        bool Φ = false; foreach (var o in Ξ) { string Υ = o.Remove(o.IndexOf(" ")); if (Υ == G) { Φ = true; break; } }
        foreach (var o in Ó)
        {
            if (o == G)
            {
                Φ =
true; break;
            }
        }
        if (!Φ) { MyDefinitionId D = ǌ(G); double Τ = Math.Ceiling(e(D)); ĸ.Append("\n" + G + " " + Τ + " = " + Τ); }
    }
    List<string> Η = ĸ.
ToString().Split('\n').ToList(); StringBuilder ƃ = new StringBuilder(); Η.RemoveAll(o => o.IndexOfAny(ɔ) <= 0); try
    {
        IOrderedEnumerable<
string> Ρ; Ρ = Η.OrderBy(Ȼ => Ȼ.Substring(0, Ȼ.IndexOf(" "))); foreach (var o in Ρ)
        {
            bool Π = false; string G = o.Remove(o.IndexOf(" "));
            string Ο = o.Replace(G, ""); foreach (var É in ɲ) { if (É == G) { Π = true; break; } }
            if (Π) ƃ.Append(G + Ο + "\n");
        }
    }
    catch { }
    return ƃ;
}
void Σ(
StringBuilder ĸ)
{
    if (ĸ.Length == 0)
    {
        ĸ.Append("Autocrafting error!\n\nNo items for crafting available!\n\nIf you hid all items, check the custom data of the first autocrafting panel and reenable some of them.\n\nOtherwise, store or build new items manually!"
); ĸ = ɱ[0].ŏ(ĸ, 2, false); ɱ[0].WriteText(ĸ); return;
    }
    var Ĵ = ĸ.ToString().TrimEnd('\n').Split('\n'); int ĳ = Ĵ.Length; int Ĳ = 0; float
Ί = 0; foreach (var µ in ɱ)
    {
        float Ž = µ.Ň(); int ı = µ.Ń(); int Ķ = 0; List<string> ƃ = new List<string>(); if (µ == ɱ[0] ||
headerOnEveryScreen)
        {
            string Ό = ɖ; if (headerOnEveryScreen && ɱ.Count > 1)
            {
                Ό += " " + (ɱ.IndexOf(µ) + 1) + "/" + ɱ.Count; try { Ό += " [" + Ĵ[Ĳ][0] + "-#]"; }
                catch
                {
                    Ό +=
" [Empty]";
                }
            }
            ƃ.Add(Ό); ƃ.Add(µ.ŉ('=', µ.ň(Ό)).ToString() + "\n"); string Ύ = "Component "; string Ώ = "Current | Wanted "; Ί = µ.ň("Wanted ");
            string Ǉ = µ.ŉ(' ', Ž - µ.ň(Ύ) - µ.ň(Ώ)).ToString(); ƃ.Add(Ύ + Ǉ + Ώ + "\n"); Ķ = 5;
        } while ((Ĳ < ĳ && Ķ < ı) || (µ == ɱ[ɱ.Count - 1] && Ĳ < ĳ))
        {
            var o = Ĵ[Ĳ].Split
(' '); o[0] += " "; o[1] = o[1].Replace('$', ' '); string Ǉ = µ.ŉ(' ', Ž - µ.ň(o[0]) - µ.ň(o[1]) - Ί).ToString(); string ΐ = o[0] + Ǉ + o[1] + o[2]
; ƃ.Add(ΐ); Ĳ++; Ķ++;
        }
        if (headerOnEveryScreen && ɱ.Count > 1) { ƃ[0] = ƃ[0].Replace('#', Ĵ[Ĳ - 1][0]); }
        µ.WriteText(String.Join("\n", ƃ));
    }
}
void Α()
{
    ɱ.Clear(); GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(ɱ, µ => µ.IsSameConstructAs(Me) && µ.CustomName.Contains
(autocraftingKeyword)); if (ɱ.Count == 0) return; if (ɫ.Count == 0)
    {
        Ʀ(
"No assemblers found!\nBuild assemblers to enable autocrafting!"); return;
    }
    ʸ(); List<MyDefinitionId> Θ = new List<MyDefinitionId>(); var Η = Μ().ToString().TrimEnd('\n').Split('\n');
    StringBuilder ƃ = new StringBuilder(); foreach (var o in Η)
    {
        string G = ""; try { G = o.Substring(0, o.IndexOf(" ")); } catch { continue; }
        MyDefinitionId D = ǌ(G); if (D == null) continue; double Ι = Math.Ceiling(e(D)); string Ζ = o.Substring(o.IndexOfAny(ɔ) + 1); double Ε = 0; double.
                           TryParse(System.Text.RegularExpressions.Regex.Replace(Ζ, @"\D", ""), out Ε); if (Ζ.Contains("-"))
        {
            if (!ɱ[0].CustomData.Contains(ɕ)) ɱ[0
].CustomData = ɕ; ɱ[0].CustomData += "\n" + G; continue;
        }
        ƚ(D, Ε); double Δ = Math.Abs(Ε - Ι); bool Γ; MyDefinitionId Ñ = Ƭ(D, out Γ); double
Β = ƞ(Ñ); if (Ι >= Ε + Ε * assembleMargin && Β > 0 && !o.Contains("[D:"))
        {
            ʰ(Ñ); Β = 0; Ɍ = "Removed '" + D.SubtypeId.ToString() +
"' from the assembling queue.";
        }
        if (Ι <= Ε - Ε * disassembleMargin && Β > 0 && o.Contains("[D:"))
        {
            ʰ(Ñ); Β = 0; Ɍ = "Removed '" + D.SubtypeId.ToString() +
"' from the disassembling queue.";
        }
        string ƛ = ""; if (Β > 0 || Δ > 0)
        {
            if (enableAutodisassembling && Ι > Ε + Ε * disassembleMargin) { ƛ = "$[D:"; }
            else if (enableAutocrafting && Ι <
Ε - Ε * assembleMargin) { ƛ = "$[A:"; }
            if (ƛ != "") { if (Β == 0) { ƛ += "Wait]"; } else { ƛ += Math.Round(Β) + "]"; } }
        }
        if (!Γ) ƛ = "$[NoBP!]"; string Ü = "";
        if (Γ && Ζ.Contains("!")) { Θ.Add(Ñ); Ü = "!"; }
        string ˉ = "$=$ "; if (Ι > Ε) ˉ = "$>$ "; if (Ι < Ε) ˉ = "$<$ "; ƃ.Append(G + " " + Ι + ƛ + ˉ + Ε + Ü + "\n"); if (ƛ
 .Contains("[D:Wait]")) { ʭ(Ñ, Δ); }
        else if (ƛ.Contains("[A:Wait]"))
        {
            ˀ(Ñ, Δ); Ɍ = "Queued " + Δ + " '" + D.SubtypeId.ToString() +
"' in the assemblers.";
        }
        else if (ƛ.Contains("[NoBP!]") && Ε > Ι)
        {
            Ʀ("Can't craft\n'" + D.SubtypeId.ToString() +
"'\nThere's no blueprint stored for this item!\nTag an assembler with the '" + learnKeyword + "' keyword and queue\nit up about 100 times to learn the blueprint.");
        }
    }
    ʷ(); ˈ(Θ); Σ(ƃ);
}
void ʘ()
{
    if (ɧ.Count
> 0) return; MyDefinitionId ʒ = MyDefinitionId.Parse(ǵ + Ƕ + "/Stone"); MyDefinitionId Ñ = MyDefinitionId.Parse(Ȁ +
"StoneOreToIngotBasic"); double ʙ = e(ʒ); if (ʙ > 0)
    {
        double ʚ = Math.Floor(ʙ / 500 / ɮ.Count); if (ʚ < 1) return; foreach (var ʛ in ɮ)
        {
            if (ʛ.IsQueueEmpty) ʛ.
AddQueueItem(Ñ, ʚ);
        }
    }
}
void ʜ()
{
    if (ɾ == 0) ɾ += ʝ(ɵ, Ƕ, true); if (ɾ == 1) ɾ += ʝ(ɶ, Ƿ, true); if (ɾ == 2) ɾ += ʝ(ʇ, Ǹ, true); if (ɾ == 3) ɾ += ʝ(ʅ, Ǽ, true); if (ɾ == 4) ɾ
              += ʝ(ʄ, ǹ, true); if (ɾ == 5) ɾ += ʝ(ʃ, "ContainerObject", true); ɾ++; if (ɾ > 5) ɾ = 0;
}
int ʝ(List<IMyTerminalBlock> ȹ, string ʞ = "", bool ʟ =
false)
{
    if (ʟ) ȹ.RemoveAll(ŝ => ŝ.InventoryCount == 2 || ŝ.BlockDefinition.TypeIdString.Contains("OxygenGenerator") || ŝ.BlockDefinition
.TypeIdString.Contains("OxygenTank")); if (ȹ.Count < 2) { return 1; }
    Dictionary<MyItemType, double> ʠ = new Dictionary<MyItemType,
double>(); for (int J = 0; J < ȹ.Count; J++)
    {
        var B = new List<MyInventoryItem>(); ȹ[J].GetInventory(0).GetItems(B); foreach (var É in B)
        {
            if
(!É.Type.TypeId.ToString().Contains(ʞ)) continue; MyItemType D = É.Type; if (ʠ.ContainsKey(D)) { ʠ[D] += (double)É.Amount; }
            else
            {
                ʠ[D
] = (double)É.Amount;
            }
        }
    }
    Dictionary<MyItemType, double> ʡ = new Dictionary<MyItemType, double>(); foreach (var É in ʠ)
    {
        ʡ[É.Key] = (
int)(É.Value / ȹ.Count);
    }
    for (int ʫ = 0; ʫ < ȹ.Count; ʫ++)
    {
        if (ƻ()) return 0; var ʪ = new List<MyInventoryItem>(); ȹ[ʫ].GetInventory(0).
GetItems(ʪ); Dictionary<MyItemType, double> ʩ = new Dictionary<MyItemType, double>(); foreach (var É in ʪ)
        {
            MyItemType D = É.Type; if (ʩ.
ContainsKey(D)) { ʩ[D] += (double)É.Amount; }
            else { ʩ[D] = (double)É.Amount; }
        }
        double Ğ = 0; foreach (var É in ʠ)
        {
            ʩ.TryGetValue(É.Key, out Ğ);
            double ʨ = ʡ[É.Key]; if (Ğ <= ʨ + 1) continue; for (int ʧ = 0; ʧ < ȹ.Count; ʧ++)
            {
                if (ȹ[ʫ] == ȹ[ʧ]) continue; double ę = e(É.Key, ȹ[ʧ]); if (ę >= ʨ - 1)
                    continue; double ʦ = ʨ - ę; if (ʦ > Ğ - ʨ) ʦ = Ğ - ʨ; if (ʦ > 0) { Ğ -= Å(É.Key.ToString(), ȹ[ʫ], 0, ȹ[ʧ], 0, ʦ, true); if (Ğ.Ū(ʨ - 1, ʨ + 1)) break; }
            }
        }
    }
    return ƻ() ? 0 :
1;
}
void ʥ()
{
    if (ɧ.Count == 0) return; if (Ɋ == 0) ǲ = ʺ(Ǳ, ɩ); if (Ɋ == 1) ǳ = ʺ(Ǳ, ɪ); if (enableScriptRefineryFilling)
    {
        if (Ɋ == 2) ʎ(ɩ, ǲ); if (Ɋ == 3
) ʎ(ɪ, ǳ); if (Ɋ == 4) ʑ(ɩ, ǲ); if (Ɋ == 5) ʑ(ɪ, ǳ); if (Ɋ == 6 && ɩ.Count > 0 && ɪ.Count > 0) { bool ʤ = false; ʤ = ʔ(ɩ, ɪ, ǲ); if (!ʤ) ʔ(ɪ, ɩ, ǳ); }
    }
    else
    {
        if (Ɋ > 1
) Ɋ = 6;
    }
    Ɋ++; if (Ɋ > 6) Ɋ = 0;
}
void ʣ()
{
    if (ɋ == 0) ɋ += ʝ(ɩ.ToList<IMyTerminalBlock>()); if (ɋ == 1) ɋ += ʝ(ɪ.ToList<IMyTerminalBlock>()); ɋ++;
    if (ɋ > 1) ɋ = 0;
}
void ʢ(List<IMyRefinery> ʗ, List<MyItemType> ʉ)
{
    foreach (IMyRefinery Ö in ʗ)
    {
        var Ù = Ö.GetInventory(0); var B = new
List<MyInventoryItem>(); Ù.GetItems(B); if (B.Count < 2) continue; bool ʊ = false; int ʋ = 0; string ʌ = ""; foreach (var ʍ in ʉ)
        {
            for (int J = 0
; J < B.Count; J++)
            {
                if (B[J].Type == ʍ) { ʋ = J; ʌ = ʍ.SubtypeId; ʊ = true; break; }
                if (ʍ.SubtypeId == "Iron" || ʍ.SubtypeId == "Nickel" || ʍ.
SubtypeId == "Silicon") { if (B[J].Type.SubtypeId == "Stone") { ʋ = J; ʌ = "Stone"; ʊ = true; } }
            }
            if (ʊ) break;
        }
        if (ʋ != 0)
        {
            Ù.TransferItemTo(Ù, ʋ, 0, true);
            Ɍ = "Sorted the refining queue.\n'" + ʌ + "' is now at the front of the queue.";
        }
    }
}
void ʎ(List<IMyRefinery> ʏ, List<MyItemType> ʉ)
{
    if (ʏ.Count == 0) { Ɋ++; return; }
    MyItemType ʐ = new MyItemType(); foreach (var ʍ in ʉ) { if (e(ʍ) > 100) { ʐ = ʍ; break; } }
    if (!ʐ.ToString().
Contains(Ƕ)) return; for (int J = 0; J < ʏ.Count; J++)
    {
        if (ƻ()) return; var Ù = ʏ[J].GetInventory(0); if ((float)Ù.CurrentVolume > (float)Ù.
MaxVolume * 0.75f)
        {
            var B = new List<MyInventoryItem>(); Ù.GetItems(B); foreach (var É in B)
            {
                if (É.Type == ʐ) return; if (ʐ.SubtypeId == "Iron" ||
ʐ.SubtypeId == "Nickel" || ʐ.SubtypeId == "Silicon") { if (É.Type.SubtypeId == "Stone") return; }
            }
            IMyTerminalBlock Â = W(ʏ[J], ɵ); if (Â !=
null) { Å("", ʏ[J], 0, Â, 0); }
        }
    }
    if (!ƻ()) Ɋ++;
}
void ʑ(List<IMyRefinery> ʏ, List<MyItemType> ʉ)
{
    if (ʏ.Count == 0) { Ɋ++; return; }
    var ă = new
List<IMyTerminalBlock>(); ă.AddRange(ɀ); ă.AddRange(ʂ); MyItemType ʒ = MyItemType.MakeOre("Stone"); foreach (var ʍ in ʉ)
    {
        MyItemType
D = ʍ; if (e(ʍ) == 0)
        {
            if (ʍ.SubtypeId == "Iron" || ʍ.SubtypeId == "Nickel" || ʍ.SubtypeId == "Silicon") { if (e(ʒ) > 0) { D = ʒ; } else { continue; } }
            else { continue; }
        }
        IMyTerminalBlock ʓ = S(D, true); if (ʓ == null) continue; for (int J = 0; J < ʏ.Count; J++)
        {
            if (ƻ()) return; var Ù = ʏ[J].
GetInventory(0); if ((float)Ù.CurrentVolume > (float)Ù.MaxVolume * 0.98f) continue; Å(D.ToString(), ʓ, 0, ʏ[J], 0);
        }
    }
    if (!ƻ()) Ɋ++;
}
bool ʔ(List<
IMyRefinery> ʕ, List<IMyRefinery> ʖ, List<MyItemType> ʉ)
{
    for (int J = 0; J < ʕ.Count; J++)
    {
        if ((float)ʕ[J].GetInventory(0).CurrentVolume > 0.05f)
            continue; for (int ƕ = 0; ƕ < ʖ.Count; ƕ++)
        {
            if ((float)ʖ[ƕ].GetInventory(0).CurrentVolume > 0)
            {
                foreach (var ʍ in ʉ)
                {
                    Å(ʍ.ToString(), ʖ[ƕ], 0, ʕ[
J], 0, -0.5);
                }
                return true;
            }
        }
    }
    return false;
}
List<MyItemType> ʺ(List<string> ʻ, List<IMyRefinery> ʏ)
{
    if (ʏ.Count == 0)
    {
        Ɋ++; return
null;
    }
    List<string> ʼ = new List<string>(ʻ); ʼ.Sort((Ȼ, ŕ) => (e(MyDefinitionId.Parse(ǵ + Ƿ + "/" + Ȼ)) / Ƙ(Ȼ)).CompareTo((e(MyDefinitionId.
  Parse(ǵ + Ƿ + "/" + ŕ)) / Ƙ(ŕ)))); ʼ.InsertRange(0, fixedRefiningList); List<MyItemType> ʽ = new List<MyItemType>(); MyItemType D; foreach (
                  var É in ʼ) { D = MyItemType.MakeOre(É); foreach (var Ö in ʏ) { if (Ö.GetInventory(0).CanItemsBeAdded(1, D)) { ʽ.Add(D); break; } } }
    if (!ƻ(
)) Ɋ++; return ʽ;
}
void ʾ()
{
    foreach (var Ɯ in ɫ)
    {
        if (Ɯ.GetOwnerFactionTag() == Me.GetOwnerFactionTag())
        {
            var Ù = Ɯ.GetInventory(0);
            if ((float)Ù.CurrentVolume == 0) continue; if (Ɯ.IsQueueEmpty || Ɯ.Mode == MyAssemblerMode.Disassembly || (float)Ù.CurrentVolume > (
                 float)Ù.MaxVolume * 0.98f) { IMyTerminalBlock Â = W(Ɯ, ɶ); if (Â != null) Å("", Ɯ, 0, Â, 0); }
        }
    }
}
void ʿ()
{
    foreach (IMyAssembler Ɯ in ɫ)
    {
        if (Ɯ.
Mode == MyAssemblerMode.Disassembly) continue; if (Ɯ.CustomData.Contains("skipQueueSorting")) { Ɯ.CustomData = ""; continue; }
        var ƛ = new
List<MyProductionItem>(); Ɯ.GetQueue(ƛ); if (ƛ.Count < 2) continue; double ˇ = Double.MaxValue; int ʋ = 0; string ʌ = ""; for (int J = 0; J < ƛ.
Count; J++) { MyDefinitionId D = ǈ(ƛ[J].BlueprintId); double ˆ = e(D); if (ˆ < ˇ) { ˇ = ˆ; ʋ = J; ʌ = D.SubtypeId.ToString(); } }
        if (ʋ != 0)
        {
            Ɯ.
MoveQueueItemRequest(ƛ[ʋ].ItemId, 0); Ɍ = "Sorted the assembling queue.\n'" + ʌ + "' is now at the front of the queue.";
        }
    }
}
void ˈ(List<
MyDefinitionId> ˁ)
{
    if (ˁ.Count == 0) return; if (ˁ.Count > 1) ˁ.Sort((Ȼ, ŕ) => e(ǈ(Ȼ)).CompareTo(e(ǈ(ŕ)))); foreach (var Ɯ in ɫ)
    {
        var ƛ = new List<
MyProductionItem>(); Ɯ.GetQueue(ƛ); if (ƛ.Count < 2) continue; foreach (var Ñ in ˁ)
        {
            int ƍ = ƛ.FindIndex(J => J.BlueprintId == Ñ); if (ƍ == -1) continue; if (
ƍ == 0) { Ɯ.CustomData = "skipQueueSorting"; break; }
            Ɯ.MoveQueueItemRequest(ƛ[ƍ].ItemId, 0); Ɯ.CustomData = "skipQueueSorting"; Ɍ =
"Sorted the assembler queue by priority.\n'" + ǈ(Ñ).SubtypeId.ToString() + "' is now at the front of the queue."; break;
        }
    }
}
void ˀ(MyDefinitionId Ñ, double À)
{
    List<
IMyAssembler> ʬ = new List<IMyAssembler>(); foreach (IMyAssembler Ɯ in ɫ)
    {
        if (Ɯ.CustomName.Contains(disassembleKeyword)) continue; if (Ɯ.Mode
== MyAssemblerMode.Disassembly && !Ɯ.IsQueueEmpty) continue; if (Ɯ.Mode == MyAssemblerMode.Disassembly)
        {
            Ɯ.Mode = MyAssemblerMode.
Assembly;
        }
        if (Ɯ.CanUseBlueprint(Ñ)) { ʬ.Add(Ɯ); }
    }
    ʮ(ʬ, Ñ, À);
}
void ʭ(MyDefinitionId Ñ, double À)
{
    List<IMyAssembler> ʬ = new List<
IMyAssembler>(); foreach (IMyAssembler Ɯ in ɫ)
    {
        if (Ɯ.CustomName.Contains(assembleKeyword)) continue; if (Ɯ.Mode == MyAssemblerMode.Assembly
&& Ɯ.IsProducing) continue; if (Ɯ.Mode == MyAssemblerMode.Assembly) { Ɯ.ClearQueue(); Ɯ.Mode = MyAssemblerMode.Disassembly; }
        if (Ɯ.Mode
== MyAssemblerMode.Assembly) continue; if (Ɯ.CanUseBlueprint(Ñ)) { ʬ.Add(Ɯ); }
    }
    ʮ(ʬ, Ñ, À);
}
void ʮ(List<IMyAssembler> ʬ,
MyDefinitionId Ñ, double À)
{
    if (ʬ.Count == 0) return; double ʯ = Math.Ceiling(À / ʬ.Count); foreach (IMyAssembler Ɯ in ʬ)
    {
        if (ʯ > À) ʯ = Math.Ceiling(À)
; if (À > 0) { Ɯ.InsertQueueItem(0, Ñ, ʯ); À -= ʯ; } else { break; }
    }
}
void ʰ(MyDefinitionId Ñ)
{
    foreach (IMyAssembler Ɯ in ɫ)
    {
        var ƛ = new
List<MyProductionItem>(); Ɯ.GetQueue(ƛ); for (int J = 0; J < ƛ.Count; J++) { if (ƛ[J].BlueprintId == Ñ) Ɯ.RemoveQueueItem(J, ƛ[J].Amount); }
    }
}
void ʸ() { foreach (IMyAssembler Ɯ in ɫ) { Ɯ.UseConveyorSystem = true; Ɯ.CooperativeMode = false; Ɯ.Repeating = false; } }
void ʷ()
{
    List
<IMyAssembler> ʶ = new List<IMyAssembler>(ɫ); ʶ.RemoveAll(Ȼ => Ȼ.IsQueueEmpty); if (ʶ.Count == 0) return; List<IMyAssembler> ʹ = new
List<IMyAssembler>(ɫ); ʹ.RemoveAll(Ȼ => !Ȼ.IsQueueEmpty); foreach (var ʵ in ʶ)
    {
        if (ʹ.Count == 0) return; var ƛ = new List<
MyProductionItem>(); ʵ.GetQueue(ƛ); double ʴ = (double)ƛ[0].Amount; if (ʴ <= 10) continue; double ʳ = Math.Ceiling(ʴ / 2); foreach (var ʲ in ʹ)
        {
            if (!ʲ.
CanUseBlueprint(ƛ[0].BlueprintId)) continue; if (ʵ.Mode == MyAssemblerMode.Assembly && ʲ.CustomName.Contains(disassembleKeyword)) continue; if (ʵ
.Mode == MyAssemblerMode.Disassembly && ʲ.CustomName.Contains(assembleKeyword)) continue; ʲ.Mode = ʵ.Mode; if (ʲ.Mode != ʵ.Mode)
                continue; ʲ.AddQueueItem(ƛ[0].BlueprintId, ʳ); ʵ.RemoveQueueItem(0, ʳ); ʹ.Remove(ʲ); break;
        }
    }
}
void ʱ()
{
    if (ɯ.Count == 0) return; double ų =
iceFillLevelPercentage / 100; MyDefinitionId Ƞ = MyDefinitionId.Parse(ǵ + Ƕ + "/Ice"); string ǘ = Ƞ.ToString(); double Ð = 0.00037; foreach (IMyGasGenerator ć
in ɯ)
    {
        var Ù = ć.GetInventory(0); double Ĉ = e(Ƞ, ć); double ĉ = Ĉ * Ð; double Ċ = (double)Ù.MaxVolume; if (ĉ > Ċ * (ų + 0.001))
        {
            IMyTerminalBlock
Â = W(ć, ɵ); if (Â != null) { double î = (ĉ - Ċ * ų) / Ð; Å(ǘ, ć, 0, Â, 0, î); }
        }
        else if (ĉ < Ċ * (ų - 0.001))
        {
            IMyTerminalBlock U = S(Ƞ, true); if (U != null)
            {
                double î = (Ċ * ų - ĉ) / Ð; Å(ǘ, U, 0, ć, 0, î);
            }
        }
    }
    double ċ = 0; double Č = 0; foreach (var ć in ɯ)
    {
        ċ += e(Ƞ, ć); var Ù = ć.GetInventory(0); Č += (double)Ù.
MaxVolume;
    }
    double č = (ċ * Ð) / Č; foreach (var Ď in ɯ)
    {
        var A = Ď.GetInventory(0); double Ğ = e(Ƞ, Ď); double Ĝ = Ğ * Ð; double ě = (double)A.MaxVolume
; if (Ĝ > ě * (č + 0.001))
        {
            foreach (var Ě in ɯ)
            {
                if (Ď == Ě) continue; var P = Ě.GetInventory(0); double ę = e(Ƞ, Ě); double Ę = ę * Ð; double ĝ = (
double)P.MaxVolume; if (Ę < ĝ * (č - 0.001))
                {
                    double ė = ((ĝ * č) - Ę) / Ð; if ((Ğ - ė) * Ð >= ě * č && ė > 5) { Ğ -= Å(ǘ, Ď, 0, Ě, 0, ė); continue; }
                    if ((Ğ - ė) * Ð < ě * č && ė >
5) { double ĕ = (Ğ * Ð - ě * č) / Ð; Å(ǘ, Ď, 0, Ě, 0, ĕ); break; }
                }
            }
        }
    }
}
void Ĕ()
{
    if (ɰ.Count == 0) return; MyDefinitionId ē = MyDefinitionId.Parse(ǵ +
Ƿ + "/Uranium"); string Ē = ē.ToString(); double đ = 0; double Đ = 0; foreach (IMyReactor ď in ɰ)
    {
        ď.UseConveyorSystem = false; double Ė = e
(ē, ď); double ą = uraniumAmountLargeGrid; if (ď.CubeGrid.GridSize == 0.5f) ą = uraniumAmountSmallGrid; Đ += ą; if (Ė > ą + 0.05)
        {
            IMyTerminalBlock Â = W(ď, ɶ); if (Â != null) { double î = Ė - ą; Å(Ē, ď, 0, Â, 0, î); }
        }
        else if (Ė < ą - 0.05)
        {
            IMyTerminalBlock U = S(ē, true); if (U != null)
            {
                double î =
ą - Ė; Å(Ē, U, 0, ď, 0, î);
            }
        }
        đ += e(ē, ď);
    }
    double ï = đ / Đ; foreach (var ð in ɰ)
    {
        double ñ = e(ē, ð); double ò = ï * uraniumAmountLargeGrid; if (ð.
CubeGrid.GridSize == 0.5f) ò = ï * uraniumAmountSmallGrid; if (ñ > ò + 0.05)
        {
            foreach (var ó in ɰ)
            {
                if (ð == ó) continue; double ô = e(ē, ó); double õ = ï *
uraniumAmountLargeGrid; if (ó.CubeGrid.GridSize == 0.5f) õ = ï * uraniumAmountSmallGrid; if (ô < õ - 0.05)
                {
                    ñ = e(ē, ð); double ö = õ - ô; if (ñ - ö >= ò)
                    {
                        Å(Ē, ð, 0, ó, 0, ö);
                        continue;
                    }
                    if (ñ - ö < ò) { ö = ñ - ò; Å(Ē, ð, 0, ó, 0, ö); break; }
                }
            }
        }
    }
}
StringBuilder ø(IMyTextSurface µ, bool ù = true, bool ú = true, bool û = true, bool Ą
= true, bool Ă = true)
{
    bool ā = false; StringBuilder h = new StringBuilder(); if (ù)
    {
        h.Append("Isy's Inventory Manager\n"); h.Append(
µ.ŉ('=', µ.ň(h))).Append("\n\n");
    }
    if (ú && Ľ != null) { h.Append("Warning!\n" + Ľ + "\n\n"); ā = true; }
    if (û)
    {
        h.Append(ÿ(µ, ɵ, "Ores")); h.
Append(ÿ(µ, ɶ, "Ingots")); h.Append(ÿ(µ, ʇ, "Components")); h.Append(ÿ(µ, ʅ, "Tools")); h.Append(ÿ(µ, ʄ, "Ammo")); h.Append(ÿ(µ, ʃ,
"Bottles")); h.Append("=> " + Ƚ.Count + " type containers: Balancing " + (balanceTypeContainers ? "ON" : "OFF") + "\n\n"); ā = true;
    }
    if (Ą)
    {
        h.
Append("Managed blocks:\n"); float Ā = µ.ň(ɂ.Count.ToString()); h.Append(ɂ.Count + " Inventories (total) / " + ɀ.Count +
" have items to sort\n"); if (ʂ.Count > 0) { h.Append(µ.ŉ(' ', Ā - µ.ň(ʂ.Count.ToString())).ToString() + ʂ.Count + " Special Containers\n"); }
        if (ɧ.Count > 0)
        {
            h
.Append(µ.ŉ(' ', Ā - µ.ň(ɧ.Count.ToString())).ToString() + ɧ.Count + " Refineries: "); h.Append("Ore Balancing " + (
enableOreBalancing ? "ON" : "OFF") + "\n");
        }
        if (ɯ.Count > 0)
        {
            h.Append(µ.ŉ(' ', Ā - µ.ň(ɯ.Count.ToString())).ToString() + ɯ.Count + " O2/H2 Generators: ");
            h.Append("Ice Balancing " + (enableIceBalancing ? "ON" : "OFF") + "\n");
        }
        if (ɰ.Count > 0)
        {
            h.Append(µ.ŉ(' ', Ā - µ.ň(ɰ.Count.ToString())
).ToString() + ɰ.Count + " Reactors: "); h.Append("Uranium Balancing " + (enableUraniumBalancing ? "ON" : "OFF") + "\n");
        }
        if (ɫ.Count > 0
)
        {
            h.Append(µ.ŉ(' ', Ā - µ.ň(ɫ.Count.ToString())).ToString() + ɫ.Count + " Assemblers: "); h.Append("Craft " + (enableAutocrafting ?
                  "ON" : "OFF") + " | "); h.Append("Uncraft " + (enableAutodisassembling ? "ON" : "OFF") + " | "); h.Append("Cleanup " + (
                                enableAssemblerCleanup ? "ON" : "OFF") + "\n");
        }
        if (ɮ.Count > 0)
        {
            h.Append(µ.ŉ(' ', Ā - µ.ň(ɮ.Count.ToString())).ToString() + ɮ.Count + " Survival Kits: "); h.
Append("Ingot Crafting " + (enableBasicIngotCrafting ? "ON" : "OFF") + (ɧ.Count > 0 ? " (Auto OFF - refineries exist)" : "") + "\n");
        }
        h.Append
("\n"); ā = true;
    }
    if (Ă && Ɍ != "") { h.Append("Last Action:\n" + Ɍ); ā = true; }
    if (!ā) { h.Append("-- No informations to show --"); }
    return
h;
}
StringBuilder ÿ(IMyTextSurface µ, List<IMyTerminalBlock> ă, string F)
{
    double þ = 0, ý = 0; foreach (var V in ă)
    {
        var Ù = V.
GetInventory(0); þ += (double)Ù.CurrentVolume; ý += (double)Ù.MaxVolume;
    }
    string È = ă.Count + "x " + F + ":"; string ü = þ.Ÿ(); string í = ý.Ÿ();
    StringBuilder ğ = Ǎ(µ, È, þ, ý, ü, í); return ğ;
}
void Ġ(string ĸ = null)
{
    if (ɏ.Count == 0) { ə++; return; }
    for (int J = ɥ; J < ɏ.Count; J++)
    {
        if (ƻ()) return; ɥ
++; var Ĥ = ɏ[J].Ƅ(mainLCDKeyword); foreach (var ĥ in Ĥ)
        {
            var ħ = ĥ.Key; var ì = ĥ.Value; if (!ħ.GetText().EndsWith("\a"))
            {
                ħ.Font =
defaultFont; ħ.FontSize = defaultFontSize; ħ.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT; ħ.ContentType = VRage.Game.GUI.
TextPanel.ContentType.TEXT_AND_IMAGE;
            }
            bool ù = ì.ƅ("showHeading"); bool ú = ì.ƅ("showWarnings"); bool û = ì.ƅ("showContainerStats"); bool
Ą = ì.ƅ("showManagedBlocks"); bool Ă = ì.ƅ("showLastAction"); bool Ĺ = ì.ƅ("scrollTextIfNeeded"); StringBuilder h = new
StringBuilder(); if (ĸ != null) { h.Append(ĸ); } else { h = ø(ħ, ù, ú, û, Ą, Ă); }
            h = ħ.ŏ(h, ù ? 3 : 0, Ĺ); ħ.WriteText(h.Append("\a"));
        }
    }
    ə++; ɥ = 0;
}
void ĺ()
{
    if (ɐ
.Count == 0) { ə++; return; }
    StringBuilder Ļ = new StringBuilder(); if (ɟ.Count == 0) { Ļ.Append("- No problems detected -"); }
    else
    {
        int
ļ = 1; foreach (var Ľ in ɟ) { Ļ.Append(ļ + ". " + Ľ.Replace("\n", " ") + "\n"); ļ++; }
    }
    for (int J = ɛ; J < ɐ.Count; J++)
    {
        if (ƻ()) return; ɛ++; var
Ĥ = ɐ[J].Ƅ(warningsLCDKeyword); foreach (var ĥ in Ĥ)
        {
            var ħ = ĥ.Key; var ì = ĥ.Value; if (!ħ.GetText().EndsWith("\a"))
            {
                ħ.Font =
defaultFont; ħ.FontSize = defaultFontSize; ħ.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT; ħ.ContentType = VRage.Game.GUI.
TextPanel.ContentType.TEXT_AND_IMAGE;
            }
            bool ù = ì.ƅ("showHeading"); bool Ĺ = ì.ƅ("scrollTextIfNeeded"); StringBuilder h = new
StringBuilder(); if (ù) { h.Append("Isy's Inventory Manager Warnings\n"); h.Append(ħ.ŉ('=', ħ.ň(h))).Append("\n\n"); }
            h.Append(Ļ); h = ħ.ŏ(h, ù ?
3 : 0, Ĺ); ħ.WriteText(h.Append("\a"));
        }
    }
    ə++; ɛ = 0;
}
void ľ()
{
    if (ɑ.Count == 0) { ə++; return; }
    for (int J = Ɇ; J < ɑ.Count; J++)
    {
        if (ƻ())
            return; Ɇ++; var Ĥ = ɑ[J].Ƅ(performanceLCDKeyword); foreach (var ĥ in Ĥ)
        {
            var ħ = ĥ.Key; var ì = ĥ.Value; if (!ħ.GetText().EndsWith("\a"))
            {
                ħ
.Font = defaultFont; ħ.FontSize = defaultFontSize; ħ.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT; ħ.ContentType = VRage.
Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
            }
            bool ù = ì.ƅ("showHeading"); bool Ĺ = ì.ƅ("scrollTextIfNeeded"); StringBuilder h =
new StringBuilder(); if (ù) { h.Append("Isy's Inventory Manager Performance\n"); h.Append(ħ.ŉ('=', ħ.ň(h))).Append("\n\n"); }
            h.
Append(Ƹ); h = ħ.ŏ(h, ù ? 3 : 0, Ĺ); ħ.WriteText(h.Append("\a"));
        }
    }
    ə++; Ɇ = 0;
}
void ķ()
{
    if (ɒ.Count == 0) { ə++; return; }
    Dictionary<
IMyTextSurface, string> į = new Dictionary<IMyTextSurface, string>(); Dictionary<IMyTextSurface, string> ġ = new Dictionary<IMyTextSurface,
string>(); List<IMyTextSurface> Ģ = new List<IMyTextSurface>(); List<IMyTextSurface> ģ = new List<IMyTextSurface>(); foreach (var Q in ɒ
)
    {
        var Ĥ = Q.Ƅ(inventoryLCDKeyword); foreach (var ĥ in Ĥ)
        {
            if (ĥ.Value.Contains(inventoryLCDKeyword + ":"))
            {
                į[ĥ.Key] = ĥ.Value; Ģ.Add
(ĥ.Key);
            }
            else { ġ[ĥ.Key] = ĥ.Value; ģ.Add(ĥ.Key); }
        }
    }
    HashSet<string> Ħ = new HashSet<string>(); foreach (var ħ in į)
    {
        Ħ.Add(System.
Text.RegularExpressions.Regex.Match(ħ.Value, inventoryLCDKeyword + @":[A-Za-z]+").Value);
    }
    Ħ.RemoveWhere(Ĩ => Ĩ == ""); List<string> ĩ
= Ħ.ToList(); for (int J = ɇ; J < ĩ.Count; J++)
    {
        if (ƻ()) return; ɇ++; var Ī = į.Where(ī => ī.Value.Contains(ĩ[J])); var Ĭ = from pair in Ī
                                                                                    orderby System.Text.RegularExpressions.Regex.Match(pair.Value, inventoryLCDKeyword + @":\w+").Value ascending
                                                                                    select pair;
        IMyTextSurface ĭ = Ĭ.ElementAt(0).Key; string ì = Ĭ.ElementAt(0).Value; StringBuilder h = Į(ĭ, ì); if (!ì.ToLower().Contains("noscroll"))
        {
            int ĵ = 0
; foreach (var İ in Ĭ) { ĵ += İ.Key.Ń(); }
            h = ĭ.ŏ(h, 0, true, ĵ);
        }
        var Ĵ = h.ToString().Split('\n'); int ĳ = Ĵ.Length; int Ĳ = 0; int ı, Ķ;
        foreach (var İ in Ĭ)
        {
            IMyTextSurface ħ = İ.Key; ħ.FontSize = ĭ.TextureSize.Y / ħ.TextureSize.Y * ĭ.FontSize; ħ.Font = ĭ.Font; ħ.TextPadding = ĭ.
     TextPadding; ħ.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT; ħ.ContentType = VRage.Game.GUI.TextPanel.ContentType.
         TEXT_AND_IMAGE; ı = ħ.Ń(); Ķ = 0; h.Clear(); while (Ĳ < ĳ && Ķ < ı) { h.Append(Ĵ[Ĳ] + "\n"); Ĳ++; Ķ++; }
            ħ.WriteText(h);
        }
    }
    for (int J = Ɉ; J < ģ.Count; J++)
    {
        if (ƻ())
            return; Ɉ++; IMyTextSurface ħ = ģ[J]; string ì = ġ[ħ]; StringBuilder h = Į(ħ, ì); if (!ì.ToLower().Contains("noscroll")) { h = ħ.ŏ(h, 0); }
        ħ.
WriteText(h); ħ.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT; ħ.ContentType = VRage.Game.GUI.TextPanel.ContentType.
TEXT_AND_IMAGE;
    }
    ə++; ɇ = 0; Ɉ = 0;
}
StringBuilder Į(IMyTextSurface µ, string ì)
{
    StringBuilder h = new StringBuilder(); var Ì = ì.Split('\n').ToList
(); Ì.RemoveAll(k => k.StartsWith("@") || k.Length <= 1); bool m = true; try { if (Ì[0].Length <= 1) m = false; } catch { m = false; }
    if (!m)
    {
        h.
Append("Put an item or type name in the custom data.\n\n" + "Examples:\nComponent\nIngot\nSteelPlate\nInteriorPlate\n\n" +
"Optionally, add a max amount for the bars as a 2nd parameter.\n\n" + "Example:\nIngot 100000\n\n" + "At last, add any of these modifiers.\n" + "There are 5 modifiers at this point:\n\n" +
"'noHeading' to hide the heading\n" + "'singleLine' to force one line per item\n" + "'noBar' to hide the bars\n" +
"'noScroll' to prevent the text from scrolling\n" + "'hideEmpty' to hide items that have an amount of 0\n\n" +
"Examples:\nComponent 100000 noBar\nSteelPlate noHeading noBar hideEmpty\n\n" + "To display multiple different items, use a new line for every item!\n\n" +
"Hint: One 'noScroll' modifier per screen is enough!\n\n"); µ.Font = defaultFont; µ.FontSize = defaultFontSize;
    }
    else
    {
        foreach (var o in Ì)
        {
            var q = o.Split(' '); double r = -1; bool u = false;
            bool v = false; bool w = false; bool z = false; if (q.Length >= 2) { try { r = Convert.ToDouble(q[1]); } catch { r = -1; } }
            if (o.ToLower().Contains(
"noheading")) u = true; if (o.ToLower().Contains("nobar")) v = true; if (o.ToLower().Contains("hideempty")) w = true; if (o.ToLower().Contains(
"singleline")) z = true; h.Append(ª(µ, q[0], r, u, v, w, z)); if (Ì.Count > 1 && o != Ì[Ì.Count - 1]) h.Append("\n");
        }
    }
    return h;
}
StringBuilder ª(
IMyTextSurface µ, string º, double r, bool u = false, bool v = false, bool w = false, bool z = false)
{
    StringBuilder h = new StringBuilder(); bool Ê = r ==
-1 ? true : false; foreach (var É in ȑ)
    {
        if (É.ToString().ToLower().Contains(º.ToLower()))
        {
            if (h.Length == 0 && !u)
            {
                string È =
"Items containing '" + char.ToUpper(º[0]) + º.Substring(1).ToLower() + "'\n\n"; h.Append(µ.ŉ(' ', (µ.Ň() - µ.ň(È)) / 2)).Append(È);
            }
            double À = e(É); if (À ==
0 && w) continue; if (Ê) r = ƙ(É); h.Append(Ǎ(µ, É.SubtypeId.ToString(), À, r, À.Ŷ(), r.Ŷ(), v, z));
        }
    }
    if (h.Length == 0)
    {
        h.Append(
"Error!\n\n"); h.Append("No items containing '" + º +
"' found!\nCheck the custom data of this LCD and enter a valid type or item name!\n");
    }
    return h;
}
void Ç()
{
    if (ǃ == 99) { ǃ = 0; } else { ǃ++; }
    Echo("Isy's Inventory Manager " + ɍ[Ɏ] + "\n====================\n"); if (Ľ !=
null) { Echo("Warning!\n" + Ľ + "\n"); }
    StringBuilder h = new StringBuilder(); h.Append("Script is running in " + (ǣ ? "station" : "ship") +
" mode\n\n"); h.Append("Task: " + ǡ[ɣ] + "\n"); h.Append("Script step: " + ɣ + " / " + (ǡ.Length - 1) + "\n\n"); Ƹ = h.Append(Ƹ); if (ɳ.Count > 0)
    {
        Ƹ.
Append("Excluded grids:\n============\n\n"); foreach (var Æ in ɳ) { Ƹ.Append(Æ.CustomName + "\n"); }
    }
    Echo(Ƹ.ToString()); if (ɏ.Count == 0
)
    {
        Echo("Hint:\nBuild a LCD and add the main LCD\nkeyword '" + mainLCDKeyword +
      "' to its name to get\nmore informations about your base\nand the current script actions.\n");
    }
}
double Å(string Ä, IMyTerminalBlock U, int Ã, IMyTerminalBlock Â, int Á, double À = -1, bool f = false)
{
    var A = U.GetInventory(Ã
); var P = Â.GetInventory(Á); if (!A.IsConnectedTo(P))
    {
        Ʀ("'" + U.CustomName + "'\nis not connected to '" + Â.CustomName +
"'\nItem transfer aborted!"); return 0;
    }
    if ((float)P.CurrentVolume > (float)P.MaxVolume * 0.98f) return 0; var B = new List<MyInventoryItem>(); A.GetItems(B);
    if (B.Count == 0) return 0; double C = 0; MyDefinitionId D = new MyDefinitionId(); MyDefinitionId E = new MyDefinitionId(); string F = "";
    string G = ""; bool H = false; string I = ""; if (À == -0.5) I = "halfInventory"; if (À == -1) I = "completeInventory"; for (int J = B.Count - 1; J >= 0; J--)
    {
        D = B[J].Type; if (f ? D.ToString() == Ä : D.ToString().Contains(Ä))
        {
            if (I != "" && D != E) C = 0; E = D; F = D.TypeId.ToString().Replace(ǵ, ""); G =
D.SubtypeId.ToString(); H = true; if (!A.CanTransferItemTo(P, D))
            {
                Ʀ("'" + G + "' couldn't be transferred\nfrom '" + U.CustomName +
"'\nto '" + Â.CustomName + "'\nThe conveyor type is too small!"); return 0;
            }
            double K = (double)B[J].Amount; double L = 0; if (I ==
"completeInventory") { A.TransferItemTo(P, J, null, true); }
            else if (I == "halfInventory")
            {
                double M = Math.Ceiling((double)B[J].Amount / 2); A.
TransferItemTo(P, J, null, true, (VRage.MyFixedPoint)M);
            }
            else
            {
                if (!F.Contains(Ƿ)) À = Math.Ceiling(À); A.TransferItemTo(P, J, null, true, (VRage.
MyFixedPoint)À);
            }
            B.Clear(); A.GetItems(B); try { if ((MyDefinitionId)B[J].Type == D) { L = (double)B[J].Amount; } } catch { L = 0; }
            double N = K - L; C += N; À
-= N; if (À <= 0 && I == "") break;
        }
    }
    if (!H) return 0; if (C > 0)
    {
        string O = Math.Round(C, 2) + " " + G + " " + F; Ɍ = "Moved: " + O + "\nfrom: '" + U.
CustomName + "'\nto: '" + Â.CustomName + "'";
    }
    else
    {
        string O = Math.Round(À, 2) + " " + Ä.Replace(ǵ, ""); if (I == "completeInventory") O = "all items";
        if (I == "halfInventory") O = "half of the items"; Ʀ("Couldn't move '" + O + "'\nfrom '" + U.CustomName + "'\nto '" + Â.CustomName +
               "'\nCheck conveyor connection and owner/faction!");
    }
    return C;
}
double e(MyDefinitionId D, IMyTerminalBlock Q, int R = 0) { return (double)Q.GetInventory(R).GetItemAmount(D); ; }
IMyTerminalBlock S(MyDefinitionId D, bool T = false, IMyTerminalBlock U = null)
{
    try { if (ɽ.GetInventory(0).FindItem(D) != null) { return ɽ; } } catch { }
    foreach (var V in ɀ)
    {
        if (D.SubtypeId.ToString() == "Ice" && V.GetType().ToString().Contains("MyGasGenerator")) continue; if (V.
GetInventory(0).FindItem(D) != null) { ɽ = V; return V; }
    }
    if (T)
    {
        foreach (var V in ʂ)
        {
            if (U != null) { if (Ú(V) <= Ú(U)) continue; }
            if (V.GetInventory(0)
.FindItem(D) != null) { ɽ = V; return V; }
        }
    }
    return null;
}
IMyTerminalBlock W(IMyTerminalBlock X, List<IMyTerminalBlock> Y)
{
    IMyTerminalBlock Z = null; Z = d(X, Y); if (Z != null) return Z; Z = d(X, ȿ); if (Z == null) Ʀ("'" + X.CustomName +
                        "'\nhas no empty containers to move its items!"); return Z;
}
IMyTerminalBlock d(IMyTerminalBlock X, List<IMyTerminalBlock> Y)
{
    var Ë = X.GetInventory(0); foreach (var V in Y)
    {
        if (V == X) continue; var Ù = V.GetInventory(0); if ((float)Ù.CurrentVolume < (float)Ù.MaxVolume * 0.95f)
        {
            if (!V.GetInventory(0).
IsConnectedTo(Ë)) continue; return V;
        }
    }
    return null;
}
int Ú(IMyTerminalBlock Q)
{
    string Û = System.Text.RegularExpressions.Regex.Match(Q.
CustomName, @"\[p(\d+|max|min)\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups[1].Value.ToLower(); int Ü = 0; bool Ý =
true; if (Û == "max") { Ü = int.MinValue; } else if (Û == "min") { Ü = int.MaxValue; } else { Ý = Int32.TryParse(Û, out Ü); }
    if (!Ý)
    {
        string Æ = Q.
IsSameConstructAs(Me) ? "" : "1"; Int32.TryParse(Æ + Q.EntityId.ToString().Substring(0, 4), out Ü);
    }
    return Ü;
}
void Þ(IMyTerminalBlock Q, int ß)
{
    string à = System.Text.RegularExpressions.Regex.Match(Q.CustomName, @"\[p(\d+|max|min)\]", System.Text.RegularExpressions.
    RegexOptions.IgnoreCase).Value; string á = ""; if (ß == int.MaxValue) { á = "[PMax]"; } else if (ß == int.MinValue) { á = "[PMin]"; } else { á = "[P" + ß + "]"; }
    if (à == á) { return; } else if (à != "") { Q.CustomName = Q.CustomName.Replace(à, á); } else { Q.CustomName = Q.CustomName + " " + á; }
}
string â(
string ã)
{ ê(); var ä = Storage.Split('\n'); foreach (var o in ä) { if (o.Contains(ã)) { return o.Replace(ã + "=", ""); } } return ""; }
void ë(
string ã, string ß = "")
{
    ê(); var ä = Storage.Split('\n'); string é = ""; foreach (var o in ä)
    {
        if (o.Contains(ã)) { é += ã + "=" + ß + "\n"; }
        else
        {
            é
+= o + "\n";
        }
    }
    Storage = é.TrimEnd('\n');
}
void ê()
{
    var ä = Storage.Split('\n'); if (ä.Length != ɗ.Count)
    {
        string é = ""; foreach (var É in
ɗ) { é += É.Key + "=" + É.Value + "\n"; }
        Storage = é.TrimEnd('\n');
    }
}
void è(IMyTerminalBlock V)
{
    foreach (var ç in Ǚ.Keys.ToList())
    {
        Ǚ[ç]
= "0";
    }
    List<string> æ = V.CustomData.Replace(" ", "").TrimEnd('\n').Split('\n').ToList(); æ.RemoveAll(o => !o.Contains("=") || o.
   Length < 8); bool å = false; foreach (var o in æ)
    {
        var Ø = o.Split('='); if (!Ǚ.ContainsKey(Ø[0]))
        {
            MyDefinitionId D; if (MyDefinitionId.
TryParse(ǵ + Ø[0], out D)) { Õ(D); å = true; }
        }
        Ǚ[Ø[0]] = Ø[1];
    }
    if (å) Ô(); List<string> Í = new List<string>{"Special Container modes:","",
"Positive number: stores wanted amount, removes excess (e.g.: 100)","Negative number: doesn't store items, only removes excess (e.g.: -100)",
"Keyword 'all': stores all items of that subtype (like a type container)",""}; foreach (var É in Ǚ) { Í.Add(É.Key + "=" + É.Value); }
    V.CustomData = string.Join("\n", Í);
}
void Î()
{
    ɲ.Clear(); ɲ.AddRange(ȍ); ɲ.
AddRange(Ȍ); ɲ.AddRange(ȋ); ɲ.AddRange(Ȋ); ɲ.AddRange(Ȑ); ɲ.AddRange(Ȇ); Ǚ.Clear(); foreach (var É in ȍ) { Ǚ[Ǹ + "/" + É] = "0"; }
    foreach (var É
in ȏ) { Ǚ[Ƕ + "/" + É] = "0"; }
    foreach (var É in Ȏ) { Ǚ[Ƿ + "/" + É] = "0"; }
    foreach (var É in Ȍ) { Ǚ[ǹ + "/" + É] = "0"; }
    foreach (var É in ȋ)
    {
        Ǚ[Ǻ + "/" +
É] = "0";
    }
    foreach (var É in Ȋ) { Ǚ[ǻ + "/" + É] = "0"; }
    foreach (var É in Ȑ) { Ǚ[Ǽ + "/" + É] = "0"; }
    foreach (var É in ȉ) { Ǚ[ǽ + "/" + É] = "0"; }
    foreach (var É in ȇ) { Ǚ[Ǿ + "/" + É] = "0"; }
    foreach (var É in Ȇ) { Ǚ[ǿ + "/" + É] = "0"; }
}
void Ï()
{
    for (int J = ɉ; J < ɂ.Count; J++)
    {
        if (ƻ()) return; if (ɉ
>= ɂ.Count - 1) { ɉ = 0; }
        else { ɉ++; }
        var B = new List<MyInventoryItem>(); ɂ[J].GetInventory(0).GetItems(B); foreach (var É in B)
        {
            MyDefinitionId D = É.Type; if (ȑ.Contains(D)) continue; string F = D.TypeId.ToString().Replace(ǵ, ""); string G = D.SubtypeId.ToString(); Ɍ =
                         "Found new item!\n" + G + " (" + F + ")"; if (F == Ƕ)
            {
                ȏ.Add(G); Ɩ(G); if (!G.Contains("Ice"))
                {
                    foreach (var Ö in ɧ)
                    {
                        if (Ö.GetInventory(0).CanItemsBeAdded(1, D
)) { Ǳ.Add(G); break; }
                    }
                }
            }
            else if (F == Ƿ) { Ȏ.Add(G); }
            else if (F == Ǹ) { ȍ.Add(G); }
            else if (F == ǹ) { Ȍ.Add(G); }
            else if (F == Ǻ) { ȋ.Add(G); }
            else if (F == ǻ) { Ȋ.Add(G); } else if (F == Ǽ) { Ȑ.Add(G); } else if (F == ǽ) { ȉ.Add(G); } else if (F == Ǿ) { ȇ.Add(G); } else if (F == ǿ) { Ȇ.Add(G); }
            Õ(D)
; Ƣ(D);
        }
    }
}
void Õ(MyDefinitionId D)
{
    Ɵ(); if (Me.CustomData.Contains(D.ToString())) return; var Ó = Me.CustomData.Split('\n').
ToList(); for (int J = Ó.Count - 1; J >= 0; J--) { if (Ó[J].Contains(";")) { Ó.Insert(J + 1, D + ";noBP"); break; } }
    Me.CustomData = String.Join("\n", Ó
); ǋ(D);
}
bool Ô()
{
    Ɵ(); var Ó = Me.CustomData.Split('\n'); GridTerminalSystem.GetBlocksOfType(ɫ); foreach (var o in Ó)
    {
        var Ò = o.
Split(';'); if (Ò.Length < 2) continue; MyDefinitionId D; if (!MyDefinitionId.TryParse(Ò[0], out D)) continue; MyDefinitionId Ñ; if (
MyDefinitionId.TryParse(Ò[1], out Ñ)) { if (ɫ.Count == 0) return false; if (ơ(Ñ)) { Ǌ(D, Ñ); } else { Ƨ(D); continue; } }
        string F = D.TypeId.ToString().
Replace(ǵ, ""); string G = D.SubtypeId.ToString(); if (F == Ƕ)
        {
            ȏ.Add(G); Ɩ(G); if (!G.Contains("Ice"))
            {
                foreach (var Ö in ɧ)
                {
                    if (Ö.
GetInventory(0).CanItemsBeAdded(1, D)) { Ǳ.Add(G); break; }
                }
            }
        }
        else if (F == Ƿ) { Ȏ.Add(G); }
        else if (F == Ǹ) { ȍ.Add(G); }
        else if (F == ǹ) { Ȍ.Add(G); }
        else if (F == Ǻ) { ȋ.Add(G); }
        else if (F == ǻ) { Ȋ.Add(G); }
        else if (F == Ǽ) { Ȑ.Add(G); }
        else if (F == ǽ) { ȉ.Add(G); }
        else if (F == Ǿ) { ȇ.Add(G); }
        else
if (F == ǿ) { Ȇ.Add(G); }
        ǋ(D);
    }
    return true;
}
void Ɵ() { if (!Me.CustomData.Contains(ɘ)) { Me.CustomData = (ǣ ? ǚ : Ǜ) + ɘ; } }
void Ơ()
{
    if (ȼ !=
null)
    {
        var B = new List<MyInventoryItem>(); ȼ.GetInventory(1).GetItems(B); var ƛ = new List<MyProductionItem>(); ȼ.GetQueue(ƛ); if (B.
          Count == 0) return; ȼ.CustomName = ǰ; MyDefinitionId Ñ = ƛ[0].BlueprintId; MyDefinitionId D = B[0].Type; if (B.Count == 1 && ƛ.Count == 1 && ȼ.Mode
                       == MyAssemblerMode.Assembly && Ñ == Ȓ)
        {
            if (ǰ.Contains(learnKeyword) && !ǰ.Contains(learnManyKeyword)) ȼ.CustomName = ǰ.Replace(" " +
learnKeyword, "").Replace(learnKeyword + " ", ""); ȼ.ClearQueue(); ȼ = null; Ȓ = new MyDefinitionId(); Ɍ = "Learned new Blueprint!\n'" + Ñ.ToString(
).Replace(ǵ, "") + "'\nproduces: '" + D.ToString().Replace(ǵ, "") + "'"; Ǌ(D, Ñ); ƨ(D, Ñ); return;
        }
        else if (ƛ.Count != 1)
        {
            Ʀ(
"Blueprint learning aborted!\nExactly 1 itemstack in the queue is needed to learn new recipes!");
        }
    }
    ȼ = null; Ȓ = new MyDefinitionId(); foreach (var Ɯ in ɭ)
    {
        var ƛ = new List<MyProductionItem>(); Ɯ.GetQueue(ƛ); if (ƛ.Count == 1 && Ɯ.
Mode == MyAssemblerMode.Assembly)
        {
            if (Ɯ.GetInventory(1).ItemCount != 0)
            {
                IMyTerminalBlock Â = W(Ɯ, ʇ); if (Â != null) { Å("", Ɯ, 1, Â, 0); }
                else
                { Ʀ("Can't learn blueprint!\nNo free containers to clear the output inventory found!"); return; }
            }
            ȼ = Ɯ; Ȓ = ƛ[0].BlueprintId; ǰ = Ɯ
.CustomName; Ɯ.CustomName = "Learning " + Ȓ.SubtypeName + " in: " + Ɯ.CustomName; return;
        }
    }
}
bool ơ(MyDefinitionId Ñ)
{
    try
    {
        foreach (
var Ɯ in ɫ) { if (Ɯ.CanUseBlueprint(Ñ)) return true; }
    }
    catch { return false; }
    return false;
}
void Ƣ(MyDefinitionId D)
{
    if (ɫ.Count == 0)
        return; if (D.TypeId.ToString() == ǵ + Ƕ || D.TypeId.ToString() == ǵ + Ƿ) return; MyDefinitionId Ñ; bool ƣ = Ȃ.TryGetValue(D, out Ñ); if (ƣ) ƣ = ơ(Ñ)
                             ; if (!ƣ)
    {
        var Ƥ = new List<string> { "BP", "", "Component", "Magazine", "_Blueprint" }; bool ƫ = false; foreach (var ƪ in Ƥ)
        {
            string Ʃ = Ȁ + D
.SubtypeId.ToString().Replace("Item", "") + ƪ; MyDefinitionId.TryParse(Ʃ, out Ñ); ƫ = ơ(Ñ); if (ƫ) { Ǌ(D, Ñ); ƨ(D, Ñ); ƣ = true; return; }
        }
    }
}
void ƨ(MyDefinitionId D, MyDefinitionId Ñ)
{
    Ɵ(); var Ó = Me.CustomData.Split('\n'); for (var J = 0; J < Ó.Length; J++)
    {
        if (!Ó[J].Contains(
D.ToString())) continue; var Ò = Ó[J].Split(';'); Ó[J] = Ò[0] + ";" + Ñ.ToString(); Me.CustomData = String.Join("\n", Ó); return;
    }
}
void Ƨ
(MyDefinitionId D)
{
    Ɵ(); var Ó = Me.CustomData.Split('\n').ToList(); Ó.RemoveAll(J => J.Contains(D.ToString())); Me.CustomData =
String.Join("\n", Ó);
}
void Ʀ(string ĸ) { ɟ.Add(ĸ); ɞ.Add(ĸ); Ľ = ɟ.ElementAt(0); }
void ƥ()
{
    Me.CustomData = ""; foreach (var V in ʂ)
    {
        List<
string> Ó = V.CustomData.Replace(" ", "").TrimEnd('\n').Split('\n').ToList(); Ó.RemoveAll(o => !o.Contains("=") || o.Contains("=0")); V.
CustomData = string.Join("\n", Ó);
    }
    Echo("Stored items deleted!\n"); if (ʂ.Count > 0) Echo("Also deleted itemlists of " + ʂ.Count +
" Special containers!\n"); Echo("Please hit 'Recompile'!\n\nScript stopped!");
}
void Ɛ()
{
    ȅ.Clear(); List<IMyTerminalBlock> Ƒ = ɬ.ToList<
IMyTerminalBlock>(); List<IMyTerminalBlock> ƒ = ɨ.ToList<IMyTerminalBlock>(); Ɠ(ɂ, 0); Ɠ(Ƒ, 1); Ɠ(ƒ, 1);
}
void Ɠ(List<IMyTerminalBlock> Ɣ, int R)
{
    for
(int J = 0; J < Ɣ.Count; J++)
    {
        var B = new List<MyInventoryItem>(); Ɣ[J].GetInventory(R).GetItems(B); for (int ƕ = 0; ƕ < B.Count; ƕ++)
        {
            MyDefinitionId D = B[ƕ].Type; if (ȅ.ContainsKey(D)) { ȅ[D] += (double)B[ƕ].Amount; } else { ȅ[D] = (double)B[ƕ].Amount; }
        }
    }
}
double e(MyDefinitionId D
)
{ double Ə; ȅ.TryGetValue(D, out Ə); return Ə; }
void Ɩ(string Ɨ) { if (!Ǵ.ContainsKey(Ɨ)) { Ǵ[Ɨ] = 0.5; } }
double Ƙ(string Ɨ)
{
    double Ə
; Ɨ = Ɨ.Replace(ǵ + Ƕ + "/", ""); Ǵ.TryGetValue(Ɨ, out Ə); return Ə != 0 ? Ə : 0.5;
}
void Ɲ()
{
    Ȅ.Clear(); foreach (IMyAssembler Ɯ in ɫ)
    {
        var ƛ =
new List<MyProductionItem>(); Ɯ.GetQueue(ƛ); if (ƛ.Count > 0 && !Ɯ.IsProducing)
        {
            if (Ɯ.Mode == MyAssemblerMode.Assembly) Ʀ("'" + Ɯ.
CustomName + "' has a queue but is currently not assembling!\nAre there enough ingots for the craft?"); if (Ɯ.Mode == MyAssemblerMode.
Disassembly) Ʀ("'" + Ɯ.CustomName + "' has a queue but is currently not disassembling!\nAre the items to disassemble missing?");
        }
        foreach
(var É in ƛ) { MyDefinitionId Ñ = É.BlueprintId; if (Ȅ.ContainsKey(Ñ)) { Ȅ[Ñ] += (double)É.Amount; } else { Ȅ[Ñ] = (double)É.Amount; } }
    }
}
double ƞ(MyDefinitionId Ñ) { double Ə; Ȅ.TryGetValue(Ñ, out Ə); return Ə; }
void ƚ(MyDefinitionId D, double À) { ȃ[D] = À; }
double ƙ(
MyDefinitionId Ñ)
{ double Ə; if (!ȃ.TryGetValue(Ñ, out Ə)) Ə = 100000; return Ə; }
MyDefinitionId Ƭ(MyDefinitionId D, out bool ƣ)
{
    MyDefinitionId
Ñ; ƣ = Ȃ.TryGetValue(D, out Ñ); return Ñ;
}
MyDefinitionId ǈ(MyDefinitionId Ñ)
{
    MyDefinitionId D; ȁ.TryGetValue(Ñ, out D); return D;
}
bool ǉ(MyDefinitionId Ñ) { return ȁ.ContainsKey(Ñ); }
void Ǌ(MyDefinitionId D, MyDefinitionId Ñ) { Ȃ[D] = Ñ; ȁ[Ñ] = D; }
void ǋ(
MyDefinitionId D)
{ ȑ.Add(D); ǯ[D.SubtypeId.ToString()] = D; }
MyDefinitionId ǌ(string G)
{
    MyDefinitionId D = new MyDefinitionId(); ǯ.TryGetValue
(G, out D); return D;
}
StringBuilder Ǎ(IMyTextSurface µ, string È, double ß, double Ǘ, string ǖ = null, string Ǖ = null, bool v = false,
bool ǔ = false)
{
    string ü = ß.ToString(); string í = Ǘ.ToString(); if (ǖ != null) { ü = ǖ; }
    if (Ǖ != null) { í = Ǖ; }
    float Ş = µ.FontSize; float Ž = µ.Ň()
; char ǒ = ' '; float Ǒ = µ.ŀ(ǒ); StringBuilder ǐ = new StringBuilder(" " + ß.ŵ(Ǘ)); ǐ = µ.ŉ(ǒ, µ.ň("99999.9%") - µ.ň(ǐ)).Append(ǐ);
    StringBuilder Ǐ = new StringBuilder(ü + " / " + í); StringBuilder ǎ = new StringBuilder(); StringBuilder Ǔ = new StringBuilder(); StringBuilder Ǉ;
    if (Ǘ == 0) { ǎ.Append(È + " "); Ǉ = µ.ŉ(ǒ, Ž - µ.ň(ǎ) - µ.ň(ü)); ǎ.Append(Ǉ).Append(ü); return ǎ.Append("\n"); }
    double Ʈ = 0; if (Ǘ > 0) Ʈ = ß / Ǘ >= 1 ?
1 : ß / Ǘ; if (ǔ && !v)
    {
        if (Ş <= 0.5 || (Ş <= 1 && Ž > 512))
        {
            ǎ.Append(ƭ(µ, Ž * 0.25f, Ʈ) + " " + È); Ǉ = µ.ŉ(ǒ, Ž * 0.75 - µ.ň(ǎ) - µ.ň(ü + " /")); ǎ.Append(Ǉ).
Append(Ǐ); Ǉ = µ.ŉ(ǒ, Ž - µ.ň(ǎ) - µ.ň(ǐ)); ǎ.Append(Ǉ); ǎ.Append(ǐ);
        }
        else
        {
            ǎ.Append(ƭ(µ, Ž * 0.3f, Ʈ) + " " + È); Ǉ = µ.ŉ(ǒ, Ž - µ.ň(ǎ) - µ.ň(ǐ)); ǎ.
Append(Ǉ); ǎ.Append(ǐ);
        }
    }
    else
    {
        ǎ.Append(È + " "); if (Ş <= 0.6 || (Ş <= 1 && Ž > 512))
        {
            Ǉ = µ.ŉ(ǒ, Ž * 0.5 - µ.ň(ǎ) - µ.ň(ü + " /")); ǎ.Append(Ǉ).Append(Ǐ)
; Ǉ = µ.ŉ(ǒ, Ž - µ.ň(ǎ) - µ.ň(ǐ)); ǎ.Append(Ǉ).Append(ǐ); if (!v) { Ǔ = ƭ(µ, Ž, Ʈ).Append("\n"); }
        }
        else
        {
            Ǉ = µ.ŉ(ǒ, Ž - µ.ň(ǎ) - µ.ň(Ǐ)); ǎ.Append(Ǉ
).Append(Ǐ); if (!v) { Ǔ = ƭ(µ, Ž - µ.ň(ǐ), Ʈ); Ǔ.Append(ǐ).Append("\n"); }
        }
    }
    return ǎ.Append("\n").Append(Ǔ);
}
StringBuilder ƭ(
IMyTextSurface µ, float ş, double Ʈ)
{
    StringBuilder Ư, ư; char Ʊ = '['; char Ʋ = ']'; char Ƴ = 'I'; char ƴ = '.'; float Ƶ = µ.ŀ(Ʊ); float ƶ = µ.ŀ(Ʋ); float Ʒ
= ş - Ƶ - ƶ; Ư = µ.ŉ(Ƴ, Ʒ * Ʈ); ư = µ.ŉ(ƴ, Ʒ - µ.ň(Ư)); return new StringBuilder().Append(Ʊ).Append(Ư).Append(ư).Append(Ʋ);
}
StringBuilder Ƹ
= new StringBuilder("No performance Information available!"); Dictionary<string, int> ƹ = new Dictionary<string, int>(); List<int
> ƺ = new List<int>(new int[100]); List<double> ǆ = new List<double>(new double[100]); double ǅ, Ǆ; int ǃ = 0; DateTime ǂ; void ǁ(
string ǀ, bool Ƈ = false)
{
    if (Ƈ) { ǂ = DateTime.Now; return; }
    ǃ = ǃ >= 99 ? 0 : ǃ + 1; int ƿ = Runtime.CurrentInstructionCount; if (ƿ > ǅ) ǅ = ƿ; ƺ[ǃ] = ƿ;
    double ƾ = ƺ.Sum() / ƺ.Count; Ƹ.Clear(); Ƹ.Append("Instructions: " + ƿ + " / " + Runtime.MaxInstructionCount + "\n"); Ƹ.Append(
                   "Max. Instructions: " + ǅ + " / " + Runtime.MaxInstructionCount + "\n"); Ƹ.Append("Avg. Instructions: " + Math.Floor(ƾ) + " / " + Runtime.
                            MaxInstructionCount + "\n\n"); double ƽ = (DateTime.Now - ǂ).TotalMilliseconds; if (ƽ > Ǆ && ƹ.ContainsKey(ǀ)) Ǆ = ƽ; ǆ[ǃ] = ƽ; double Ƽ = ǆ.Sum() / ǆ.Count; Ƹ.
                                                     Append("Last runtime: " + Math.Round(ƽ, 4) + " ms\n"); Ƹ.Append("Max. runtime: " + Math.Round(Ǆ, 4) + " ms\n"); Ƹ.Append("Avg. runtime: " +
                                                                 Math.Round(Ƽ, 4) + " ms\n\n"); Ƹ.Append("Instructions per Method:\n"); ƹ[ǀ] = ƿ; foreach (var É in ƹ.OrderByDescending(J => J.Value))
    {
        Ƹ
.Append("- " + É.Key + ": " + É.Value + "\n");
    }
    Ƹ.Append("\n");
}
bool ƻ(double ß = 10)
{
    return Runtime.CurrentInstructionCount > ß * 1000;
}
List<IMyTerminalBlock> Ǝ(string Œ, string[] ũ = null)
{
    string œ = "[IsyLCD]"; var Ŕ = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(Ŕ, ŕ => ŕ.IsSameConstructAs(Me) && (ŕ.CustomName.Contains(Œ) || (ŕ.CustomName.Contains
     (œ) && ŕ.CustomData.Contains(Œ)))); var Ŗ = Ŕ.FindAll(ŕ => ŕ.CustomName.Contains(Œ)); foreach (var µ in Ŗ)
    {
        µ.CustomName = µ.
CustomName.Replace(Œ, "").Replace(" " + Œ, "").TrimEnd(' '); bool ŗ = false; if (µ is IMyTextSurface)
        {
            if (!µ.CustomName.Contains(œ)) ŗ = true;
            if (!µ.CustomData.Contains(Œ)) µ.CustomData = "@0 " + Œ + (ũ != null ? "\n" + String.Join("\n", ũ) : "");
        }
        else if (µ is
IMyTextSurfaceProvider)
        {
            if (!µ.CustomName.Contains(œ)) ŗ = true; int Ř = (µ as IMyTextSurfaceProvider).SurfaceCount; for (int J = 0; J < Ř; J++)
            {
                if (!µ.
CustomData.Contains("@" + J)) { µ.CustomData += (µ.CustomData == "" ? "" : "\n\n") + "@" + J + " " + Œ + (ũ != null ? "\n" + String.Join("\n", ũ) : ""); break; }
            }
        }
        else { Ŕ.Remove(µ); }
        if (ŗ) µ.CustomName += " " + œ;
    }
    return Ŕ;
}
}class ř : IComparer<MyDefinitionId>
{
    public int Compare(MyDefinitionId Ś, MyDefinitionId ś)
    {
        return Ś.ToString().CompareTo(ś.
ToString());
    }
}
class Ŝ : IEqualityComparer<MyInventoryItem>
{
    public bool Equals(MyInventoryItem Ś, MyInventoryItem ś)
    {
        return Ś.
ToString() == ś.ToString();
    }
    public int GetHashCode(MyInventoryItem É) { return É.ToString().GetHashCode(); }
}
public static partial
class ŧ
{
    private static Dictionary<char, float> ť = new Dictionary<char, float>(); public static void Ť(string ţ, float Ţ)
    {
        foreach (
char ŝ in ţ) { ť[ŝ] = Ţ; }
    }
    public static void š()
    {
        if (ť.Count > 0) return; Ť(
"3FKTabdeghknopqsuy£µÝàáâãäåèéêëðñòóôõöøùúûüýþÿāăąďđēĕėęěĝğġģĥħĶķńņňŉōŏőśŝşšŢŤŦũūŭůűųŶŷŸșȚЎЗКЛбдекруцяёђћўџ", 18); Ť("ABDNOQRSÀÁÂÃÄÅÐÑÒÓÔÕÖØĂĄĎĐŃŅŇŌŎŐŔŖŘŚŜŞŠȘЅЊЖф□", 22); Ť("#0245689CXZ¤¥ÇßĆĈĊČŹŻŽƒЁЌАБВДИЙПРСТУХЬ€", 20); Ť(
"￥$&GHPUVY§ÙÚÛÜÞĀĜĞĠĢĤĦŨŪŬŮŰŲОФЦЪЯжы†‡", 21); Ť("！ !I`ijl ¡¨¯´¸ÌÍÎÏìíîïĨĩĪīĮįİıĵĺļľłˆˇ˘˙˚˛˜˝ІЇії‹›∙", 9); Ť("？7?Jcz¢¿çćĉċčĴźżžЃЈЧавийнопсъьѓѕќ", 17); Ť(
"（）：《》，。、；【】(),.1:;[]ft{}·ţťŧț", 10); Ť("+<=>E^~¬±¶ÈÉÊË×÷ĒĔĖĘĚЄЏЕНЭ−", 19); Ť("L_vx«»ĹĻĽĿŁГгзлхчҐ–•", 16); Ť("\"-rª­ºŀŕŗř", 11); Ť("WÆŒŴ—…‰", 32); Ť("'|¦ˉ‘’‚", 7)
; Ť("@©®мшњ", 26); Ť("mw¼ŵЮщ", 28); Ť("/ĳтэє", 15); Ť("\\°“”„", 13); Ť("*²³¹", 12); Ť("¾æœЉ", 29); Ť("%ĲЫ", 25); Ť("MМШ", 27); Ť("½Щ", 30);
        Ť("ю", 24); Ť("ј", 8); Ť("љ", 23); Ť("ґ", 14); Ť("™", 31);
    }
    public static Vector2 Ŧ(this IMyTextSurface ħ, StringBuilder ĸ)
    {
        š();
        Vector2 ş = new Vector2(); if (ħ.Font == "Monospace") { float Ş = ħ.FontSize; ş.X = (float)(ĸ.Length * 19.4 * Ş); ş.Y = (float)(28.8 * Ş); return ş; }
        else
        {
            float Ş = (float)(ħ.FontSize * 0.779); foreach (char ŝ in ĸ.ToString()) { try { ş.X += ť[ŝ] * Ş; } catch { } }
            ş.Y = (float)(28.8 * ħ.FontSize)
; return ş;
        }
    }
    public static float ň(this IMyTextSurface µ, StringBuilder ĸ) { Vector2 Ŀ = µ.Ŧ(ĸ); return Ŀ.X; }
    public static float
ň(this IMyTextSurface µ, string ĸ)
    { Vector2 Ŀ = µ.Ŧ(new StringBuilder(ĸ)); return Ŀ.X; }
    public static float ŀ(this
IMyTextSurface µ, char Ł)
    { float ł = ň(µ, new string(Ł, 1)); return ł; }
    public static int Ń(this IMyTextSurface µ)
    {
        Vector2 ń = µ.SurfaceSize;
        float Ņ = µ.TextureSize.Y; ń.Y *= 512 / Ņ; float ņ = ń.Y * (100 - µ.TextPadding * 2) / 100; Vector2 Ŀ = µ.Ŧ(new StringBuilder("T")); return (int)(ņ /
                              Ŀ.Y);
    }
    public static float Ň(this IMyTextSurface µ)
    {
        Vector2 ń = µ.SurfaceSize; float Ņ = µ.TextureSize.Y; ń.X *= 512 / Ņ; return ń.X *
(100 - µ.TextPadding * 2) / 100;
    }
    public static StringBuilder ŉ(this IMyTextSurface µ, char Ŋ, double ŋ)
    {
        int Ō = (int)(ŋ / ŀ(µ, Ŋ)); if (
Ō < 0) Ō = 0; return new StringBuilder().Append(Ŋ, Ō);
    }
    private static DateTime ō = DateTime.Now; private static Dictionary<int, List
<int>> Ŏ = new Dictionary<int, List<int>>(); public static StringBuilder ŏ(this IMyTextSurface µ, StringBuilder ĸ, int Ő = 3, bool
Ĺ = true, int ı = 0)
    {
        int Š = µ.GetHashCode(); if (!Ŏ.ContainsKey(Š)) { Ŏ[Š] = new List<int> { 1, 3, Ő, 0 }; }
        int ő = Ŏ[Š][0]; int Ũ = Ŏ[Š][1]; int
ź = Ŏ[Š][2]; int Ż = Ŏ[Š][3]; var ż = ĸ.ToString().TrimEnd('\n').Split('\n'); List<string> Ĵ = new List<string>(); if (ı == 0) ı = µ.Ń();
        float Ž = µ.Ň(); StringBuilder Ā, q = new StringBuilder(); for (int J = 0; J < ż.Length; J++)
        {
            if (J < Ő || J < ź || Ĵ.Count - ź > ı || µ.ň(ż[J]) <= Ž)
            {
                Ĵ.Add
(ż[J]);
            }
            else
            {
                try
                {
                    q.Clear(); float ž, ſ; var ƀ = ż[J].Split(' '); string Ɓ = System.Text.RegularExpressions.Regex.Match(ż[J],
         @"\d+(\.|\:)\ ").Value; Ā = µ.ŉ(' ', µ.ň(Ɓ)); foreach (var Ƃ in ƀ)
                    {
                        ž = µ.ň(q); ſ = µ.ň(Ƃ); if (ž + ſ > Ž)
                        {
                            Ĵ.Add(q.ToString()); q = new StringBuilder(Ā + Ƃ +
" ");
                        }
                        else { q.Append(Ƃ + " "); }
                    }
                    Ĵ.Add(q.ToString());
                }
                catch { Ĵ.Add(ż[J]); }
            }
        }
        if (Ĺ)
        {
            if (Ĵ.Count > ı)
            {
                if (DateTime.Now.Second != Ż)
                {
                    Ż =
DateTime.Now.Second; if (Ũ > 0) Ũ--; if (Ũ <= 0) ź += ő; if (ź + ı - Ő >= Ĵ.Count && Ũ <= 0) { ő = -1; Ũ = 3; }
                    if (ź <= Ő && Ũ <= 0) { ő = 1; Ũ = 3; }
                }
            }
            else { ź = Ő; ő = 1; Ũ = 3; }
            Ŏ[Š][
0] = ő; Ŏ[Š][1] = Ũ; Ŏ[Š][2] = ź; Ŏ[Š][3] = Ż;
        }
        else { ź = Ő; }
        StringBuilder ƃ = new StringBuilder(); for (var o = 0; o < Ő; o++)
        {
            ƃ.Append(Ĵ[o] + "\n"
);
        }
        for (var o = ź; o < Ĵ.Count; o++) { ƃ.Append(Ĵ[o] + "\n"); }
        return ƃ;
    }
    public static Dictionary<IMyTextSurface, string> Ƅ(this
IMyTerminalBlock Q, string Œ, Dictionary<string, string> ƌ = null)
    {
        var Ƌ = new Dictionary<IMyTextSurface, string>(); if (Q is IMyTextSurface)
        {
            Ƌ[Q
as IMyTextSurface] = Q.CustomData;
        }
        else if (Q is IMyTextSurfaceProvider)
        {
            var Ɗ = System.Text.RegularExpressions.Regex.Matches(Q
.CustomData, @"@(\d) *(" + Œ + @")"); int Ɖ = (Q as IMyTextSurfaceProvider).SurfaceCount; foreach (System.Text.RegularExpressions.
Match ƈ in Ɗ)
            {
                int ƍ = -1; if (int.TryParse(ƈ.Groups[1].Value, out ƍ))
                {
                    if (ƍ >= Ɖ) continue; string Ó = Q.CustomData; int Ƈ = Ó.IndexOf("@" + ƍ
); int Ɔ = Ó.IndexOf("@", Ƈ + 1) - Ƈ; string ì = Ɔ <= 0 ? Ó.Substring(Ƈ) : Ó.Substring(Ƈ, Ɔ); Ƌ[(Q as IMyTextSurfaceProvider).GetSurface(ƍ)]
= ì;
                }
            }
        }
        return Ƌ;
    }
    public static bool ƅ(this string ì, string ã)
    {
        var Ó = ì.Replace(" ", "").Split('\n'); foreach (var o in Ó)
        {
            if (o
.StartsWith(ã + "=")) { try { return Convert.ToBoolean(o.Replace(ã + "=", "")); } catch { return true; } }
        }
        return true;
    }
    public static
string Ź(this string ì, string ã)
    {
        var Ó = ì.Replace(" ", "").Split('\n'); foreach (var o in Ó)
        {
            if (o.StartsWith(ã + "="))
            {
                return o.
Replace(ã + "=", "");
            }
        }
        return "";
    }
}
public static partial class ŧ
{
    public static bool Ū(this double ß, double ū, double í, bool Ŭ = false,
bool ŭ = false)
    { bool Ů = ŭ ? ß > ū : ß >= ū; bool ů = Ŭ ? ß < í : ß <= í; return Ů && ů; }
}
public static partial class ŧ
{
    public static string Ű(this
char ű, int Ų)
    { if (Ų <= 0) { return ""; } return new string(ű, Ų); }
}
public static partial class ŧ
{
    public static string Ÿ(this double ß
)
    {
        string ŷ = "kL"; if (ß < 1) { ß *= 1000; ŷ = "L"; }
        else if (ß >= 1000 && ß < 1000000) { ß /= 1000; ŷ = "ML"; }
        else if (ß >= 1000000 && ß < 1000000000)
        {
            ß /=
1000000; ŷ = "BL";
        }
        else if (ß >= 1000000000) { ß /= 1000000000; ŷ = "TL"; }
        return Math.Round(ß, 1) + " " + ŷ;
    }
}
public static partial class ŧ
{
    public static string Ŷ(this double ß)
    {
        string ŷ = ""; if (ß >= 1000 && ß < 1000000) { ß /= 1000; ŷ = " k"; }
        else if (ß >= 1000000 && ß < 1000000000)
        {
            ß /=
1000000; ŷ = " M";
        }
        else if (ß >= 1000000000) { ß /= 1000000000; ŷ = " B"; }
        return Math.Round(ß, 1) + ŷ;
    }
}
public static partial class ŧ
{
    public
static string ŵ(this double Ŵ, double Ć)
    { double ų = Math.Round(Ŵ / Ć * 100, 1); if (Ć == 0) { return "0%"; } else { return ų + "%"; } }
    public static
string ŵ(this float Ŵ, float Ć)
    { double ų = Math.Round(Ŵ / Ć * 100, 1); if (Ć == 0) { return "0%"; } else { return ų + "%"; } }