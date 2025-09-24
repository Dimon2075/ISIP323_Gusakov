using System;
using System.Collections.Generic;

class TextSt
{
    public string Text { get; set; }

    public int WordCount { get; set; }
    public string ShortesWord { get; set; }
    public string LongesWord { get; set; }
    public int SentencCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public Dictionary<char, int> LetterFreq { get; set; }

    public TextSt(string Text)
    {
        Text = Text;
        LetterFreq = new Dictionary<char, int>();
    }
}

class Pr
{
    static readonly char[] SentDelimetres = { '.', '?', '!' };
    static readonly char[] WordSepar = { ' ', '\t', '\n', '\r', ',', ';', ':', '-', '(', ')', '"', '«', '»', '—' };
    static readonly HashSet<char> Vowels = new HashSet<char> { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
                                                               'А', 'Е', 'Ё', 'И', 'О', 'У', 'Ы', 'Э', 'Ю', 'Я'};
    static readonly HashSet<char> Consonants = new HashSet<char> { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к',
                                                                   'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф',
                                                                   'х', 'ц', 'ч', 'ш', 'щ',
                                                                   'Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К',
                                                                   'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф',
                                                                   'Х', 'Ц', 'Ч', 'Ш', 'Щ' };

    static List<TextSt> StaticHistory = new List<TextSt>();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while(true)
        {
            string UsInput = ReadUsInput();

            TextSt stats = AnalyzeText(UsInput);

            StaticHistory.Add(stats);

            PrintStatistics(stats);

            Console.WriteLine("\nНовый текст? (д/н)");
            string answer = Console.ReadLine().Trim().ToLower();
            if (answer != "д" && answer != "да")
                break;
        }

        Console.WriteLine("\nВывести статистику по всем текстам? (д/н)");
        string showAll = Console.ReadLine().Trim().ToLower();
        if (showAll =="д" || showAll == "да")
        {
            for (int i = 0; i < StaticHistory.Count; i++)
            {
                Console.WriteLine($"\nСтатистика для текста #{i + 1}:");
                PrintStatistics(StaticHistory[i]);
            }
        }    
    }

    static string ReadUsInput()
    {
        string input;
        do
        {
            Console.WriteLine("Введи текст (минимально соточка слов - 100 слов)");
            input = Console.ReadLine();
            if (input == null) input = "";

            if (input.Length < 100)
                Console.WriteLine($"Текст маловат будет браток {input.Length}, введи больше");
        } while (input.Length < 100);

        return input;
    }

    static TextSt AnalyzeText(string text)
    {
        TextSt stats = new TextSt(text);

        stats.SentencCount = CountSentenes(text);

        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;

        if (words.Length > 0)
        {
            stats.ShortesWord = words[0];
            stats.LongesWord = words[0];

            for(int i  = 1; i < words.Length;i++)
            {
                string word = words[i];
                if (word.Length < stats.ShortesWord.Length)
                    stats.ShortesWord = word;

                if (word.Length > stats.LongesWord.Length)
                    stats.LongesWord = word;
            }
        }
        else
        {
            stats.ShortesWord = "";
            stats.LongesWord = "";
        }

        foreach (char c in text)
        {
            if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                char lowerChar = char.ToLower(c);

                if (Vowels.Contains(c))
                    stats.VowelCount++;
                else if (Consonants.Contains(c))
                    stats.ConsonantCount++;

                if (stats.LetterFreq.ContainsKey(lowerChar))
                    stats.LetterFreq[lowerChar]++;
                else
                    stats.LetterFreq[lowerChar] = 1;
            }    
        }
        return stats;
    }

    static int CountSentenes(string text)
    {
        int count = 0;
        
        for (int i = 0; i < text.Length;i++)
        {
            char ch = text[i];
            for (int j = 0; j < SentDelimetres.Length; j++)
            {
                if(ch == SentDelimetres[j])
                {
                    count++;
                    break;
                }
            }
        }
        return count;
    }

    static string[] SplitIntoWords(string text)
    {
        List<string> wordList = new List<string>();

        int start = -1;

        for (int i = 0; i < text.Length;i++)
        {
            char ch = text[i];
            bool isSeparator = false;

            for (int j = 0; j < WordSepar.Length; j++)
            {
                if (ch == WordSepar[j])
                {
                    isSeparator = true;
                    break;
                }
            }

            if (!isSeparator)
            {
                if (start == -1)
                    start = i;
            }
            else
            {
                if (start != - 1)
                {
                    int length = i - start;
                    if (length > 0)
                    {
                        string word = text.Substring(start, length);
                        word = TrimWord(word);
                        if (word.Length > 0)
                            wordList.Add(word);
                    }
                    start = -1;
                }
            }
        }
        if (start != -1 && start <  text.Length)
        {
            string word = text.Substring(start, text.Length - start);
            word = TrimWord(word);
            if (word.Length > 0)
                wordList.Add(word);
        }
        return wordList.ToArray();
    }
    // Метод обрезки нежелательных знаков препинания с начала и конца слова
    static string TrimWord(string word)
    {
        int left = 0;
        int right = word.Length - 1;

        while (left <= right && !IsLetterOrDigit(word[left]))
            left++;

        while (right >= left && !IsLetterOrDigit(word[right]))
            right--;

        if (left > right)
            return "";

        return word.Substring(left, right - left + 1);
    }

    static bool IsLetterOrDigit(char c)
    {
        return Char.IsLetter(c) || Char.IsDigit(c);
    }

    static void PrintStatistics(TextSt stats)
    {
        Console.WriteLine("\nСтатистика анализа текста:");
        Console.WriteLine($"Общее количество слов: {stats.WordCount}");
        Console.WriteLine($"Самое короткое слово: {stats.ShortesWord}");
        Console.WriteLine($"Самое длинное слово: {stats.LongesWord}");
        Console.WriteLine($"Количество предложений: {stats.SentencCount}");
        Console.WriteLine($"Количество гласных букв: {stats.VowelCount}");
        Console.WriteLine($"Количество согласных букв: {stats.ConsonantCount}");
        Console.WriteLine("Статистика частоты встречаемости букв:");

        List<char> letters = new List<char>(stats.LetterFreq.Keys);
        letters.Sort();

        foreach (char letter in letters)
        {
            Console.WriteLine($"'{letter}': {stats.LetterFreq[letter]}");
        }
    }
}