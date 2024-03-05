using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InstantSaver
{
    internal class Const
    {
        // -------------------------------------------
        // 共通
        // -------------------------------------------
        
        // 保存先のフォルダ名
        public const string FOLDER_NAME = "InstantFiles";

        // 保存先のパス
        public static readonly string SAVE_PATH = 
            Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            + Path.DirectorySeparatorChar + Const.FOLDER_NAME;


        // -------------------------------------------
        // タスクトレイ
        // -------------------------------------------

        // タスクトレイの名称
        public const string PROCESS_NAME = "InstantSaver";
        
        // タスクトレイのアイコンファイルパス
        public const string AWAKE_ICON_PATH = "./Images/AwakeIcon.ico";
        public const string STOP_ICON_PATH = "./Images/StopIcon.ico";
        
        // タスクトレイで表示する内容
        public const string CONTEXT_MENU_STOP = "停止";
        public const string CONTEXT_MENU_RESTART = "再開";
        public const string CONTEXT_OPEN_DIRECTORY = "保存先を開く";
        public const string CONTEXT_QUITE = "終了";


        // -------------------------------------------
        // キーフック
        // -------------------------------------------

        // キーフックに必要なキーコード
        public const int WH_KEYBOARD_LL = 0x0D;
        public const int WM_KEYBOARD_DOWN = 0x100;
        public const int WM_SYSKEY_DOWN = 0x104;

        // コールバック関数のdelegate 定義
        public delegate IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam);

        // キーボードフックに必要なDLLのインポート
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookCallback lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);


        // -------------------------------------------
        // 保存ダイアログ
        // -------------------------------------------

        // 保存ダイアログの検索開始を待機する時間
        public const int SEARCH_WAIT_TIME = 50;
        // 保存ダイアログを再検索する回数
        public const int SEARCH_RETRY = 10;
        // 保存ダイアログのクラス定数
        public const string TARGET_CLASS = "#32770";
        // ファイル保存用フォーマット
        public const string FILE_NAME_FORMAT = "{0:yyyyMMdd_HHmmss}";

        // 保存ダイアログに必要なDLLのインポート
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        // -------------------------------------------
        // 一時ファイル監視
        // -------------------------------------------

        // 実行周期(ms)  一時間を指定中
        public const int OBSERVE_WAIT_TIME = 3600000;

        // 保持する間隔(日)
        public const int PRESERVE_TIME = 7;
    }
}
