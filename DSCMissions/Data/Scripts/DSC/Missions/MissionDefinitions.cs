using System;
using System.Collections.Generic;
using System.Text;

namespace DSC.Missions
{

    public class DSC_Mission
    {
        protected int MissionId { get; set; }
        protected int DependsOn { get; set; }
        protected string MissionName { get; set; }
        protected string MissionDescription { get; set; }
        protected DSC_MissionReward MissionReward { get; set; }

        protected DSC_Mission() { }

    }


    // Search Missions
    public class DSC_MissionSearch:DSC_Mission
    {

        public DSC_MissionSearch(int missionId, int dependsOn, string missionName, string missionDescription, DSC_MissionReward missionReward)
        {
            this.MissionId = missionId;
            this.DependsOn = dependsOn;
        }

        public void Start()
        {

        }

    }


    // Destroy block missions
    public class DSC_MissionDestroy:DSC_Mission
    {

        public void Start()
        {

        }

    }


    // Deliver item mission
    public class DSC_MissionDeliverItem 
    { 
        public int MissionId { get; }
        public int DependsOn { get; }


        public DSC_MissionDeliverItem() { }

        public DSC_MissionDeliverItem(string name)
        {

        }

        public void Start()
        {

        }

    }


    // Deliver grid mission
    public class DSC_MissionDeliverGrid
    {
        public int MissionId { get; }
        public int DependsOn { get; }


        public DSC_MissionDeliverGrid() { }

        public DSC_MissionDeliverGrid(string name)
        {

        }

        public void Start()
        {

        }
    }


    // Defend block mission


    // Escort mission



    // Mission reward
    public class DSC_MissionReward
    {

    }
}
