using System;

namespace Mike.Spikes.RefactoringExample
{
    public class BadCodeRunner
    {
        private BadCode badCode = new BadCode();

        public void RunsProcessWithKeyValueString()
        {
            var headers = "temporary=false|color=blue|endDate=2013-01-24";
            string errors;

            var result = badCode.Process(headers, "AB", out errors);

            Console.Out.WriteLine("errors = {0}", errors);
            Console.Out.WriteLine("result = {0}", result);
        }

        public void RunsProcessWithXml()
        {
            var xml =
@"<record>
    <temporary>false</temporary>
    <color>blue</color>
    <endDate>2013-01-24</endDate>
</record>";
            string errors;

            var result = badCode.Process(xml, "AB", out errors);

            Console.Out.WriteLine("errors = {0}", errors);
            Console.Out.WriteLine("result = {0}", result);
        }
             
    }
}