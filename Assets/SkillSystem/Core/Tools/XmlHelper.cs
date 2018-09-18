using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TSSkill
{
    public sealed class XmlHelper
    {

        #region 变量
        private XmlDocument _currentDocument = null;

        public XmlDocument R_CurrentDocument { get { return _currentDocument; } private set { _currentDocument = value; } }

        private XmlElement _rootElement = null;

        public XmlElement R_RootElement { get { return _rootElement; } private set { _rootElement = value; } }

        private List<XmlNode> _childNodes = null;

        public List<XmlNode> R_ChildNodes { get { return _childNodes; } private set { _childNodes = value; } }

        private bool _ignoreComments = true;


        #endregion


        public XmlHelper CreateRoot(string rootName, Hashtable attributes = null)
        {
            R_RootElement = CreateElement(rootName, R_CurrentDocument, attributes);
            //R_CurrentDocument.AppendChild(R_RootElement);
            return this;
        }

        public XmlElement CreateElement(string name, XmlNode parent, Hashtable attributes = null)
        {
            XmlElement xml = R_CurrentDocument.CreateElement(name);
            if (attributes != null)
            {
                foreach (DictionaryEntry item in attributes)
                {
                    xml.SetAttribute(item.Key.ToString(), item.Value.ToString());
                }
            }
            parent.AppendChild(xml);
            return xml;
        }

        public XmlNode FindElement(string xPath, Hashtable attributes = null)
        {
            XmlNodeList nodeList = R_RootElement.SelectNodes(xPath);
            foreach (XmlNode item in nodeList)
            {
                if (item is XmlElement)
                {
                    if (attributes != null)
                    {
                        if (item.Attributes != null && item.Attributes.Count > 0)
                        {
                            bool isEquals = true;
                            foreach (DictionaryEntry attr in attributes)
                            {
                                XmlAttribute value = item.Attributes[attr.Key.ToString()];
                                if (value == null)
                                {
                                    isEquals = false;
                                    continue;
                                }
                                else if (value.Value != attr.Value.ToString())
                                {
                                    isEquals = false;
                                    continue;
                                }
                            }
                            if (isEquals)
                            {
                                return item;
                            }
                        }
                    }
                    else
                    {
                        return item;
                    }
                }

            }
            return null;
        }

        public List<XmlNode> FindElements(string xPath, Hashtable attributes = null)
        {
            XmlNodeList nodeList = R_RootElement.SelectNodes(xPath);
            List<XmlNode> tempList = new List<XmlNode>();
            foreach (XmlNode item in nodeList)
            {
                if (item is XmlElement)
                {
                    if (attributes != null)
                    {
                        if (item.Attributes != null && item.Attributes.Count > 0)
                        {
                            bool isEquals = true;
                            foreach (DictionaryEntry attr in attributes)
                            {
                                XmlAttribute value = item.Attributes[attr.Key.ToString()];
                                if (value == null)
                                {
                                    isEquals = false;
                                    continue;
                                }
                                else if (value.Value != attr.Value.ToString())
                                {
                                    isEquals = false;
                                    continue;
                                }
                            }
                            if (isEquals)
                            {
                                tempList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        tempList.Add(item);
                    }
                }

            }
            return tempList;
        }

        public List<XmlNode> GetElements(string xPath)
        {
            XmlNodeList nodeList = R_RootElement.SelectNodes(xPath);
            List<XmlNode> tempList = new List<XmlNode>();
            foreach (XmlNode item in nodeList)
            {
                if (item is XmlElement)
                {
                    tempList.Add(item);
                }
                else
                {
                    if (!_ignoreComments)
                    {
                        tempList.Add(item);
                    }
                }
            }
            return tempList;
        }

        public bool Remove(XmlNode node)
        {
            return node.ParentNode.RemoveChild(node) != null;
        }

        public bool Remove(string xPath, Hashtable attributes = null)
        {
            XmlNode node = FindElement(xPath, attributes);
            if (node == null)
            {
                return false;
            }
            return node.ParentNode.RemoveChild(node) != null;
        }

        public bool RemoveAll(string xPath, Hashtable attributes)
        {
            List<XmlNode> nodes = FindElements(xPath, attributes);
            foreach (var item in nodes)
            {
                item.ParentNode.RemoveChild(item);
            }
            return true;
        }

        public bool RemoveAll(string xPath)
        {
            List<XmlNode> nodes = GetElements(xPath);
            foreach (var item in nodes)
            {
                item.ParentNode.RemoveChild(item);
            }
            return true;
        }


        public XmlHelper Save(string path)
        {
            if (R_CurrentDocument != null)
            {
                R_CurrentDocument.Save(path);
            }
            return this;
        }

        #region 构造函数

        private XmlHelper()
        { }
        /// <summary>
        /// 加载XML，默认忽略注释
        /// </summary>
        /// <param name="xml"></param>
        public XmlHelper(string xml) : this(xml, true) { }
        /// <summary>
        /// 加载XML，默认忽略注释
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="ignoreComments"></param>
        public XmlHelper(string xml, bool ignoreComments)
        {
            R_ChildNodes = new List<XmlNode>();
            _ignoreComments = ignoreComments;
            if (string.IsNullOrEmpty(xml))
            {
                R_CurrentDocument = new XmlDocument();
                return;
            }
            R_CurrentDocument = new XmlDocument();
            R_CurrentDocument.LoadXml(xml);
            R_RootElement = R_CurrentDocument.DocumentElement;
            XmlNodeList nodes = R_RootElement.ChildNodes;
            foreach (XmlNode item in nodes)
            {
                if (item is XmlElement)
                {
                    R_ChildNodes.Add(item);
                }
                else
                {
                    if (!_ignoreComments)
                    {
                        R_ChildNodes.Add(item);
                    }
                }
            }
        }

        #endregion

    }
}
