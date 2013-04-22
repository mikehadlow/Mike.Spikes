using System;
using Sprache;

namespace Mike.Spikes.SpracheSpikes
{
    // http://stackoverflow.com/questions/15219976/sprache-parser-with-custom-fields

    public class SpracheParserWithCustomFields
    {
        public static readonly Parser<DateTime> DateTimeParser =
            from day in Parse.Number
            from s1 in Parse.Char('/')
            from month in Parse.Number
            from s2 in Parse.Char('/')
            from year in Parse.Number
            select new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

        public static readonly Parser<DataFilterEntity> CustomParser =
            from a1 in Parse.String("custom").Token()
            from fromDateTime in DateTimeParser.Token()
            from toDateTime in DateTimeParser.Token()
            select new DataFilterEntity(fromDateTime.ToShortDateString() + " -> " + toDateTime.ToShortDateString());

        public static readonly Parser<DataFilterEntity> TimeFilter =
            Parse.String("today").Return(DataFilterEntity.Today)
                .Or(Parse.String("yesterday").Return(DataFilterEntity.Yesterday)
                .Or(Parse.String("last week").Return(DataFilterEntity.LastWeek)
                .Or(Parse.String("last month").Return(DataFilterEntity.LastMonth)
                .Or(Parse.String("none").Return(DataFilterEntity.None))
                .Or(CustomParser))));

        public void TestIt()
        {
            var result = TimeFilter.Parse("custom 21/3/2013 10/4/2013");
            Console.Out.WriteLine("result.Value = {0}", result.Value);
        }
    }

    public class DataFilterEntity
    {
        public static DataFilterEntity Today
        {
            get { return new DataFilterEntity("Today"); }
        }

        public static DataFilterEntity Yesterday
        {
            get { return new DataFilterEntity("Yesterday"); }
        }

        public static DataFilterEntity LastWeek
        {
            get { return new DataFilterEntity("LastWeek"); }
        }

        public static DataFilterEntity LastMonth
        {
            get { return new DataFilterEntity("LastMonth"); }
        }

        public static DataFilterEntity None 
        {
            get { return new DataFilterEntity("None"); }
        }

        public static DataFilterEntity Custom()
        {
            return new DataFilterEntity("Custom");
        }

        public string Value { get; private set; }

        public DataFilterEntity(string value)
        {
            Value = value;
        }
    }
}