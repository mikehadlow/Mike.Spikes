using System;
using System.IO;
using System.Linq;
using EasyNetQ.SystemMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mike.Spikes.EasyNetQ
{
    public class ErrorQueueMessageAnalysis
    {
        private const string directoryPath = @"D:\Temp\CWT-PRD-Error_Queue";
        private const string firstMessage = "EasyNetQ_Default_Error_Queue.0.message.txt";

        public void LoadMessages()
        {
            var messages = Directory
                .GetFiles(directoryPath)
                .Select(File.ReadAllText)
                .Select(ParseJson);

            Console.Out.WriteLine("messages.Count() = {0}", messages.Count());

            var exchanges = messages.GroupBy(x => x.Error.Exchange);
            foreach (var exchange in exchanges)
            {
                Console.Out.WriteLine("{0} -- count:{1}", exchange.Key, exchange.Count());
                Console.Out.WriteLine("Excpetions:");
                var exceptions = exchange.GroupBy(x => x.Error.Exception.Substring(0, 250));
                foreach (var exception in exceptions)
                {
                    Console.Out.WriteLine("\t{0} -- count:{1}", exception.Key, exception.Count());
                }
            }
        }

        public void LoadOneMessage()
        {
            var messagePath = Path.Combine(directoryPath, firstMessage);
            var text = File.ReadAllText(messagePath);

            var error = ParseJson(text);

            Console.Out.WriteLine("error.Error.Exchange = {0}", error.Error.Exchange);
        }

        public ErrorInfo ParseJson(string text)
        {
            var error = JsonConvert.DeserializeObject<Error>(text);
            return new ErrorInfo
                {
                    Error = error,
                    Message = JObject.Parse(error.Message)
                };
        }
    }

    public class ErrorInfo
    {
        public Error Error { get; set; }
        public JObject Message { get; set; }
    }
}