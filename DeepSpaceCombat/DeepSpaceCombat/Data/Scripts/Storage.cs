using ProtoBuf;
using System;
using System.Collections.Generic;
using Sandbox.ModAPI;

namespace DeepSpaceCombat
{
    //Storage-Container for Raffi (untested)

    [ProtoContract]
    class Storage //: Dictionary<string,string>
    {
        [ProtoMember(1)]
        private string name;

        [ProtoMember(2)]
        private Dictionary<string,string> DicTable;

        #region constructors
        public Storage()
        {
            name = "DSC_Unnamed";
            DicTable = new Dictionary<string, string>();
        }
        public Storage(string pName)
        {
            name = pName;
            DicTable = new Dictionary<string,string>();
        }
        public Storage(string pName,Dictionary<string,string> pDicTable)
        {
            name = pName;
            DicTable = pDicTable;
        }
        #endregion

        #region get_set
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //Quick check whether a key exists
        public bool Contains(string pVarName) { return DicTable.ContainsKey(pVarName); }

        //Get a String
        public string Get(string pVarName) { return DicTable.GetValueOrDefault(pVarName); }
        //Set a String
        public void Set(string pVarName,string pValue){DicTable[pVarName]=pValue;}

        //Get arbitary variable
        public T GetVar<T>(string pVarName)
        {
            string sValue = DicTable.GetValueOrDefault(pVarName);
            if (null == sValue) {return default(T);}
            return (T)Convert.ChangeType(sValue,typeof(T));
        }

        //Get arbitary variable
        //Includes option to check whether everything went smoothly
        public T GetVar<T>(string pVarName,out bool check)
        {
            string sValue = DicTable.GetValueOrDefault(pVarName);
            if (null == sValue)
            {
                check = false;
                return default(T);
            }
            T returnValue = (T)Convert.ChangeType(sValue, typeof(T));
            if(null== returnValue)
                check= false;
            check= true;
            return returnValue;
        }

        //Set arbitary variable
        public bool SetVar<T>(string pVarName, T pValue)
        {
            bool changeFlag = false;
            if (DicTable.ContainsKey(pVarName))
                changeFlag = true;
            DicTable[pVarName] = pValue.ToString();
            return changeFlag;
        }
        #endregion

        #region save_load
        //Save
        public void Save()
        {
            //String-Version
            //MyAPIGateway.Utilities.SetVariable(name, MyAPIGateway.Utilities.SerializeToXML(DicTable);
            //Table-Version
            MyAPIGateway.Utilities.SetVariable<Dictionary<string,string>>(name,DicTable);
        }

        //Load
        public bool Load()
        {
            Dictionary<string, string> AccTable;
            //String-Version
            string tableXML;
            MyAPIGateway.Utilities.GetVariable<string>(name, out tableXML);
            AccTable = MyAPIGateway.Utilities.SerializeFromXML<Dictionary<string, string>>(tableXML);
            if (null == AccTable)
                return false;
            else
            {
                DicTable = AccTable;
                return true;
            }
            //Table-Version
            //MyAPIGateway.Utilities.GetVariable<Dictionary<string, string>> (name, out DicTable);
        }

        //Static load
        public static Storage LoadData(string pName)
        {
            Storage ret = new Storage(pName);
            ret.Load();
            return ret;
        }
        #endregion
    }
}
