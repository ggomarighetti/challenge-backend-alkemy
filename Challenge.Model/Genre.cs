using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Model
{
    public class Genre
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public List<Production> Productions { get; set; }
    }
}
