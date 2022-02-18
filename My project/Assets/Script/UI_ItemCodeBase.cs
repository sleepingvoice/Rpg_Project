using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item_Code
{
    [System.Serializable]
    public class Data
    {
        public string Item_Code;
        public string name;
        public string explain;
        public string function;
        public string PrefabName;
    }

    [System.Serializable]
    public class Database
    {
        public List<Data> DataArray = new List<Data>();
    }
}
