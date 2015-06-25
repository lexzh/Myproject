namespace AppServerParam
{
    using System;
    using System.IO;
    using System.Xml;

    public class XMLFile
    {
        private string string_0 = string.Empty;

        public XMLFile(string string_1)
        {
            this.FileName = string_1;
        }

        public string GetConfig(XmlNode xmlNode_0, string string_1)
        {
            string innerText = string.Empty;
            XmlElement element = (XmlElement) xmlNode_0.SelectSingleNode(string.Format("//{0}", string_1));
            if (element != null)
            {
                innerText = element.InnerText;
            }
            return innerText;
        }

        public string GetWebConfig(string string_1)
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.FileName);
            string attribute = string.Empty;
            XmlElement element = (XmlElement) document.SelectSingleNode("//appSettings").SelectSingleNode("//add[@key=\"" + string_1 + "\"]");
            if (element != null)
            {
                attribute = element.GetAttribute("value");
            }
            return attribute;
        }

        public void SetConfig(string string_1, string string_2)
        {
            XmlDocument document = new XmlDocument();
            if (!System.IO.File.Exists(this.FileName))
            {
                XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.AppendChild(newChild);
                XmlNode node = document.CreateElement("Data");
                XmlNode node2 = document.CreateElement("param");
                XmlNode node3 = document.CreateElement("isNeedPWDUpdate");
                XmlNode node4 = document.CreateElement("ModuleId");
                XmlNode node5 = document.CreateElement("IP1");
                XmlNode node6 = document.CreateElement("IP2");
                node5.InnerText = "";
                node6.InnerText = "0";
                node4.InnerText = "4096";
                node3.InnerText = "0";
                node2.AppendChild(node5);
                node2.AppendChild(node6);
                node2.AppendChild(node3);
                node2.AppendChild(node4);
                node.AppendChild(node2);
                document.AppendChild(node);
            }
            else
            {
                document.Load(this.FileName);
            }
            XmlNode node7 = document.SelectSingleNode("//param");
            if (node7 == null)
            {
                XmlNode node8 = document.CreateElement("param");
                document.AppendChild(node8);
                node7 = document.SelectSingleNode("//param");
            }
            XmlElement element = (XmlElement) node7.SelectSingleNode("//" + string_1);
            if (element != null)
            {
                element.InnerText = string_2;
            }
            else
            {
                XmlElement element2 = document.CreateElement(string_1);
                element2.InnerText = string_2;
                node7.AppendChild(element2);
            }
            document.Save(this.FileName);
        }

        public void SetWebConfigByKey(string string_1, string string_2)
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.FileName);
            XmlNode node = document.SelectSingleNode("//appSettings");
            XmlElement element = (XmlElement) node.SelectSingleNode("//add[@key='" + string_1 + "']");
            if (element != null)
            {
                element.SetAttribute("value", string_2);
            }
            else
            {
                XmlElement newChild = document.CreateElement("add");
                newChild.SetAttribute("key", string_1);
                newChild.SetAttribute("value", string_2);
                node.AppendChild(newChild);
            }
            document.Save(this.FileName);
        }

        public string FileName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

