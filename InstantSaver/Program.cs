using InstantSaver.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
 
 namespace InstantSaver
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // タスクトレイに登録
            new Tasktray();

            // キーフックの開始
            KeyHook.Execute();

            // 一時保存先のフォルダが無い場合は作成する
            InstantFileObserver.CreateFolder();


            // タイマーで一時ファイルの監視を走らせる
            Timer FileObserveTimer = new Timer();
            FileObserveTimer.Tick += new EventHandler(TimerEvent);
            FileObserveTimer.Interval = Const.OBSERVE_WAIT_TIME;
            FileObserveTimer.Start();

            // アプリケーション実行(終了までここに留まる)
            Application.Run();

            // キーフックの終了
            KeyHook.UnHook();

            // 一時ファイル監視終了
            FileObserveTimer.Stop();
        }

        private static void TimerEvent(object sender, EventArgs e)
        {
            // タイマー実行時のイベントを登録
            InstantFileObserver.Execute();
        }
    }
}





