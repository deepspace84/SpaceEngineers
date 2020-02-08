using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game.Entity;
using SpaceEngineers.Game.ModAPI;
using Sandbox.ModAPI;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Multiplayer;
using System;
using System.Text;
using VRageMath;
using VRage.Game;
using VRage;

using Logic = Sandbox.Game.MyVisualScriptLogicProvider;

namespace IndustrialAutomaton_FactionOfflineProtection
{
  [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Decoy), false, new string[] { "Offline" })]
  public class FactionOfflineProtection : MyGameLogicComponent
  {

#region Initialise

		private bool _isInit;
    private bool _runOnce;
		public IMyFunctionalBlock _fblock;
    private List<IMyCubeGrid> _myGrids;
    private List<string> _factionList;
    private int _timeOut;
    public bool _isActive;

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      return Container.Entity.GetObjectBuilder(copy);
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      if (_isInit) return;
      try
      {
        _fblock = Container.Entity as IMyFunctionalBlock;
        _myGrids = new List<IMyCubeGrid>();
        _factionList = new List<string>();
        _timeOut = 0;
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        _isInit = true;
      }
      catch (Exception ex)
      {
//        MyAPIGateway.Utilities.ShowMessage(ex.Message, ex.StackTrace);
      }
    }

    public override void UpdateOnceBeforeFrame()
    {      
			this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }
    
#endregion

#region BlockActions		

    public override void UpdateBeforeSimulation100()
    {	
      if (_fblock.CubeGrid.IsStatic && _fblock.IsWorking && _fblock.IsFunctional)
      {
        if (_isActive)
          runChecks();
        else
          initChecks();
      }
      else
      {
        if (_isActive)
        {
          foreach (var grid in _myGrids)
            deactivate(grid);
          _myGrids.Clear();
          _isActive = false;
        }
      }
    }

    private void deactivate(IMyCubeGrid grid)
    {
    // Make grid destructible and editable, and remove visual effect
      var id = grid.EntityId;
      Logic.SetName(id, id.ToString());
      Logic.SetGridDestructible(id.ToString(), true);
      Logic.SetGridEditable(id.ToString(), true);
      var sbList = new List<IMySlimBlock>();
      grid.GetBlocks(sbList);
      foreach (var sb in sbList)
        sb.Dithering = 0f;
      _timeOut = 0;
    }

    private void runChecks()
    {      
      var myGrids = new List<IMyCubeGrid>();
      foreach (var grid in _myGrids)
        myGrids.Add(grid);
      _myGrids.Clear();
      initChecks();
      foreach (var grid in myGrids)
        if (!_myGrids.Contains(grid))
          deactivate(grid);
    }

		private void initChecks()
		{
      
      // Check faction ownership conditions
      _factionList.Clear();
      
      // Don't activate if owned by Nobody, or a player with no faction
      if (_fblock.GetOwnerFactionTag()=="")
        {
          _fblock.Enabled = false;
          return;
        }
      _factionList.Add(_fblock.GetOwnerFactionTag());

      // All terminal blocks on this grid must be of the same faction, or an allied faction
      List<IMySlimBlock> slimList = new List<IMySlimBlock>();
      if (!allied(_fblock.CubeGrid))
        {
          _fblock.Enabled = false;
          return;
        }

      // If a player from factions represented on this grid is online duriong countdown, reset it to zero
      if (playerOnline())
        {
          _timeOut=0;
          return;
        }

      // Reduce timeout delay if server just restarted
      if (Session._newRun && _timeOut==0)
      {
        Session._newRun = false;
        _timeOut = 33;
      }
      
      // Timeout delay x 1.4 (36 = 1 min)
      if (++_timeOut<36)
        return;

      // All conditions met!      
      addGrids();
      foreach (var grid in _myGrids)
        activate(grid);
    }
    
    private void addGrids()
    {
      _myGrids.Clear();
      
      _isActive = true;
      _myGrids.Add(_fblock.CubeGrid);

      var slimList = new List<IMySlimBlock>();
      _fblock.CubeGrid.GetBlocks(slimList, b => b.FatBlock!=null);
      
      // Get any connected grids (rotor/piston/connector) which pass ownership conditions
      foreach (var slim in slimList)
      {
        if (slim.FatBlock is IMyMechanicalConnectionBlock)
        {                    
          var other = (slim.FatBlock as IMyMechanicalConnectionBlock).Top;
          if (other==null)
            continue;
          if (!allied(other.CubeGrid))
            continue;
          if (playerOnline())
            continue;
          if (!_myGrids.Contains(other.CubeGrid))
            _myGrids.Add(other.CubeGrid);
        }        
        else if (slim.FatBlock is IMyShipConnector)
        {
          if ((slim.FatBlock as Sandbox.ModAPI.Ingame.IMyShipConnector).Status!=Sandbox.ModAPI.Ingame.MyShipConnectorStatus.Connected)
            continue;
          var other = (slim.FatBlock as IMyShipConnector).OtherConnector;
          if (!allied(other.CubeGrid))
            continue;
          if (playerOnline())
            continue;
          if (!_myGrids.Contains(other.CubeGrid))
            _myGrids.Add(other.CubeGrid);
        }                
      }
    }
      
    private void activate(IMyCubeGrid grid)
    {
      // Make grid indestructible and uneditable, and apply visual effect
      var id = grid.EntityId;
      Logic.SetName(id, id.ToString());
      Logic.SetGridDestructible(id.ToString(), false);
      Logic.SetGridEditable(id.ToString(), false);
      var sbList = new List<IMySlimBlock>();
      grid.GetBlocks(sbList);
      foreach (var sb in sbList)
        sb.Dithering = -0.5f;
    }

    // Return true if there are no players online from any factions in the list
    private bool playerOnline()
    {
      var playerList = MyVisualScriptLogicProvider.GetOnlinePlayers();
			foreach (var player in playerList)
        if (_factionList.Contains(MyVisualScriptLogicProvider.GetPlayersFactionTag(player)))
					return true;
      return false;      
    }
    
    // Return true if all terminal blocks on this grid are of the same, or an allied, faction
    private bool allied(IMyCubeGrid grid)
    {
      var myFactionId = MyAPIGateway.Session.Factions.TryGetFactionByTag(_fblock.GetOwnerFactionTag()).FactionId;
      List<IMySlimBlock> slimList = new List<IMySlimBlock>();
      grid.GetBlocks(slimList, b => b.FatBlock is IMyTerminalBlock);
      foreach (var slim in slimList)
      {
        var tag = slim.FatBlock.GetOwnerFactionTag();
        if (tag=="")
          continue;
        if (MyAPIGateway.Session.Factions.AreFactionsEnemies(myFactionId,
            MyAPIGateway.Session.Factions.TryGetFactionByTag(tag).FactionId))
          return false;
        if (!_factionList.Contains(slim.FatBlock.GetOwnerFactionTag()))
          _factionList.Add(slim.FatBlock.GetOwnerFactionTag());
      }
      return true;
    }
	}

#endregion

  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class Session : MySessionComponentBase
  {

    private static bool _isInit;
    public static bool _newRun;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      if (!_isInit)
      {
        base.Init(sessionComponent);
        _isInit = true;
        _newRun = true;
      }
    }

    public override void UpdateBeforeSimulation()
    {
      if (MyAPIGateway.Multiplayer.IsServer && !MyAPIGateway.Utilities.IsDedicated)
        return;
      if (!_newRun)
        return;
      HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
      MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
      if (entList.Count==0)
        return;
      foreach (var ent in entList)
      {
        var grid = ent as IMyCubeGrid;
        var id = grid.EntityId;
        Logic.SetName(id, id.ToString());
        Logic.SetGridDestructible(id.ToString(), true);
        Logic.SetGridEditable(id.ToString(), true);
        var sbList = new List<IMySlimBlock>();
        grid.GetBlocks(sbList);
        foreach (var sb in sbList)
          sb.Dithering = 0f;
      }
      _newRun = false;      
    }
    
  }
}