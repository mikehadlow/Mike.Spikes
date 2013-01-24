using System;
using System.IO;
using System.Xml;

namespace Mike.Spikes.RefactoringExample
{
    public class BadCode
    {
        public bool Process(string properties, string customer, out string errors)
        {
            var result = true;
            errors = "";

            var p = properties.Split('|');
            bool t = false;
            WidgetColor c = WidgetColor.SomeShadeOfGrey;
            DateTime d = new DateTime();
            DataAccess da = new DataAccess();

            if (!properties.StartsWith("<record>"))
            {
                for (int i = 0; i < p.Length; i++)
                {
                    var kv = p[i].Split('=');
                    if (kv.Length != 2)
                    {
                        result = false;
                        if (errors != "")
                        {
                            errors += Environment.NewLine;
                        }
                        errors += "invalid value found in properties";
                        continue;
                    }
                    var key = kv[0];
                    var value = kv[1];

                    switch (key)
                    {
                        case "temporary":
                            if (!bool.TryParse(value, out t))
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
                            switch (value)
                            {
                                case "blue":
                                    c = WidgetColor.Blue;
                                    break;
                                case "red":
                                    c = WidgetColor.Red;
                                    break;
                                case "green":
                                    c = WidgetColor.Green;
                                    break;
                                case "someshadeofgrey":
                                    c = WidgetColor.SomeShadeOfGrey;
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
                            if (!DateTime.TryParse(value, out d))
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
                    document.Load(new StringReader(properties));
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
                            if (!bool.TryParse(node.Value, out t))
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
                                    c = WidgetColor.Blue;
                                    break;
                                case "red":
                                    c = WidgetColor.Red;
                                    break;
                                case "green":
                                    c = WidgetColor.Green;
                                    break;
                                case "someshadeofgrey":
                                    c = WidgetColor.SomeShadeOfGrey;
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
                            if (!DateTime.TryParse(node.Value, out d))
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

            if (c == WidgetColor.Blue && customer == "AB")
            {
                t = false;
            }

            if (t)
            {
                if (d >= DateTime.Now)
                {
                    try
                    {
                        da.Update(customer, c.ToString(), d);
                    }
                    catch (Exception e)
                    {
                        result = false;
                        if (errors != "")
                        {
                            errors += Environment.NewLine;
                        }
                        errors += "invalid endDate";
                    }
                }
            }
            else
            {
                try
                {
                    da.Update(customer, c.ToString(), d);
                }
                catch (Exception e)
                {
                    result = false;
                    if (errors != "")
                    {
                        errors += Environment.NewLine;
                    }
                    errors += "invalid endDate";
                }
            }

            return result;
        }     

    }
}