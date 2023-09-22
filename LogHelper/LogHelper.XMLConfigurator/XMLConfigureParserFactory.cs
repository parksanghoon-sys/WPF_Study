using LogHelper.Appender;
using System.Reflection;
using System.Xml;

namespace LogHelper.XMLConfigurator
{
    internal class XMLConfigureParserFactory
    {
        private const string _xmlRootNodeName = "LogHelper";
        private const string _nodeName = "appender";

        public Dictionary<string, AppenderBase> ExcuteParser(string strXML)
        {
            if(string.IsNullOrEmpty(strXML))
                return (new Dictionary<string, AppenderBase>());
            Dictionary<string,AppenderBase> dictionary = new Dictionary<string,AppenderBase>();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(strXML);
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("LogHelper/appender");
                if (xmlNodeList != null && xmlNodeList.Count > 0)
                {
                    foreach (XmlNode configurePropertyNode in xmlNodeList)
                    {
                        if(configurePropertyNode.Attributes.Count >= 2 && configurePropertyNode.Attributes["name"] != null &&
                            configurePropertyNode.Attributes["type"] != null)
                        {
                            string typeName = configurePropertyNode.Attributes["type"].Value;
                            Type? type = Type.GetType(typeName);
                            AppenderBase? appenderClass = !(type != (Type)null) ? Activator.CreateInstanceFrom(Assembly.GetEntryAssembly().CodeBase, typeName).Unwrap() as AppenderBase
                                : Activator.CreateInstance(type) as AppenderBase;
                            if (appenderClass == null)
                                throw new Exception("Configure xml Parsing Error");
                            this.SetProperyForAppender(ref appenderClass, configurePropertyNode);
                            dictionary.Add(configurePropertyNode.Attributes["name"].Value, appenderClass);
                        }
                    }
                }
                return dictionary;
            }
            catch 
            {
                throw new Exception("Configure xml parsing error");
            }
        }

        private void SetProperyForAppender(ref AppenderBase appenderClass, XmlNode configurePropertyNode)
        {
            if (configurePropertyNode == null)
                return;
            try
            {
                foreach (XmlNode childNode in configurePropertyNode.ChildNodes)
                {
                    PropertyInfo property = appenderClass.GetType().GetProperty(childNode.Name);
                    if (property != (PropertyInfo)null && property.CanWrite && childNode.Attributes.Count > 0 && childNode.Attributes[0].Name == "value")
                        property.SetValue((object)appenderClass, Convert.ChangeType((object)childNode.Attributes[0].Value, property.PropertyType));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Configure RollingFile xml parsing error - {0}", (object)ex.Message));
            }
        }
    }
}