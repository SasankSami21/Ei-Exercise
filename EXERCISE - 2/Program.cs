using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace AstronautScheduleOrganizer
{
    // Enum for Priority Levels
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    // Observer Pattern Interfaces
    public interface IObserver
    {
        void Update(string message);
    }

    public interface IObservable
    {
        void RegisterObserver(IObserver observer);
        void UnregisterObserver(IObserver observer);
        void NotifyObservers(string message);
    }

    // Task Class
    public class Task
    {
        public string Description { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public PriorityLevel Priority { get; private set; }
        public bool IsCompleted { get; private set; } // Property to mark task as completed

        public Task(string description, TimeSpan startTime, TimeSpan endTime, PriorityLevel priority)
        {
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Priority = priority;
            IsCompleted = false; // Initialize as not completed
        }

        // Method to mark task as completed
        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "Completed" : "Pending";
            return $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}: {Description} [{Priority}] - {status}";
        }
    }

    // Factory Pattern: TaskFactory Class
    public static class TaskFactory
    {
        public static Task CreateTask(string description, string startTimeStr, string endTimeStr, string priorityStr)
        {
            // Validate and parse start time
            if (!TimeSpan.TryParse(startTimeStr, out TimeSpan startTime))
                throw new FormatException("Invalid start time format. Use HH:MM format.");

            // Validate and parse end time
            if (!TimeSpan.TryParse(endTimeStr, out TimeSpan endTime))
                throw new FormatException("Invalid end time format. Use HH:MM format.");

            // Validate and parse priority
            if (!Enum.TryParse(priorityStr, true, out PriorityLevel priority))
                throw new ArgumentException("Invalid priority level. Use Low, Medium, or High.");

            return new Task(description, startTime, endTime, priority);
        }
    }

    // Singleton Pattern: ScheduleManager Class
    public class ScheduleManager : IObservable
    {
        private static readonly Lazy<ScheduleManager> _instance = new Lazy<ScheduleManager>(() => new ScheduleManager());
        private List<Task> tasks;
        private List<IObserver> observers;
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

        // Private constructor to prevent instantiation
        private ScheduleManager()
        {
            tasks = new List<Task>();
            observers = new List<IObserver>();
        }

        // Public property to get the single instance
        public static ScheduleManager Instance => _instance.Value;

        // Add a task after checking for conflicts
        public void AddTask(Task newTask)
        {
            try
            {
                if (IsOverlapping(newTask))
                {
                    string conflictMessage = $"Error: Task '{newTask.Description}' conflicts with existing tasks.";
                    NotifyObservers(conflictMessage);
                    logger.Error(conflictMessage);
                    throw new InvalidOperationException($"Task '{newTask.Description}' conflicts with existing tasks.");
                }

                tasks.Add(newTask);
                tasks = tasks.OrderBy(t => t.StartTime).ToList();
                Console.WriteLine("Task added successfully. No conflicts.");
                NotifyObservers($"Task '{newTask.Description}' has been added.");
                logger.Info($"Task '{newTask.Description}' added.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to add task '{newTask.Description}'.");
                throw;
            }
        }

        // Remove a task by description
        public void RemoveTask(string description)
        {
            try
            {
                var task = tasks.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
                if (task == null)
                {
                    string notFoundMessage = $"Error: Task '{description}' not found.";
                    NotifyObservers(notFoundMessage);
                    logger.Error(notFoundMessage);
                    throw new KeyNotFoundException($"Task '{description}' not found.");
                }

                tasks.Remove(task);
                Console.WriteLine("Task removed successfully.");
                NotifyObservers($"Task '{description}' has been removed.");
                logger.Info($"Task '{description}' removed.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to remove task '{description}'.");
                throw;
            }
        }

        // Edit an existing task by description
        public void EditTask(string description, string newDescription, string newStartTimeStr, string newEndTimeStr, string newPriorityStr)
        {
            try
            {
                var task = tasks.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
                if (task == null)
                {
                    string notFoundMessage = $"Error: Task '{description}' not found.";
                    NotifyObservers(notFoundMessage);
                    logger.Error(notFoundMessage);
                    throw new KeyNotFoundException($"Task '{description}' not found.");
                }

                // Create a temporary task for validation
                Task tempTask = TaskFactory.CreateTask(newDescription, newStartTimeStr, newEndTimeStr, newPriorityStr);

                // Remove the old task temporarily
                tasks.Remove(task);

                // Check for conflicts
                if (IsOverlapping(tempTask))
                {
                    // Re-add the old task before throwing exception
                    tasks.Add(task);
                    string conflictMessage = $"Error: Task '{tempTask.Description}' conflicts with existing tasks.";
                    NotifyObservers(conflictMessage);
                    logger.Error(conflictMessage);
                    throw new InvalidOperationException($"Task '{tempTask.Description}' conflicts with existing tasks.");
                }

                // Update the task details
                tasks.Add(tempTask);
                tasks = tasks.OrderBy(t => t.StartTime).ToList();
                Console.WriteLine("Task edited successfully. No conflicts.");
                NotifyObservers($"Task '{description}' has been edited to '{tempTask.Description}'.");
                logger.Info($"Task '{description}' edited to '{tempTask.Description}'.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to edit task '{description}'.");
                throw;
            }
        }

        // Mark a task as completed by description
        public void CompleteTask(string description)
        {
            try
            {
                var task = tasks.FirstOrDefault(t => t.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
                if (task == null)
                {
                    string notFoundMessage = $"Error: Task '{description}' not found.";
                    NotifyObservers(notFoundMessage);
                    logger.Error(notFoundMessage);
                    throw new KeyNotFoundException($"Task '{description}' not found.");
                }

                if (task.IsCompleted)
                {
                    string alreadyCompletedMessage = $"Error: Task '{description}' is already completed.";
                    NotifyObservers(alreadyCompletedMessage);
                    logger.Error(alreadyCompletedMessage);
                    throw new InvalidOperationException($"Task '{description}' is already completed.");
                }

                task.MarkAsCompleted();
                Console.WriteLine("Task marked as completed.");
                NotifyObservers($"Task '{description}' has been marked as completed.");
                logger.Info($"Task '{description}' marked as completed.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to complete task '{description}'.");
                throw;
            }
        }

        // View all tasks sorted by start time
        public void ViewTasks()
        {
            try
            {
                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks scheduled for the day.");
                    return;
                }

                Console.WriteLine("Scheduled Tasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine(task.ToString());
                }

                logger.Info("Viewed all tasks.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to view tasks.");
                throw;
            }
        }

        // View tasks filtered by priority level
        public void ViewTasksByPriority(PriorityLevel priority)
        {
            try
            {
                var filteredTasks = tasks.Where(t => t.Priority == priority).OrderBy(t => t.StartTime).ToList();

                if (filteredTasks.Count == 0)
                {
                    Console.WriteLine($"No tasks with priority '{priority}' scheduled for the day.");
                    logger.Info($"Viewed tasks by priority '{priority}'. No matching tasks found.");
                    return;
                }

                Console.WriteLine($"Scheduled Tasks with Priority '{priority}':");
                foreach (var task in filteredTasks)
                {
                    Console.WriteLine(task.ToString());
                }

                logger.Info($"Viewed tasks by priority '{priority}'.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to view tasks by priority '{priority}'.");
                throw;
            }
        }

        // Check if the new task overlaps with existing tasks
        private bool IsOverlapping(Task newTask)
        {
            foreach (var task in tasks)
            {
                if (newTask.StartTime < task.EndTime && newTask.EndTime > task.StartTime)
                {
                    return true;
                }
            }
            return false;
        }

        // Observer Pattern Methods
        public void RegisterObserver(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void UnregisterObserver(IObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in observers)
            {
                observer.Update(message);
            }
        }
    }

    // Logger Class (Observer) for Logging Messages using NLog
    public class Logger : IObserver
    {
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public void Update(string message)
        {
            // Determine log level based on message content
            if (message.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
            {
                logger.Error(message);
            }
            else if (message.StartsWith("Warn", StringComparison.OrdinalIgnoreCase))
            {
                logger.Warn(message);
            }
            else
            {
                logger.Info(message);
            }
        }
    }

    // Main Program Class
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize NLog from config
            var loggerConfig = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            LogManager.Configuration = loggerConfig;

            // Initialize Logger and register as observer
            Logger logger = new Logger();
            ScheduleManager.Instance.RegisterObserver(logger);

            bool exit = false;

            Console.WriteLine("===== Astronaut Daily Schedule Organizer =====");
            DisplayCommands();

            while (!exit)
            {
                Console.Write("\nEnter command: ");
                string input = Console.ReadLine().Trim();

                // Split input by spaces to handle commands with parameters
                var commandParts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (commandParts.Length == 0)
                {
                    Console.WriteLine("No command entered. Please try again.");
                    continue;
                }

                string command = commandParts[0].ToLower();
                string parameters = commandParts.Length > 1 ? commandParts[1] : string.Empty;

                try
                {
                    switch (command)
                    {
                        case "add":
                            AddTaskFlow();
                            break;
                        case "remove":
                            RemoveTaskFlow();
                            break;
                        case "edit":
                            EditTaskFlow();
                            break;
                        case "complete":
                            CompleteTaskFlow();
                            break;
                        case "view":
                            ScheduleManager.Instance.ViewTasks();
                            break;
                        case "view_priority":
                            ViewTasksByPriorityFlow();
                            break;
                        case "help":
                            DisplayCommands();
                            break;
                        case "exit":
                            exit = true;
                            Console.WriteLine("Exiting the application.");
                            break;
                        default:
                            Console.WriteLine("Invalid command. Type 'help' to see available commands.");
                            break;
                    }
                }
                catch (FormatException fe)
                {
                    Console.WriteLine($"Input Error: {fe.Message}");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument Error: {ae.Message}");
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine($"Operation Error: {ioe.Message}");
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine($"Lookup Error: {knfe.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error: {ex.Message}");
                }
            }

            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation faults on Linux)
            LogManager.Shutdown();
        }

        // Method to display available commands
        static void DisplayCommands()
        {
            Console.WriteLine("\nAvailable Commands:");
            Console.WriteLine("1. add            - Add a new task");
            Console.WriteLine("2. remove         - Remove an existing task");
            Console.WriteLine("3. edit           - Edit an existing task");
            Console.WriteLine("4. complete       - Mark a task as completed");
            Console.WriteLine("5. view           - View all tasks");
            Console.WriteLine("6. view_priority  - View tasks by priority level");
            Console.WriteLine("7. help           - Display available commands");
            Console.WriteLine("8. exit           - Exit the application");
        }

        // Flow to add a new task
        static void AddTaskFlow()
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine().Trim();

            Console.Write("Enter start time (HH:MM): ");
            string startTime = Console.ReadLine().Trim();

            Console.Write("Enter end time (HH:MM): ");
            string endTime = Console.ReadLine().Trim();

            Console.Write("Enter priority level (Low, Medium, High): ");
            string priority = Console.ReadLine().Trim();

            Task newTask = TaskFactory.CreateTask(description, startTime, endTime, priority);
            ScheduleManager.Instance.AddTask(newTask);
        }

        // Flow to remove an existing task
        static void RemoveTaskFlow()
        {
            Console.Write("Enter task description to remove: ");
            string description = Console.ReadLine().Trim();

            ScheduleManager.Instance.RemoveTask(description);
        }

        // Flow to edit an existing task
        static void EditTaskFlow()
        {
            Console.Write("Enter task description to edit: ");
            string description = Console.ReadLine().Trim();

            Console.Write("Enter new task description: ");
            string newDescription = Console.ReadLine().Trim();

            Console.Write("Enter new start time (HH:MM): ");
            string newStartTime = Console.ReadLine().Trim();

            Console.Write("Enter new end time (HH:MM): ");
            string newEndTime = Console.ReadLine().Trim();

            Console.Write("Enter new priority level (Low, Medium, High): ");
            string newPriority = Console.ReadLine().Trim();

            ScheduleManager.Instance.EditTask(description, newDescription, newStartTime, newEndTime, newPriority);
        }

        // Flow to mark a task as completed
        static void CompleteTaskFlow()
        {
            Console.Write("Enter task description to mark as completed: ");
            string description = Console.ReadLine().Trim();

            ScheduleManager.Instance.CompleteTask(description);
        }

        // Flow to view tasks by priority level
        static void ViewTasksByPriorityFlow()
        {
            Console.Write("Enter priority level to view (Low, Medium, High): ");
            string priorityStr = Console.ReadLine().Trim();

            if (!Enum.TryParse(priorityStr, true, out PriorityLevel priority))
            {
                Console.WriteLine("Invalid priority level. Use Low, Medium, or High.");
                return;
            }

            ScheduleManager.Instance.ViewTasksByPriority(priority);
        }
    }
}
