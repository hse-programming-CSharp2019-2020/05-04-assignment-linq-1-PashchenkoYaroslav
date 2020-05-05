using System;
using System.Collections.Generic;
using System.Linq;

/*Все действия по обработке данных выполнять с использованием LINQ
 * 
 * Объявите перечисление Manufacturer, состоящее из элементов
 * Dell (код производителя - 0), Asus (1), Apple (2), Microsoft (3).
 * 
 * Обратите внимание на класс ComputerInfo, он содержит поле типа Manufacturer
 * 
 * На вход подается число N.
 * На следующих N строках через пробел записана информация о компьютере: 
 * фамилия владельца, код производителя (от 0 до 3) и год выпуска (в диапазоне 1970-2020).
 * Затем с помощью средств LINQ двумя разными способами (как запрос или через методы)
 * отсортируйте коллекцию следующим образом:
 * 1. Первоочередно объекты ComputerInfo сортируются по фамилии владельца в убывающем порядке
 * 2. Для объектов, у которых фамилии владельцев сопадают, 
 * сортировка идет по названию компании производителя (НЕ по коду) в возрастающем порядке.
 * 3. Если совпадают и фамилия, и имя производителя, то сортировать по году выпуска в порядке возрастания.
 * 
 * Выведите элементы каждой коллекции на экран в формате:
 * <Фамилия_владельца>: <Имя_производителя> [<Год_производства>]
 * 
 * Пример ввода:
 * 3
 * Ivanov 1970 0
 * Ivanov 1971 0
 * Ivanov 1970 1
 * 
 * Пример вывода:
 * Ivanov: Asus [1970]
 * Ivanov: Dell [1971]
 * Ivanov: Dell [1970]
 * 
 * Ivanov: Asus [1970]
 * Ivanov: Dell [1971]
 * Ivanov: Dell [1970]
 * 
 * 
 *  * Обрабатывайте возможные исключения путем вывода на экран типа этого исключения 
 * (не использовать GetType(), пишите тип руками).
 * Например, 
 *          catch (SomeException)
            {
                Console.WriteLine("SomeException");
            }
 * При некорректных входных данных (не связанных с созданием объекта) выбрасывайте FormatException
 * При невозможности создать объект класса ComputerInfo выбрасывайте ArgumentException!
 */
namespace Task03
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = -1;
            List<ComputerInfo> computerInfoList = new List<ComputerInfo>();
            try
            {
                N = StringParseToInt();
                for (int i = 0; i < N; i++)
                {
                    string[] data = Console.ReadLine().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    computerInfoList.Add(new ComputerInfo(data));
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ArgumentNullException");
            }
            catch (FormatException)
            {
                Console.WriteLine("FormatException");
            }
            catch (OverflowException)
            {
                Console.WriteLine("OverflowException");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("InvalidOperationException");
            }
            catch (Exception)
            {
                Console.WriteLine("Exception");
            }

            // выполните сортировку одним выражением
            var computerInfoQuery = from pc in computerInfoList
                                    orderby pc.Owner descending, pc.ComputerManufacturer.Fabricator, pc.ComputerManufacturer.Data descending
                                    select pc;


            PrintCollectionInOneLine(computerInfoQuery);

            Console.WriteLine();

            // выполните сортировку одним выражением
            var computerInfoMethods = computerInfoList.OrderByDescending(pc => pc.Owner)
                .ThenBy(pc => pc.ComputerManufacturer.Fabricator).ThenByDescending(pc => pc.ComputerManufacturer.Data);
            PrintCollectionInOneLine(computerInfoMethods);

        }
        static string separator = "\n";
        // выведите элементы коллекции на экран с помощью кода, состоящего из одной линии (должна быть одна точка с запятой)
        public static void PrintCollectionInOneLine(IEnumerable<ComputerInfo> collection)
        {
            Console.WriteLine(collection.Select(col => col.ToString()).Aggregate((current, item) => current + separator + item));
        }
        /// <summary>
        /// Метод превращает строку в int.
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>   
        public static int StringParseToInt()
        {
            string strs = Console.ReadLine();
            int res;
            if (!int.TryParse(strs, out res) || res < 0)
                throw new ArgumentException();
            return res;
        }
    }


    class ComputerInfo
    {
        public ComputerInfo(string[] data)
        {
            Owner = data[0];
            ComputerManufacturer = new Manufacturer();
            int date;
            if (!int.TryParse(data[1], out date))
                throw new ArgumentException();
            ComputerManufacturer.Data = date;
            int type;
            if (!int.TryParse(data[2], out type) || type > 3 || type < 0)
                throw new ArgumentException();
            switch (type)
            {
                case 0:
                    ComputerManufacturer.Fabricator = "Dell";
                    break;
                case 1:
                    ComputerManufacturer.Fabricator = "Asus";
                    break;
                case 2:
                    ComputerManufacturer.Fabricator = "Apple";
                    break;
                case 3:
                    ComputerManufacturer.Fabricator = "Microsoft";
                    break;
            }
        }

        public string Owner { get; set; }
        public Manufacturer ComputerManufacturer { get; set; }

        public override string ToString()
        {
            return $"{Owner}: {ComputerManufacturer}";
        }
    }
    class Manufacturer
    {
        public string Fabricator { get; set; }
        public int Data { get; set; }

        public override string ToString()
        {
            return $"{Fabricator} [" + $"{Data}]";
        }
    }

}
