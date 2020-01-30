using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Generator
{
    public class Frame
    {
        public int Number { get; }

        public TimeSpan Time { get; }

        public Frame(int number, TimeSpan time)
        {
            Number = number;
            Time = time;
        }
    }
}
