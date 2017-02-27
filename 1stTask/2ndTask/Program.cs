using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace _2ndTask
{
    class Program
    {
        static string prefixes = "http://localhost:4444/";
        static string path = @"E:\1stTask\";
        static List<string> mainHtml = new List<string>() { "default.html", "main.html", "index.html" };
        static Dictionary<string, string> mapMimeTypeAndFileExtension = new Dictionary<string, string>() // лимитирование доступа к файлам по расширению, конфигурируемый список (*.config и т.п.)
        {
            { "image/x-png", "png"},
            { "image/pjpeg", "jpeg"},
            { "text/css", "css"},
            { "text/html", "html"}
        };

        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Совсем не поддерживаем");
                return;
            }

            if (prefixes == null || prefixes.Length == 0) // заполните урл пжлста
                throw new ArgumentException("prefixes");

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefixes);

            listener.Start(); // погнали
            Console.WriteLine("Server START");

            Console.WriteLine("Listening...");
            while (true)
            {
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                result.AsyncWaitHandle.WaitOne();
            }
            listener.Stop();
            Console.WriteLine("Server STOP");

            Console.ReadLine();
        }

        /// <summary>
        /// Погнали
        /// </summary>
        /// <param name="result"></param>
        public static void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            byte[] buffer = new byte[] { };

            var subPath = request.Url.ToString().Replace(prefixes, "");// с урла вытаскиваем что они хотят

            subPath = FindMainHtml(subPath); // пытаемся найти главную страницу, если таковую не указали в Url

            if (subPath == "serverstop") // стоп слово
                return;

            if (!File.Exists(path + subPath)) // если файла внезапно нет, кинем 404 и в кансольку ругнемся
            {
                LogBadRequest(request);
                NotFound(response);
            }
            else // все ок, идем читать
            {

                buffer = File.ReadAllBytes(path + subPath);
                if (buffer != null) // ну всякое бывает, совсем ничегоне было, хотя вряд ли такое будет
                {
                    LogGoodRequest(request);
                    DefineContentType(response, buffer); // определим контент тайп

                    Action act = () =>
                    {
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    };

                    CheckAllow(response, request, act); // проверим разрешено ли данное расширение                  
                }
                else
                {
                    LogBadRequest(request);
                    NotFound(response);
                }
            }
        }

        private static string FindMainHtml(string subPath) // в случае если пользователь не указал конкретное имя страницы, должен выполняться поиск страницы по-умолчанию
        {
            if (string.IsNullOrEmpty(subPath)) // пытаемся определить корень сия сайта
            {
                foreach (var indx in mainHtml)
                {
                    if (File.Exists(path + indx))
                    {
                        subPath = indx;
                        break;
                    }
                    continue;
                }
            }

            return subPath;
        }

        private static void CheckAllow(HttpListenerResponse response, HttpListenerRequest request, Action nextAct)
        {
            var notAllowFormatOfFileList = ConfigurationManager.AppSettings["notAllowFormatOfFile"].Split(',').ToList();
            var valOfEx = string.Empty;
            mapMimeTypeAndFileExtension.TryGetValue(response.ContentType, out valOfEx);
            if (!string.IsNullOrEmpty(valOfEx) && notAllowFormatOfFileList.Contains(mapMimeTypeAndFileExtension[response.ContentType]))
            {
                LogBadRequest(request); 
                NotAllowed(response);
            }
            else
            {
                if (nextAct != null)
                    nextAct();
            }
        }

        /// <summary>
        /// Определить контент тайп
        /// </summary>
        /// <param name="response"></param>
        /// <param name="buffer"></param>
        private static void DefineContentType(HttpListenerResponse response, byte[] buffer) // в ответах должен быть заполнен Content-type
        {
            UInt32 mimetype;
            FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
            System.IntPtr mimeTypePtr = new IntPtr(mimetype);
            string mime = Marshal.PtrToStringUni(mimeTypePtr);
            Marshal.FreeCoTaskMem(mimeTypePtr);
            mime = mime.Replace("plain", "css");//закостылю пока, не понимат почему он не работат
            response.ContentType = mime;
        }


        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
              System.UInt32 pBC,
              [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
              [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
              System.UInt32 cbSize,
              [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
              System.UInt32 dwMimeFlags,
              out System.UInt32 ppwzMimeOut,
              System.UInt32 dwReserverd
          );

        /// <summary>
        /// Хороший лог
        /// </summary>
        /// <param name="request"></param>
        private static void LogGoodRequest(HttpListenerRequest request) // трассировка - показ на экране текущей обрабатываемой страницы/документа
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   * File loaded -" + request.Url);
        }

        /// <summary>
        /// Плохой лог
        /// </summary>
        /// <param name="request"></param>
        private static void LogBadRequest(HttpListenerRequest request) // трассировка - показ на экране текущей обрабатываемой страницы/документа
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   * File not avalible or not found - " + request.Url);
        }

        /// <summary>
        /// Вернем 404
        /// </summary>
        /// <param name="response"></param>
        private static void NotFound(HttpListenerResponse response) // в случае если запрашиваемый ресурс не найден, должен возвращаться соответствующий HTTP статус (404)
        {
            response.StatusCode = 404;
            response.StatusDescription = "Page not found";
            Stream output = response.OutputStream;
            output.Close();
        }

        /// <summary>
        /// Вернем 405
        /// </summary>
        /// <param name="response"></param>
        private static void NotAllowed(HttpListenerResponse response)
        {
            response.StatusCode = 405;
            response.StatusDescription = "Requested file is not allowed";
            Stream output = response.OutputStream;
            output.Close();
        }
    }
}
    
