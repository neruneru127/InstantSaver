using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstantSaver.Model
{
    /// <summary>
    /// 保存ダイアログのコントローラ.
    /// 
    /// </summary>
    internal class SaveDialogController
    {
        /// <summary>
        /// 保存ダイアログを検知する.
        /// 
        /// </summary>
        /// <returns>true = 存在する/false = 存在しない</returns>
        public static bool SearchDialog()
        {
            // 最前面のウィンドウハンドルを取得
            var wHandle = Const.GetForegroundWindow();

            // 表示されているか判定する
            if (!Const.IsWindowVisible(wHandle))
            {
                return false;
            }

            // クラス名の取得
            var builder = new StringBuilder(1000);
            Const.GetClassName(wHandle, builder, builder.Capacity);
            var className = builder.ToString();

            // クラス名で保存ダイアログを判定する
            if (className.Equals(Const.TARGET_CLASS))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// ダイアログを操作し保存する.
        /// 
        /// </summary>
        public static void DialogControl()
        {
            var fileName = String.Format(Const.FILE_NAME_FORMAT, DateTime.Now);
            var savePath = Const.SAVE_PATH + Path.DirectorySeparatorChar + fileName;

            // フォーマットに従ってファイル名を設定する
            // フルパスを指定することにより、保存先も一緒に指定
            SendKeys.SendWait(savePath);
            SendKeys.SendWait("{ENTER}");
        }
    }
}
