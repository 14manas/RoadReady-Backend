//using System.ComponentModel.DataAnnotations;

//namespace RoadReady.DTO
//{
//    public class BookedDTO
//    {
//        public int UserId { get; set; }
//        public int CarId { get; set; }
//        public DateTime StartDate { get; set; }
//        public DateTime ReturnDate { get; set; }
//    }
//}
using System;
using System.ComponentModel.DataAnnotations;

namespace RoadReady.DTO
{
    public class BookedDTO
    {
       // [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

       // [Required(ErrorMessage = "Car ID is required.")]
        public int CarId { get; set; }

       // [Required(ErrorMessage = "Start date is required.")]
       // [DataType(DataType.DateTime, ErrorMessage = "Start date must be a valid date and time.")]
        public DateTime StartDate { get; set; }

       // [Required(ErrorMessage = "Return date is required.")]
       // [DataType(DataType.DateTime, ErrorMessage = "Return date must be a valid date and time.")]
       // [GreaterThan(nameof(StartDate), ErrorMessage = "Return date must be after the start date.")]
        public DateTime ReturnDate { get; set; }
    }

    // Custom Validation Attribute
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessage ?? "Make sure the date is greater than the comparison date.";
            if (value == null || !(value is DateTime))
                return new ValidationResult("Valid date is required.");

            var currentValue = (DateTime)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
