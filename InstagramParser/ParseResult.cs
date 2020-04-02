using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramParser
{
    class ParseResult
    {
        /// <summary>
        /// Число записанных людей
        /// </summary>
        private int count;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ParseResult()
        {
            count = 0;
        }

        /// <summary>
        /// Увеличить счетсчик
        /// </summary>
        public void increaseCount()
        {
            count++;
        }

        /// <summary>
        /// Вернуть количество людей
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            return count;
        }
    }
}
