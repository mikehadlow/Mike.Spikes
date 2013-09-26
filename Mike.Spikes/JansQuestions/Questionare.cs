using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mike.Spikes.JansQuestions
{
    public class Questionare
    {
        public void Quesion3()
        {
            var data = new[] {
                 new Car{ Model="A", Colour="Blue"},
                 new Car{ Model="B", Colour="Blue"},
                 new Car{ Model="A", Colour="Red"},
                 new Car{ Model="C", Colour="Green"},
                 new Car{ Model="D", Colour="Blue"},
                 new Car{ Model="C", Colour="Red"},
            };

            var qry = data.GroupBy(c => c.Colour)
                          .Where(g => g.Count() > 1);
 
            foreach (var result in qry)
            {
                 Console.WriteLine("{0}-{1}", result.Key, result.Count());
            }            
        }

        public void Question4()
        {
            var data1 = new object[] { "a", "b", "c", 1, 2, 3 };
            var source = data1.Cast<string>();
            var qry = from item in source
                      select item.ToUpper();

            foreach (var result in qry)
            {
                Console.WriteLine(result);
            }
        }

        public void Question5()
        {
            var a = new[] {"1", "2", "1", "3"};
            var data = Enumerable.Range(1, 5);
            var qry = from i in data
                      let x =
                           from y in data
                           where y > i
                           select y
                      where x.Any()
                      select new { i, x };
            foreach (var result in qry)
            {
                Console.WriteLine("{0}-{1}", result.i, result.x.Min());
            }            
        }

        public void Question8()
        {
            var qry = from x in new int[] { 1, 2, 3, 4, 5 }
                      join y in new int[] { 1, 4, 6, 10, }
                      on new { val = x * 2 } equals new { val = y }
                      select new { x, y };

            foreach (var result in qry)
            {
                Console.WriteLine("{0}{1}", result.x, result.y);
            }            
        }

        public void Question9()
        {
            Expression<Func<int, int>> expression = x => x * 2;

            var f = expression.Compile();
            Console.Out.WriteLine(f.GetType().Name);
        }

        public void Question6()
        {
//            var vehicle = new { Doors = 4 };
//            vehicle.Doors = 5;
//            Console.WriteLine(vehicle.Doors);            
        }
    }

    public class Car
    {
        public string Model { get; set; }
        public string Colour { get; set; }
    }
}