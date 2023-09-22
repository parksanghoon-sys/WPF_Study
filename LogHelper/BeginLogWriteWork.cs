using System.Collections.Concurrent;

namespace LogHelper
{
    public sealed class BeginLogWriteWork : IDisposable
    {
        private BlockingCollection<Action> _logWriteActionCollection = new BlockingCollection<Action> ();
        private Thread _workerThread;
        private bool _isDisposed;
        public BeginLogWriteWork()
        {
            this._logWriteActionCollection = new BlockingCollection<Action>();
            this._workerThread = new Thread(new ThreadStart(this.DoWork))
            {
                Name = "LogWriteThread",
                IsBackground = true 
            };
            this._workerThread.Start();
        }

        ~BeginLogWriteWork() => this.DoDispose(false);
        public BlockingCollection<Action> LogWriteActionCollection => this._logWriteActionCollection;
        public void AddWorkToLogWrite(Action action)
        {
            try
            {
                this._logWriteActionCollection.Add(action);
            }
            catch (Exception ex)
            {
                throw new Exception("BeginAddWork Exception", ex);
            }
        }
        private void DoWork()
        {
            while(!_isDisposed)
            {
                try
                {
                    Action? action;
                    if(this._logWriteActionCollection!.TryTake(out action, -1)) 
                    {
                        action();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("BeginLogWriteWrok Exception", ex);
                }
                this._logWriteActionCollection.Dispose();
                this._logWriteActionCollection = (BlockingCollection<Action>)null;
            }
        }
        public void DoDispose(bool v)
        {
            this._isDisposed = true;
            if (_isDisposed == false)
                return;
            this._logWriteActionCollection.CompleteAdding();
            this._workerThread = (Thread)null;
        }
        public void Dispose()
        {
            this.DoDispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}
