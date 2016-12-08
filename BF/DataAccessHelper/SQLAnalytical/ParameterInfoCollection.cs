using System;
using System.Collections.Generic;

namespace BF.DataAccessHelper.SQLAnalytical
{
    [Serializable]
    public class ParameterInfoCollection : List<ParameterInfo>
    {
        public ParameterInfo this[string name]
        {
            get
            {
                foreach (ParameterInfo info in this)
                {
                    if (string.Compare(info.Name, name, true) == 0) return info;
                }
                return null;
            }
        }
    }
}
