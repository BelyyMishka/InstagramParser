using Newtonsoft.Json.Linq;

namespace InstagramParser
{
    class JSON
    {
        /// <summary>
        /// Начальный символ скрипта
        /// </summary>
        private const char SCRIPT_START = '{';

        /// <summary>
        /// Конечный символ скрипта
        /// </summary>
        private const char SCRIPT_END = ';';

        /// <summary>
        /// Подготавливаеи скрипт для парсинга
        /// </summary>
        /// <param name="script">Текст скрипта</param>
        /// <returns></returns>
        private static string prepareScript(string script)
        {
            int start = script.IndexOf(SCRIPT_START);
            int end = script.LastIndexOf(SCRIPT_END);
            return "[" + script.Substring(start, end - start) + "]";
        }

        /// <summary>
        /// Парсим в JSON
        /// </summary>
        /// <param name="script">Текст скрипта</param>
        /// <returns></returns>
        private static dynamic parseScript(string script)
        {
            return JArray.Parse(script);
        }

        /// <summary>
        /// Возвращаем JSON
        /// </summary>
        /// <param name="script">Текст скрипта</param>
        /// <returns></returns>
        public static dynamic getScript(string script)
        {
            return parseScript(prepareScript(script));
        }
    }
}
