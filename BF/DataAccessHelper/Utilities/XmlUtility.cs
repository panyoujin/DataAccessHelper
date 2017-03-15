using System;
using System.Xml;

namespace DataAccessHelper.Utilities
{
    /// <summary>
    /// XML 操作的工具
    /// </summary>
    public class XmlUtility
    {
        #region Get Xml Node/Attribute Value For Any Type
        /// <summary>
        /// Get the XmlNode 's innerText value for string
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string getNodeStringValue(XmlNode node)
        {
            if (node == null) return null;
            return node.InnerText;
        }
        /// <summary>
        /// get The Numeric Type of XmlNode
        /// </summary>
        /// <param name="node">Current XmlNode</param>
        /// <returns>Int32</returns>
        public static int getNodeIntValue(XmlNode node)
        {
            return getNodeIntValue(node, 0);
        }
        /// <summary>
        /// 返回节点的数字
        /// </summary>
        /// <param name="node"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int getNodeIntValue(XmlNode node, int defaultValue)
        {
            if (node == null) return defaultValue;
            int retValue;
            if (int.TryParse(node.InnerText, out retValue))
            {
                return retValue;
            }
            return defaultValue;
        }
        /// <summary>
        /// 获取节点的字节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNode getSubNode(XmlNode parent, string xPath)
        {
            return parent.SelectSingleNode(xPath);
        }
        /// <summary>
        /// 获取节点的多个字节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static XmlNodeList getSubNodeList(XmlNode parent, string xPath)
        {
            return parent.SelectNodes(xPath);
        }
        /// <summary>
        /// get the current node's double value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static double getNodedoubleValue(XmlNode node)
        {
            return getNodedoubleValue(node, 0.0f);
        }
        /// <summary>
        /// 返回节点的浮点值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double getNodedoubleValue(XmlNode node, double defaultValue)
        {
            if (node == null) return defaultValue;
            double retValue;
            if (double.TryParse(node.InnerText, out retValue))
            {
                return retValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// Get the XmlNode 's attribute  value for string
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string getNodeAttributeStringValue(XmlNode node, string attr)
        {
            return getNodeAttributeStringValue(node, attr, null);
        }
        /// <summary>
        /// 返回属性的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string getNodeAttributeStringValue(XmlNode node, string attr, string defaultValue)
        {
            if (node == null || node.Attributes[attr] == null)
            {
                //if (string.IsNullOrEmpty(defaultValue))
                //{
                //    throw new Exception(string.Format("Attribute {0} not exist!", attr));
                //}
                return defaultValue;
            }
            return node.Attributes[attr].Value;
        }
        /// <summary>
        /// get the node's attribute's int value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int getNodeAttributeIntValue(XmlNode node, string attr)
        {
            return getNodeAttributeIntValue(node, attr, 0);
        }
        /// <summary>
        /// get the node's attribute's boolean value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static bool getNodeAttributeBooleanValue(XmlNode node, string attr)
        {
            string attValue = getNodeAttributeStringValue(node, attr, string.Empty);
            return attValue.ToLower().Equals(Boolean.TrueString.ToLower());
        }
        /// <summary>
        /// get the node's attribute's boolean value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static bool getNodeAttributeBooleanValue(XmlNode node, string attr, bool defaultValue)
        {
            string attValue = getNodeAttributeStringValue(node, attr, defaultValue.ToString());
            return attValue.ToLower().Equals(Boolean.TrueString.ToLower());
        }
        /// <summary>
        /// 返回属性的数字
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int getNodeAttributeIntValue(XmlNode node, string attr, int defaultValue)
        {
            if (node.Attributes[attr] == null)
            {
                return defaultValue;
            }
            int retValue;
            if (int.TryParse(node.Attributes[attr].Value, out retValue))
            {
                return retValue;
            }
            return defaultValue;
        }
        /// <summary>
        /// Get current operation's XmlNode's attr Attribute;s foat value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static double getNodeAttributedoubleValue(XmlNode node, string attr)
        {
            return getNodeAttributedoubleValue(node, attr, 0.0F);
        }
        /// <summary>
        /// 返回属性的浮点值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double getNodeAttributedoubleValue(XmlNode node, string attr, double defaultValue)
        {
            if (node.Attributes[attr] == null)
            {
                return defaultValue;
            }
            double retValue;
            if (double.TryParse(node.Attributes[attr].Value, out retValue))
            {
                return retValue;
            }
            return defaultValue;
        }
        #endregion


        #region Create Object For Xml
        /// <summary>
        /// Create XmlNode
        /// </summary>
        /// <param name="parentNode">XmlNode's Parent</param>
        /// <param name="nodeName">XmlNode's Name</param>
        /// <param name="nodeValue">XmlNode;s InnerText</param>
        /// <returns></returns>
        public static XmlNode addChildNode(XmlNode parentNode, string nodeName, string nodeValue)
        {
            return addChildNode(parentNode, nodeName, nodeValue, string.Empty);
        }
        /// <summary>
        /// 添加字节点
        /// </summary>
        /// <param name="parentNode">欲添加字节点的父节点</param>
        /// <param name="nodeName">节点的名字</param>
        /// <param name="nodeValue">节点的值</param>
        /// <param name="nodeNameSpace">节点的命名空间</param>
        /// <returns>添加的节点</returns>
        public static XmlNode addChildNode(XmlNode parentNode, string nodeName, string nodeValue, string nodeNameSpace)
        {
            return addChildNode(parentNode, nodeName, nodeValue, nodeNameSpace, false);
        }
        /// <summary>
        /// 添加字节点
        /// </summary>
        /// <param name="parentNode">欲添加字节点的父节点</param>
        /// <param name="nodeName">节点的名字</param>
        /// <param name="innerXml">节点的值</param>
        /// <param name="nodeNameSpace">节点的命名空间</param>
        /// <param name="setToInnerXml">把innerXml作为内部的XML</param>
        /// <returns>添加的节点</returns>
        public static XmlNode addChildNode(XmlNode parentNode, string nodeName, string innerXml, string nodeNameSpace, bool setToInnerXml)
        {
            XmlNode node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, nodeName, nodeNameSpace);
            if (setToInnerXml)
            {
                node.InnerXml = innerXml;
            }
            else
            {
                node.InnerText = innerXml;
            }
            parentNode.AppendChild(node);
            return node;
        }
        /// <summary>
        /// 添加字节点
        /// </summary>
        /// <param name="parentNode">欲添加字节点的父节点</param>
        /// <param name="nodeName">节点的名字</param>
        /// <param name="innerXml">节点的值</param>
        /// <param name="setInnerTextOrInnerXml">把innerXml作为内部的XML</param>
        /// <returns>添加的节点</returns>
        public static XmlNode addChildNode(XmlNode parentNode, string nodeName, string innerXml, bool setInnerTextOrInnerXml)
        {
            return addChildNode(parentNode, nodeName, innerXml, string.Empty, setInnerTextOrInnerXml);
        }
        /// <summary>
        /// 拷贝节点
        /// </summary>
        /// <param name="parentNode">欲添加字节点的父节点</param>
        /// <param name="outXmlNode">外部节点</param>
        /// <param name="userOuterXml">直接当作XML节点</param>
        /// <returns>添加的节点</returns>
        public static XmlNode addChildNode(XmlNode parentNode, XmlNode outXmlNode, bool userOuterXml)
        {
            if (userOuterXml)
            {
                XmlNode node = parentNode.OwnerDocument.ImportNode(outXmlNode, true);
                parentNode.AppendChild(node);
                return node;
            }
            foreach (XmlNode child in outXmlNode.ChildNodes)
            {
                addChildNode(parentNode, child, true);
            }
            return parentNode;
        }
        /// <summary>
        /// Create Attribute for XmlNode
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="attName"></param>
        /// <param name="attValue"></param>
        /// <returns></returns>
        public static XmlAttribute addAttribute(XmlNode parentNode, string attName, string attValue)
        {
            if (parentNode == null)
            {
                return null;
            }
            XmlAttribute attr = parentNode.Attributes[attName];
            if (attr == null)
            {
                attr = parentNode.OwnerDocument.CreateAttribute(attName);
                parentNode.Attributes.Append(attr);
            }
            attr.Value = attValue;
            return attr;
        }
        /// <summary>
        /// 创建节点的属性
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="attName">属性名称</param>
        /// <param name="attValue">属性值</param>
        /// <param name="nodeNameSpace">属性命名空间</param>
        /// <returns>创建的属性</returns>
        public static XmlAttribute addAttribute(XmlNode parentNode, string attName, string attValue, string nodeNameSpace)
        {
            if (parentNode == null)
            {
                return null;
            }
            string n = parentNode.GetNamespaceOfPrefix(nodeNameSpace);
            XmlAttribute attr = parentNode.Attributes[attName];
            if (attr == null)
            {
                XmlNode at = parentNode.OwnerDocument.CreateNode(XmlNodeType.Attribute, attName, n);
                at.Value = attValue;
                parentNode.Attributes.SetNamedItem(at);

                return (XmlAttribute)at;
            }
            else
            {
                attr.Value = attValue;
                return attr;
            }
        }
        #endregion

        /// <summary>
        /// 创建命名空间，针对Excel文档的Xml文件
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlNamespaceManager createXmlNameSpace(XmlDocument xml)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("html", "http://www.w3.org/TR/REC-html40");
            return nsmgr;
        }
    }
}
