using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerHelper
    {
        public HtmlNode GetElementWithAttribute(HtmlNode root, string elementName, string attributeName, string attributeValue)
        {
            attributeName = attributeName.ToLower();
            attributeValue = attributeValue.ToLower();
            elementName = elementName.ToLower();
            HtmlNode result = null;

            var node =
                root
                    .Descendants()
                    .Where(n => n.Name == elementName)
                    .SelectMany(n =>
                        n
                            .Attributes
                            .Where(a => a.Name == attributeName)
                            .Select(a =>
                                new
                                {
                                    Element = n,
                                    ClassName = a.Value.ToLower()
                                }))
                    .FirstOrDefault(l =>
                        l.ClassName.Contains(attributeValue));

            if (node == null)
            {
                Console.WriteLine("{0} for {1}={2} is null", elementName, attributeName, attributeValue);
            }
            else
            {
                result = node.Element;
            }

            return result;
        } 

        private string SafeGetAttributeValue(HtmlNodeCollection collection, string name)
        {
            string result = string.Empty;
            var attr =
                collection
                .Select(n => n.Attributes.FirstOrDefault(a => a.Name == name))
                .FirstOrDefault();
            if (attr != null)
            {
                result = attr.Value;
            }
            return result;
        }

        private string SafeGetAttributeValue(HtmlNode node, string name)
        {
            string result = string.Empty;
            var attr =
                node
                .Attributes
                .FirstOrDefault(a => a.Name == name);
            if (attr != null)
            {
                result = attr.Value;
            }
            return result;
        }
    }
}
