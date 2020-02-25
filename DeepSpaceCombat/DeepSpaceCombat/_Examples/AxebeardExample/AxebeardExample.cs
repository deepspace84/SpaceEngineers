using System;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;


namespace Klime.AxebeardExample
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class AxebeardExample : MySessionComponentBase
    {
        //Set up our variables

        private string cargo_prefab_name = "contract_cargo";
        private IMySlimBlock contract_block;
        private int timer;
        private MyContractCustom new_contract = null;
        private List<CargoContract> active_contracts = new List<CargoContract>();
        private List<CargoContract> remove_contracts = new List<CargoContract>();

        private long current_contract_id = 0;
        private IMyPlayer current_player = null;



        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            if (MyAPIGateway.Session.IsServer) //We only want to run stuff serverside
            {
                MyAPIGateway.ContractSystem.CustomActivateContract += ContractSystem_CustomActivateContract;
                MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
                MyAPIGateway.Utilities.MessageRecieved += Utilities_MessageRecieved;
                MyVisualScriptLogicProvider.PrefabSpawnedDetailed += Detailed_Prefab;
            }
        }



        public class CargoContract //Class to hold our data
        {
            public long contract_id { get; set; }
            public IMyCubeGrid cargo_grid { get; set; }
            public Vector3D destination { get; set; }
            public IMyPlayer player { get; set; }

            public CargoContract(long incoming_contract_id, IMyCubeGrid incoming_grid, Vector3D incoming_destination, IMyPlayer incoming_player)
            {
                contract_id = incoming_contract_id;
                cargo_grid = incoming_grid;
                destination = incoming_destination;
                player = incoming_player;
            }
        }

        private void Utilities_MessageRecieved(ulong steamId, string message_text) //Chat message recieved by server
        {
            if (MyAPIGateway.Session.IsServer && message_text == "/add custom")
            {
                List<IMyPlayer> all_players = new List<IMyPlayer>();
                MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);
                IMyPlayer request_player = null;
                foreach (var player in all_players)
                {
                    if (player.SteamUserId == steamId)
                    {
                        request_player = player;
                        break;
                    }
                }
                if (request_player != null && request_player.Character != null)
                {
                    GetContractBlock(request_player);
                }
            }
        }

        private void Utilities_MessageEntered(string messageText, ref bool sendToOthers) //Chat message entered by server (i.e offline mode or local MP)
        {
            if (MyAPIGateway.Session.IsServer && messageText == "/add custom")
            {
                if (MyAPIGateway.Session.Player != null && MyAPIGateway.Session.Player.Character != null)
                {
                    GetContractBlock(MyAPIGateway.Session.Player);
                }
            }
        }

        private void GetContractBlock(IMyPlayer incoming_player) //Raycast to find contract block
        {
            MatrixD player_headmatrix = incoming_player.Character.GetHeadMatrix(true, true);
            Vector3D start_cast = player_headmatrix.Translation + player_headmatrix.Forward * 0.5;
            Vector3D end_cast = start_cast + player_headmatrix.Forward * 100;
            IHitInfo ray_hit;

            MyAPIGateway.Physics.CastRay(start_cast, end_cast, out ray_hit);
            if (ray_hit != null && ray_hit.HitEntity is IMyCubeGrid)
            {
                IMyCubeGrid hit_grid = ray_hit.HitEntity as IMyCubeGrid;
                if (hit_grid.Physics != null)
                {
                    Vector3I hit_local_pos = hit_grid.WorldToGridInteger(ray_hit.Position + player_headmatrix.Forward * 0.1);
                    IMySlimBlock hit_block = hit_grid.GetCubeBlock(hit_local_pos);
                    if (hit_block != null && hit_block.FatBlock != null && hit_block.FatBlock.BlockDefinition.SubtypeId.Contains("ContractBlock"))
                    {
                        contract_block = hit_block;
                    }
                }
            }
        }

        private void ContractSystem_CustomActivateContract(long contractId, long requester_id) // Activates when someone accepts the contract
        {
            List<IMyPlayer> all_players = new List<IMyPlayer>();
            MyAPIGateway.Multiplayer.Players.GetPlayers(all_players);
            IMyPlayer quest_player = null;
            foreach (var player in all_players)
            {
                if (player.IdentityId == requester_id)
                {
                    quest_player = player;
                    break;
                }
            }

            if (quest_player != null && quest_player.Character != null)
            {
                MatrixD player_worldmatrix = quest_player.Character.WorldMatrix;
                MyPlanet closest_planet = MyGamePruningStructure.GetClosestPlanet(player_worldmatrix.Translation);

                if (closest_planet != null)
                {
                    Vector3D search_point = player_worldmatrix.Translation + player_worldmatrix.Forward * 20 + player_worldmatrix.Right * 20;
                    Vector3D nearest_planet_surface_pos = closest_planet.GetClosestSurfacePointGlobal(search_point);
                    Vector3D up_from_planet = nearest_planet_surface_pos - closest_planet.PositionComp.GetPosition();
                    Vector3D final_spawn_pos = nearest_planet_surface_pos + Vector3D.Normalize(up_from_planet) * 10;

                    MyVisualScriptLogicProvider.SpawnPrefab(cargo_prefab_name, final_spawn_pos, player_worldmatrix.Forward, player_worldmatrix.Up, requester_id); //Spawn the grid on planet
                }
                else
                {
                    Vector3D final_spawn_pos = player_worldmatrix.Translation + player_worldmatrix.Forward * 20 + player_worldmatrix.Right * 20;
                    MyVisualScriptLogicProvider.SpawnPrefab(cargo_prefab_name, final_spawn_pos, player_worldmatrix.Forward, player_worldmatrix.Up, requester_id); //Spawn the grid in space
                }

                current_contract_id = contractId;
                current_player = quest_player;
            }
        }

        private void Detailed_Prefab(long entityId, string prefabName) //Tracks the spawned prefab
        {
            var grid = MyAPIGateway.Entities.GetEntityById(entityId) as IMyCubeGrid;
            if (grid.Physics != null && grid.CustomName == cargo_prefab_name && current_contract_id != 0 && current_player != null)
            {
                Vector3D destination = grid.WorldMatrix.Translation + MyUtils.GetRandomInt(20000, 25000) * grid.WorldMatrix.Forward + MyUtils.GetRandomInt(-10000, 10000) * grid.WorldMatrix.Right;

                MyPlanet destination_planet = MyGamePruningStructure.GetClosestPlanet(destination);
                if (destination_planet != null)
                {
                    destination = destination_planet.GetClosestSurfacePointGlobal(destination); //Gets a random destination point. Either in space, or planet surface
                }

                MyVisualScriptLogicProvider.AddGPS("Destination", current_contract_id.ToString(), destination, Color.Green, 0, current_player.IdentityId);
                CargoContract new_contract_to_add = new CargoContract(current_contract_id, grid, destination, current_player);
                active_contracts.Add(new_contract_to_add);
                current_contract_id = 0;
                current_player = null;
            }
        }

        public override void UpdateAfterSimulation()
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                timer += 1;
                if (contract_block != null && new_contract == null)
                {
                    MyDefinitionId def_id;
                    MyDefinitionId.TryParse("MyObjectBuilder_ContractTypeDefinition/CustomContract", out def_id);

                    long start_block_id = contract_block.FatBlock.EntityId;
                    int reward = 10000;
                    int collateral = 100;
                    int duration = 0;
                    string contract_name = "Cargo Hauling";
                    string contract_description = "Haul the container, using any means, to its destination";
                    new_contract = new Sandbox.ModAPI.Contracts.MyContractCustom(def_id, start_block_id, reward, collateral, duration, contract_name, contract_description,
                        0, 0,null);
                    MyAPIGateway.ContractSystem.AddContract(new_contract);
                    contract_block = null;
                    new_contract = null;
                }

                if (timer%60 == 0) //Only check once per second to reduce performance hit
                {
                    remove_contracts.Clear();
                    foreach (var contract in active_contracts)
                    {
                        if (contract.cargo_grid != null && !contract.cargo_grid.MarkedForClose) //If grid is destroyed, we fail immediately
                        {
                            if ((contract.cargo_grid.WorldMatrix.Translation - contract.destination).Length() <= 5) //If grid is within 5m of the destination, we finish the contract
                            {
                                MyAPIGateway.ContractSystem.TryFinishCustomContract(contract.contract_id);
                                contract.player.RequestChangeBalance(10000); //Give player the money
                                MyVisualScriptLogicProvider.RemoveGPS("Destination", contract.player.IdentityId);
                                contract.cargo_grid.Close(); //Remove the grid
                                remove_contracts.Add(contract);
                            }
                        }
                        else
                        {
                            MyAPIGateway.ContractSystem.TryFailCustomContract(contract.contract_id);
                            MyVisualScriptLogicProvider.RemoveGPS("Destination", contract.player.IdentityId);
                            remove_contracts.Add(contract);
                        }
                    }

                    foreach (var contract in remove_contracts)
                    {
                        active_contracts.Remove(contract);
                    }
                }
            }
        }


        protected override void UnloadData()
        {
            //Important to unreigster from events

            MyAPIGateway.ContractSystem.CustomActivateContract -= ContractSystem_CustomActivateContract;
            MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
            MyAPIGateway.Utilities.MessageRecieved -= Utilities_MessageRecieved;
            MyVisualScriptLogicProvider.PrefabSpawnedDetailed -= Detailed_Prefab;
        }
    }
}