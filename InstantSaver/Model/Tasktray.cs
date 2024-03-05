using InstantSaver.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstantSaver
{
    internal class Tasktray : ApplicationContext
    {
        // タスクトレイのアイコン
        public static NotifyIcon notifyIcon;
        // 実行状態を保持するフラグ true = 実行中, false = 停止中
        private bool executeFlg = true;

        /// <summary>
        /// タスクトレイに登録を行う.
        /// 
        /// </summary>
        public Tasktray()
        {
            this.setComponents();
        }

        private void setComponents()
        {
            notifyIcon = new NotifyIcon();
            // アイコンの設定
            notifyIcon.Icon = new Icon(Const.AWAKE_ICON_PATH);
            // アイコンを表示する
            notifyIcon.Visible = true;
            // アイコンにマウスポインタを合わせたときのテキスト
            notifyIcon.Text = Const.PROCESS_NAME;

            // アイコンを右クリックしたときのメニューを設定
            var menu = new ContextMenuStrip();

            // 実行/停止用メニュー
            menu.Items.Add(Const.CONTEXT_MENU_STOP, null, (s, e) =>
            {
                var item = menu.Items[0];

                // 実行フラグの反転
                executeFlg = !executeFlg;

                // メニューテキストを再設定
                item.Text = executeFlg ? Const.CONTEXT_MENU_STOP : Const.CONTEXT_MENU_RESTART;

                // メニューの更新
                menu.Items.RemoveAt(0);
                menu.Items.Insert(0, item);

                // アイコンの変更
                notifyIcon.ContextMenuStrip = menu;
                notifyIcon.Icon = new Icon(executeFlg ? Const.AWAKE_ICON_PATH : Const.STOP_ICON_PATH);


                if (executeFlg)
                {
                    // キーフックの再開
                    KeyHook.Execute();
                }
                else
                {
                    // キーフックの停止
                    KeyHook.UnHook();
                }
            });

            // 保存先フォルダを開く用メニュー
            menu.Items.Add(Const.CONTEXT_OPEN_DIRECTORY, null, (s, e) => {

                System.Diagnostics.Process.Start(Const.SAVE_PATH);

            });

            // 終了用メニュー
            menu.Items.Add(Const.CONTEXT_QUITE, null, (s, e) => {
                Application.Exit();
            });

            notifyIcon.ContextMenuStrip = menu;
        }

    }
}
