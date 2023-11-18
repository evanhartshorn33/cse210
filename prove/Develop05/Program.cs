using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Base class for goals
public abstract class Goal
{
    protected string name;
    protected bool isCompleted;
    public string Description { get; set; }
    public DateTime Deadline { get; set; }

    public string GetName()
    {
        return name;
    }

    public abstract int CalculatePoints();

    public virtual void MarkComplete()
    {
        isCompleted = true;
    }

    public virtual string DisplayStatus()
    {
        return isCompleted ? "[X]" : "[ ]";
    }

    public bool IsDeadlinePassed()
    {
        return DateTime.Now > Deadline;
    }
}

// Derived classes: SimpleGoal, EternalGoal, ChecklistGoal
public class SimpleGoal : Goal
{
    private int pointsValue;

    public SimpleGoal(string name, int pointsValue)
    {
        this.name = name;
        this.pointsValue = pointsValue;
    }

    public override int CalculatePoints()
    {
        return isCompleted ? pointsValue : 0;
    }
}

public class EternalGoal : Goal
{
    private int pointsPerAction;

    public EternalGoal(string name, int pointsPerAction)
    {
        this.name = name;
        this.pointsPerAction = pointsPerAction;
    }

    public override int CalculatePoints()
    {
        return pointsPerAction;
    }

    public override void MarkComplete()
    {
        // Eternal goals should not be marked as completed explicitly
        // They keep giving points without being "completed"
        // You can leave this method empty as it is or add a comment to signify the intentional behavior
    }
}

public class ChecklistGoal : Goal
{
    private int timesToComplete;
    private int pointsPerAction;
    private int completedTimes;
    private int bonusPoints;

    public int CompletedTimes
    {
        get { return completedTimes; }
        set { completedTimes = value; }
    }

    public int TimesToComplete
    {
        get { return timesToComplete; }
        set { timesToComplete = value; }
    }

    public int BonusPoints
    {
        get { return bonusPoints; }
        set { bonusPoints = value; }
    }

    public ChecklistGoal(string name, int timesToComplete, int pointsPerAction)
    {
        this.name = name;
        this.timesToComplete = timesToComplete;
        this.pointsPerAction = pointsPerAction;
    }

    public override int CalculatePoints()
    {
        return completedTimes < timesToComplete ? pointsPerAction : pointsPerAction * timesToComplete + BonusPoints; // Bonus on completion
    }

    public override void MarkComplete()
    {
        if (completedTimes < timesToComplete)
        {
            completedTimes++;
            if (completedTimes == timesToComplete) // Check if the bonus condition is met
            {
                isCompleted = true; // Mark the goal as completed when the bonus condition is met
            }
        }
        // No need to mark as completed when the bonus condition is met
    }

    public override string DisplayStatus()
    {
        string status = base.DisplayStatus();
        return $"{status} -- currently completed: {completedTimes}/{timesToComplete}";
    }
}

// User class to manage goals
public class User
{
    public List<Goal> Goals { get; set; }
    public int TotalScore { get; set; }

    public User()
    {
        Goals = new List<Goal>();
    }

    public void DisplayGoals()
    {
        for (int i = 0; i < Goals.Count; i++)
        {
            var goal = Goals[i];
            string description = string.IsNullOrEmpty(goal.Description) ? "No description available" : goal.Description;
            Console.WriteLine($"{i + 1}. {goal.DisplayStatus()}: {goal.GetName()} - Deadline: {goal.Deadline} ({description})");
        }
    }

    public void RecordEvent(int goalIndex)
    {
        if (goalIndex >= 0 && goalIndex < Goals.Count)
        {
            Goal selectedGoal = Goals[goalIndex];
            selectedGoal.MarkComplete();
            TotalScore += selectedGoal.CalculatePoints();
            Console.WriteLine($"Event recorded for {selectedGoal.GetName()}. You've earned {selectedGoal.CalculatePoints()} points.");
        }
        else
        {
            Console.WriteLine("Invalid goal index.");
        }
    }

    public string GetProgress()
    {
        string progress = "Goals Progress:\n";
        foreach (var goal in Goals)
        {
            string description = string.IsNullOrEmpty(goal.Description) ? "No description available" : goal.Description;
            progress += $"{goal.GetName()} - {goal.DisplayStatus()} - Deadline: {goal.Deadline} - {description}\n";
        }
        return progress;
    }
}

// Main program class for user interactions
public class EternalQuestProgram
{
    private User currentUser;

    public EternalQuestProgram()
    {
        currentUser = new User();
    }

    public void StartProgram()
    {
        LoadGoals(); // Load previously saved goals (if any)
        DisplayMenu();
    }

    private void DisplayMenu()
    {
        while (true)
        {
            Console.WriteLine($"You have {currentUser.TotalScore} points.");
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Save Goals");
            Console.WriteLine("4. Load Goals");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. Quit");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateNewGoal();
                        break;
                    case 2:
                        currentUser.DisplayGoals();
                        break;
                    case 3:
                        SaveGoals();
                        break;
                    case 4:
                        LoadGoals();
                        break;
                    case 5:
                        RecordEvent();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }

    private void CreateNewGoal()
    {
        Console.WriteLine("The types of goals are:");
        Console.WriteLine("1. Simple goal");
        Console.WriteLine("2. Eternal goal");
        Console.WriteLine("3. Checklist goal");
        Console.WriteLine("Which type of goal would you like to create?");

        int goalTypeChoice;
        if (int.TryParse(Console.ReadLine(), out goalTypeChoice))
        {
            string goalTypeDescription = string.Empty;

            switch (goalTypeChoice)
            {
                case 1:
                    goalTypeDescription = "Simple goal";
                    CreateSimpleGoal();
                    break;
                case 2:
                    goalTypeDescription = "Eternal goal";
                    CreateEternalGoal();
                    break;
                case 3:
                    goalTypeDescription = "Checklist goal";
                    CreateChecklistGoal();
                    break;
                default:
                    Console.WriteLine("Invalid goal type choice.");
                    return;
            }

            Console.WriteLine($"New {goalTypeDescription} created successfully!");
        }
        else
        {
            Console.WriteLine("Invalid goal type choice.");
        }
    }

    private void CreateChecklistGoal()
    {
        Console.WriteLine("Enter goal name:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter a short description for the Checklist goal:");
        string description = Console.ReadLine();

        Console.WriteLine("Enter points for each completion of the goal:");
        if (!int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Invalid points entered.");
            return;
        }

        Console.WriteLine("How many times does this goal need to be accomplished for a bonus?");
        if (!int.TryParse(Console.ReadLine(), out int timesToComplete))
        {
            Console.WriteLine("Invalid input for times to complete.");
            return;
        }

        Console.WriteLine("What is the bonus for accomplishing it that many times?");
        if (!int.TryParse(Console.ReadLine(), out int bonusPoints))
        {
            Console.WriteLine("Invalid input for bonus points.");
            return;
        }

        Console.WriteLine("Enter deadline for the goal (YYYY-MM-DD HH:MM):");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
        {
            currentUser.Goals.Add(new ChecklistGoal(name, timesToComplete, points)
            {
                Description = description,
                BonusPoints = bonusPoints,
                Deadline = deadline
            });
            Console.WriteLine($"New Checklist goal \"{name}\" created successfully!");
        }
        else
        {
            Console.WriteLine("Invalid deadline format. Goal not created.");
        }
    }

    private void CreateEternalGoal()
    {
        Console.WriteLine("Enter goal name:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter a short description for the Eternal goal:");
        string description = Console.ReadLine();

        Console.WriteLine("Enter points for the goal:");
        if (int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Enter deadline for the goal (YYYY-MM-DD HH:MM):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
            {
                currentUser.Goals.Add(new EternalGoal(name, points)
                {
                    Description = description,
                    Deadline = deadline
                });
                Console.WriteLine($"New Eternal goal \"{name}\" created successfully!");
            }
            else
            {
                Console.WriteLine("Invalid deadline format. Goal not created.");
            }
        }
        else
        {
            Console.WriteLine("Invalid points entered. Goal not created.");
        }
    }

    private void CreateSimpleGoal()
    {
        Console.WriteLine("Enter goal name:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter a short description for the Simple goal:");
        string description = Console.ReadLine();

        Console.WriteLine("Enter points for the goal:");
        if (int.TryParse(Console.ReadLine(), out int points))
        {
            Console.WriteLine("Enter deadline for the goal (YYYY-MM-DD HH:MM):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
            {
                currentUser.Goals.Add(new SimpleGoal(name, points)
                {
                    Description = description,
                    Deadline = deadline
                });
                Console.WriteLine($"New Simple goal \"{name}\" created successfully!");
            }
            else
            {
                Console.WriteLine("Invalid deadline format. Goal not created.");
            }
        }
        else
        {
            Console.WriteLine("Invalid points entered. Goal not created.");
        }
    }

    private void RecordEvent()
    {
        Console.WriteLine("Enter the goal number to record event:");
        if (int.TryParse(Console.ReadLine(), out int goalNumber) && goalNumber > 0 && goalNumber <= currentUser.Goals.Count)
        {
            currentUser.RecordEvent(goalNumber - 1);
        }
        else
        {
            Console.WriteLine("Invalid input or goal number out of range.");
        }
    }

    private void SaveGoals()
    {
        try
        {
            string progress = currentUser.GetProgress();
            string json = JsonSerializer.Serialize(new { Goals = currentUser.Goals, Progress = progress });
            File.WriteAllText("goals.json", json);
            Console.WriteLine("Goals saved successfully.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error saving goals: {ex.Message}");
        }
    }

    private void LoadGoals()
    {
        if (File.Exists("goals.json"))
        {
            try
            {
                string json = File.ReadAllText("goals.json");
                var data = JsonSerializer.Deserialize<dynamic>(json);

                currentUser.Goals = JsonSerializer.Deserialize<List<Goal>>(data["Goals"].ToString());
                Console.WriteLine("Goals loaded successfully.");

                string progress = data["Progress"].ToString();
                Console.WriteLine("User Progress:");
                Console.WriteLine(progress);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No saved goals found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        EternalQuestProgram program = new EternalQuestProgram();
        program.StartProgram();
    }
}
