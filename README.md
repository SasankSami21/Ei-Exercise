# Exercise - 1

## Overview

This exercise demonstrates the implementation of various design patterns categorized into behavioral, creational, and structural patterns within a C# console application. The application includes features for logging significant events, managing user interactions, and showcasing robust task management using design patterns that enhance code maintainability and scalability.

### Table of Contents

1. [Behavioral Patterns](#behavioral-patterns)
   - [Observer Pattern](#observer-pattern)
   - [Command Pattern](#command-pattern)
2. [Creational Patterns](#creational-patterns)
   - [Factory Method Pattern](#factory-method-pattern)
   - [Singleton Pattern](#singleton-pattern)
3. [Structural Patterns](#structural-patterns)
   - [Adapter Pattern](#adapter-pattern)
   - [Facade Pattern](#facade-pattern)
4. [Technologies Used](#technologies-used)

### Behavioral Patterns

#### Observer Pattern

The Observer Pattern is implemented to enable a `Student` class to notify registered `Teacher` observers about changes in its state, specifically grade updates. This decouples the student and teachers, allowing for flexible and maintainable interactions.

**Key Components**:
- **IObservable**: Interface that defines methods for registering, unregistering, and notifying observers.
- **IObserver**: Interface for observers to receive updates.
- **Student**: Concrete class that manages the list of observers and sends notifications on grade changes.
- **Teacher**: Concrete observer that receives updates when a student’s grade changes.

#### Command Pattern

The Command Pattern encapsulates commands as objects, allowing for parameterization of objects with operations. A `ControlPanel` class manages commands to start and stop lectures, improving code organization and reusability.

**Key Components**:
- **ICommand**: Interface for command execution.
- **Lecture**: Class representing a lecture with methods to start and stop it.
- **StartLectureCommand**: Command implementation for starting a lecture.
- **StopLectureCommand**: Command implementation for stopping a lecture.

### Creational Patterns

#### Factory Method Pattern

The Factory Method Pattern is used to create instances of different types of courses without specifying the exact class of object that will be created. The `CourseFactory` creates either an `OnlineCourse` or `InPersonCourse` based on user input.

**Key Components**:
- **Course**: Abstract base class defining the course interface.
- **OnlineCourse**: Concrete class representing an online course.
- **InPersonCourse**: Concrete class representing an in-person course.
- **CourseFactory**: Static factory class for creating course instances based on type.

#### Singleton Pattern

The Singleton Pattern ensures that there is only one instance of the `CourseCatalog` class, which manages all courses throughout the application. This pattern provides a global point of access to the course catalog.

**Key Components**:
- **CourseCatalog**: Class designed to hold a list of courses and maintain a single instance.

### Structural Patterns

#### Adapter Pattern

The Adapter Pattern allows incompatible interfaces to work together, enabling the application to use different payment processors seamlessly. `PayPalAdapter` and `CreditCardAdapter` adapt their respective payment methods to a common interface.

**Key Components**:
- **IPaymentProcessor**: Interface for processing payments.
- **PayPal**: Concrete payment processing class for PayPal.
- **CreditCard**: Concrete payment processing class for credit card payments.
- **PayPalAdapter**: Adapter for integrating PayPal with the payment processor interface.
- **CreditCardAdapter**: Adapter for integrating credit card payments with the payment processor interface.

#### Facade Pattern

The Facade Pattern simplifies interactions with the underlying system. The `CourseEnrollmentFacade` class provides a simplified interface to the complex process of enrolling in a course and processing payments.

**Key Components**:
- **CourseEnrollmentFacade**: A class that unifies the enrollment process and payment processing through a single method.

### Technologies Used

- C#/.NET
- Git for version control

# Exercise - 2
## Astronaut Schedule Manager

### Overview

The Astronaut Schedule Manager is a console application designed to efficiently manage astronaut schedules through various behavioral, creational, and structural design patterns. The application provides functionalities for logging, user interaction, and task management, ensuring high performance and robust error handling.

### Table of Contents

1. [Technologies Used](#technologies-used)


### Technologies Used

- C#/.NET
- NLog for logging
- Git for version control

