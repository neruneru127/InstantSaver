using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstantSaver.Model
{
    /// <summary>
    /// 一時保存ファイルの監視クラス.
    /// 
    /// </summary>
    internal class InstantFileObserver
    {

        /// <summary>
        /// 最終更新から、指定された期間以上経過した一時ファイルを削除.
        /// 
        /// </summary>
        public static void Execute()
        {
            // 一時保存先のフォルダをチェック
            CreateFolder();

            // 全てのファイルをチェック
            foreach (var path in GetAllFilePath(Const.SAVE_PATH))
            {
                if (!isWritableFile(path))
                {
                    // ファイルが開けないならスキップ
                    continue;
                }

                // ファイルの最終更新時間を取得し、保存期間と比較
                var spanTime = DateTime.Now - File.GetLastWriteTime(path);
                if (spanTime.TotalDays > Const.PRESERVE_TIME)
                {
                    // ファイルの削除
                    File.Delete(path);

                    //// ファイルを削除する(ゴミ箱に移動)
                    //FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }
        }

        private static bool isWritableFile(string filePath)
        {
            try
            {
                // 書き込みモードでファイルを開けるか確認
                using (FileStream fp = File.Open(filePath, FileMode.Open, FileAccess.Write))
                {
                    // 開ける
                    return true;
                }
            }
            catch
            {
                // 開けない
                return false;
            }
        }



        private static List<string> GetAllFilePath(string directoryPath)
        {
            // 全ファイルパスを格納したリスト
            var retList = new List<string>();

            // 子ディレクトリを探す
            foreach (var path in Directory.GetDirectories(directoryPath))
            {
                // 再帰的に子ディレクトリのファイル名を取得する
                retList.AddRange(GetAllFilePath(path));
            }

            // 現在のディレクトリに存在するファイルパスを格納
            retList.AddRange(Directory.GetFiles(directoryPath));

            return retList;

        }



        /// <summary>
        /// 一時保存先のフォルダが存在しない場合に作成を行う.
        /// 
        /// </summary>
        public static void CreateFolder()
        {
            if (!Directory.Exists(Const.SAVE_PATH))
            {
                Directory.CreateDirectory(Const.SAVE_PATH);
            }
        }
    }
}
