using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace task3_csharp
{
    class Parser
    {
        private List<String> args;
        public Parser(string[] args)
        {
            this.args = new List<String>();
            foreach(string arg in args){
                this.args.Add(arg);
            }
        }

        public Dictionary<Int32, String> parse()
        {
            if(args.Count < 3)
            {
                throw new ArgumentException("Illegal args size: " + args.Count + ". Must be >= 3.");
            }else if(args.Count % 2 == 0)
            {
                throw new ArgumentException("Illegal number of args: " + args.Count + ". Must be odd.");
            }else if (HasDuplicates())
            {
                throw new ArgumentException("Illegal args. Choices mustn't contain duplicates.");
            }
            Dictionary<Int32, String> choices = new Dictionary<Int32, String>();
            for(int i = 0; i < args.Count; ++i)
            {
                choices.Add(i + 1, args[i]);
            }
            return choices;
        }

        private bool HasDuplicates()
        {
            List<String> copy = args.ConvertAll(s => s.ToLower());
            return args.Count != copy.Distinct().ToList().Count;

        }


    }
}
