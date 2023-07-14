using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace Dictionary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictinary dictionary = new Dictinary(Dictinary.Language.English, Dictinary.Language.Russia);
            bool isDone = false;
            while (!isDone)
            {
                int number;
                Console.WriteLine("0-exit");
                Console.WriteLine("1-Get");
                Console.WriteLine("2-Add");
                Console.WriteLine("3-Delete");
                Console.WriteLine("4-Replace");
                if (!int.TryParse(Console.ReadLine(), out number))
                {
                    number = -1;
                }
                switch (number)
                {
                    case 0: isDone = true; dictionary.Save(); break;
                    case 1:
                        bool isDone1 = false;
                        while (!isDone1)
                        {
                            int number1;
                            Console.WriteLine("0-exit");
                            Console.WriteLine("1-GetWord");
                            Console.WriteLine("2-GetWordFile");

                            if (!int.TryParse(Console.ReadLine(), out number1))
                            {
                                number1 = -1;
                            }
                            switch (number1)
                            {
                                case 0: isDone1 = true; break;
                                case 1:
                                    Console.WriteLine("Write word:");
                                    string str = Console.ReadLine();
                                    Console.WriteLine($"Result={dictionary.GetWord(str)}");
                                    break;
                                case 2:
                                    Console.WriteLine("Write word:");
                                    string str1 = Console.ReadLine();
                                    dictionary.GetWordFile(str1);
                                    break;
                            }
                        }
                        break;
                    case 2:
                        bool isDone2 = false;
                        while (!isDone2)
                        {
                            int number1;
                            Console.WriteLine("0-exit");
                            Console.WriteLine("1-AddWord");
                            Console.WriteLine("2-AddWordOrValue");

                            if (!int.TryParse(Console.ReadLine(), out number1))
                            {
                                number1 = -1;
                            }
                            switch (number1)
                            {
                                case 0: isDone2 = true; break;
                                case 1:
                                    Console.WriteLine("Write word:");
                                    string str = Console.ReadLine();
                                    Console.WriteLine("Write value:");
                                    string strvalue = Console.ReadLine();
                                    if (!dictionary.AddWord(str, strvalue))
                                    {
                                        Console.WriteLine("Write not right word");
                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("Write word:");
                                    string str1 = Console.ReadLine();
                                    Console.WriteLine("Write value:");
                                    string str1value = Console.ReadLine();
                                    if (!dictionary.AddWordOrValue(str1, str1value))
                                    {
                                        Console.WriteLine("Write not right value");
                                    }
                                    break;
                            }
                        }

                        break;
                    case 3:
                        bool isDone3 = false;
                        while (!isDone3)
                        {
                            int number1;
                            Console.WriteLine("0-exit");
                            Console.WriteLine("1-DeleteWord");
                            Console.WriteLine("2-DeleteValue");

                            if (!int.TryParse(Console.ReadLine(), out number1))
                            {
                                number1 = -1;
                            }
                            switch (number1)
                            {
                                case 0: isDone3 = true; break;
                                case 1:
                                    Console.WriteLine("Write word:");
                                    string str = Console.ReadLine();
                                    if (!dictionary.DeleteWord(str))
                                    {
                                        Console.WriteLine("Write not right word");
                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("Write word:");
                                    string str1 = Console.ReadLine();
                                    Console.WriteLine("Write value:");
                                    string str1value = Console.ReadLine();
                                    if (!dictionary.DeleteValue(str1, str1value))
                                    {
                                        Console.WriteLine("Write not right value");
                                    }
                                    break;
                            }
                        }

                        break;
                    case 4:
                        bool isDone4 = false;
                        while (!isDone4)
                        {
                            int number1;
                            Console.WriteLine("0-exit");
                            Console.WriteLine("1-ReplaxeWord");
                            Console.WriteLine("2-ReplaceValue");

                            if (!int.TryParse(Console.ReadLine(), out number1))
                            {
                                number1 = -1;
                            }
                            switch (number1)
                            {
                                case 0: isDone4 = true; break;
                                case 1:
                                    Console.WriteLine("Write word:");
                                    string str = Console.ReadLine();
                                    Console.WriteLine("Write new Word");
                                    string strnew = Console.ReadLine();
                                    if (!dictionary.ReplaceWord(str, strnew))
                                    {
                                        Console.WriteLine("Write not right word");
                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("Write word:");
                                    string str1 = Console.ReadLine();
                                    Console.WriteLine("Write value:");
                                    string str1value = Console.ReadLine();
                                    if (!dictionary.ReplaceValue(str1, new List<string>() { str1value }))
                                    {
                                        Console.WriteLine("Write not right value");
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }
        class Dictinary
        {
            static string path = Directory.GetCurrentDirectory().ToString() + "/Dictionaries";
            Regex RegexWord;
            Regex RegexValue;
            Dictionary<string, List<string>> dictionary;
            string _languageWord;
            string _languageValue;

            static Regex Russia = new Regex("[а-я- ]+");
            static Regex English = new Regex("[a-z- ]+");
            bool isChange = false;
            public bool Chenged
            {
                get => isChange;
            }
            public enum Language
            {
                Russia,
                English
            }
            void SetRegex(Language languageWord, Language languageValue)
            {

                if (languageWord == Language.Russia)
                {
                    RegexWord = Russia;
                    _languageWord = "Russia";
                }
                if (languageValue == Language.Russia)
                {
                    RegexValue = Russia;
                    _languageValue = "Russia";
                }
                if (languageWord == Language.English)
                {
                    RegexWord = English;
                    _languageWord = "English";
                }
                if (languageValue == Language.English)
                {
                    RegexValue = English;
                    _languageValue = "English";
                }
            }
            public Dictinary(Language languageWord, Language languageValue)
            {
                dictionary = new Dictionary<string, List<string>>();
                SetRegex(languageWord, languageValue);
                Open();
                Directory.CreateDirectory(path);
            }
            public bool AddWord(string word, string value)
            {
                string Word = word.Trim().ToLower();
                string Value = value.Trim().ToLower();
                if (!RegexWord.IsMatch(Word) || !RegexValue.IsMatch(Value) || dictionary.ContainsKey(Word))
                {
                    return false;
                }
                List<string> newList = new List<string>()
            {
                Value
            };
                dictionary.Add(Word, newList);
                isChange = true;
                return true;
            }
            public bool AddWordOrValue(string word, string value)
            {
                string Word = word.Trim().ToLower();
                string Value = value.Trim().ToLower();
                if (!RegexWord.IsMatch(Word) || !RegexValue.IsMatch(Value))
                {
                    return false;
                }
                if (!dictionary.ContainsKey(Word))
                {
                    List<string> newList = new List<string>()
                {
                    Value
                };
                    dictionary.Add(Word, newList);
                }
                else
                {
                    if (dictionary[Word].Contains(Value))
                    {
                        return false;
                    }
                    dictionary[Word].Add(Value);
                }
                isChange = true;
                return true;
            }
            public bool ReplaceWord(string word, string newWord)
            {
                string Word = word.Trim().ToLower();
                string NewWord = newWord.Trim().ToLower();
                if (!dictionary.ContainsKey(Word) || !RegexWord.IsMatch(NewWord))
                {
                    return false;
                }
                var oldList = dictionary[Word];
                dictionary.Remove(Word);
                dictionary.Add(NewWord, oldList);
                isChange = true;
                return true;
            }
            public bool ReplaceValue(string word, List<string> newValue)
            {
                string Word = word.Trim().ToLower();
                if (!dictionary.ContainsKey(Word))
                {
                    return false;
                }
                for (int i = 0; i < newValue.Count; i++)
                {
                    newValue[i] = newValue[i].Trim().ToLower();
                    if (!RegexValue.IsMatch(newValue[i]))
                    {
                        return false;
                    }
                }
                dictionary[Word] = newValue;
                isChange = true;
                return true;
            }
            public bool DeleteWord(string word)
            {
                string Word = word.Trim().ToLower();
                if (!dictionary.ContainsKey(Word))
                {
                    return false;
                }
                dictionary.Remove(Word);
                isChange = true;
                return true;
            }
            public bool DeleteValue(string word, string value)
            {
                string Word = word.Trim().ToLower();
                string Value = value.Trim().ToLower();
                if (!dictionary.ContainsKey(Word))
                {
                    return false;
                }
                if (dictionary[Word].Count == 1)
                {
                    return false;
                }
                if (!dictionary[Word].Remove(Value)) { return false; }
                isChange = true;
                return true;
            }
            public string GetWord(string word)
            {
                string Word = word.Trim().ToLower();

                if (!dictionary.ContainsKey(Word))
                {
                    return null;
                }
                string str = "";
                for (int i = 0; i < dictionary[Word].Count; i++)
                {
                    if (i > 0)
                    {
                        str += ",";
                    }
                    str += dictionary[Word][i];
                }
                return str;
            }
            public void GetWordFile(string word)
            {
                string Word = word.Trim().ToLower();

                if (!dictionary.ContainsKey(Word))
                {
                    return;
                }
                string mypath = path + "/Result";
                Directory.CreateDirectory(mypath);
                string myFile = $"{mypath}/{Word}_result.txt";
                using (FileStream fs = new FileStream(myFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    string str = $"{Word}-";
                    for (int i = 0; i < dictionary[Word].Count; i++)
                    {
                        if (i > 0)
                        {
                            str += ",";
                        }
                        str += dictionary[word][i];
                    }
                    byte[] data = Encoding.UTF8.GetBytes(str);
                    fs.Write(data, 0, data.Length);
                }
            }
            public void Save()
            {
                if (!isChange)
                {
                    return;
                }
                File.Delete($"{path}/{_languageWord}_{_languageValue}.txt");
                using (FileStream fs = new FileStream($"{path}/{_languageWord}_{_languageValue}.txt", FileMode.Create, FileAccess.Write))
                {
                    string str = "";
                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        if (i > 0)
                        {
                            str += ";";
                        }
                        var Key = dictionary.ElementAt(i).Key;
                        str += Key + "-";
                        for (int j = 0; j < dictionary[Key].Count; j++)
                        {
                            if (j > 0)
                            {
                                str += ",";
                            }
                            str += dictionary[Key][j];
                        }
                    }
                    byte[] data = Encoding.UTF8.GetBytes(str);
                    fs.Write(data, 0, data.Length);
                }
            }
            private void Open()
            {
                using (FileStream fs = new FileStream($"{path}/{_languageWord}_{_languageValue}.txt", FileMode.OpenOrCreate, FileAccess.Read))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    if (data.Length > 0)
                    {
                        string str = Encoding.UTF8.GetString(data);
                        string[] musStr1 = str.Split(';');
                        for (int i = 0; i < musStr1.Length; i++)
                        {
                            string[] musStr2 = musStr1[i].Split('-');
                            string word = musStr2[0];
                            List<string> newList = new List<string>(musStr2[1].Split(','));
                            dictionary.Add(word, newList);
                        }
                    }
                }
            }
        }
    }
}
