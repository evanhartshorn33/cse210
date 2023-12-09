using System;
using System.Collections.Generic;

// Base Activity class
abstract class Activity
{
    protected DateTime date;
    protected int durationInMinutes;

    public Activity(DateTime date, int durationInMinutes)
    {
        this.date = date;
        this.durationInMinutes = durationInMinutes;
    }

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    public virtual string GetSummary()
    {
        return $"{date.ToShortDateString()} {GetType().Name} ({durationInMinutes} min): " +
               $"Distance {GetDistance():F2} miles, Speed {GetSpeed():F2} mph, Pace: {GetPace():F2} min per mile";
    }
}

// Running derived class
class Running : Activity
{
    private double distance;

    public Running(DateTime date, int durationInMinutes, double distance)
        : base(date, durationInMinutes)
    {
        this.distance = distance;
    }

    public override double GetDistance()
    {
        return distance;
    }

    public override double GetSpeed()
    {
        return distance / (durationInMinutes / 60);
    }

    public override double GetPace()
    {
        return durationInMinutes / distance;
    }
}

// Stationary Bicycle derived class
class StationaryBicycle : Activity
{
    private double speed;

    public StationaryBicycle(DateTime date, int durationInMinutes, double speed)
        : base(date, durationInMinutes)
    {
        this.speed = speed;
    }

    public override double GetDistance()
    {
        return speed * (durationInMinutes / 60);
    }

    public override double GetSpeed()
    {
        return speed;
    }

    public override double GetPace()
    {
        return 60 / speed;
    }
}

// Swimming derived class
class Swimming : Activity
{
    private int laps;

    public Swimming(DateTime date, int durationInMinutes, int laps)
        : base(date, durationInMinutes)
    {
        this.laps = laps;
    }

    public override double GetDistance()
    {
        return laps * 50 / 1000; // Distance in kilometers
    }

    public override double GetSpeed()
    {
        return GetDistance() / (durationInMinutes / 60); // Speed in km/h
    }

    public override double GetPace()
    {
        return durationInMinutes / GetDistance(); // Pace in minutes per kilometer
    }

    public override string GetSummary()
    {
        return $"{date.ToShortDateString()} Swimming ({durationInMinutes} min): " +
               $"Distance {GetDistance():F2} km, Speed {GetSpeed():F2} kph, Pace: {GetPace():F2} min per km";
    }
}

class Program
{
    static void Main()
    {
        List<Activity> activities = new List<Activity>();

        // Creating sample activities
        Activity runningActivity = new Running(new DateTime(2022, 11, 3), 30, 3.0);
        Activity cyclingActivity = new StationaryBicycle(new DateTime(2022, 11, 3), 30, 6.0);
        Activity swimmingActivity = new Swimming(new DateTime(2022, 11, 3), 30, 10);

        // Adding activities to the list
        activities.Add(runningActivity);
        activities.Add(cyclingActivity);
        activities.Add(swimmingActivity);

        // Displaying summaries of activities
        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}