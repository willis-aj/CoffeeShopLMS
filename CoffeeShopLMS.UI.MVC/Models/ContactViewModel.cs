using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//for metadata

namespace CoffeeShopLMS.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Your Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Your Email is required")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "*Your Message is required")]
        [UIHint("MultilineText")]
        public string Message { get; set; }

    }
}