namespace LogHelper.Appender
{
    public abstract class AppenderBase : ILog, IDisposable
    {
        private Lazy<BeginLogWriteWork> _lazyBeginLogWorker = new Lazy<BeginLogWriteWork>((Func<BeginLogWriteWork>)(() => new BeginLogWriteWork()));        
        ~AppenderBase() => this.DoDispose(false);
        public virtual void Init() => throw new Exception("상속받아 구현해야 합니다.");

        public int BeginTaskCount => this._lazyBeginLogWorker.IsValueCreated ? this._lazyBeginLogWorker.Value.LogWriteActionCollection.Count : 0;

        public virtual void Error(string channel, Exception ex) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void Error(Exception ex) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void Write(string channel, string strFormat, params object[] listObj) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void Write(string strFormat, params object[] listObj) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void Debug(string channel, string strFormat, params object[] listObj) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void Debug(string strFormat, params object[] listObj) => throw new Exception("상속받아 구현해야 합니다.");

        public virtual void BeginError(string channel, Exception ex) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Error(channel, ex)));

        public virtual void BeginError(Exception ex) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Error(ex)));

        public virtual void BeginWrite(string channel, string strFormat, params object[] listObj) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Write(channel, strFormat, listObj)));

        public virtual void BeginWrite(string strFormat, params object[] listObj) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Write(strFormat, listObj)));

        public virtual void BeginDebug(string channel, string strFormat, params object[] listObj) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Debug(channel, strFormat, listObj)));

        public virtual void BeginDebug(string strFormat, params object[] listObj) => this._lazyBeginLogWorker.Value.AddWorkToLogWrite((Action)(() => this.Debug(strFormat, listObj)));

        protected virtual void DoDispose(bool disposing)
        {
            if (!disposing || !this._lazyBeginLogWorker.IsValueCreated)
                return;
            this._lazyBeginLogWorker.Value.DoDispose(true);
        }
        public void Dispose()
        {
            this.DoDispose(true);
            GC.SuppressFinalize((object)this);
        }

    }
}