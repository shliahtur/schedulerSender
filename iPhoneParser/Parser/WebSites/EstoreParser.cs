using System;
using System.Linq;
using AngleSharp.Dom.Html;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace iPhoneParser.Parser
{
    class EstoreParser : IParser<string>
    {

        public string Parse(IHtmlDocument document)
        {

            var priceContainer = document.GetElementById("product-price-20866");
            string price = "бэд реквест";

            if (priceContainer != null)
            {
                price = priceContainer.TextContent;
            }
       
            return price;
        }
    }
}

