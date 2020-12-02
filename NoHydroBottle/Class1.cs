using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRage.Game.Entity;
using VRage.Voxels;

namespace UnstableBlocks
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CargoContainer), true, "AdminCloneBlock")]
    public class CloneBlock : MyGameLogicComponent
    {

        Sandbox.ModAPI.IMyTerminalBlock TerminalalBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;

            TerminalalBlock = Entity as Sandbox.ModAPI.IMyTerminalBlock;
        }

        public override void UpdateBeforeSimulation100()
        {
            base.UpdateBeforeSimulation100();


            IMyInventory inventory = ((Sandbox.ModAPI.IMyTerminalBlock)Entity).GetInventory(0) as IMyInventory;
            double targetAmount = 10000;

            if (TerminalalBlock.CustomData != null && TerminalalBlock.CustomData != "")
            {
                try
                {
                    targetAmount = double.Parse(TerminalalBlock.CustomData);
                }
                catch (Exception e) { }
            }

            try
            {
                for (int i = 0; i < inventory.ItemCount; i++)
                {
                    MyObjectBuilder_PhysicalObject builder = GetItemBuilder(inventory.GetItemAt(i).Value);
                    if (builder == null) continue;

                    double amount = targetAmount - (double)inventory.GetItemAmount(builder);
                    if (amount < 0) inventory.RemoveItemsOfType((MyFixedPoint)Math.Abs(amount), builder);
                    else if (amount > 0) inventory.AddItems((MyFixedPoint)amount, builder);
                }
            }
            catch (Exception e) { }

            TerminalalBlock.RefreshCustomInfo();

        }


        public MyObjectBuilder_PhysicalObject GetItemBuilder(VRage.Game.ModAPI.Ingame.MyInventoryItem item)
        {
            switch (item.Type.TypeId)
            {
                case "MyObjectBuilder_Component":
                    return new MyObjectBuilder_Component() { SubtypeName = item.Type.SubtypeId };
                case "MyObjectBuilder_AmmoMagazine":
                    return new MyObjectBuilder_AmmoMagazine() { SubtypeName = item.Type.SubtypeId };
                case "MyObjectBuilder_Ingot":
                    return new MyObjectBuilder_Ingot() { SubtypeName = item.Type.SubtypeId };
                case "MyObjectBuilder_Ore":
                    return new MyObjectBuilder_Ore() { SubtypeName = item.Type.SubtypeId };
                case "MyObjectBuilder_PhysicalObject":
                    return new MyObjectBuilder_PhysicalObject() { SubtypeName = item.Type.SubtypeId };
                case "MyObjectBuilder_ConsumableItem":
                    return new MyObjectBuilder_ConsumableItem() { SubtypeName = item.Type.SubtypeId };
                default: return null;
            }
        }

    }
}