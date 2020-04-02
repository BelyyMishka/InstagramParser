using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace InstagramParser
{
    class DB
    {
        /// <summary>
        /// Перевод коллекции в строку
        /// </summary>
        /// <param name="array">Коллекция</param>
        /// <returns></returns>
        private static object arrayToString(IEnumerable<string> array)
        {
            if (array == null) return DBNull.Value;
            var data = string.Empty;
            foreach (var item in array) data += getValue(item) + ", ";
            data = data.TrimEnd(',', ' ');
            return data;
        }

        /// <summary>
        /// Меняем кодировку
        /// </summary>
        /// <param name="data">Значение</param>
        /// <returns></returns>
        private static object getValue(string data)
        {
            if (data == null) return DBNull.Value;
            var srcEncoding = Encoding.UTF8;
            var dstEncoding = Encoding.Default;
            var buffer = srcEncoding.GetBytes(data);
            var value = dstEncoding.GetString(Encoding.Convert(srcEncoding, dstEncoding, buffer));
            return value;
        }

        /// <summary>
        /// Добавляем запись в БД
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="port">Порт</param>
        /// <param name="databaseName">Имя БД</param>
        /// <param name="password">Пароль</param>
        /// <param name="login">Логин</param>
        /// <param name="userId">Id</param>
        /// <param name="fullName">Имя</param>
        /// <param name="webSite">Веб-сайт</param>
        /// <param name="followers">Подписчики</param>
        /// <param name="geolocations">Геолокации</param>
        public static void newRecord(string server, string port, string databaseName, string password, string login, string userId, string fullName, string webSite, string followers, HashSet<string> geolocations)
        {
            var database = new NpgsqlConnection($"Server = {server}; Port = {port}; Database = {databaseName}; User Id = {login}; Password = {password};");
            database.Open();
            var sql = new NpgsqlCommand("INSERT INTO instagram_data_collector VALUES" + "(@user_id, @full_name, @website, @followers, @geolocations)", database);

            sql.Parameters.Add("user_id", NpgsqlDbType.Varchar);
            sql.Parameters["user_id"].Value = getValue(userId);

            sql.Parameters.Add("full_name", NpgsqlDbType.Varchar);
            sql.Parameters["full_name"].Value = getValue(fullName);

            sql.Parameters.Add("website", NpgsqlDbType.Varchar);
            sql.Parameters["website"].Value = getValue(webSite);

            sql.Parameters.Add("followers", NpgsqlDbType.Integer);
            sql.Parameters["followers"].Value = int.Parse(followers);

            sql.Parameters.Add("geolocations", NpgsqlDbType.Varchar);
            sql.Parameters["geolocations"].Value = arrayToString(geolocations);

            sql.ExecuteNonQuery();
            database.Close();
        }
    }
}
