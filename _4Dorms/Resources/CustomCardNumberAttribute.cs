using System.ComponentModel.DataAnnotations;

public class CustomCardNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cardNumber = value as string;

        if (string.IsNullOrEmpty(cardNumber))
        {
            return new ValidationResult("Card number is required.");
        }

        // Add any custom logic here
        // For example, check if the length is 16 digits
        if (cardNumber.Length == 16)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Card number must be 16 digits.");
    }
}