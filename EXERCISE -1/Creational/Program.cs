using System;
using System.Collections.Generic;

// Abstract Course Class
public abstract class Course
{
    public abstract void Enroll(); // Abstract method to enroll in a course
}

// Online Course Class
public class OnlineCourse : Course
{
    public override void Enroll()
    {
        Console.WriteLine("Enrolled in an online course."); // Implementation of enrollment for online course
    }
}

// In-Person Course Class
public class InPersonCourse : Course
{
    public override void Enroll()
    {
        Console.WriteLine("Enrolled in an in-person course."); // Implementation of enrollment for in-person course
    }
}

// Course Factory Class
public static class CourseFactory
{
    public static Course CreateCourse(string type)
    {
        // Create a course based on the type specified
        if (type.Equals("online", StringComparison.OrdinalIgnoreCase))
        {
            return new OnlineCourse(); // Return an instance of OnlineCourse
        }
        else if (type.Equals("in-person", StringComparison.OrdinalIgnoreCase))
        {
            return new InPersonCourse(); // Return an instance of InPersonCourse
        }
        throw new ArgumentException("Invalid course type."); // Throw exception if type is invalid
    }
}

// Singleton Course Catalog Class
public class CourseCatalog
{
    private static CourseCatalog _instance; // Holds the single instance of CourseCatalog
    private List<Course> courses = new List<Course>(); // List to hold courses

    // Private constructor to prevent instantiation from outside
    private CourseCatalog() { }

    // Public method to get the single instance of the CourseCatalog
    public static CourseCatalog GetInstance()
    {
        if (_instance == null) // Check if the instance already exists
        {
            _instance = new CourseCatalog(); // Create the instance if it does not exist
        }
        return _instance; // Return the single instance
    }

    // Method to add a course to the catalog
    public void AddCourse(Course course)
    {
        courses.Add(course); // Add the course to the list
        Console.WriteLine("Course added to the catalog."); // Confirmation message
    }

    // Method to display all courses
    public void DisplayCourses()
    {
        Console.WriteLine("Courses in Catalog:");
        foreach (var course in courses) // Iterate over the courses list
        {
            Console.WriteLine(course.GetType().Name); // Display course type (class name)
        }
    }
}

// Main Program
public class Program
{
    public static void Main(string[] args)
    {
        // Factory Method Pattern Example
        Console.WriteLine("===== Factory Method Pattern Example =====");

        Course onlineCourse = CourseFactory.CreateCourse("online"); // Create an online course using the factory
        onlineCourse.Enroll(); // Enroll in the online course

        Course inPersonCourse = CourseFactory.CreateCourse("in-person"); // Create an in-person course using the factory
        inPersonCourse.Enroll(); // Enroll in the in-person course

        Console.WriteLine();

        // Singleton Pattern Example
        Console.WriteLine("===== Singleton Pattern Example =====");

        CourseCatalog catalog1 = CourseCatalog.GetInstance(); // Get instance of CourseCatalog
        CourseCatalog catalog2 = CourseCatalog.GetInstance(); // Get instance of CourseCatalog again

        // Ensure both variables point to the same instance
        if (catalog1 == catalog2)
        {
            Console.WriteLine("Both catalogs are the same instance."); // Confirm they are the same instance
        }

        // Add courses to the catalog
        catalog1.AddCourse(onlineCourse); // Add online course to the catalog
        catalog1.AddCourse(inPersonCourse); // Add in-person course to the catalog

        // Display courses in the catalog
        catalog1.DisplayCourses(); // Show all courses in the catalog
    }
}
