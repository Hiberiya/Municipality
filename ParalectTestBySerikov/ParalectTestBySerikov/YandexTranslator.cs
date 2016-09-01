using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Web;

namespace ParalectTestBySerikov
{
    
    class YandexTranslator : Translator
    {
        public YandexTranslator(string translateText)
        {
            this.translateText = translateText;
            Key = GetKey();
            Url = GetUrl();
        }
        string GetKey()
        {
            //you can input your key in "Resourses" folder
            string path = Path.GetFullPath(@"Resourses\YandexKey.txt");
            string key = File.ReadAllText(path);
            return key;
        }
        string GetUrl()
        {
            //you can input your url in "Resourses" folder
            string path = Path.GetFullPath(@"Resourses\YandexUrl.txt");
            string key = File.ReadAllText(path);
            return key;
        }
        private void LanguageList(XDocument XlangList)
        {
            LangList = new List<Language>();
            foreach (XElement lang in XlangList.Element("Langs").Element("langs").Elements("Item"))
            {
                LangList.Add(new Language
                {
                    number = LangList.Count + 1,
                    langCode = lang.Attribute("key").Value,
                    langName = lang.Attribute("value").Value
                });
            }
            ShowLangList();
        }
        private void SelectMyTranslate()
        {
            int num = numOfTranslate;
            var translateLang = LangList.First(lang => lang.number == num).langCode;
            nameOfTranslate = translateLang;
        }
        private void ShowLangList()
        {
            List<Language> myLangList = LangList;
            Console.WriteLine();
            Console.WriteLine("Available languages:");
            foreach (var lang in myLangList)
            {
                Console.WriteLine("{0}-{1}", lang.number, lang.langName);
            }
        }
        public enum RequestType
        {
            GetLangs,
            Translate,
            Detect
        }
        private string GetAction(RequestType type)
        {
            switch (type)
            {
                case RequestType.GetLangs:
                    return "getLangs?";
                case RequestType.Detect:
                    return "detect?";
                case RequestType.Translate:
                    return "translate?";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public struct TranslateCommand
        {
            public RequestType Type;
            public Dictionary<string, string> Parameters;
            public Func<XElement, string> ResultSelector;
        }
        public void ShowLanguages(YandexTranslator api)
        {
            TranslateCommand languageOfText = new TranslateCommand();
            languageOfText.Type = RequestType.GetLangs;
            api.PrepareUrl(languageOfText);
        }
        public void DetectLanguage(YandexTranslator api)
        {
            TranslateCommand languageOfText = new TranslateCommand
            {
                Type = RequestType.Detect,
                Parameters = new Dictionary<string, string> { { "text", translateText } },
                ResultSelector = x => x.Attribute("lang").Value
            };
            api.PrepareUrl(languageOfText);
        }
        public void Translate(YandexTranslator api, int numberOfLang)
        {
            numOfTranslate = numberOfLang;
            SelectMyTranslate();
            string translateLanguage = translateLang + "-" + nameOfTranslate;
            TranslateCommand translate= new TranslateCommand
            {
                Type = RequestType.Translate,
                Parameters = new Dictionary<string, string> { { "lang", translateLanguage }, { "text", translateText } },
                ResultSelector = x => x.Element("text").Value
            };
            api.PrepareUrl(translate);
        }
        private void PrepareUrl(TranslateCommand command)
        {
            string strUrl = "";
            if (RequestType.GetLangs.ToString() != command.Type.ToString())
            {
                strUrl = Url + GetAction(command.Type) + "key=" + Key;
                foreach (var parameter in command.Parameters)
                    strUrl += "&" + parameter.Key + "=" + HttpUtility.UrlEncode(parameter.Value);
            }
            else
            {
                strUrl = Url + GetAction(command.Type) + "ui=" + translateLang + "&key=" + Key;
            }
            XDocument document = DownloadUrl(strUrl);
            command = YandexTranslateLogic(command, document);
        }
        private TranslateCommand YandexTranslateLogic(TranslateCommand command, XDocument document)
        {
            if (command.Type == RequestType.Detect)
            {
                string textTranslate = command.ResultSelector(document.Root);
                if (textTranslate == "") { Console.WriteLine("Enter the correct text!"); Console.WriteLine(); Program.StartTranslate(); }
                translateLang = textTranslate;
                Console.WriteLine("Language of text: " + translateLang);
                Console.WriteLine("Wait a second please...");
            }
            if (command.Type == RequestType.GetLangs) { LanguageList(document); }
            if (command.Type == RequestType.Translate)
            {
                string textTranslate = command.ResultSelector(document.Root);
                Console.WriteLine("Translate: " + textTranslate);
                string path = Path.GetFullPath(@"Resourses\Check.txt");
                File.WriteAllText(path, textTranslate);
            }
            return command;
        }
        private XDocument DownloadUrl(string strUrl)
        {
            WebClient webClitnt = new WebClient();
            webClitnt.Encoding = Encoding.UTF8;
            string stringXml = webClitnt.DownloadString(strUrl);
            XDocument document = XDocument.Parse(stringXml);
            return document;
        }
    }
}
