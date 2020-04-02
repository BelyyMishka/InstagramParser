using System.Collections.Generic;

namespace InstagramParser
{
    class InstagramData
    {
        /// <summary>
        /// Геолокации
        /// </summary>
        /// <param name="JSON">JSON-объект</param>
        /// <returns></returns>
        public static HashSet<string> geolocation(dynamic JSON)
        {
            HashSet<string> geolocations = new HashSet<string>();

            for (int i = 0; i < JSON[0].entry_data.ProfilePage[0].graphql.user.edge_owner_to_timeline_media.edges.Count; i++)
            {
                if (JSON[0].entry_data.ProfilePage[0].graphql.user.edge_owner_to_timeline_media.edges[i].node.location != null)
                {
                    geolocations.Add(JSON[0].entry_data.ProfilePage[0].graphql.user.edge_owner_to_timeline_media.edges[i].node.location.name.Value.ToString());
                }
            }

            return geolocations;
        }

        /// <summary>
        /// Подписчики
        /// </summary>
        /// <param name="JSON">JSON-объект</param>
        /// <returns></returns>
        public static string followers(dynamic JSON)
        {
            return JSON[0].entry_data.ProfilePage[0].graphql.user.edge_follow.count.Value.ToString();
        }

        /// <summary>
        /// Веб-сайт
        /// </summary>
        /// <param name="JSON">JSON-объект</param>
        /// <returns></returns>
        public static string webSite(dynamic JSON)
        {
            string webSite = string.Empty;

            if(JSON[0].entry_data.ProfilePage[0].graphql.user.external_url != null)
            {
                webSite = JSON[0].entry_data.ProfilePage[0].graphql.user.external_url.Value.ToString();
            }
            return webSite;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public static string fullName(dynamic JSON)
        {
            return JSON[0].entry_data.ProfilePage[0].graphql.user.full_name.Value.ToString();
        }
    }
}
