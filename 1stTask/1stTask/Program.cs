using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsQuery;

namespace _1stTask
{
    class Program
    {
        static string _address = "https://habrahabr.ru/";
        static string path = @"E:\1stTask";
        static List<int> levelDownload = new List<int>() // пока работает только 2 уровня, для 3 и более необходимо доработка замены в url предка ссылки на url'ы потомков
        {
            0, 1
        };
        static Dictionary<string, string> typeOfDomWithAtr = new Dictionary<string, string>()
            {
                { "img", "src"},
                { "script", "src"},
                { "link", "href"},
                { "a", "href"}
            };

        /// <summary>
        /// Погнали
        /// </summary>
        static async void Run()
        {
            Console.WriteLine("Program START");
            string indexHtml = string.Empty;
            HttpClient client = new HttpClient();
            Directory.CreateDirectory(path + @"\files"); // директория для всего кроме index'a первого уровня
            CQ cq = new CQ();
            HttpResponseMessage response = new HttpResponseMessage();

            List <string> nextLevelUrls = new List<string>(); // адреса нового уровня
            List<string> levelUrls = new List<string>(); // адреса уровня по которому бужим
            nextLevelUrls.Add(_address); // добавим 0 уровень
                        
            foreach(var level in levelDownload)// уровни загрузки
            {
                levelUrls = nextLevelUrls;
                nextLevelUrls = new List<string>(); // на новом уровне чистим список сайтов
                foreach (var url in levelUrls)
                {
                    #region Try conn
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("___________________________________________________");
                    Console.WriteLine("Try connect to " + url);
                    try
                    {
                        response = await client.GetAsync(url);
                    }
                    catch
                    {
                        SiteNotAvaLog();
                        continue;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        SiteNotAvaLog();
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Site available");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("___________________________________________________");
                    #endregion Try conn

                    var html = await response.Content.ReadAsStringAsync();                    

                    Console.WriteLine("Download files");
                    cq = CQ.Create(html);

                    indexHtml = await SaveDocFiles(client, html, nextLevelUrls, cq, level, url, indexHtml);

                    saveMainIndex(indexHtml);
                }
            }

            Console.WriteLine("Program FINISH");
            Console.WriteLine("___________________________________________________");
        }

        /// <summary>
        /// Сохранить главный индекс
        /// </summary>
        /// <param name="html"></param>
        private static void saveMainIndex(string html)
        {
            try // ну а вдруг, поругайемся, но пойдем дальше
            {
                File.WriteAllText(path + @"\index.html", html); // сохранять будем каждый раз, чтоб если не дойдем до конца, что-то да осталось
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!!!!!!!!!!!!! НЕ СМОГЛИ СОХРАНИТЬ ОСНОВНОЙ INDEX !!!!!!!!!!!!!!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Логи о недоступности сайта
        /// </summary>
        private static void SiteNotAvaLog()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Site not available");
        }

        /// <summary>
        /// Сохраним все документы сайта + сам индекс
        /// </summary>
        /// <param name="client"></param>
        /// <param name="html">текущая страница</param>
        /// <param name="nextLevelUrls"></param>
        /// <param name="cq"></param>
        /// <param name="level">уровень на котором мы находимся</param>
        /// <param name="thisUrl">адресс текущего сайта</param>
        /// <param name="indexHtml">главная страница 0го уровня</param>
        /// <returns></returns>
        private static async Task<string> SaveDocFiles(HttpClient client, string html, List<string> nextLevelUrls, CQ cq, int level, string thisUrl, string indexHtml)
        {
            try // если какой то файл не удалось загрузить и в коде это не обработано, не огорчаемся идем дальше
            {
                foreach (var dom in typeOfDomWithAtr)
                {
                    html = await SaveDocsContin(client, html, cq, dom.Key, dom.Value, nextLevelUrls, level);
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("___________________________________________________");

                Console.WriteLine("Save index.html");

                string index = level == levelDownload.First() ? @"\index.html" : null;
                if (string.IsNullOrEmpty(index)) // если уже не нулевой уровень
                {
                    index = Guid.NewGuid().ToString();
                    indexHtml = indexHtml.Replace(thisUrl, @"files/" + index);
                    File.WriteAllText(path + @"\files\" + index, html); // сохранять будем каждый раз, чтоб если не дойдем до конца, что-то да осталось
                };
                Console.WriteLine("___________________________________________________");

                if (level == levelDownload.First())
                    indexHtml = html;
            }
            catch
            { }

            return indexHtml;
        }

        /// <summary>
        /// Собственно загрузка и сохраниение файлов
        /// </summary>
        /// <param name="client"></param>
        /// <param name="html"></param>
        /// <param name="cq"></param>
        /// <param name="dom"></param>
        /// <param name="atr"></param>
        /// <param name="nextLevelUrls"></param>
        /// <returns></returns>
        private static async Task<string> SaveDocsContin(HttpClient client, string html, CQ cq, string dom, string atr, List<string> nextLevelUrls, int level)
        {
            try // на всякий, чтобы не прерывать работу программы
            {
                foreach (IDomObject obj in cq.Find(dom))
                {
                    var subUrl = obj.GetAttribute(atr);
                    if (subUrl == null)
                        continue;
                    else if (dom == "a")//выходим на новый уровень, просто сохраним ссылки
                    {
                        nextLevelUrls.Add(subUrl);
                        continue;
                    }
                    var g = Guid.NewGuid().ToString();

                    try
                    {
                        HttpResponseMessage subResponse = await client.GetAsync(subUrl);
                        var res = await subResponse.Content.ReadAsByteArrayAsync();
                        Console.ForegroundColor = res.Count() > 0 ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine("    - " + subUrl);
                        File.WriteAllBytes(path + @"\files\" + g, res);
                    }
                    catch { }

                    html = html.Replace(subUrl, (level == levelDownload.First() ? @"files/" : "") + g); // роут в папку только для 0го уровня, остальное и так там уже
                }
            }
            catch
            { }

            return html;
        }

        static void Main(string[] args)
        {
            Run();
            Console.ReadLine();
        }

        #region history
        //static void Main(string[] args)
        //{
        //    var mainUrl = "shop.kz";
        //    var url = "https://" + mainUrl;
        //    var path = @"E:\1stTask";

        //    int checkedPos = 0;//счетчик для позиции символа страницы
        //    int counterAdditionalFiles = 0;
        //    //HttpClient client = new HttpClient();
        //    //var reply = client.GetStringAsync(url);


        //    WebClient client = new WebClient();

        //    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

        //    Stream data = client.OpenRead(url);
        //    StreamReader reader = new StreamReader(data);
        //    string page = reader.ReadToEnd();

        //    var urlWitoutSymbols = url.Replace(':', ' ').Replace((char)92, ' ').Replace((char)46, ' ').Replace((char)47, ' ').ToString();
        //    Directory.CreateDirectory(path + @"\" + urlWitoutSymbols);
        //    Directory.CreateDirectory(path + @"\" + urlWitoutSymbols + @"\files");

        //    DownloadExtFiles("src='", url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);
        //    checkedPos = 0;
        //    DownloadExtFiles("src=" + (char)34, url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);
        //    checkedPos = 0;
        //    DownloadExtFiles("url(", url, path, ref checkedPos, ref counterAdditionalFiles, client, ref page, urlWitoutSymbols);

        //    File.WriteAllText(path + @"\" + urlWitoutSymbols + @"\" + urlWitoutSymbols + ".html", page);
        //    Console.ReadKey();
        //}

        //private static void DownloadExtFiles(string startStr, string url, string path, ref int checkedPos, ref int counterAdditionalFiles, WebClient client, ref string page, string urlWitoutSymbols)
        //{
        //    while (true)//найдем и сохраним все внутренние ресурсы сайта
        //    {
        //        string subUr = getBetween(ref checkedPos, page, startStr, ".");
        //        List<string> rightFiles = new List<string>() { "png", "jpg", "gif", "js" };              
        //        if (string.IsNullOrEmpty(subUr))
        //            break;
        //        if (!rightFiles.Contains(subUr.Split('.').Last()))
        //            continue;
        //        byte[] dat = new byte[] { };
        //        try
        //        {
        //            dat = client.DownloadData(url + subUr);
        //        }
        //        catch
        //        { }
        //        string extraPatch = @"files/" + counterAdditionalFiles.ToString() + "." + subUr.Split((char)46).Last();
        //        page = page.Replace(subUr, extraPatch);
        //        File.WriteAllBytes(path + @"/" + urlWitoutSymbols + @"/" + extraPatch, dat);
        //        counterAdditionalFiles++;
        //    }
        //}

        //public static string getBetween(ref int checkedPos, string page, string strStart, string strEnd)
        //{
        //    int Start, End;
        //    page = page.Substring(checkedPos, page.Count() - checkedPos); // отсеем то, что уже проверили
        //    if (page.Contains(strStart) && page.Contains(strEnd))
        //    {
        //        Start = page.IndexOf(strStart, 0) + strStart.Length;
        //        End = page.IndexOf(strEnd, Start);
        //        checkedPos += End; // запомним индекс где мы остановились
        //        var res = page.Substring(Start, End - Start + 4); // +4 это точка и символы расширения файла

        //        if (res.Last() == 39 || res.Last() == 63) // бывает расширение из 2х символов, уберем не нужный символ ' из имени
        //            res = res.Substring(0, res.Count() - 1);
        //        return res;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        #endregion
    }
}
