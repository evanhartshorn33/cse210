using System;
using System.Collections.Generic;

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; }
    private List<Comment> comments = new List<Comment>();

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {Length} seconds");
        Console.WriteLine($"Number of comments: {GetNumberOfComments()}");
        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine($"- {comment.Commenter}: {comment.Text}");
        }
        Console.WriteLine();
    }
}

class Comment
{
    public string Commenter { get; set; }
    public string Text { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        // Creating videos
        Video video1 = new Video
        {
            Title = "Video 1",
            Author = "Author 1",
            Length = 120 // 2 minutes
        };
        video1.AddComment(new Comment { Commenter = "User1", Text = "Great video!" });
        video1.AddComment(new Comment { Commenter = "User2", Text = "Nice content!" });

        Video video2 = new Video
        {
            Title = "Video 2",
            Author = "Author 2",
            Length = 180 // 3 minutes
        };
        video2.AddComment(new Comment { Commenter = "User3", Text = "Interesting topic!" });
        video2.AddComment(new Comment { Commenter = "User4", Text = "Could be better." });

        // Add videos to the list
        videos.Add(video1);
        videos.Add(video2);

        // Display video information
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}