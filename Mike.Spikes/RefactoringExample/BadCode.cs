using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Mike.Spikes.RefactoringExample
{
    public class Widget
    {
        public bool Temporary { get; private set; }
        public WidgetColor WidgetColor { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Customer { get; private set; }

        public Widget(bool temporary, WidgetColor widgetColor, DateTime endDate, string customer)
        {
            Temporary = temporary;
            WidgetColor = widgetColor;
            EndDate = endDate;
            Customer = customer;
        }
    }

    public class BadCode
    {
        public class ProcessParams
        {
            private readonly string properties;
            private readonly string customer;

            public ProcessParams(string properties, string customer)
            {
                this.properties = properties;
                this.customer = customer;
            }

            public string Properties
            {
                get { return properties; }
            }

            public string Customer
            {
                get { return customer; }
            }
        }

        public bool Process(ProcessParams processParams, out string returnedErrors)
        {
            if(processParams.Properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            if(processParams.Customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var result = true;
            var errors = "";

            Action<string> fail = message =>
            {
                result = false;
                if (errors != "")
                {
                    errors += Environment.NewLine;
                }
                errors += message;
            };

            var splitProperties = processParams.Properties.Split('|');
            bool temporary = false;
            var widgetColor = WidgetColor.SomeShadeOfGrey;
            var endDate = new DateTime();
            var dataAccess = new DataAccess();

            if (!processParams.Properties.StartsWith("<record>"))
            {
                for (int i = 0; i < splitProperties.Length; i++)
                {
                    var kv = splitProperties[i].Split('=');
                    if (kv.Length != 2)
                    {
                        throw new BadCodeException("invalid value found in properties");
                        continue;
                    }
                    var key = kv[0];
                    var value = kv[1];

                    switch (key)
                    {
                        case "temporary":
                            if (!bool.TryParse(value, out temporary))
                            {
                                fail("invalid temporary value");
                            }
                            break;
                        case "color":
                            switch (value)
                            {
                                case "blue":
                                    widgetColor = WidgetColor.Blue;
                                    break;
                                case "red":
                                    widgetColor = WidgetColor.Red;
                                    break;
                                case "green":
                                    widgetColor = WidgetColor.Green;
                                    break;
                                case "someshadeofgrey":
                                    widgetColor = WidgetColor.SomeShadeOfGrey;
                                    break;
                                default:
                                    result = false;
                                    if (errors != "")
                                    {
                                        errors += Environment.NewLine;
                                    }
                                    errors += "invalid widget color";
                                    break;
                            }
                            break;
                        case "endDate":
                            if (!DateTime.TryParse(value, out endDate))
                            {
                                result = false;
                                if (errors != "")
                                {
                                    errors += Environment.NewLine;
                                }
                                errors += "invalid endDate";
                            }
                            break;
                        default:
                            result = false;
                            if (errors != "")
                            {
                                errors += Environment.NewLine;
                            }
                            errors += "invalid endDate";
                            break;
                    }
                }
            }
            else
            {
                var document = new XmlDocument();

                try
                {
                    document.Load(new StringReader(processParams.Properties));
                }
                catch (Exception e)
                {
                    result = false;
                    if (errors != "")
                    {
                        errors += Environment.NewLine;
                    }
                    errors += "invalid xml";
                }

                foreach (XmlElement node in document.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "temporary":
                            if (!bool.TryParse(node.Value, out temporary))
                            {
                                result = false;
                                if (errors != "")
                                {
                                    errors += Environment.NewLine;
                                }
                                errors += "invalid temporary value";
                            }
                            break;
                        case "color":
                            switch (node.Value)
                            {
                                case "blue":
                                    widgetColor = WidgetColor.Blue;
                                    break;
                                case "red":
                                    widgetColor = WidgetColor.Red;
                                    break;
                                case "green":
                                    widgetColor = WidgetColor.Green;
                                    break;
                                case "someshadeofgrey":
                                    widgetColor = WidgetColor.SomeShadeOfGrey;
                                    break;
                                default:
                                    result = false;
                                    if (errors != "")
                                    {
                                        errors += Environment.NewLine;
                                    }
                                    errors += "invalid widget color";
                                    break;
                            }
                            break;
                        case "endDate":
                            if (!DateTime.TryParse(node.Value, out endDate))
                            {
                                result = false;
                                if (errors != "")
                                {
                                    errors += Environment.NewLine;
                                }
                                errors += "invalid endDate";
                            }
                            break;
                        default:
                            result = false;
                            if (errors != "")
                            {
                                errors += Environment.NewLine;
                            }
                            errors += "invalid endDate";
                            break;
                    }
                }
            }

            if (widgetColor == WidgetColor.Blue && processParams.Customer == "AB")
            {
                temporary = false;
            }

            if (temporary)
            {
                if (endDate >= DateTime.Now)
                {
                    try
                    {
                        var message = "";
                        var dbOk = dataAccess.Update(processParams.Customer, widgetColor.ToString(), endDate, out message);
                        if (!dbOk)
                        {
                            result = false;
                            if (errors != "")
                            {
                                errors += Environment.NewLine;
                            }
                            errors += message;
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        if (errors != "")
                        {
                            errors += Environment.NewLine;
                        }
                        errors += e.ToString();
                    }
                }
                else
                {
                    try
                    {
                        var message = "";
                        var dbOk = dataAccess.UpdateTemporaryStore(processParams.Customer, widgetColor.ToString(), out message);
                        if (!dbOk)
                        {
                            result = false;
                            if (errors != "")
                            {
                                errors += Environment.NewLine;
                            }
                            errors += message;
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        if (errors != "")
                        {
                            errors += Environment.NewLine;
                        }
                        errors += e.ToString();
                    }
                }
            }
            else
            {
                try
                {
                    var message = "";
                    var dbOk = dataAccess.Update(processParams.Customer, widgetColor.ToString(), endDate, out message);
                    if (!dbOk)
                    {
                        result = false;
                        if (errors != "")
                        {
                            errors += Environment.NewLine;
                        }
                        errors += message;
                    }
                }
                catch (Exception e)
                {
                    result = false;
                    if (errors != "")
                    {
                        errors += Environment.NewLine;
                    }
                    errors += e.ToString();
                }
            }

            returnedErrors = errors;
            return result;
        }     

    }

    [Serializable]
    public class BadCodeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BadCodeException()
        {
        }

        public BadCodeException(string message) : base(message)
        {
        }

        public BadCodeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BadCodeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}