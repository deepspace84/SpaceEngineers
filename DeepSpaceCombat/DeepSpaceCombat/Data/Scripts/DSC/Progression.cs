using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.Game.SessionComponents;

namespace DSC
{
    class DSC_Progress
    {

        public DSC_Progress() {


        }

        /*

        public void test()
        {

            // Init calls?!?
            MyVisualScriptLogicProvider.PlayerResearchClearAll();
            MyVisualScriptLogicProvider.ResearchListWhitelist(true);

            MySessionComponentResearch.Static.UnlockResearchDirect(p.Character.EntityId, MyVisualScriptLogicProvider.GetDefinitionId(names[1], names[2]));
            MySessionComponentResearch.Static.UnlockResearch

            MyDefinitionId id = MyVisualScriptLogicProvider.GetDefinitionId(names[1], names[2]);
            MyDefinitionId id = MyVisualScriptLogicProvider.GetDefinitionId(names[1], null);


            MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId,id);




            DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> defset = MyDefinitionManager.Static.GetAllDefinitions();
            Dictionary<MyDefinitionId, MyDefinitionBase>.ValueCollection.Enumerator enumerator = defset.GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                { MyVisualScriptLogicProvider.PlayerResearchUnlock(p.IdentityId, enumerator.Current.Id); }
                catch (Exception exin) { MyVisualScriptLogicProvider.SendChatMessage("Error: " + exin.Message + "ID: " + enumerator.Current.Id.ToString()); }
            }
            enumerator.Dispose();

            //MySessionComponentResearch.Static.LockResearch(p.Character.EntityId, MyVisualScriptLogicProvider.GetDefinitionId(names[1], names[2]));
            MyDefinitionId id = MyVisualScriptLogicProvider.GetDefinitionId(names[1], names[2]);
            //MyVisualScriptLogicProvider.PlayerResearchLock(p.IdentityId, MyVisualScriptLogicProvider.GetDefinitionId(names[1], names[2]));



            // Update
            MyVisualScriptLogicProvider.PlayerResearchClear(pid);
            MyVisualScriptLogicProvider.ClearAllToolbarSlots(pid);
            foreach (string defid in getAvailableResearch())
            {
                MyVisualScriptLogicProvider.SendChatMessage("Unlocked: " + defid);
                MyVisualScriptLogicProvider.PlayerResearchUnlock(pid, MyDefinitionId.Parse(defid));
            }
        }

    */



    }
}
