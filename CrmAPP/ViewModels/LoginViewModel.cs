using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrmAPP.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-Mail ist erforderlich!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich!")]
        public string Passwort { get; set; }

        //public bool RememberMe { get; set; }
    }
}