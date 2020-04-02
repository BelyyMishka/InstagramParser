using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InstagramParser
{
    class HTML
    {
        /// <summary>
        /// URL-адрес
        /// </summary>
        private const string INSTAGRAM_URL = "https://www.instagram.com/";

        /// <summary>
        /// Скрипт с основной информацией о пользователе
        /// </summary>
        private const string INSTAGRAM_SCRIPT = "/html/body/script[1]";

        /// <summary>
        /// Получаем разметку страницы
        /// </summary>
        /// <param name="URL">URL-адрес</param>
        /// <returns></returns>
        private static string getRequest(string URL)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Загружаем разметку и возвращаем документ
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        private static HtmlAgilityPack.HtmlDocument getDocument(string id)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(getRequest(INSTAGRAM_URL + id));
            return document;
        }

        /// <summary>
        /// Получаем текст скрипта
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        public static string getScript(string id)
        {
            return getDocument(id).DocumentNode.SelectSingleNode(INSTAGRAM_SCRIPT).InnerText;
        }
    }
}
