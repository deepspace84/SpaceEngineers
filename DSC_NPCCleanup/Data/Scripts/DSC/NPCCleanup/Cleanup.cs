using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace DSC.NPCCleanup
{
    public class DSC_Cleanup
    {

        public DSC_Cleanup(){}


        public Dictionary<long, string> NPCPlayers = new Dictionary<long, string>();
        public Dictionary<long, DSC_Grid> Gridcache = new Dictionary<long, DSC_Grid>();

        /*
         * register handlers 
         */
        public void Load()
        {
            // Get all players
            List<IMyPlayer> list = new List<IMyPlayer>();
            List<IMyIdentity> ilist = new List<IMyIdentity>();

            MyAPIGateway.Players.GetAllIdentites(ilist);

            foreach (IMyIdentity ident in ilist)
            {
                NPCPlayers.Add(ident.IdentityId, ident.DisplayName);
            }

            // Get all grid entities
            HashSet<IMyEntity> entList = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entList, e => e is IMyCubeGrid);
            if (entList.Count > 0)
            {
                // Loop through all Grids
                foreach (IMyEntity ent in entList)
                {
                    MyCubeGrid grid = ent as MyCubeGrid;

                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Available grid=> -" + grid.DisplayName + "- | EntityId=> -" + grid.EntityId.ToString() + " Owner=>" + grid.BigOwners.FirstOrDefault().ToString() + " - Name=>" + MyVisualScriptLogicProvider.GetPlayersName(grid.BigOwners.FirstOrDefault()));
                    long ownerId = grid.BigOwners.FirstOrDefault();
                    if (ownerId != 0)
                    {
                        if (NPCPlayers.ContainsKey(ownerId))
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Is NPC");
                        }
                        else
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Is no NPC");
                        }
                    }


                }



            }

            MyVisualScriptLogicProvider.PrefabSpawnedDetailed += PrefabDetailed_Event;
        }


        /*
         * Unregister handlers 
         */
        public void Unload()
        {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= PrefabDetailed_Event;
        }



        private void PrefabDetailed_Event(long entityId, string prefabName)
        {
            try
            {

                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Cleanup::PrefabDetailed_Event Ship spawned=> -" + prefabName + "- | EntityId=> -" + entityId.ToString());







            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "Cleanup::PrefabDetailed_Event failed");
            }
        }





    }
}
