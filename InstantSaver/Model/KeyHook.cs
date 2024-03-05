using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static InstantSaver.Const;


namespace InstantSaver.Model
{
    /// <summary>
    /// キーフック用クラス.
    /// 
    /// </summary>
    internal class KeyHook
    {
        // フックプロシージャのハンドル
        private static IntPtr hookHandle = IntPtr.Zero;
        // フック時のコールバック関数
        private static HookCallback callback;
        // 1つ前に押されたキーを保持
        private static Keys oldKey = Keys.None;
        // 遅延実行用のタイマー
        private static Timer timer = new Timer();
        // リトライ用カウンタ
        private static int retryCnt = 0;


        static KeyHook()
        {
            // タイマーの設定
            timer.Tick += new EventHandler(TimerEvent);
            timer.Interval = Const.SEARCH_WAIT_TIME;
        }

        /// <summary>
        /// キーをフックし、Ctrl + Sが押下された場合に処理を行う.
        /// 
        /// </summary>
        public static void Execute()
        {
            // コールバックの設定
            callback = CallbackProc;

            // キーフックの開始
            using (Process process = Process.GetCurrentProcess())
            {
                using (ProcessModule module = process.MainModule)
                {
                    hookHandle = SetWindowsHookEx(
                       // フックするイベントの種類
                       WH_KEYBOARD_LL,
                       // フック時のコールバック関数
                       callback,
                       // インスタンスハンドル
                       GetModuleHandle(module.ModuleName),
                       // スレッドID（0：全てのスレッドでフック）
                       0
                   );
                }
            }
        }

        private static IntPtr CallbackProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // キーを押しているか判定
            if ((int)wParam == Const.WM_KEYBOARD_DOWN || (int)wParam == Const.WM_SYSKEY_DOWN)
            {
                Keys key = (Keys)(short)Marshal.ReadInt32(lParam);

                // Ctrl + S が押下されたか判定
                // Ctrlについては事前に押しているかどうかで判定を行う
                if (oldKey == Keys.LControlKey && key == Keys.S)
                {
                    retryCnt = 0;

                    timer.Start();
                }

                // Ctrl用に今回押したキーを保存
                oldKey = key;
            }

            return Const.CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        private static void TimerEvent(object sender, EventArgs e)
        {
            retryCnt++;
            if (retryCnt > Const.SEARCH_RETRY)
            {
                // リトライ回数を超えた場合は処理を中断
                timer.Stop();
            }

            // 保存ダイアログを探す
            if (SaveDialogController.SearchDialog())
            {
                timer.Stop();
                // ダイアログに対して操作を行う
                SaveDialogController.DialogControl();
                
            }
        }

        /// <summary>
        /// キーフックの終了.
        /// 
        /// </summary>
        public static void UnHook()
        {
            UnhookWindowsHookEx(hookHandle);
            hookHandle = IntPtr.Zero;
        }
    }
}
