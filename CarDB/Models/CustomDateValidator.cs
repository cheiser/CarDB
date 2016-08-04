using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
//using System.Text.RegularExpressions;

namespace CarDB.Models
{
    /**
      * This class is used for custom data validation. It is used to validate the year attribute for
      * the cars. This is primarily used for testing the creation of a custom validator in MVC 5.
      */
    public class CustomDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                DateTime carYear = (DateTime) value; // cast to DateTime... Or is it string?
                if(carYear.Year < 1900)
                {
                    // invalid year value
                    return new ValidationResult("There existed no cars at this time");
                }
                else if(carYear.Year > (DateTime.Today.Year + 2))
                {
                    // invalid year value, the value was greater than todays year + 2 years
                    return new ValidationResult("This car does not exist yet");
                }

            }
            return ValidationResult.Success;
            // it is within the acceptable span so we check the base validation aswell
            //return base.IsValid(value, validationContext);
            //return (base.IsValid(value) == true? ValidationResult.Success : new ValidationResult("ERROR"));
        }
    }
}