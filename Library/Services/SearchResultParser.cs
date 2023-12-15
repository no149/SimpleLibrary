using HtmlAgilityPack;
using Library.Models;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Library.Services
{
    class SearchResultParser
    {
        private int[] _persianNumerals = new int[] { 0x06F0, 0x06F1, 0x06F2, 0x06F3, 0x06F4, 0x06F5, 0x06F6, 0x06F7, 0x06F8, 0x06F9 };
        public FindResult[] Parse(string htmlContent)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(new System.IO.StringReader(htmlContent));
            return FindResults(doc);
        }

        private string ConvertFromPersianNumerals(string number)
        {
            var sb = new StringBuilder(number);
            for (int i = 0; i < number.Length; i++)
            {
                var n = (int)(number[i]);
                var lantinNumeral = Array.IndexOf(_persianNumerals, n);
                sb[i] = Convert.ToChar('0' + lantinNumeral);
            }
            return sb.ToString();
        }
        private FindResult[] FindResults(HtmlAgilityPack.HtmlDocument doc)
        {
        
            var title = GetValue(FindTitleNode(doc));
            var author = GetValue(FindAuthorNode(doc));
            var publisher = GetValue(FindPublisherNode(doc));
            var translator = GetValue(FindTranslatorNode(doc));
            var subject = GetValue(FindSubjectNode(doc));
            var pageCnt = ConvertFromPersianNumerals(GetValue(FindPageCountNode(doc)));
            var isbn = GetValue(FindIsbnNode(doc));

            var cultInfo = (CultureInfo)CultureInfo.GetCultureInfo("fa").Clone();
            cultInfo.NumberFormat.DigitSubstitution = DigitShapes.Context;
            short pageCntInt= 0;
            short.TryParse(pageCnt,cultInfo,out pageCntInt);
            return new FindResult[] { new FindResult {
                Author = author,
                Publisher = publisher,
                Title = title,
                Translator = translator,
                PageCount =pageCntInt ,
                ISBN= isbn,
                 Subject=subject

            } };
           
        }


        private string GetValue(HtmlNode htmlNode)
        {
            if(htmlNode==null)
            return "";
            var sb = new StringBuilder(htmlNode.InnerText);
            sb = sb.Replace("\n", "");
            var val = sb.ToString().Trim();
            val = string.Join(',', val.Split('،').Select(x => x.Trim()));
            return val;
        }

        private HtmlNode FindIsbnNode(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"شابک\")]/following-sibling::span/span")?[0];
        }
        private HtmlNode FindTitleNode(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//a[contains(@href,'/bibliography')]/text()")?[0];
        }

        private HtmlNode FindAuthorNode(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"پدید\")]//span//text()")?[0];
        }
        private HtmlNode FindPublisherNode(HtmlAgilityPack.HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"نشر\")]/following-sibling::span")?[0];
        }
        private HtmlNode FindTranslatorNode(HtmlAgilityPack.HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"مترجم\")]/following-sibling::span")?[0];
        }
        private HtmlNode FindPageCountNode(HtmlAgilityPack.HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"صفحات\")]/following-sibling::span")?[0];
        }
        private HtmlNode FindSubjectNode(HtmlAgilityPack.HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//span[contains(text(),\"موضوع\")]/following-sibling::span")?[0];
        }
    }
}