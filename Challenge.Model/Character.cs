using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Model
{
    public class Character
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public string History { get; set; }
        public List<Production> Productions { get; set; }

        public bool Validate()
        {
            ValidationContext context = new(this, serviceProvider: null, items: null);
            List<ValidationResult> results = new();

            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}
