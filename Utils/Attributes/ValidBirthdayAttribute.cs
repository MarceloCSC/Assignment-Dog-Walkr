using System;
using System.ComponentModel.DataAnnotations;

namespace DogWalkr.Utils.Attributes
{
    public class ValidBirthdayAttribute : ValidationAttribute
    {
        public ValidBirthdayAttribute()
        { }

        public string GetErrorMessage() => "Por favor, insira uma data de nascimento válida.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var date = (DateTime)value;

                if (DateTime.Compare(date, DateTime.Now) >= 0) return new ValidationResult(GetErrorMessage());
                else return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}