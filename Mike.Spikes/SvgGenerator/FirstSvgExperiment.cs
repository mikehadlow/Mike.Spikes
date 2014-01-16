using System;
using System.IO;
using System.Xml.Linq;

namespace Mike.Spikes.SvgGenerator
{
    public class FirstSvgExperiment
    {
        public void Do()
        {
            var svg = new Svg();

            //svg.SetUnit("");

            // draw some rectangles
            for (int x = 0; x < 10; x++)
            {
                svg.Move(10, 0);
                for (int y = 0; y < 10; y++)
                {
                    svg.Move(0, 10);
                    svg.Rectangle(8, 8);
                }
                svg.Move(0, -100);
            }

            // draw some circles
            svg.SetPosition(59, 59);
            for (int i = 1; i < 6; i++)
            {
                svg.Circle(i * 10);
            }

            // draw a zigzag
            svg.SetPosition(50, 10);
            var up = true;
            for (int i = 0; i < 10; i++)
            {
                up = !up;
                var x = up ? 10 : -10;
                svg.Line(x, 10);
            }

            Console.Out.WriteLine(svg);

            File.WriteAllText(@"D:\Temp\SVG_Experiments\test.svg", svg.ToString());
        }
    }

    public class Svg
    {
        private readonly XDocument doc;
        private readonly XElement svg;
        private readonly XNamespace ns;

        private string unit = "mm";
        private int currentX = 0;
        private int currentY = 0;

        public Svg()
        {
            doc = new XDocument();
            ns = "http://www.w3.org/2000/svg";
            svg = new XElement(ns + "svg");
            svg.Add(new XAttribute(XNamespace.Xmlns + "svg", ns));
            svg.Add(new XAttribute("version", "1.1"));
            doc.Add(svg);
        }

        public void Rectangle(int width, int height)
        {
            var e = Element("rect");

            e.Add(SizeAttribute("width", width));
            e.Add(SizeAttribute("height", height));
            e.Add(SizeAttribute("x", currentX));
            e.Add(SizeAttribute("y", currentY));
            e.Add(Style());

            svg.Add(e);
        }

        public void Circle(int r)
        {
            Elipse(r, r);
        }

        public void Elipse(int rx, int ry)
        {
            var e = Element("ellipse");

            e.Add(SizeAttribute("rx", rx));
            e.Add(SizeAttribute("ry", ry));
            e.Add(SizeAttribute("cx", currentX));
            e.Add(SizeAttribute("cy", currentY));
            e.Add(Style());

            svg.Add(e);
        }

        public void Line(int x, int y)
        {
            var e = Element("line");

            e.Add(SizeAttribute("x1", currentX));
            e.Add(SizeAttribute("y1", currentY));

            Move(x, y);

            e.Add(SizeAttribute("x2", currentX));
            e.Add(SizeAttribute("y2", currentY));
            e.Add(Style());

            svg.Add(e);


        }

        public XElement Element(string name)
        {
            return new XElement(ns + name);
        }

        public XAttribute SizeAttribute(string name, int value)
        {
            return new XAttribute(name, value.ToString() + GetUnit());
        }

        public XAttribute Style()
        {
            return new XAttribute("style", "fill:none;stroke-width:1;stroke:rgb(0,0,0)");
        }

        private string GetUnit()
        {
            return unit;
        }

        public void SetUnit(string unit)
        {
            this.unit = unit;
        }

        public override string ToString()
        {
            return doc.ToString();
        }

        public void Move(int x, int y)
        {
            currentX += x;
            currentY += y;
        }

        public void SetPosition(int x, int y)
        {
            currentX = x;
            currentY = y;
        }
    }
}