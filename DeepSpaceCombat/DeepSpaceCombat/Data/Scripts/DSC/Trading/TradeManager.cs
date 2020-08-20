using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Scripting;
using VRageMath;

namespace DSC
{
    public class DSC_TradeManager
    {

        public DSC_TradeManager() { }

        public DSC_Storage_Trade Storage;
        public DSC_Config_Trade Config;

        private Dictionary<string, List<long>> TradeStationsPlayers = new Dictionary<string, List<long>>();
        private Dictionary<string, string> TradeStationTypes = new Dictionary<string, string>();

        public void Load()
        {
            // Internal Storage
            // Check if file exists
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Storage_Trade", typeof(DSC_Storage_Trade)))
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadBinaryFileInWorldStorage("DSC_Storage_Trade", typeof(DSC_Storage_Trade));
                    Storage = MyAPIGateway.Utilities.SerializeFromBinary<DSC_Storage_Trade>(reader.ReadBytes((int)reader.BaseStream.Length));
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Storage_Trade found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Trade loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Storage_Trade found, create default");
                // Create default values
                Storage = new DSC_Storage_Trade
                {
                    Trades = new Dictionary<long, List<string>>(),
                    TradeMalus = new Dictionary<long, float>()
                };
            }

            // Load config xml
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage("DSC_Config_Trade", typeof(DSC_Config_Trade)))
            {
                try
                {
                    System.IO.TextReader reader = MyAPIGateway.Utilities.ReadFileInWorldStorage("DSC_Config_Trade", typeof(DSC_Config_Trade));
                    var xmlData = reader.ReadToEnd();
                    Config = MyAPIGateway.Utilities.SerializeFromXML<DSC_Config_Trade>(xmlData);
                    reader.Dispose();
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("DSC_Config_Trade found and loaded");
                }
                catch (Exception e)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Config_Trade loading failed");
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("No DSC_Config_Trade found, create default");
                // Create default values
                Config = new DSC_Config_Trade
                {
                    Stations = new List<DSC_Config_Trade.Station>(),
                    Types = new List<DSC_Config_Trade.TradeType>()
                };
            }

            // Load all stations
            LoadTradeStations();

            // Give NPC's enough money
            MyAPIGateway.Players.RequestChangeBalance(DeepSpaceCombat.Instance.NPCPlayerID, 999999999);
            MyAPIGateway.Players.RequestChangeBalance(DeepSpaceCombat.Instance.EnemyPlayerID, 999999999);

            // Register Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered += Event_Area_Trade_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left += Event_Area_Trade_Left;

        }

        public void Save()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Trade>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_Trade", typeof(DSC_Storage_Trade));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();

            // Save Config
            var xmlData = MyAPIGateway.Utilities.SerializeToXML<DSC_Config_Trade>(Config);
            System.IO.TextWriter writerConfig = MyAPIGateway.Utilities.WriteFileInWorldStorage("DSC_Config_Trade", typeof(DSC_Config_Trade));
            writerConfig.Write(xmlData);
            writerConfig.Flush();
            writerConfig.Close();
        }

        public void Unload()
        {
            // Remove Area handlers
            MyVisualScriptLogicProvider.AreaTrigger_Entered -= Event_Area_Trade_Entered;
            MyVisualScriptLogicProvider.AreaTrigger_Left -= Event_Area_Trade_Left;
        }

        private void LoadTradeStations()
        {
            // Loop through all needed blocks
            foreach (DSC_Config_Trade.Station tradeStation in Config.Stations)
            {
                // Add block to the global storage
                long blockId = DeepSpaceCombat.Instance.DSCReference.AddBlockWithName(tradeStation.Name);
                if (blockId > 0)
                {
                    // Get Block Position
                    IMyEntity blockEntity;
                    if (MyAPIGateway.Entities.TryGetEntityById(blockId, out blockEntity))
                    {
                        // Change owner to our npc
                        MyCubeBlock block = MyAPIGateway.Entities.GetEntityById(blockId) as MyCubeBlock;
                        if (null != block)
                        {
                            block.ChangeOwner(DeepSpaceCombat.Instance.NPCPlayerID, MyOwnershipShareModeEnum.All);
                        }
                        else
                        {
                            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadtTradeStations: Could not change ownership of block=>" + tradeStation.Name);
                        }

                        // Because we cant remove triggers at unload any longer, we have to be sure that no old is active, so delete it
                        for (int i = 0; i < 10; i++)
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager:: Removed areas =>" + tradeStation.Name);
                            MyVisualScriptLogicProvider.RemoveTrigger(tradeStation.Name);

                        }

                        // Create area
                        MyVisualScriptLogicProvider.CreateAreaTriggerOnPosition(blockEntity.GetPosition(), 5, tradeStation.Name);

                        // Add to reference with empty user list
                        TradeStationsPlayers.Add(tradeStation.Name, new List<long>());
                        TradeStationTypes.Add(tradeStation.Name, tradeStation.Type);

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::LoadtTradeStations: Successfully added TradeStation=>" + tradeStation.Name);
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadResearchStations: Could not find entity with id=>" + blockId.ToString());
                    }
                }
                else
                {
                    // Block could not be added
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadtTradeStations: Could not add/find block in Reference. Blockname=>" + tradeStation.Name + " | Error=>" + blockId.ToString());
                }
            }
        }

        private void Event_Area_Trade_Entered(string name, long playerId)
        {
            

            // Check if player is in an active faction
            if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Check if this area is for trade
            if (TradeStationsPlayers.ContainsKey(name))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::Event_Area_Trade_Entered name =>" + name + " | player=>" + playerId);

                // Check if someone is in the area
                if (TradeStationsPlayers[name].Count > 0)
                {
                    // Check if all from the same faction or its a retrigger
                    bool factionCheck = true;
                    foreach (long playerIdCheck in TradeStationsPlayers[name])
                    {
                        if (playerId == playerIdCheck)
                        {
                            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::Event_Area_Trade_Entered Retrigger name =>" + name + " | player=>" + playerId);
                            return;
                        }

                        if (DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId] != DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerIdCheck])
                        {
                            // Not from the same ally
                            factionCheck = false;
                            break;
                        }
                    }

                    if (!factionCheck)
                    {
                        // Someone else joined the area, so delete all orders
                        RemoveOrders(name);
                    }

                    // Add player to list
                    TradeStationsPlayers[name].Add(playerId);
                }
                else
                {
                    // Noone is in the area, so build the orders for this ally
                    AddOrders(name, DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId]);

                    // Add player to list
                    TradeStationsPlayers[name].Add(playerId);
                }
            }
        }

        private void Event_Area_Trade_Left(string name, long playerId)
        {
            

            // Check if player is in an active faction
            if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Check if this area is for research
            if (TradeStationsPlayers.ContainsKey(name))
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::Event_Area_Trade_Left name =>" + name + " | player=>" + playerId);

                // Remove player and check new conditions
                TradeStationsPlayers[name].Remove(playerId);

                // Check if someone is left in the area
                if (TradeStationsPlayers[name].Count > 0)
                {
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::Event_Area_Trade_Left Still someone in here=>" + TradeStationsPlayers[name].Count.ToString());

                    // Check if all from the same faction
                    bool factionCheck = true;
                    long targetFaction = 0;
                    foreach (long playerIdCheck in TradeStationsPlayers[name])
                    {
                        // Set the faction id, if we fail, we dont use it
                        targetFaction = DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId];

                        foreach (long playerIdCheckAgain in TradeStationsPlayers[name])
                        {
                            if (DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId] != DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerIdCheckAgain])
                            {
                                // Not from the same ally
                                factionCheck = false;
                                break;
                            }
                        }
                    }

                    if (!factionCheck)
                    {
                        // Someone else joined the area, so delete all orders
                        RemoveOrders(name);
                    }
                    else
                    {
                        AddOrders(name, targetFaction);
                    }
                }
                else
                {
                    // Noone is in the area now so delete all orders
                    RemoveOrders(name);
                }
            }
        }

        private void RemoveOrders(string name)
        {
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::RemoveOrders for block =>" + name);

            // get blockid
            long blockId = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(name);

            // Load block
            IMyStoreBlock storeBlock = MyAPIGateway.Entities.GetEntityById(blockId) as IMyStoreBlock;
            if (storeBlock != null)
            {
                List<Sandbox.ModAPI.Ingame.MyStoreQueryItem> storeItems = new List<Sandbox.ModAPI.Ingame.MyStoreQueryItem>();
                storeBlock.GetPlayerStoreItems(storeItems);
                foreach (var item in storeItems)
                {
                    storeBlock.CancelStoreItem(item.Id);
                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::RemoveOrders BlockList Item =>" + item.Id.ToString());
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::RemoveOrders Could not find block=>" + name);
            }
        }

        private void AddOrders(string name, long factionId)
        {
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddOrders for block =>" + name + " | faction=>" + factionId);

            // get blockid
            long blockId = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(name);
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddOrders for blockId =>" + blockId.ToString());

            // Load block
            IMyStoreBlock storeBlock = MyAPIGateway.Entities.GetEntityById(blockId) as IMyStoreBlock;
            if (storeBlock != null)
            {
                // Get type and loop through all items
                foreach(DSC_Config_Trade.TradeType type in Config.Types)
                {
                    if(type.Name == TradeStationTypes[name])
                    {
                        // Loop through orders
                        foreach(DSC_Config_Trade.TradeType.TradeItem item in type.Items)
                        {
                            // Check if items exists
                            if (DeepSpaceCombat.Instance.Definitions.StoreItems.ContainsKey(item.ItemName))
                            {
                                // Prepare store item
                                MyStoreItemData storeItem = new MyStoreItemData(DeepSpaceCombat.Instance.Definitions.StoreItems[item.ItemName], item.Price, item.MaxAmount, BuyCallback, null);

                                long storeItemId;
                                storeBlock.InsertOrder(storeItem, out storeItemId);
                                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddOrders storeItem=>" + storeItemId.ToString());
                            }
                            else
                            {
                                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddOrders Item not in definitions =>" + item.ItemName);
                            }
                        }
                        
                        break;
                    }
                }
            }
            else
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddOrders Could not find block=>" + name);
            };
        }

        public float GetMalus(long factionId)
        {
            float malus = 1;

            // Calculate


            return malus;
        }

        private void BuyCallback(int amount, int left, long totalPrice, long sellerPlayerId, long playerId)
        {
            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::BuyCallback called unknown=>" + sellerPlayerId.ToString());

            // Check if player is in an active faction
            if (!DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction.ContainsKey(playerId)) return;

            // Get faction id
            long factionId = DeepSpaceCombat.Instance.Factions.Storage.PlayersToFaction[playerId];

            // Check if trade entry exists in storage
            if (!Storage.Trades.ContainsKey(factionId))
            {
                Storage.Trades.Add(factionId, new List<string>());
            }

            // Add trade
            Storage.Trades[factionId].Add(DateTime.Now.ToUnixTimestamp().ToString() + "_" + totalPrice.ToString());
            DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Added tarde=>" + DateTime.Now.ToUnixTimestamp().ToString() + "_" + totalPrice.ToString());

            // Recalculate factionMalus
            CalcMalus(factionId);

            // Rebuild trade prices
        }

        private void CalcMalus(long factionId)
        {
            try { 

                // Delete all trades older than one day
                int now = (int)DateTime.Now.ToUnixTimestamp();
                now = now - (60 * 60 * 24); // Minus 24h

                int totalSum = 0;

                foreach (string trade in Storage.Trades[factionId])
                {
                    string[] data = trade.Split('_');

                    DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Data =>" + data[0] + "-" + data[1]);


                    int dTime = Int32.Parse(data[0]);

                    if (now < dTime)
                    {
                        Storage.Trades[factionId].Remove(trade);
                    }
                    else
                    {
                        totalSum += Int32.Parse(data[1]);
                    }

                }

                // Now calculate TODO

                DeepSpaceCombat.Instance.ServerLogger.WriteInfo("Total Sum=>" + totalSum.ToString());
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Trade loading failed");
            }
        }

    }
}
