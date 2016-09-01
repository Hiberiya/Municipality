using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParalectTestBySerikov
{
    abstract class Translator
    {
        public struct Language
        {
            public int number;
            public string langCode;
            public string langName;
        }
        public string Url { get; set; } //API url
        public string Key { get; set; } //API key
        public string translateText { get; set; } //input text
        public string translateLang { get; set; } //Language of text
        public string nameOfTranslate { get; set; } // the name of selected language
        public int numOfTranslate { get; set; } // the number of selected language
        public List<Language> LangList { get; set; } //Languages list
        //api - API of translator
        //numberOfLang - number of user-selected language
        public void DetectLanguage(Translator api) { } //Auto Detect language text
        public void ShowLanguages(Translator api) { } //Show list of languages
        public void Translate(Translator api, int numberOfLang) { } //Enter the number from the list of languages and Translate!
    }
}
