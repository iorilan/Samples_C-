using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyboardHock
{
    public abstract class BaseHook
    {
        #region memebers

        private const int WH_KEYBOARD_LL = 13;
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        private HookProc KeyBoardHookProcedure; //委托变量
        private static int hHook = 0;
        private const int KEYEVENTF_KEYUP = 2;

        //键盘Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]

        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        #endregion


        #region DllImport
        //设置钩子 
        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);

        [DllImport("user32.dll")]
        //调用下一个钩子 
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        //模拟键盘事件 

        [DllImport("User32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        
        #endregion

        #region 暴露两个方法，挂钩和卸载
        public void InstallHook()
        {
            this.StartHook();
        }

        public void UnInstallHook()
        {
            this.ClearHook();
        }

        #endregion

        #region 两个虚方法用于扩展
        public virtual bool IsFilter(int keyCode)
        {
            return false;
        }

        public virtual int ParseKeyChr(int keyCode)
        {
            return keyCode;
        }

        #endregion

        /// <summary>
        /// 开始挂钩
        /// </summary>
        private void StartHook()
        {

            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc); //委托变量赋值

                ////全局挂钩
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                          KeyBoardHookProcedure,
                        GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    UnInstallHook();
                    throw new Exception("设置Hook失败!");
                }
            }
        }

        /// <summary>
        /// 取消挂钩
        /// </summary>
        private void ClearHook()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
            //如果去掉钩子失败. 
            if (!retKeyboard) throw new Exception("UnhookWindowsHookEx failed.");
        }

        /// <summary>
        /// 获得键盘消息的函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">记录键盘状态，是按下还是抬起</param>
        /// <param name="lParam">键值，但要通过一个显示转换获得，因此要先提供一个KEY_MSG类</param>
        /// <returns></returns>
        private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
            int keyChr = ((char)kbh.vkCode);

            ////过滤
            if (this.IsFilter(keyChr))
            {
                return 1;
            }
            //替换
            int keyVal = this.ParseKeyChr(keyChr);

            if (wParam == 0x100)//键盘按下
            {
                keybd_event((byte)keyVal, 0, 0, 0);
                keybd_event((byte)keyVal, 0, KEYEVENTF_KEYUP, 0);
                return 1;
            }

            return CallNextHookEx(hHook, nCode, wParam, lParam);

        }


    }
}
