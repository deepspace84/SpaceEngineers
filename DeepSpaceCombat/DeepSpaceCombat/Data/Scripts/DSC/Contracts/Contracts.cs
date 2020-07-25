using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using Sandbox.Game.Multiplayer;
using Sandbox.Definitions;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Contracts;

namespace DSC
{
    public class DSC_Contracts
    {

        public void test()
        {

            
        }

    }

    public enum DSC_ContractType
    {
        DSC_ContractSearch = 1,
        DSC_ContractDeliver = 2,
        DSC_ContractDestroy = 3
    };

    public class DSC_Contract
    {

        internal Dictionary<string, string> Data = new Dictionary<string, string>();
        internal DSC_ContractTemplate Template;
        internal long PlayerId;

        public DSC_Contract(){ }

        public bool TCheck()
        {
            // Check template data
            if (!DSC_ContractTemplates.Templates.ContainsKey(Template.TemplateName))
                return false;

            return true;
        }

    }

    public class DSC_ContractSearch : DSC_Contract
    {

        public DSC_ContractSearch() {}

        public DSC_ContractSearch(DSC_ContractTemplateSearch template, long playerId, Dictionary<string, string> data)
        {
            Template = template;
            PlayerId = playerId;
            Data = data;
        }

        public void Check()
        {
            // Get child template
            DSC_ContractTemplateSearch ContractTemplate = (DSC_ContractTemplateSearch) Template;

            // Check if its loaded allready
            if (Data.ContainsKey("init"))
            {
                
            }
            else // Search init
            {
                



                // Set init value
                Data.Add("init", "true");
            }
        }

    }

    public class DSC_ContractDeliver : DSC_Contract
    {
        public DSC_ContractDeliver() { }

        public DSC_ContractDeliver(DSC_ContractTemplate template, Dictionary<string, string> data)
        {
            Template = template;
            Data = data;
        }


    }

    public class DSC_ContractDestroy : DSC_Contract
    {
        public DSC_ContractDestroy() { }

        public DSC_ContractDestroy(DSC_ContractTemplateDestroy template, Dictionary<string, string> data)
        {
            Template = template;
            Data = data;
        }


    }

}
