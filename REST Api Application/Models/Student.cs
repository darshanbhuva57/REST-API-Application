using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace REST_API_Application.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        
        [RegularExpression(@"^[1-9]-[A-Fa-f]$")]
        public string Class { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [GenderValidation]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$",ErrorMessage ="Mobile Number must be in this formate xxx-xxx-xxxx")]
        [AllowNull]
        public string MobileNumber { get; set; }
    }

    public class GenderValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value).ToLower() == "male" || Convert.ToString(value).ToLower() == "female")
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage="Invalid Gender");
        }
    }
}
