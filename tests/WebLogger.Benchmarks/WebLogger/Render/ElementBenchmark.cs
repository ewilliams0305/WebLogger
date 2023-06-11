using System.Drawing;
using BenchmarkDotNet.Attributes;
using WebLogger.Render;

namespace WebLogger.Benchmarks.WebLogger.Render
{
    [MemoryDiagnoser()]
    public class ElementBenchmark
    {
        private readonly List<string> _columns = new();

        [GlobalSetup]
        public void GlobalSetup()
        {
            const int items = 20;

            for (var i = 0; i < items; i++)
            {
                _columns.Add($"item");
            }
        }

        //[Benchmark]
        //public void ElementOfStruc()
        //{
        //    HtmlElement element;

        //    foreach (var col in _columns)
        //        element = new HtmlElement(Element.Div, col);
        //}

        //[Benchmark]
        //public void ElementOfClass()
        //{
        //    HtmlParent element;

        //    foreach (var col in _columns)
        //        element = new HtmlParent(Element.Div, col);
        //}

        //[Benchmark]
        //public void AppendElementOfStruc()
        //{
        //    HtmlElement parent = new HtmlElement(Element.Div);

        //    foreach (var col in _columns)
        //    {
        //        parent.Append(new HtmlElement(Element.Div, col));
        //    }
               
        //}

        //[Benchmark]
        //public void AppendElementOfClass()
        //{
        //    HtmlParent parent = new HtmlParent(Element.Div);

        //    foreach (var col in _columns)
        //    {
        //        parent.Append(new HtmlParent(Element.Div, col));
        //    }
                
        //}

        //[Benchmark]
        //public void InsertElementOfStruc()
        //{
        //    HtmlElement parent = new HtmlElement(Element.Div);

        //    foreach (var col in _columns)
        //    {
        //        parent.Insert(new HtmlElement(Element.Div, col));
        //    }
               
        //}

        //[Benchmark]
        //public void InsertElementOfClass()
        //{
        //    HtmlParent parent = new HtmlParent(Element.Div);

        //    foreach (var col in _columns)
        //    {
        //        parent.Insert(new HtmlParent(Element.Div, col));
        //    }
                
        //}

        [Benchmark]
        public string InsertElementOfChildIntoParentLegacy()
        {
            var parent = new HtmlElement(Element.Div);

            foreach (var col in _columns)
            {
                parent.Append(new HtmlElement(Element.Div, col, new HtmlElementOptions(Color.AliceBlue, "", 20, "")));
            }

            return parent.Render();
        }


    }
}
