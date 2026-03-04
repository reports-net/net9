using Microsoft.Data.Sqlite;
using System.Data;

namespace SvgPreviewMobile
{
    /// <summary>
    /// 共有ユーティリティ
    /// SQLite ローカルDB接続
    /// </summary>
    static class Util
    {
        // prepd ファイルパス（ローカルリソース）
        internal static string SharePath = "";

        /// <summary>
        /// EmbeddedResource のファイルを AppDataDirectory にコピー
        /// （LoadDefFile() がファイルパスを必要とするため）
        /// Android / iOS / Windows すべてで動作する。
        /// </summary>
        internal static Task CopyResourcesAsync()
        {
            // SharePath を先に設定（UIスレッド上で即座に完了）
            string destDir = FileSystem.AppDataDirectory + Path.DirectorySeparatorChar;
            SharePath = destDir;

            // ファイルコピーはバックグラウンドで実行
            // （コンストラクタで例外を投げない＋UIスレッドをブロックしない）
            return Task.Run(() =>
            {
                var assembly = typeof(Util).Assembly;

                string[] files = {
                    "report-def.prepd", "simple.prepd",
                    "zip1.prepd", "zip2.prepd",
                    "cover.prepd", "estimate.prepd", "invoice.prepd",
                    "product-list.prepd", "stamp.png",
                    "image1-1.jpg", "image1-2.jpg", "image2-1.jpg", "image2-2.jpg",
                    "image3-1.jpg", "image3-2.jpg", "image4-1.jpg", "image4-2.jpg",
                    "sample.db"
                };

                foreach (var file in files)
                {
                    string dest = destDir + file;
                    if (!File.Exists(dest))
                    {
                        using var stream = assembly.GetManifestResourceStream(file)
                            ?? throw new FileNotFoundException(
                                $"EmbeddedResource not found: {file}");
                        using var fs = File.Create(dest);
                        stream.CopyTo(fs);
                    }
                }
            });
        }

        internal static SqliteConnection ConnectDatabase()
        {
            var conn = new SqliteConnection($"Data Source={SharePath}sample.db");
            conn.Open();
            return conn;
        }

        /// <summary>
        /// SQLクエリを実行し DataTable を返す。
        /// Microsoft.Data.Sqlite には DataAdapter がないためヘルパーとして用意。
        /// </summary>
        internal static DataTable ExecuteQuery(string sql, SqliteConnection conn)
        {
            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
    }
}
