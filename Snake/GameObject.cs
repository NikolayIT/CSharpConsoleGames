using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public abstract class GameObject
    {
        public Position Position { get; set; }

        public char Symbol { get; set; }
    }
}
