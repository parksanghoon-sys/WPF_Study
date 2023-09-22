using LogHelper.Appender;
using LogHelper.Utill;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LogHelper.LogHelper.Appender
{
    internal class UdpAppender : AppenderBase
    {
        private Encoding _encoding = Encoding.Default;
        private UdpClient _client;
        private IPEndPoint _remoteEndPoint;
        ~UdpAppender() => this.DoDispose(false);
        public string Header { get; set; }
        public string HostAddress { get; set; }
        public int LocalPort { get; set; }
        public Encoding Encoder
        {
            get => this._encoding;
            set => _encoding = value;
        }
        private void Connection()
        {
            try
            {
                if (this.LocalPort == 0)
                    this._client = new UdpClient(IPAddress.Parse(this.HostAddress).AddressFamily);
                else
                    this._client = new UdpClient(this.LocalPort, IPAddress.Parse(this.HostAddress).AddressFamily);
            }catch(Exception ex)
            {
                this._client = (UdpClient)null;
                throw new Exception("Could not initialize the UdpClient connection on port : " + (object)this.LocalPort, ex);
            }

        }
        private void SendLog(string logMessage)
        {
            if (string.IsNullOrWhiteSpace(this.HostAddress) || this.LocalPort <= 0)
                throw new AggregateException("The required property 'HostAddress' and 'LocalPort' was not specified.");
            this.Connection();
            try
            {
                byte[] bytes = this._encoding.GetBytes(logMessage.ToCharArray());
                if (this._remoteEndPoint == null)
                {
                    this._remoteEndPoint = new IPEndPoint(IPAddress.Parse(this.HostAddress), this.LocalPort);
                    this._client.Send(bytes, bytes.Length, this._remoteEndPoint);
                }
                else
                    this._client.Send(bytes, bytes.Length, this._remoteEndPoint);
                this._client.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to send log message to remote host : " + this.HostAddress + "on port : " + (object)this.LocalPort, ex);
            }
        }
        public override void Init()
        {
        }

        public override void Error(string channel, Exception ex)
        {
            this.SendLog(ex.Message);
            this.SendLog(ex.StackTrace);
        }

        public override void Write(string channel, string strFormat, params object[] listObj) => this.SendLog(LogHelperUtil.GetMessage(true, this.Header, strFormat, listObj));

        public override void Debug(string channel, string strFormat, params object[] listObj) => this.SendLog(LogHelperUtil.GetMessage(true, this.Header, strFormat, listObj));

        protected override void DoDispose(bool disposing) => base.DoDispose(disposing);
    }
}
