using AngleSharp.Parser.Html;
using System;

namespace iPhoneParser.Parser
{
    class ParserWorker<T> where T : class
    {
        IParser<T> parser;
        HtmlLoader loader;
        IParserSettings parserSettings;


        public IParser<T> Parser
        {
            get { return parser; }
            set { parser = value; }

        }

        public IParserSettings Settings
        {
            get { return parserSettings; }
            set { loader = new HtmlLoader(value); }
        }


        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings parserSettings) : this(parser)
        {
            this.parserSettings = parserSettings;
        }

        public event Action<object, T> OnNewData;

        public void Start()
        {
            Worker();
        }

        private async void Worker()

        {
            var source = await loader.GetSourceByPageId();
            var domParser = new HtmlParser();
            var document = await domParser.ParseAsync(source);
            var result = parser.Parse(document);
            OnNewData?.Invoke(this, result);
        }


    }
}
