using System;
using System.Threading;
class Activity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }

    public Activity(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public virtual void Start()
    {
        Console.WriteLine($"Starting {Name} activity...");
        Console.WriteLine(Description);
        Console.WriteLine($"Duration: {Duration} seconds");
        Console.WriteLine("Prepare to begin in 3 seconds...");
        Thread.Sleep(3000);
    }

    public virtual void End()
    {
        Console.WriteLine("Well done!!");
        Console.WriteLine($"You have completed {Duration} seconds of the {Name} activity.");
        Console.WriteLine("Returning to the main menu...");
        Thread.Sleep(3000);
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing Activity", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.") { }

    public override void Start()
    {
        base.Start();
        int elapsedSeconds = 0;

        while (elapsedSeconds < Duration)
        {
            Console.WriteLine("Breathe in...");

            for (int countdown = 4; countdown >= 1; countdown--)
            {
                Console.Write(countdown + ", ");
                Thread.Sleep(1000);
            }
            Console.WriteLine();

            elapsedSeconds += 4;

            if (elapsedSeconds >= Duration)
                break;

            Console.WriteLine("Breathe out...");

            for (int countdown = 6; countdown >= 1; countdown--)
            {
                Console.Write(countdown + ", ");
                Thread.Sleep(1000);
            }
            Console.WriteLine();

            elapsedSeconds += 6;
        }
        base.End();
    }
}
class ReflectionActivity : Activity
{
    private List<string> reflectionQuestions = new List<string>
    {
        "Think of a time when you did something really difficult",
        "Can you think of a situation where you faced a significant challenge or adversity? What was the challenge, and how did you handle it?",
        "Have there been moments when you had to overcome self-doubt or uncertainty? How did you find the strength to move forward?",
        "Reflect on a time when you achieved a goal that required determination and resilience. What kept you motivated?",
        "Think about a difficult decision you had to make. How did you make that decision, and what was the outcome?",
        "Were there occasions when you faced criticism or setbacks? How did you respond, and what did you learn from those experiences?",
        "Consider a time when you supported someone else during a tough period. How did your support make a difference?",
        "Have you ever turned a failure into an opportunity for growth? How did you approach this transformation?",
        "Reflect on moments when you felt out of your comfort zone but pushed yourself to try something new. What did you discover about your capabilities?",
        "Think about a situation where you demonstrated patience and perseverance. How did these qualities contribute to a positive outcome?",
        "Were there times when you had to adapt to unexpected changes or challenges? How did your adaptability help you navigate those situations?",
    };

    public ReflectionActivity() : base("Reflection Activity", "This activity will help you reflect on times in your life when you have shown strength and resilience.") { }

    public override void Start()
    {
        base.Start();

        Random random = new Random();

        while (reflectionQuestions.Count > 0)
        {
            int index = random.Next(reflectionQuestions.Count);
            string question = reflectionQuestions[index];
            reflectionQuestions.RemoveAt(index);

            Console.WriteLine("Think about a time when you demonstrated strength.");
            Console.WriteLine($"Reflect on the following question: {question}");
            Console.WriteLine("When you have something in mind, press Enter to continue.");
            Console.ReadLine();

            Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
            Thread.Sleep(2000);
            Console.WriteLine("You may begin in: 3");
            Thread.Sleep(1000);
            Console.WriteLine("2");
            Thread.Sleep(1000);
            Console.WriteLine("1");

            // Simulate a pause before asking the questions
            
            Thread.Sleep(2000);

            Console.WriteLine("How did you feel when it was complete?");
            Thread.Sleep(10000); // Allow 10 seconds for reflection

            Console.WriteLine("What is your favorite thing about this experience?");
            Thread.Sleep(10000); // Allow 10 seconds for reflection
            
            // Wait for the user to press Enter
            Console.ReadLine();
        }

        base.End();
    }
}
class ListingActivity : Activity
{
    private static string[] prompts = {
        "When have you felt the Holy Ghost this month?",
        "Share your favorite childhood memory.",
        "What are your goals for the next year?",
        "List your top 5 favorite books.",
        "Name 10 things that make you happy.",
        "List the places you want to visit.",
        "What are your favorite hobbies.",
        "Write down your favorite quotes.",
        "Name people who inspire you.",
        "What are your life's biggest achievements."
    };

    public ListingActivity() : base("Listing Activity", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.") { }

    public override void Start()
    {
        base.Start();

        Console.WriteLine("Get ready...");

        string randomPrompt = prompts[new Random().Next(prompts.Length)];
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine(randomPrompt);

        Console.WriteLine("You may begin in:");
        Thread.Sleep(1000);
        Console.WriteLine("3");
        Thread.Sleep(1000);
        Console.WriteLine("2");
        Thread.Sleep(1000);
        Console.WriteLine("1");

        List<string> userResponses = new List<string>();
        DateTime endTime = DateTime.Now.AddSeconds(Duration);

        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string response = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(response))
                break;
            userResponses.Add(response);
        }

        Console.WriteLine($"You listed {userResponses.Count} items!");
        base.End();
    }
}

class MeditationActivity : Activity
{
    public MeditationActivity() : base("Meditation Activity", "This activity will guide you through a meditation session to promote relaxation and inner peace.") { }

    public override void Start()
    {
        base.Start();

        int elapsedSeconds = 0;
        while (elapsedSeconds < Duration)
        {
            Console.WriteLine("Focus on your breath and clear your mind...");
            Console.WriteLine("Transport your imagination to a tranquil forest bathed in a warm, golden light. You'll find solace under an ancient oak tree, its branches forming a protective canopy. As you sit, the forest's serenity envelops you, and the quietude is only interrupted by the gentle rustling of leaves in the breeze. Inhale the pure forest air deeply, and with each exhale, release any stress or worries, allowing them to dissipate like morning mist. As you embark on a peaceful journey along a forest path, wildflowers and the songs of birds accompany your steps, filling you with a sense of grace and calm.");
            Console.WriteLine("Close your eyes and breathe");

            Thread.Sleep(10000); // Wait for 10 seconds

            elapsedSeconds += 10;
        }

        base.End();
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.WriteLine("5. Meditation Activity");
            Console.Write("Select a choice from the menu: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        BreathingActivity breathing = new BreathingActivity();
                        SetDuration(breathing);
                        breathing.Start();
                        break;
                    case 2:
                        ReflectionActivity reflection = new ReflectionActivity();
                        SetDuration(reflection);
                        reflection.Start();
                        break;
                    case 3:
                        ListingActivity listing = new ListingActivity();
                        SetDuration(listing);
                        listing.Start();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    case 5:
                        MeditationActivity meditation = new MeditationActivity();
                        SetDuration(meditation);
                        meditation.Start();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Thread.Sleep(2000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                Thread.Sleep(2000);
            }
        }
    }

        static void SetDuration(Activity activity)
    {
        Console.WriteLine($"Welcome to the {activity.Name}.");
        Console.WriteLine(activity.Description);
        Console.Write("How long, in seconds, would you like for your session? ");
        if (int.TryParse(Console.ReadLine(), out int duration))
        {
            activity.Duration = duration;
        }
        else
        {
            Console.WriteLine("Invalid duration. Using the default value of 60 seconds.");
            activity.Duration = 60;
        }
    }

}
