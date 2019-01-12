using AngleSharp.Dom.Html;

namespace iPhoneParser.Parser
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
