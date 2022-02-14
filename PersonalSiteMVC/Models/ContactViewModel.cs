using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//Add for server-side validation

namespace PersonalSiteMVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "* Name is required *")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Email is required *")]
        [DataType(DataType.EmailAddress)] //pattern match to valid email format
        public string Email { get; set; }

        [DisplayFormat(NullDisplayText = "~ no subject ~")] //supply message if no subject is supplied
        public string Subject { get; set; }

        [Required(ErrorMessage = "* Message is required *")]
        [UIHint("MultilineText")]
        public string Message { get; set; }

    }
}
