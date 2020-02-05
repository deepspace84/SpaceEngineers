using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRageMath;

namespace Stollie.Progression
{
    public abstract class CoreBase : MySessionComponentBase
    {

        bool _initialized = false;
        bool _haveHandler = false;
        public static VRage.ModAPI.IMyInput input = null;
        public static IMySession session = MyAPIGateway.Session;
        public static IMyPlayer player = session.Player;
        public static IMyCharacter character = player.Character;

        public bool InInitialization { get; private set; } = false;

        public abstract void Deinitialize();

        public new bool Initialized { get { return _initialized; } }

        public abstract bool Initialize(out MyUpdateOrder order);

        public override string ToString()
        {
            return "Phoera's Core Base class";
        }

        public virtual void UpdateBeforeSim()
        {
        }

        public virtual void UpdateAfterSim()
        {

        }

        public override void UpdateBeforeSimulation()
        {
            if (MyAPIGateway.Session == null)
                return;

           
            //KeyBoardInput();
            //MyAPIGateway.Session.Player.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;

            if (!_initialized)
            {
                InInitialization = true;
                MyUpdateOrder order;
                _initialized = Initialize(out order);
                InInitialization = false;
                if (!_initialized)
                {
                    return;
                }
                ChangeUpdateOrder(order);
                if (order.HasFlag(MyUpdateOrder.BeforeSimulation))
                    UpdateBeforeSim();
                return;
            }
            UpdateBeforeSim();
        }

        protected void ChangeUpdateOrder(MyUpdateOrder value)
        {
            MyAPIGateway.Utilities.InvokeOnGameThread(() => SetUpdateOrder(value));
        }

        public override void UpdateAfterSimulation()
        {
            UpdateAfterSim();
        }

        protected override void UnloadData()
        {
            if (_initialized)
            {
                Deinitialize();
            }
            _initialized = false;
        }

        public void KeyBoardInput()
        {
            input = MyAPIGateway.Input;
            try
            {
                //Ctrl + G
                if (input.IsKeyPress((MyKeys)17) && input.WasKeyPress((MyKeys)71))
                {
                    MyVisualScriptLogicProvider.ShowNotificationToAll("running", 10000, "Green");
                }
            }
            catch
            {

            }
        }

        /*
         * public void Controller_ControlledEntityChanged(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
            if (arg2 != null && arg2.Entity is IMyCharacter)
            {
                MyVisualScriptLogicProvider.ShowNotificationToAll("running", 3000, "Green");
                input = MyAPIGateway.Input;
                try
                {
                    //Ctrl + G
                    if (input.IsKeyPress((MyKeys)17) && input.WasKeyPress((MyKeys)71))
                    {
                        MyVisualScriptLogicProvider.ShowNotificationToAll("running", 10000, "Green");
                    }
                }
                catch
                {

                }
                MyAPIGateway.Session.Player.Controller.ControlledEntityChanged -= Controller_ControlledEntityChanged;
            }
        }
        */
    }
}