using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _1stTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainUrl = "shop.kz";
            var url = "https://" + mainUrl;
            var path = @"E:\1stTask";

            int checkedPos = 0;//счетчик для позиции символа страницы
            int counterAdditionalFiles = 0;
            //HttpClient client = new HttpClient();
            //var reply = client.GetStringAsync(url);


            WebClient client = new WebClient();

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string page = reader.ReadToEnd();

            var urlWitoutSymbols = url.Replace(':', ' ').Replace((char)92, ' ').Replace((char)46, ' ').Replace((char)47, ' ').ToString();
            Directory.CreateDirectory(path + @"\" + urlWitoutSymbols);
            Directory.CreateDirectory(path + @"\" + urlWitoutSymbols + @"\files");

            DownloadExtFiles("src='", url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);
            checkedPos = 0;
            DownloadExtFiles("src=" + (char)34, url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);
            checkedPos = 0;
            DownloadExtFiles("url(", url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);

            File.WriteAllText(path + @"\" + urlWitoutSymbols + @"\" + urlWitoutSymbols + ".html", page);
            Console.ReadKey();
        }

        private static void DownloadExtFiles(string startStr, string url, string path, ref int checkedPos, ref int counterAdditionalFiles, WebClient client, ref string page, string urlWitoutSymbols)
        {
            while (true)//найдем и сохраним все внутренние ресурсы сайта
            {
                string subUr = getBetween(ref checkedPos, page, startStr, ".");
                List<string> rightFiles = new List<string>() { "png", "jpg", "gif", "js" };              
                if (string.IsNullOrEmpty(subUr))
                    break;
                if (!rightFiles.Contains(subUr.Split('.').Last()))
                    continue;
                byte[] dat = new byte[] { };
                try
                {
                    dat = client.DownloadData(url + subUr);
                }
                catch
                { }
                string extraPatch = @"files/" + counterAdditionalFiles.ToString() + "." + subUr.Split((char)46).Last();
                page = page.Replace(subUr, extraPatch);
                File.WriteAllBytes(path + @"/" + urlWitoutSymbols + @"/" + extraPatch, dat);
                counterAdditionalFiles++;
            }
        }

        public static string getBetween(ref int checkedPos, string page, string strStart, string strEnd)
        {
            int Start, End;
            page = page.Substring(checkedPos, page.Count() - checkedPos); // отсеем то, что уже проверили
            if (page.Contains(strStart) && page.Contains(strEnd))
            {
                Start = page.IndexOf(strStart, 0) + strStart.Length;
                End = page.IndexOf(strEnd, Start);
                checkedPos += End; // запомним индекс где мы остановились
                var res = page.Substring(Start, End - Start + 4); // +4 это точка и символы расширения файла

                if (res.Last() == 39 || res.Last() == 63) // бывает расширение из 2х символов, уберем не нужный символ ' из имени
                    res = res.Substring(0, res.Count() - 1);
                return res;
            }
            else
            {
                return "";
            }
        }
    }
}
