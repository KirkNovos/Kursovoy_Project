using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrarySumPrice
{
    public class Calculation
    {
        public List<DateTime> PopularMonths(List<DateTime> dates)
        {
            var DateTimeWithCounterList = new List<Tuple<DateTime, int>>();

            int PreviousYear = DateTime.Now.Year - 1;
            foreach (DateTime IterDate in dates)
            {
                if (IterDate.Year == PreviousYear)
                {
                    // вычисляем начало месяца для текущей даты
                    var DateMonthStart = new DateTime(IterDate.Year, IterDate.Month, 1, 0, 0, 0);

                    // ищем эту дату во временном списке
                    var index = DateTimeWithCounterList.FindIndex(item => item.Item1 == DateMonthStart);

                    // кортежи можно создавать по-разному
                    if (index == -1)
                    {
                        // такой даты нет - добавляю (используя конструктор)
                        DateTimeWithCounterList.
                            Add(new Tuple<DateTime, int>(DateMonthStart, 1));
                    }
                    else
                    {
                        // дата есть - увеличиваем счетчик
                        // свойства кортежа неизменяемые, поэтому перезаписываем текущий элемент новым кортежем, который создаем статическим методом
                        DateTimeWithCounterList[index] = Tuple.Create(DateTimeWithCounterList[index].Item1, DateTimeWithCounterList[index].Item2 + 1);
                    }
                }
            }

            return DateTimeWithCounterList
                .OrderByDescending(item => item.Item2)
                .ThenBy(item => item.Item1)
                .Select(item => item.Item1)
                .ToList();
        }
    }
}
