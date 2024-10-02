using System;
using System.Collections.Generic;

// Observer Pattern Interfaces
public interface IObservable
{
    void Register(IObserver observer);      // Method to register an observer
    void Unregister(IObserver observer);    // Method to unregister an observer
    void NotifyObservers(double grade);     // Method to notify all observers of a grade change
}

public interface IObserver
{
    void Update(double grade);              // Method for observers to receive updates
}

// Concrete implementation of the observable entity (Student)
public class Student : IObservable
{
    private List<IObserver> observers = new List<IObserver>(); // List to hold registered observers
    private double grade; // Variable to hold the student's grade

    // Method to register an observer
    public void Register(IObserver observer)
    {
        observers.Add(observer);
    }

    // Method to unregister an observer
    public void Unregister(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify all registered observers about the grade change
    public void NotifyObservers(double grade)
    {
        foreach (var observer in observers)
        {
            observer.Update(grade);
        }
    }

    // Method to set a new grade and notify observers
    public void SetGrade(double newGrade)
    {
        grade = newGrade; // Update the grade
        NotifyObservers(grade); // Notify observers about the new grade
    }
}

// Concrete implementation of the observer entity (Teacher)
public class Teacher : IObserver
{
    private string name; // Name of the teacher

    // Constructor to set the teacher's name
    public Teacher(string name)
    {
        this.name = name;
    }

    // Method to receive updates from the Student
    public void Update(double grade)
    {
        Console.WriteLine($"Teacher {name} - New grade received: {grade}");
    }
}

// Command Pattern Interfaces
public interface ICommand
{
    void Execute(); // Method to execute the command
}

// Class representing a Lecture
public class Lecture
{
    private string title; // Title of the lecture

    // Constructor to set the lecture's title
    public Lecture(string title)
    {
        this.title = title;
    }

    // Method to start the lecture
    public void Start()
    {
        Console.WriteLine($"Lecture '{title}' has started.");
    }

    // Method to stop the lecture
    public void Stop()
    {
        Console.WriteLine($"Lecture '{title}' has stopped.");
    }
}

// Command to start a lecture
public class StartLectureCommand : ICommand
{
    private Lecture lecture; // Reference to the lecture

    // Constructor to set the lecture
    public StartLectureCommand(Lecture lecture)
    {
        this.lecture = lecture;
    }

    // Execute the command to start the lecture
    public void Execute()
    {
        lecture.Start();
    }
}

// Command to stop a lecture
public class StopLectureCommand : ICommand
{
    private Lecture lecture; // Reference to the lecture

    // Constructor to set the lecture
    public StopLectureCommand(Lecture lecture)
    {
        this.lecture = lecture;
    }

    // Execute the command to stop the lecture
    public void Execute()
    {
        lecture.Stop();
    }
}

// Control panel that manages commands
public class ControlPanel
{
    private ICommand command; // Holds the current command

    // Set the command to be executed
    public void SetCommand(ICommand command)
    {
        this.command = command;
    }

    // Press the button to execute the command
    public void PressButton()
    {
        if (command != null)
        {
            command.Execute(); // Execute the command if it is set
        }
    }
}

// Main class to demonstrate the Observer and Command patterns
public class Program
{
    public static void Main(string[] args)
    {
        // Observer Pattern Example
        Console.WriteLine("===== Observer Pattern Example =====");

        // Create a student instance
        Student student = new Student();

        // Create teacher instances
        Teacher teacher1 = new Teacher("Mr. Prasanna");
        Teacher teacher2 = new Teacher("Ms. Muthu Lakshmi");

        // Register teachers as observers of the student
        student.Register(teacher1);
        student.Register(teacher2);

        // Update the student's grade, notifying the teachers
        student.SetGrade(85.5);
        student.SetGrade(90.0);

        // Unregister one teacher
        student.Unregister(teacher1);

        // Update the student's grade again
        student.SetGrade(95.0);

        Console.WriteLine();

        // Command Pattern Example
        Console.WriteLine("===== Command Pattern Example =====");

        // Create a lecture instance
        Lecture mathLecture = new Lecture("Math 101");

        // Create command instances for starting and stopping the lecture
        ICommand startLectureCommand = new StartLectureCommand(mathLecture);
        ICommand stopLectureCommand = new StopLectureCommand(mathLecture);

        // Create a control panel to manage lecture commands
        ControlPanel controlPanel = new ControlPanel();

        // Start the lecture
        controlPanel.SetCommand(startLectureCommand);
        controlPanel.PressButton();

        // Stop the lecture
        controlPanel.SetCommand(stopLectureCommand);
        controlPanel.PressButton();
    }
}
