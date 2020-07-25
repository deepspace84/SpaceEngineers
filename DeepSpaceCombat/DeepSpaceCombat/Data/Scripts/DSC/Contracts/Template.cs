using System;
using System.Collections.Generic;
using System.Text;

namespace DSC
{
    public class DSC_ContractTemplate
    {

        public readonly DSC_ContractType ContractType;
        public readonly string TemplateName;
        public readonly bool PlayerNeeded;

        public readonly string Title;
        public readonly string Description;
        public readonly string FinishText;


        public DSC_ContractTemplate() { }

    }


    public class DSC_ContractTemplateSearch : DSC_ContractTemplate
    {


        // Search contract fields
        public readonly int SearchTargetType; // Grid = 1 | Vector = 2 | SpawnTemplate = 3
        public readonly string SearchTarget; // Gridname or Vector as "xxx.xxx:yyy.yyy:zzz.zzz" or SpawnTemplateName
        public readonly int SearchTargetRadius; // Radius in meters



        public DSC_ContractTemplateSearch() { }

    }

    public class DSC_ContractTemplateDeliver : DSC_ContractTemplate
    {


        public DSC_ContractTemplateDeliver() { }

    }


    public class DSC_ContractTemplateDestroy : DSC_ContractTemplate
    {


        public DSC_ContractTemplateDestroy() { }

    }

}
