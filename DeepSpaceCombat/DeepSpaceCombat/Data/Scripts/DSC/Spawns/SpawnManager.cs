using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace DSC
{
    public class DSC_SpawnManager
    {

        private DSC_Storage_SpawnManager Storage;
        private List<DSC_SpawnShip> SpawnQueue = new List<DSC_SpawnShip>();
        private DSC_SpawnShip SpawnCache = null;
        private DateTime SpawnCacheTimer = DateTime.MinValue;



        public DSC_SpawnManager(){}

        #region load/unload/save functions for the core.cs
        /*
         * Load all data from savegame and register handlers 
         */
        public void Load()
        {
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_SpawnManager", typeof(DSC_Storage_SpawnManager)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_SpawnManager", typeof(DSC_Storage_SpawnManager));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_SpawnManager>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Storage_SpawnManager found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_SpawnManager loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Storage_SpawnManager found, create default");
                // Create default values
                Storage = new DSC_Storage_SpawnManager
                {
                    SpawnedData = new Dictionary<long, DSC_SpawnShip>(),
                    SpawnId = 0
                };
            }

            // Add listeners
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed += PrefabDetailed_Event;
        }

        /*
         * Save all data to savegame 
         */
        public void Save()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_SpawnManager>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_SpawnManager", typeof(DSC_Storage_SpawnManager));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();
        }

        /*
         * Unregister handlers 
         */
        public void Unload()
        {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= PrefabDetailed_Event;
        }
        #endregion

        

        // Checks the current state of all spawns
        public void Check()
        {
            // Check SpawnQueue and empty cache
            if(SpawnQueue.Count > 0 && null == SpawnCache)
            {
                // Span ship and set it to cache
                foreach(DSC_SpawnShip ship in SpawnQueue)
                {
                    // Set as active spawn
                    SpawnCache = ship;

                    // Create spawn
                    CreateSpawn(ship);

                    // Set Cache Timeout
                    SpawnCacheTimer = DateTime.UtcNow; 

                    // Delete it from queue
                    SpawnQueue.Remove(ship);
                    break;
                }
            }

            // Check SpanCache and timer over 2 seconds => spawn failed
            if(null != SpawnCache && 2 < (DateTime.UtcNow - SpawnCacheTimer).TotalSeconds)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::Check: SpawnCache over 2 seconds. Failed template=>" + SpawnCache.SpawnTemplateName);

                // Free spawncache
                SpawnCache = null;
                SpawnCacheTimer = DateTime.MinValue;
            }


            // Loop through all ships in cache


        }

        public void WriteStorage()
        {
            foreach(DSC_SpawnShip ship in Storage.SpawnedData.Values)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("ID=>"+ship.Id.ToString());
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("SpawnTemplateName=>" + ship.SpawnTemplateName);
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("ActiveRoute=>" + ship.ActiveRoute.ToString());
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("GridEntityId=>" + ship.GridEntityId.ToString());
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("---------------------------------------------------");
            }
        }


        public int Spawn(string templateName, string prefabName)
        {
            // Check template
            if (DeepSpaceCombat.Instance.SpawnTemplates.Spawns.ContainsKey(templateName))
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::SpawnTemplate: Template found templateName=>" + templateName);

                // Create new SpawnShip
                DSC_SpawnShip spawnObject = new DSC_SpawnShip(Storage.SpawnId++, templateName, prefabName);

                // Spawn
                SpawnQueue.Add(spawnObject);
                return spawnObject.Id;
            }
            else
            {
                // Template not found
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::SpawnTemplate: Unkown templateName=>" + templateName);
                return 0;
            }
        }
        

        private void CreateSpawn(DSC_SpawnShip spawnShip)
        {

            // Get Template
            DSC_SpawnTemplate template = DeepSpaceCombat.Instance.SpawnTemplates.Spawns[spawnShip.SpawnTemplateName];

            // Check template
            if (!template.Routes.ContainsKey(0))
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::CreateSpawn: No Route entry for start [0]");
                return;
            }

            // Check player
            long spawnPlayerId = DeepSpaceCombat.Instance.EnemyPlayerID;
            if (template.Friendly)
                spawnPlayerId = DeepSpaceCombat.Instance.NPCPlayerID;


            // Get nearest planet
            MyPlanet planet = MyGamePruningStructure.GetClosestPlanet(template.Routes[0].Target);

            if (null == planet)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::CreateSpawn: No nearby planet found -> TODO");

                // Space spawn
                //DSC_SpawnShip spawnObject = new DSC_SpawnShip(Storage.SpawnId++, template.Name);

                // Add Spawned Ship to cache
                //SpawnCache.Add("DSC_SpawnManager_" + spawnObject.Id.ToString(), spawnObject);

                //MyVisualScriptLogicProvider.SpawnPrefab(prefabName, template.Routes[0].Target, dir, Vector3D.Up, spawnPlayerId, null, "DSC_SpawnManager_" + spawnObject.Id.ToString());
            }
            else
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::CreateSpawn: Nearby planet found -> SpawnPrefabInGravity");

                // Get center vector of nearest planet and calculate up vector
                Vector3D up = Vector3D.Normalize(planet.PositionComp.WorldVolume.Center - template.Routes[0].Target);

                // Calculate direction vector crossing up vector
                Vector3D dir = template.Routes[0].Target.Cross(planet.PositionComp.WorldVolume.Center);

                // Spawn Ship
                MyVisualScriptLogicProvider.SpawnPrefabInGravity(spawnShip.PrefabName, template.Routes[0].Target, dir, spawnPlayerId);
            }
        }



        #region events
        private void PrefabDetailed_Event(long entityId, string prefabName)
        {

            if(null == SpawnCache)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("SpawnManager::PrefabDetailed_Event: Spawn Cache null");
                return;
            }

            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("SpawnManager::PrefabDetailed_Event: Ship spawned=> -" + prefabName+ "- | SpawnCache.PrefabName=> -" + SpawnCache.PrefabName+"-");

            // Check if the prefabName is the same as in cache
            if (!SpawnCache.PrefabName.Equals(prefabName))
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("SpawnManager::PrefabDetailed_Event: Unknown grid spawned=>" + prefabName);
                return;
            }

            // Set the entity id
            SpawnCache.GridEntityId = entityId;

            // Add the ship to the storage
            Storage.SpawnedData.Add(SpawnCache.Id, SpawnCache);

            // Empty cache
            SpawnCache = null;
            SpawnCacheTimer = DateTime.MinValue;
        }

        #endregion

    }


    [ProtoContract]
    [Serializable]
    public class DSC_SpawnShip
    {
        [ProtoMember(1)]
        public readonly int Id;
        [ProtoMember(2)]
        public readonly string SpawnTemplateName;
        [ProtoMember(3)]
        public readonly string PrefabName;
        [ProtoMember(4)]
        public bool init { get; } = false;
        [ProtoMember(5)]
        public long GridEntityId;
        [ProtoMember(6)]
        public int ActiveRoute;
        [ProtoMember(7)]
        public readonly DateTime CreationTime;

        public DSC_SpawnShip(){}

        public DSC_SpawnShip(int id, string spawnTemplateName, string prefabName) {
            Id = id;
            SpawnTemplateName = spawnTemplateName;
            CreationTime = DateTime.UtcNow;
            PrefabName = prefabName;
        }
    }
}
