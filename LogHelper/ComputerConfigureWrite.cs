using LogHelper.Utill;
using LogHelper.XMLConfigurator;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;

namespace LogHelper
{
    internal class ComputerConfigureWrite
    {
        private ILog _log;
        private readonly string _computerConfigureLogChannel = "ComputerConfigure.txt";
        public ComputerConfigureWrite(ILog log) => this._log = log;
        public void Write(string rootDirectoryPath)
        {
            string directoryName = DateTime.Now.ToString("yyyy-MM-dd");
            if (IOUtil.Exists(rootDirectoryPath, directoryName, this._computerConfigureLogChannel))
                return;
            try
            {
                this._log.Write(this._computerConfigureLogChannel, "Program Version : {0}", (object)Configure.Instance.ProgramVersion);
                this._log.Write(this._computerConfigureLogChannel, "========================================DNSHost 정보========================================");
                this.FindDNSHostName();
                this._log.Write(this._computerConfigureLogChannel, "========================================문화권 정보========================================");
                this._log.Write(this._computerConfigureLogChannel, "System.Globalization.CultureInfo.CurrentCulture:{0}", (object)CultureInfo.CurrentCulture);
                this._log.Write(this._computerConfigureLogChannel, "========================================OS 정보========================================");
                this._log.Write(this._computerConfigureLogChannel, "System.Environment.OSVersion:{0}", (object)Environment.OSVersion);
                this._log.Write(this._computerConfigureLogChannel, "System.Environment.OSVersion.Platform:{0}", (object)Environment.OSVersion.Platform);
                this._log.Write(this._computerConfigureLogChannel, "System.Environment.OSVersion.ServicePack:{0}", (object)Environment.OSVersion.ServicePack);
                this._log.Write(this._computerConfigureLogChannel, "System.Environment.OSVersion.Version:{0}", (object)Environment.OSVersion.Version);
                this._log.Write(this._computerConfigureLogChannel, "System.Environment.OSVersion.VersionString:{0}", (object)Environment.OSVersion.VersionString);
            }
            catch (Exception)
            {
                this._log.Write(this._computerConfigureLogChannel, "Fail to Get ComputerConfigure");
            }
        }

        private void FindDNSHostName()
        {
            try
            {
                foreach(NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    this._log.Write(this._computerConfigureLogChannel, "NIC:{0}-{1}", (object)networkInterface.Name, (object)networkInterface.NetworkInterfaceType);
                    if(networkInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (IPAddress dnsAddress in networkInterface.GetIPProperties().DnsAddresses)
                            this._log.Write(this._computerConfigureLogChannel, "Dns Server ip:{0}", (object)dnsAddress.ToString());
                    }
                }
            }
            catch (Exception)
            {
                this._log.Write(this._computerConfigureLogChannel, "Fail to Get ComputerConfigure");                
            }
        }
    }
}
