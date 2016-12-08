using BF.DataAccessHelper.Utilities;
using System;
using System.Xml;

namespace BF.DataAccessHelper.SQLAnalytical
{
    /// <summary>
    /// SQL 参数定义
    /// </summary>
    [Serializable]
    public class ParameterInfo
    {
        #region ctor..
        public ParameterInfo(XmlNode node)
        {
            Parse(node);
        }

        #endregion

        #region Property Define
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 参数的值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 参数值类型
        /// </summary>
        public string ValueType
        {
            get;
            set;
        }
        #endregion

        #region method define
        private void Parse(XmlNode node)
        {
            this.Name = XmlUtility.getNodeAttributeStringValue(node, "name");
            this.Value = XmlUtility.getNodeAttributeStringValue(node, "value");
            this.ValueType = XmlUtility.getNodeAttributeStringValue(node, "type");
        }

        #endregion
    }
}
