using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Model
{
    public class Production
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime Launch { get; set; }
        public decimal Rating { get; set; }
        public List<Character> Characters { get; set; }
    }
}
