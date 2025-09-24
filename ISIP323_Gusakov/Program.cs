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

