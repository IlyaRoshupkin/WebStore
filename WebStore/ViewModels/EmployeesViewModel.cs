using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeesViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        
        /// <summary>Имя</summary>///
        [Display(Name="First Name")]
        [Required(ErrorMessage = "First name is nesseccery.")]
        [StringLength(15,MinimumLength =3,
            ErrorMessage ="The name`s length must be from 3 to 15 characters.")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)",
            ErrorMessage ="Name format error")]
        public string FirstName { get; set; }
        
        /// <summary>Фамилия</summary>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is nesseccery.")]
        [StringLength(15, MinimumLength = 2, 
            ErrorMessage = "The last name`s length must be from 3 to 15 characters.")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)",
            ErrorMessage = "Name format error")]
        public string LastName { get; set; }
        
        /// <summary>Отчество</summary>
        [Display(Name = "Patronymic")]
        [StringLength(15, MinimumLength = 2,
            ErrorMessage = "The patronymic`s length must be from 3 to 15 characters.")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)",
            ErrorMessage = "Patronymic format error")]
        public string Patronymic { get; set; }
        
        /// <summary>Возраст</summary>
        [Range(16,90,ErrorMessage ="The employees must be from 16 to 90 years old.")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            yield return ValidationResult.Success;
        }
    }
}
