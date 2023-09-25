using LogHelper.XMLConfigurator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    public class XMLConfiguratorLoader
    {
        public static string ProgramVersion
        {
            set => Configure.Instance.ProgramVersion = value;
        }

        public static void Loader(string logHelperXMLConfigurePath) => Configure.Instance.XmlLoadByFile(logHelperXMLConfigurePath);

        public static void LoaderByXML(string xmlLogHelperConfigure) => Configure.Instance.XmlLoad(xmlLogHelperConfigure);
    }
}
