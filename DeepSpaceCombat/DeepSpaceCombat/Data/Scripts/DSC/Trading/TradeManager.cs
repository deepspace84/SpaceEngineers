using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<string, DSC_Config_Trade.Station> StationsCache = new Dictionary<string, DSC_Config_Trade.Station>();
        private Dictionary<string, DSC_Config_Trade.TradeType> TradeTypeCache = new Dictionary<string, DSC_Config_Trade.TradeType>();

        private DateTime LastCall;

        private Dictionary<string, TradeItem> TradeItemsCache = new Dictionary<string, TradeItem>();

        private struct TradeItem
        {

            public readonly MyDefinitionId ItemId;
            public readonly string TypeId;
            public readonly string SubTypeId;
            public readonly MyObjectBuilder_Base Builder;

            public TradeItem(MyDefinitionId itemId, string typeId, string subTypeId, MyObjectBuilder_Base builder)
            {
                ItemId = itemId;
                TypeId = typeId;
                SubTypeId = subTypeId;
                Builder = builder;
            }
        }

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
                    TradesBuy = new List<DSC_Storage_Trade.Trade>(),
                    TradesSell = new List<DSC_Storage_Trade.Trade>(),
                    TradeMalus = new Dictionary<long, float>()
                };
            }

            LoadConfig();

            // Reset all prices
            var allDefs = MyDefinitionManager.Static.GetAllDefinitions();
            foreach (var componenet in allDefs.OfType<MyPhysicalItemDefinition>())
                componenet.MinimalPricePerUnit = 1;


            // Load store items
            LoadStoreItems();

            // Load all stations
            LoadTradeStations();

            // Set last call
            LastCall = DateTime.Now.AddMinutes(-60);

            // Set Npc money
            SetNpcMoney();
        }

        public void Save()
        {
            // Save Storage
            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary<DSC_Storage_Trade>(Storage);
            System.IO.BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage("DSC_Storage_Trade", typeof(DSC_Storage_Trade));
            writer.Write(serialized);
            writer.Flush();
            writer.Dispose();

        }

        public void LoadConfig()
        {
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
                    Types = new List<DSC_Config_Trade.TradeType>(),
                    Treshold = 3600,
                    Malus = 1,
                };

                var xmlData = MyAPIGateway.Utilities.SerializeToXML<DSC_Config_Trade>(Config);
                System.IO.TextWriter writerConfig = MyAPIGateway.Utilities.WriteFileInWorldStorage("DSC_Config_Trade", typeof(DSC_Config_Trade));
                writerConfig.Write(xmlData);
                writerConfig.Flush();
                writerConfig.Close();
            }
        }

        public void Unload()
        {

        }

        public void LoadStoreItems()
        {
            foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
            {

                switch (def.Id.TypeId.ToString())
                {
                    
                    case "MyObjectBuilder_Component":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_Component() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    case "MyObjectBuilder_AmmoMagazine":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_AmmoMagazine() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    case "MyObjectBuilder_Ingot":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_Ingot() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    case "MyObjectBuilder_Ore":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_Ore() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    case "MyObjectBuilder_PhysicalObject":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_PhysicalObject() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    case "MyObjectBuilder_ConsumableItem":
                        TradeItemsCache.Add(def.Id.ToString(), new TradeItem(def.Id, def.Id.TypeId.ToString(), def.Id.SubtypeId.ToString(), new MyObjectBuilder_ConsumableItem() { SubtypeName = def.Id.SubtypeId.ToString() }));
                        break;
                    default:
                        continue;
                }
            }
        }

        public void SetNpcMoney()
        {
            if(DeepSpaceCombat.Instance.NPCPlayerObj != null)
            {
                long balance = 0;
                if (DeepSpaceCombat.Instance.NPCPlayerObj.TryGetBalanceInfo(out balance))
                {
                    long amount = 1000000000 - balance;
                    DeepSpaceCombat.Instance.NPCPlayerObj.RequestChangeBalance(amount);
                }
            }
            else
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("NPCPlayerObj not set");
            }
        }

        private void CalculateItemMinimalPrice(MyDefinitionId itemId, float baseCostProductionSpeedMultiplier, ref int minimalPrice)
        {
            MyPhysicalItemDefinition definition = null;
            if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(itemId, out definition) && definition.MinimalPricePerUnit != -1)
            {
                minimalPrice += definition.MinimalPricePerUnit;
                return;
            }
            MyBlueprintDefinitionBase definition2 = null;
            if (!MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(itemId, out definition2))
            {
                return;
            }
            float num = (definition.IsIngot ? 1f : MyAPIGateway.Session.AssemblerEfficiencyMultiplier); // TODO
            int num2 = 0;
            MyBlueprintDefinitionBase.Item[] prerequisites = definition2.Prerequisites;
            for (int i = 0; i < prerequisites.Length; i++)
            {
                MyBlueprintDefinitionBase.Item item = prerequisites[i];
                int minimalPrice2 = 0;
                CalculateItemMinimalPrice(item.Id, baseCostProductionSpeedMultiplier, ref minimalPrice2);
                float num3 = (float)item.Amount / num;
                num2 += (int)((float)minimalPrice2 * num3);
            }
            float num4 = (definition.IsIngot ? MyAPIGateway.Session.RefinerySpeedMultiplier : MyAPIGateway.Session.AssemblerEfficiencyMultiplier);// TODO
            for (int j = 0; j < definition2.Results.Length; j++)
            {
                MyBlueprintDefinitionBase.Item item2 = definition2.Results[j];
                if (item2.Id == itemId)
                {
                    float num5 = (float)item2.Amount;
                    if (num5 != 0f)
                    {
                        float num6 = 1f + (float)Math.Log(definition2.BaseProductionTimeInSeconds + 1f) * baseCostProductionSpeedMultiplier / num4;
                        minimalPrice += (int)((float)num2 * (1f / num5) * num6);
                        break;
                    }
                }
            }
        }

        private void LoadTradeStations()
        {
            // Loop through all needed station blocks
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
                            if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadTradeStations: Could not change ownership of block=>" + tradeStation.Name);
                        }

                        // Block is now available with: DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(tradestation.Name);
                        StationsCache.Add(tradeStation.Name, tradeStation);

                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::LoadTradeStations: Successfully added TradeStation=>" + tradeStation.Name);
                    }
                    else
                    {
                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadTradeStations: Could not find entity with id=>" + blockId.ToString());
                    }
                }
                else
                {
                    // Block could not be added
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteError("TradeManager::LoadTradeStations: Could not add/find block in Reference. Blockname=>" + tradeStation.Name + " | Error=>" + blockId.ToString());
                }

                // Directly reload all orders
                CheckTrades(true);
            }

            // Loop through all types
            foreach (DSC_Config_Trade.TradeType type in Config.Types)
            {
                TradeTypeCache.Add(type.Name, type);
            }
        }

        public void CheckTrades(bool force=false)
        {
            // Call all 60 minutes the default rebuild
            if((DateTime.Now-LastCall).TotalMinutes > 2 || force){ // TODO
                foreach(string stationName in StationsCache.Keys)
                {
                    AddTrades(stationName);
                }

                LastCall = DateTime.Now.AddMinutes(1);
            }
        }

        private void RemoveTrades(string name)
        {

            // get blockid
            long blockId = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(name);

            // Load block
            Sandbox.ModAPI.IMyStoreBlock storeBlock = MyAPIGateway.Entities.GetEntityById(blockId) as Sandbox.ModAPI.IMyStoreBlock;
            if (storeBlock != null)
            {
                // Remove all store items
                List<Sandbox.ModAPI.Ingame.MyStoreQueryItem> storeItems = new List<Sandbox.ModAPI.Ingame.MyStoreQueryItem>();
                storeBlock.GetPlayerStoreItems(storeItems);
                foreach (var item in storeItems)
                {
                    storeBlock.CancelStoreItem(item.Id);
                }

                // Remove all items from storage
                IMyInventory invent = storeBlock.GetInventory();
                invent.Clear();
            }
            else
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::RemoveOrders Could not find block=>" + name);
            }
        }

        private void AddTrades(string name)
        {
            try
            {
                // Remove all old orders for this block first
                RemoveTrades(name);

                // get blockid
                long blockId = DeepSpaceCombat.Instance.DSCReference.GetBlockWithName(name);
                if (blockId == 0) return;

                // Load block
                Sandbox.ModAPI.IMyStoreBlock storeBlock = MyAPIGateway.Entities.GetEntityById(blockId) as Sandbox.ModAPI.IMyStoreBlock;
                if (storeBlock != null)
                {
                    // Get Store Block inventory
                    IMyInventory invent = storeBlock.GetInventory();

                    // Get station
                    if (StationsCache.ContainsKey(name))
                    {
                        if (TradeTypeCache.ContainsKey(StationsCache[name].Type))
                        {
                            // First check all trades and remove old ones
                            int timetreshold = (int)DateTime.Now.ToUnixTimestamp() - Config.Treshold;

                            for (int i = Storage.TradesBuy.Count - 1; i >= 0; i--)
                            {
                                if (Storage.TradesBuy[i].Utime <= timetreshold)
                                {
                                    Storage.TradesBuy.RemoveAt(i);
                                }
                            }

                            // Loop through items
                            foreach (DSC_Config_Trade.TradeType.TradeItem item in TradeTypeCache[StationsCache[name].Type].Items)
                            {
                                // Check if items exists
                                if (TradeItemsCache.ContainsKey(item.ItemName))
                                {
                                    /*
                                     * Price calculation item.Price
                                     * ------------------
                                     */
                                    //int offerPrice = item.Price;
                                    int orderPrice = item.Price;

                                    // Check for existing trades
                                    long preAmount = 0;
                                    foreach (DSC_Storage_Trade.Trade trade in Storage.TradesBuy)
                                    {
                                        if (trade.ItemName.Equals(item.ItemName))
                                        {
                                            preAmount += trade.Amount;
                                        }
                                    }


                                    if (preAmount > 0)
                                    {
                                        // Calculate malus
                                        float finalMalus = 1 + ((preAmount / item.Multiplier) * Config.Malus);
                                        orderPrice = (int)(orderPrice * (1f / finalMalus));
                                    }


                                    // Check min price for orders, because game blocks them
                                    int minPrice = 0;
                                    CalculateItemMinimalPrice(TradeItemsCache[item.ItemName].ItemId, 1, ref minPrice);
                                    if (orderPrice < minPrice)
                                    {
                                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddTrades Item price was to low =>" + item.ItemName + " -> " + orderPrice.ToString());
                                        orderPrice = minPrice;
                                    }

                                    /*
                                    try
                                    {
                                        // First item amount for selling
                                        invent.AddItems((MyFixedPoint)item.MaxAmount, (MyObjectBuilder_PhysicalObject)TradeItemsCache[item.ItemName].Builder);
                                    }
                                    catch (Exception e)
                                    {
                                        if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "DSC_Storage_Trade loading failed");
                                    }
                                    */

                                    MyStoreItemData storeItemOrder = new MyStoreItemData(TradeItemsCache[item.ItemName].ItemId, item.MaxAmount, orderPrice, (amount, left, totalPrice, sellerPlayerId, playerId) => BuyCallback(amount, left, totalPrice, sellerPlayerId, playerId, item.ItemName, name), null);
                                    //MyStoreItemData storeItemOffer = new MyStoreItemData(TradeItemsCache[item.ItemName].ItemId, item.MaxAmount, item.Price, (amount, left, totalPrice, sellerPlayerId, playerId) => SellCallback(amount, left, totalPrice, sellerPlayerId, playerId, item.ItemName, name), null);

                                    long storeItemId;
                                    storeBlock.InsertOrder(storeItemOrder, out storeItemId);

                                    // We dont sell any more
                                    //storeBlock.InsertOffer(storeItemOffer, out storeItemId);
                                    //DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddTrades storeItem=>" + storeItemId.ToString());

                                }
                                else
                                {
                                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddTrades Item not in definitions =>" + item.ItemName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::AddTrades Could not find block=>" + name);
                };
            }
            catch (Exception e)
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "TradeManager::AddTrades failed");
            }
        }

        private void BuyCallback(int amount, int left, long totalPrice, long sellerPlayerId, long playerId, string itemName, string stationName)
        {
            try
            {
                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::BuyCallback called unknown=>" + sellerPlayerId.ToString() + " - " + itemName);

                // Add trade
                Storage.TradesBuy.Add(new DSC_Storage_Trade.Trade(itemName, amount, totalPrice, (int)DateTime.Now.ToUnixTimestamp()));

                // Set NPC Money
                SetNpcMoney();

                // Rebuild trade prices
                AddTrades(stationName);
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "TradeManager::BuyCallback failed");
            }


        }

        private void SellCallback(int amount, int left, long totalPrice, long sellerPlayerId, long playerId, string itemName, string stationName)
        {
            try 
            { 

                if (DeepSpaceCombat.Instance.isDebug) DeepSpaceCombat.Instance.ServerLogger.WriteInfo("TradeManager::SellCallback" + amount.ToString() + " - " + itemName);

                // Add trade
                Storage.TradesSell.Add(new DSC_Storage_Trade.Trade(itemName, amount, totalPrice, (int)DateTime.Now.ToUnixTimestamp()));


                // Rebuild trade prices
                AddTrades(stationName);
            }
            catch (Exception e)
            {
                DeepSpaceCombat.Instance.ServerLogger.WriteException(e, "TradeManager::SellCallback failed");
            }
        }

    }
}
