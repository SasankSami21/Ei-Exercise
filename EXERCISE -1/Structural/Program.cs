using System;
using System.Collections.Generic;

// Payment Processor Interface
public interface IPaymentProcessor
{
    void ProcessPayment(double amount);
}

// PayPal Payment Processor Class
public class PayPal : IPaymentProcessor
{
    public void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing payment of ${amount} through PayPal.");
    }
}

// Credit Card Payment Processor Class
public class CreditCard : IPaymentProcessor
{
    public void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing payment of ${amount} through Credit Card.");
    }
}

// Adapter for PayPal
public class PayPalAdapter : IPaymentProcessor
{
    private PayPal _payPal;

    public PayPalAdapter(PayPal payPal)
    {
        _payPal = payPal;
    }

    public void ProcessPayment(double amount)
    {
        _payPal.ProcessPayment(amount);
    }
}

// Adapter for Credit Card
public class CreditCardAdapter : IPaymentProcessor
{
    private CreditCard _creditCard;

    public CreditCardAdapter(CreditCard creditCard)
    {
        _creditCard = creditCard;
    }

    public void ProcessPayment(double amount)
    {
        _creditCard.ProcessPayment(amount);
    }
}

// Facade for Course Enrollment
public class CourseEnrollmentFacade
{
    private IPaymentProcessor _paymentProcessor;

    public CourseEnrollmentFacade(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public void EnrollInCourse(string courseName, double amount)
    {
        Console.WriteLine($"Enrolling in course: {courseName}");
        _paymentProcessor.ProcessPayment(amount); // Process payment for enrollment
        Console.WriteLine($"Successfully enrolled in {courseName}.\n");
    }
}

// Main Program
public class Program
{
    public static void Main(string[] args)
    {
        // Adapter Pattern Example
        Console.WriteLine("===== Adapter Pattern Example =====");
        
        IPaymentProcessor paypal = new PayPalAdapter(new PayPal());
        IPaymentProcessor creditCard = new CreditCardAdapter(new CreditCard());

        // Using PayPal
        paypal.ProcessPayment(99.99);

        // Using Credit Card
        creditCard.ProcessPayment(49.99);

        Console.WriteLine();

        // Facade Pattern Example
        Console.WriteLine("===== Facade Pattern Example =====");

        CourseEnrollmentFacade facadeWithPayPal = new CourseEnrollmentFacade(paypal);
        facadeWithPayPal.EnrollInCourse("C# Programming", 99.99);

        CourseEnrollmentFacade facadeWithCreditCard = new CourseEnrollmentFacade(creditCard);
        facadeWithCreditCard.EnrollInCourse("Design Patterns", 49.99);
    }
}
