using System;

class Program
{
    static void Main(string[] args)
    {
        List<Scripture> scriptures = ReadScripturesFromFile("scriptures.txt");

        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found in the file.");
            return;
        }

        Random random = new Random();
        var randomScripture = scriptures[random.Next(scriptures.Count)];

        while (true)
        {
            Console.Clear();
            Console.WriteLine(randomScripture);

            if (randomScripture.AllWordsHidden)
            {
                Console.WriteLine("\nAll words are hidden. Press Enter to exit.");
                Console.ReadLine();
                break;
            }

            Console.WriteLine("\nPress Enter to hide more words, or type 'quit' to exit.");
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "quit")
                break;

            randomScripture.HideRandomWords();
        }
    }

    static List<Scripture> ReadScripturesFromFile(string filePath)
    {
        List<Scripture> scriptures = new List<Scripture>();

        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] parts = line.Split('-');
            if (parts.Length == 2)
            {
                string reference = parts[0].Trim();
                string text = parts[1].Trim();
                scriptures.Add(new Scripture(reference, text));
            }
        }

        return scriptures;
    }
}

class Scripture
{
    private List<Word> words;
    private int wordsToHide;
    private int hiddenCount;

    public bool AllWordsHidden => hiddenCount == wordsToHide;

    public ScriptureReference Reference { get; }

    public Scripture(string reference, string text)
    {
        Reference = new ScriptureReference(reference);
        words = text.Split(' ').Select(word => new Word(word)).ToList();
        wordsToHide = words.Count;
    }

        public void HideRandomWords()
    {
        if (hiddenCount < wordsToHide)
        {
            var random = new Random();
            int wordsToHideNow = random.Next(1, 3);

            for (int i = 0; i < wordsToHideNow; i++)
            {
                List<Word> visibleWords = words.Where(word => !word.IsHidden && !string.IsNullOrWhiteSpace(word.Text)).ToList();

                if (visibleWords.Count > 0)
                {
                    Word wordToHide = visibleWords[random.Next(visibleWords.Count)];
                    wordToHide.Hide();
                    hiddenCount++;
                }
            }
        }
    }

    public override string ToString()
    {
        string hiddenText = string.Join(" ", words.Select(word => word.ToString()));
        return $"{Reference} - {hiddenText}";
    }
}

class ScriptureReference
{
    public string Reference { get; }

    public ScriptureReference(string reference)
    {
        Reference = reference;
    }

    public override string ToString()
    {
        return Reference;
    }
}

class Word
{
    public string Text { get; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public override string ToString()
    {
        if (IsHidden)
        {
            return new string('_', Text.Length);
        }
        else
        {
            return Text;
        }
    }
}
