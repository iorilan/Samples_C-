using System;
using System.ComponentModel;
using System.Windows.Forms;
using log4net;
using NEC.ESBU.Ticketing.Client.ValidatorBase.CardCommands;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Config;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Helpers;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Models;
using NEC.ESBU.Ticketing.Objects;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.BackgroundWorkers
{
    public class UpdateUIWorker : WorkerBase
    {
        private ILog _log = LogManager.GetLogger(typeof(UpdateUIWorker));
        private int IDLE_SEC = ValidatorEnv.IdleTime;
        public UpdateUIWorker(Control ctl, ScanningText scanningText)
        {
            ScanningText = scanningText;
            _worker.DoWork += UpdateMsg;
            _picboxCollection = SelectPicCollection(ctl);
        }

        private GatePicboxCollection _picboxCollection;
        private DateTime MsgsUpdatedAt = DateTime.Now;

        public ScanningText ScanningText { get; set; }

        private void UpdateMsg(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (ProgramIdleChecker.GetIdleTime() > IDLE_SEC * 1000
                    && ProgramIdleChecker.GetUserIdleSeconds() > IDLE_SEC)
                {
                    try
                    {
                        _picboxCollection.ShowVideo();
                    }
                    catch (Exception ex)
                    {
                        _log.Error("[UpdateMsg - Play video] -==>" + ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
                else
                {

                    UIMsg msg = MsgManager.Instance.TryTake();

                    if (msg != null && !string.IsNullOrWhiteSpace(msg.Msg))
                    {
                        _picboxCollection.ShowScan();
                        MsgsUpdatedAt = DateTime.Now;
                        ProcessUIMsg(msg);
                    }
                    else
                    {
                        if ((DateTime.Now - MsgsUpdatedAt).TotalSeconds > ValidatorEnv.MsgShowingTime)
                        {
                            MsgsUpdatedAt = DateTime.Now;
                            Reset();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        public void UpdateTitle(string time, string station, string onlineStatus)
        {
            _picboxCollection.UpdateTitleText(time, station, onlineStatus);
        }
        public void Reset()
        {
            // as long as not showing error ,user want show the video
            _picboxCollection.WaitingScan();
            ScanningText.ClearKeyboardScan();
        }

        public void ShowVideo()
        {
            _picboxCollection.ShowVideo();
        }

        private void ProcessUIMsg(UIMsg msg)
        {
            if (msg.Success)
            {
                _picboxCollection.ShowSuccess(msg.Msg);
            }
            else
            {
                _picboxCollection.ShowError(msg.Msg);
            }
        }


        private GatePicboxCollection SelectPicCollection(Control ctl)
        {
            return new GatePicboxCollection(ctl);
        }
    }
}
