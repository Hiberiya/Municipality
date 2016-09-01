using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Web;

namespace ParalectTestBySerikov
{
    class Program
    {

        public static void StartTranslate()
        {
            Console.WriteLine("Enter the text for translate");
            string translateString = Console.ReadLine();
            YandexTranslator myTranslator = new YandexTranslator(translateString);
            myTranslator.DetectLanguage(myTranslator);
            myTranslator.ShowLanguages(myTranslator);
            int myNumber = SetNumberOfLang();
            myTranslator.Translate(myTranslator, myNumber);
        }

        private static int SetNumberOfLang()
        {
            Console.WriteLine("Enter language number from the list");
            string numOfLanguage = Console.ReadLine();
            int num = 0;
            try
            {
                num = Convert.ToInt32(numOfLanguage);
            }
            catch (Exception)
            {
                Console.WriteLine("Enter the number!");
                SetNumberOfLang();
            }
            return num;
        }
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.Unicode;
            //Console.InputEncoding = Encoding.Unicode;
            Console.WriteLine("Welcome to \"Console Translator!\" application");
            StartTranslate();
            Console.WriteLine("Work completed. Check the \"Check.txt\" file if you see the bad result.");
            Console.ReadKey();
        }
    }
}