using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Fetion.Practice.ChatHelper
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        #region  注册DLL

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, System.Text.StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        private static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        #endregion

        #region 成员
        /// <summary>
        /// 系统消息，WM_GETTEXT，用于获得另一进程某控件的值
        /// </summary>
        private const uint WM_GETTEXT = 0x000D;

        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread ChatListenThread;

        /// <summary>
        /// 当前缓存的记录
        /// </summary>
        private string RecordBuffer = string.Empty;

        #endregion

        private void btnGetWindowHandler_Click(object sender, EventArgs e)
        {

            int sleepInterval = 0;
            if (!int.TryParse(txtRefreshInterval.Text, out sleepInterval))
            {
                MessageBox.Show("时间间隔应为数字");
                return;
            }
            else
            {
                if (ChatListenThread == null || ChatListenThread.ThreadState != ThreadState.Running)
                {
                    ChatListenThread = new Thread(new ThreadStart(() =>
                    {
                        while (true)
                        {
                            IntPtr hwndWindow = FindWindow(null, txtWindowName.Text); //查找聊天窗口句柄  
                            if (hwndWindow != IntPtr.Zero)
                            {

                                IntPtr hwnd1 = FindWindowEx(hwndWindow, new IntPtr(0), "FxRichEdit", null);
                                IntPtr hwndChat = FindWindowEx(hwndWindow, hwnd1, "FxRichEdit", null);
                                System.Text.StringBuilder str = new System.Text.StringBuilder(16000);
                                //GetWindowText(hwndChat, str, 1024);

                                SendMessage(hwndChat, WM_GETTEXT, str.Capacity, str);
                                if (!string.IsNullOrEmpty(str.ToString()))
                                {
                                    this.RemindMsg(str.ToString());
                                }
                            }

                            Thread.Sleep(sleepInterval * 1000);
                        }

                    }));

                    ChatListenThread.Start();
                }
                else
                {
                    MessageBox.Show("监听线程已开启.");
                    return;
                }
            }
            btnGetWindowHandler.Enabled = false;
            btnStopListen.Enabled = true;
        }

        /// <summary>
        /// 刷新聊天信息
        /// </summary>
        /// <param name="txt"></param>
        private void RemindMsg(string txt)
        {
            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                string[] keyWords = txtKeyword.Text.Split(';');
                if (keyWords.Length == 1 && string.IsNullOrEmpty(keyWords[0]))
                {
                    return;
                }

                string[] strLst = txt.Split('\n');


                for (int i = strLst.Length - 1; i >= 0; i--)
                {
                    for (int j = 0; j < keyWords.Length - 1; j++)
                    {
                        if (strLst[i].Contains(keyWords[j]) && !RecordBuffer.Contains(strLst[i]))
                        {
                            RecordBuffer += strLst[i - 1] + "\n";
                            RecordBuffer += strLst[i] + "\n";
                        }
                    }
                }
            }
            txtResult.Text = RecordBuffer;

        }

        /// <summary>
        /// 停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopListen_Click(object sender, EventArgs e)
        {
            ChatListenThread.Abort();
            btnGetWindowHandler.Enabled = true;
            btnStopListen.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ////清空结果集、缓存记录、关键字
            txtResult.Text = string.Empty;
            RecordBuffer = string.Empty;
           // txtKeyword.Text = string.Empty;
        }

    }
}
