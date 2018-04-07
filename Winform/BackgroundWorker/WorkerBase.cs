using System.ComponentModel;

namespace  .Client.ValidatorBase.BackgroundWorkers
{
    /// <summary>
    /// a wrpper of background worker
    /// </summary>
    public abstract class WorkerBase
    {
        protected BackgroundWorker _worker ;

        protected WorkerBase()
        {
            _worker = new BackgroundWorker();
        }

        public void RunAsync()
        {
            _worker.RunWorkerAsync();
        }

        public bool Busy()
        {
            return _worker.IsBusy;
        }
    }
}
