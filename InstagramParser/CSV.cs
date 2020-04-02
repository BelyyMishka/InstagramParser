using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InstagramParser
{
    class CSV
    {
        /// <summary>
        /// Название файла
        /// </summary>
        private const string FILE_NAME = "\\Instagram.csv";

        /// <summary>
        /// "Пустой" параметр
        /// </summary>
        private const string EMPTY_PARAM = "(пусто)";

        /// <summary>
        /// Записываем информацию в файл .csv
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="fullName">Имя пользователя</param>
        /// <param name="webSite">Веб-сайт</param>
        /// <param name="followers">Подписчики</param>
        /// <param name="geolocations">Геолокации</param>
        /// <param name="folder">Путь к папке</param>
        public static void writeToCsv(string userId, string fullName, string webSite, string followers, HashSet<string> geolocations, string folder)
        {
            File.AppendAllText(folder + FILE_NAME, string.Format("{0};{1};{2};{3};{4};{5}", userId, fullName == "" ? EMPTY_PARAM : fullName, webSite == "" ? EMPTY_PARAM : webSite, followers, geolocations.Count == 0 ? EMPTY_PARAM : geolocations.Aggregate((x, y) => x + ", " + y), Environment.NewLine), Encoding.UTF8);
        }
    }
}
