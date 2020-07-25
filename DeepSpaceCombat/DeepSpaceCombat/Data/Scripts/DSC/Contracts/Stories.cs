using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public static class DSC_StoryTemplates
    {
        static public Dictionary<string, Dictionary<int, DSC_ContractTemplate>> Templates = new Dictionary<string, Dictionary<int, DSC_ContractTemplate>>()
        {
            {"Teststory", new Dictionary<int, DSC_ContractTemplate>(){
                { 1, DSC_ContractTemplates.Templates["Testtemplate"] },
                { 1, DSC_ContractTemplates.Templates["Testtemplate2"] },
            }}
        };

    }

    public static class DSC_ContractTemplates {

        static public Dictionary<string, DSC_ContractTemplate> Templates = new Dictionary<string, DSC_ContractTemplate>(){
            {"Testtemplate", new DSC_ContractTemplateSearch() },
            {"Testtemplate2", new DSC_ContractTemplateSearch() }
        };

    }
}
