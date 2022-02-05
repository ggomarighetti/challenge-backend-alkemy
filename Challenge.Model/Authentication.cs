using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Model
{
    public class Authentication
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        public bool Validate()
        {
            ValidationContext context = new(this, serviceProvider: null, items: null);
            List<ValidationResult> results = new();

            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}
