using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
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
                    SpawnedData = new Dictionary<ulong, DSC_SpawnShip>(),
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
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::Check: SpawnCache over 2 seconds. Failed preFab=>" + SpawnCache.PrefabName);

                // Free spawncache
                SpawnCache = null;
                SpawnCacheTimer = DateTime.MinValue;
            }


            // Loop through all ships in cache


        }
        public ulong Spawn(DSC_SpawnShip spawnShip)
        {
            // Check if player exists
            if (null != spawnShip)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::Spawn added");

                // Set new spawnShipId
                spawnShip.Id = Storage.SpawnId++;

                // Spawn
                SpawnQueue.Add(spawnShip);
                return spawnShip.Id;
            }
            else
            {
                // Spawnship not defined
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::Spawn undefined spawnship");
                return 0;
            }
        }
        

        private void CreateSpawn(DSC_SpawnShip spawnShip)
        {

            // Check for gravity
            if (spawnShip.InGravity)
            {

                // Get nearest planet
                MyPlanet planet = MyGamePruningStructure.GetClosestPlanet(spawnShip.StartPosition);

                if (null == planet)
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::CreateSpawn: No nearby planet found -> TODO");
                }
                else
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("SpawnManager::CreateSpawn: Nearby planet found -> SpawnPrefabInGravity");

                    // Get center vector of nearest planet and calculate up vector
                    Vector3D up = Vector3D.Normalize(planet.PositionComp.WorldVolume.Center - spawnShip.StartPosition);

                    // Calculate direction vector if not set
                    if (spawnShip.StartDirection == Vector3D.Zero)
                    {
                        spawnShip.StartDirection = spawnShip.StartPosition.Cross(planet.PositionComp.WorldVolume.Center);
                    }

                    // Spawn Ship
                    MyVisualScriptLogicProvider.SpawnPrefabInGravity(spawnShip.PrefabName, spawnShip.StartPosition, spawnShip.StartDirection, spawnShip.PlayerId);
                }
            }
            else
            {
                // TODO

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
        public ulong Id;
        [ProtoMember(2)]
        public string PrefabName;
        [ProtoMember(3)]
        public long PlayerId;
        [ProtoMember(4)]
        public Vector3D StartPosition;
        [ProtoMember(5)]
        public Vector3D StartDirection;
        [ProtoMember(6)]
        public bool InGravity;
        [ProtoMember(7)]
        public bool init { get; } = false;
        [ProtoMember(8)]
        public long GridEntityId;
        [ProtoMember(9)]
        public DateTime CreationTime;

        public DSC_SpawnShip(){}

        public DSC_SpawnShip(long playerId, string prefabName, Vector3D startPosition, Vector3D startDirection = new Vector3D(), bool inGravity=false)
        {
            CreationTime = DateTime.UtcNow;
            PlayerId = playerId;
            PrefabName = prefabName;
            StartPosition = startPosition;
            StartDirection = startDirection;
            InGravity = inGravity;
        }
    }
}
