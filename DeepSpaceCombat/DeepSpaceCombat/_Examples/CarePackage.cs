using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;


namespace Klime.CarePackage
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class CarePackage : MySessionComponentBase
    {
        private string gps_of_center = "GPS:Center:133930.86337546:148374.986331328:-158572.012539918:";
        private float max_radius_to_spawn = 20000f;
        private float min_drop_from_center = 9000f;
        private int min_online_players = 1;
        private int minimum_factions = 0;

        private int spawn_plane_after_seconds = 3600;
        private int plane_despawn_after_seconds = 300;
        private int drop_despawn_after_seconds = 18000;


        private int tick_timer;
        private int clock_timer;
        private int spawn_timer;
        private int plane_flying_timer;
        private int plane_lifetime_timer;
        private int drop_lifetime_timer;

        private Vector3D center = Vector3D.Zero;
        List<IMyPlayer> all_players = new List<IMyPlayer>();

        private List<string> drop_prefab_variants = new List<string>
        {
            "drop_cargoA",
            "drop_cargoB",
            "drop_cargoC"
        };
        private string chosen_drop_prefab = "";
        private string plane_prefab_name = "plane_cargo";


        private bool debugSpawn = false;
        private string debugDropName = "";

        List<Plane> flying_planes = new List<Plane>();
        List<Plane> rem_flying_planes = new List<Plane>();

        List<Drop> dropping_drops = new List<Drop>();
        List<Drop> rem_dropping_drops = new List<Drop>();

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed += PrefabDetailed;
            MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
        }

        public class Plane
        {
            public IMyCubeGrid flying_grid { get; set; }
            public int plane_cur_timer { get; set; }
            public bool has_dropped { get; set; }

            public Plane(IMyCubeGrid incoming_grid, int incoming_timer)
            {
                flying_grid = incoming_grid;
                plane_cur_timer = incoming_timer;
                has_dropped = false;
            }
        }

        public class Drop
        {
            public IMyCubeGrid dropping_grid { get; set; }
            public int drop_cur_timer { get; set; }

            public Drop(IMyCubeGrid incoming_grid, int incoming_timer)
            {
                dropping_grid = incoming_grid;
                drop_cur_timer = incoming_timer;
            }
        }

        private void Utilities_MessageEntered(string messageText, ref bool sendToOthers)
        {
            if (MyAPIGateway.Session.IsServer && messageText.StartsWith("/tlbdrop"))
            {
                debugSpawn = true;
                List<string> message = messageText.Split(' ').ToList();
                if (message.Count == 1)
                {
                    SpawnPlane();
                }
                if (message.Count == 2)
                {
                    debugDropName = message[1];
                    SpawnPlane();
                }
            }
        }

        public override void LoadData()
        {
            if (!MyAPIGateway.Utilities.GetVariable("gps_of_center", out gps_of_center))
            {
                MyAPIGateway.Utilities.SetVariable("gps_of_center", "PUTGPSHERE");
                gps_of_center = "GPS:Center:133930.86337546:148374.986331328:-158572.012539918:";
            }

            if (!MyAPIGateway.Utilities.GetVariable<float>("max_radius_to_spawn",out max_radius_to_spawn))
            {
                MyAPIGateway.Utilities.SetVariable<float>("max_radius_to_spawn", 20000f);
                max_radius_to_spawn = 20000f;
            }

            if (!MyAPIGateway.Utilities.GetVariable<float>("min_drop_from_center", out min_drop_from_center))
            {
                MyAPIGateway.Utilities.SetVariable<float>("min_drop_from_center", 9000f);
                min_drop_from_center = 9000f;
            }

            if (!MyAPIGateway.Utilities.GetVariable<int>("min_online_players", out min_online_players))
            {
                MyAPIGateway.Utilities.SetVariable<int>("min_online_players", 1);
                min_online_players = 1;
            }

            if (!MyAPIGateway.Utilities.GetVariable<int>("minimum_factions", out minimum_factions))
            {
                MyAPIGateway.Utilities.SetVariable<int>("minimum_factions", 0);
                minimum_factions = 1;
            }

            if (!MyAPIGateway.Utilities.GetVariable<int>("spawn_plane_after_seconds", out spawn_plane_after_seconds))
            {
                MyAPIGateway.Utilities.SetVariable<int>("spawn_after_seconds", 3600);
                spawn_plane_after_seconds = 3600;
            }

            if (!MyAPIGateway.Utilities.GetVariable<int>("plane_despawn_after_seconds", out plane_despawn_after_seconds))
            {
                MyAPIGateway.Utilities.SetVariable<int>("plane_despawn_after_seconds", 300);
                plane_despawn_after_seconds = 300;
            }

            if (!MyAPIGateway.Utilities.GetVariable<int>("drop_despawn_after_seconds", out drop_despawn_after_seconds))
            {
                MyAPIGateway.Utilities.SetVariable<int>("drop_despawn_after_seconds", 1800);
                drop_despawn_after_seconds = 1800;
            }

            if (!MyAPIGateway.Utilities.GetVariable<string>("plane_prefab_name",out plane_prefab_name))
            {
                MyAPIGateway.Utilities.SetVariable<string>("plane_prefab_name", "PLANE PREFAB NAME HERE");
                plane_prefab_name = "";
            }

            string drop_cargo_string = "";
            if (MyAPIGateway.Utilities.GetVariable<string>("drop_prefab_variants", out drop_cargo_string))
            {
                drop_prefab_variants = drop_cargo_string.Split(',').ToList();
            }
            else
            {
                MyAPIGateway.Utilities.SetVariable<string>("drop_prefab_variants", "ENTER CARGO NAMES HERE");
                drop_prefab_variants = new List<string>();
            }


        }

        public override void UpdateAfterSimulation()
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                tick_timer += 1;
                if (tick_timer%60 == 0)
                {
                    spawn_timer += 1;
                    if (spawn_timer >= spawn_plane_after_seconds)
                    {
                        if (CheckSpawnConditions())
                        {
                            spawn_timer = 0;
                            SpawnPlane();
                        }
                    }

                    rem_flying_planes.Clear();
                    rem_dropping_drops.Clear();

                    foreach (var the_plane in flying_planes)
                    {
                        if (!the_plane.flying_grid.MarkedForClose && the_plane.flying_grid.Physics != null)
                        {
                            the_plane.plane_cur_timer += 1;
                            the_plane.flying_grid.Physics.LinearVelocity = the_plane.flying_grid.WorldMatrix.Forward * 100;
                            if (the_plane.plane_cur_timer == plane_despawn_after_seconds)
                            {
                                the_plane.flying_grid.Close();
                                the_plane.flying_grid = null;
                                rem_flying_planes.Add(the_plane);
                            }
                            if (the_plane.flying_grid != null && (the_plane.flying_grid.WorldMatrix.Translation - center).Length() < min_drop_from_center)
                            {
                                if (!the_plane.has_dropped)
                                {
                                    SpawnDrop(the_plane.flying_grid);
                                    the_plane.has_dropped = true;
                                }

                            }
                        }
                        else
                        {
                            rem_flying_planes.Add(the_plane);
                        }
                    }
                    foreach (var rem_p in rem_flying_planes)
                    {
                        flying_planes.Remove(rem_p);
                    }


                    foreach (var falling_drop in dropping_drops)
                    {
                        if (!falling_drop.dropping_grid.MarkedForClose)
                        {
                            falling_drop.drop_cur_timer += 1;
                            if (falling_drop.drop_cur_timer == drop_despawn_after_seconds)
                            {
                                falling_drop.dropping_grid.Close();
                                rem_dropping_drops.Add(falling_drop);
                            }
                        }
                        else
                        {
                            rem_dropping_drops.Add(falling_drop);
                        }
                    }
                    foreach (var rem_falling_drop in rem_dropping_drops)
                    {
                        dropping_drops.Remove(rem_falling_drop);
                    }
                }
                foreach (var to_move in flying_planes)
                {
                    if (!to_move.flying_grid.MarkedForClose && to_move.flying_grid.Physics != null)
                    {
                        to_move.flying_grid.Physics.LinearVelocity = to_move.flying_grid.WorldMatrix.Forward * 100;
                    }
                }
            }
        }

        private bool CheckSpawnConditions()
        {
            bool ok = false;

            if (MyAPIGateway.Multiplayer.Players.Count >= min_online_players)
            {
                all_players.Clear();
                MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);
                List<string> current_factions = new List<string>();
                foreach (var player in all_players)
                {
                    if (!current_factions.Contains(MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId)))
                    {
                        current_factions.Add(MyVisualScriptLogicProvider.GetPlayersFactionTag(player.IdentityId));
                    }
                }
                if (current_factions.Count >= minimum_factions)
                {
                    ok = true;
                }
            }

            return ok;
        }

        private void SpawnDrop(IMyCubeGrid incoming_grid)
        {
            chosen_drop_prefab = drop_prefab_variants[MyUtils.GetRandomInt(0, drop_prefab_variants.Count - 1)];
            if (debugSpawn)
            {
                if (debugDropName != "")
                {
                    chosen_drop_prefab = debugDropName;
                    debugDropName = "";
                }
                debugSpawn = false;
            }
            Vector3D pos = incoming_grid.WorldMatrix.Translation + incoming_grid.WorldMatrix.Down * 20;
            Vector3D forw = incoming_grid.WorldMatrix.Forward;
            Vector3D up = incoming_grid.WorldMatrix.Up;

            MyVisualScriptLogicProvider.SpawnPrefab(chosen_drop_prefab, pos, forw, up);
            
        }



        private void  SpawnPlane()
        {            
            Vector3D spawnPosition;
            Vector3D spawnForward;
            Vector3D spawnUp;
            MyWaypointInfo tempInfo = MyWaypointInfo.Empty;
            MyPlanet planetToSpawn;
            if (MyWaypointInfo.TryParse(gps_of_center, out tempInfo))
            {
                center = tempInfo.Coords;
                planetToSpawn = MyGamePruningStructure.GetClosestPlanet(center);
                if (planetToSpawn != null)
                {
                    Vector3D upVectorCenter = Vector3D.Normalize(planetToSpawn.GetClosestSurfacePointGlobal(center) - planetToSpawn.PositionComp.GetPosition());
                    Vector3D randomParallel = MyUtils.GetRandomPerpendicularVector(ref upVectorCenter);
                    spawnPosition = planetToSpawn.GetClosestSurfacePointGlobal(center + randomParallel * max_radius_to_spawn);
                    Vector3D upVectorSpawn = Vector3D.Normalize(spawnPosition - planetToSpawn.PositionComp.GetPosition());
                    spawnPosition = spawnPosition + upVectorSpawn * 1500;
                    spawnForward = Vector3D.Normalize((center + upVectorCenter * 3000) - spawnPosition);
                    spawnUp = upVectorSpawn;
                    if (randomParallel != null)
                    {
                        MyVisualScriptLogicProvider.SpawnPrefab(plane_prefab_name, spawnPosition, spawnForward, spawnUp);
                    }
                }
            }
        }

        private void PrefabDetailed(long entityId, string prefabName)
        {
            if (prefabName == plane_prefab_name)
            {
                IMyCubeGrid plane_grid = MyAPIGateway.Entities.GetEntityById(entityId) as IMyCubeGrid;
                plane_grid.Save = false;
                plane_grid.Name = plane_grid.EntityId.ToString();
                MyEntities.SetEntityName((MyEntity)plane_grid,true);
                MyVisualScriptLogicProvider.SetGridDestructible(plane_grid.EntityId.ToString(), false);
                MyVisualScriptLogicProvider.SetGridEditable(plane_grid.EntityId.ToString(), false);
                Plane new_plane = new Plane(plane_grid, 0);
                flying_planes.Add(new_plane);             
            }
            if (prefabName == chosen_drop_prefab)
            {
                IMyCubeGrid drop_grid = MyAPIGateway.Entities.GetEntityById(entityId) as IMyCubeGrid;
                drop_grid.Name = drop_grid.EntityId.ToString();
                MyEntities.SetEntityName((MyEntity)drop_grid, true);
                MyVisualScriptLogicProvider.SetGridDestructible(drop_grid.EntityId.ToString(), false);
                MyVisualScriptLogicProvider.SetGridEditable(drop_grid.EntityId.ToString(), false);
                Drop new_drop = new Drop(drop_grid, 0);
                dropping_drops.Add(new_drop);               
            }
        }

        protected override void UnloadData()
        {
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= PrefabDetailed;
            MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
        }
    }
}