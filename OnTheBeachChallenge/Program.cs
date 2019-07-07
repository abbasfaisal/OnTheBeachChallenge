using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheBeachChallenge
{
    class Program
    {
        static bool isInvalidSequence { get; set; }
        static List<char> DoneChars = new List<char>();
        static List<char> markedChars = new List<char>();

        public class Job
        {
            public char X { get; set; }
            public List<Job> Dependents { get; set; }
            public Job(char x)
            {
                this.X = x;
                this.Dependents = new List<Job>();
            }
        }

        public static void GetJobSequence(List<Job> jobs, List<char> markedChars)
        {
            for (var i = 0; i < jobs.Count(); i++)
            {
                if (isInvalidSequence)
                    return;

                if (!DoneChars.Contains(jobs[i].X))
                {
                    if (markedChars.Contains(jobs[i].X))
                    {
                        isInvalidSequence = true;
                        return;
                    }

                    markedChars.Add(jobs[i].X);
                    GetJobSequence(jobs[i].Dependents, markedChars);
                    DoneChars.Add(jobs[i].X);
                    markedChars.Clear();
                }
            }

        }

        static void Main(string[] args)
        {
            Job j1 = new Job('a');
            Job j2 = new Job('b');
            Job j3 = new Job('c');
            Job j4 = new Job('d');
            Job j5 = new Job('e');
            Job j6 = new Job('f');

            j2.Dependents.Add(j3);
            j3.Dependents.Add(j6);
            j4.Dependents.Add(j1);
            j5.Dependents.Add(j2);

            var jobs = new List<Job>() { j1, j2, j3, j4, j5, j6 };
            var markedChars = new List<Char>();
            GetJobSequence(jobs, markedChars);
            Console.WriteLine(isInvalidSequence);
            DoneChars.ForEach(d =>
                Console.WriteLine(d)
            );
            Console.Read();
        }
    }
}
