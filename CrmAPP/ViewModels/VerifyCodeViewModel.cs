using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CrmAPP.ViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "Bestätigungscode ist erforderlich.")]
        [Display(Name = "Bestätigungscode")]
        public string Code { get; set; }
    }
}