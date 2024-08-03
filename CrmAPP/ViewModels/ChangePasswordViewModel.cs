using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrmAPP.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Aktuelles Passwort darf nicht leer sein.")]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuelles Passwort")]
        [StringLength(64, ErrorMessage = "Maximale Länge: 64 Zeichen.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Neues Passwort darf nicht leer sein.")]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Passwort")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Das Passwort muss zwischen 6 und 64 Zeichen lang sein.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Passwortbestätigung darf nicht leer sein.")]
        [Compare("NewPassword", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Passwort bestätigen")]
        public string ConfirmPassword { get; set; }
    }
}