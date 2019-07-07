using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace OnTheBeachChallenge
{
    public partial class Solution
    {
        [TestFixture]
        public class TestClass
        {
            private Solution solution = new Solution();

            [Test]
            public void UT_ParseInput_Basic()
            {
                Console.WriteLine("Checking ParseInput basic...");

                var actualJobsList = new List<char> { 'a' };
                var parsedJobsList = this.solution.ParseInput(new List<string>() { "a =>" });

                TestJobsList(actualJobsList, parsedJobsList);

                Console.WriteLine("Checking ParseInput basic...");

                actualJobsList = new List<char> { 'a', 'b', 'c', 'd' };
                parsedJobsList = this.solution.ParseInput(new List<string>() {
                                                                        "a => b",
                                                                        "a => a",
                                                                        "b => b",
                                                                        "c => b",
                                                                        "d",
                                                                    });

                TestJobsList(actualJobsList, parsedJobsList);

                Console.WriteLine("Checking ParseInput with empty list...");

                actualJobsList = new List<char>();
                parsedJobsList = this.solution.ParseInput(new List<string>());

                TestJobsList(actualJobsList, parsedJobsList);

                Console.WriteLine("Checking ParseInput with list of unevenly space job format...");

                actualJobsList = new List<char> { 'a', 'b', 'c', 'd', 'e' };
                parsedJobsList = this.solution.ParseInput(new List<string>() {
                                                                        "a=>b",
                                                                        "a =>                   a",
                                                                        "b          => b",
                                                                        "c =>b",
                                                                        "c =>               ",
                                                                        "d                      =>",
                                                                        "    e       "
                                                                    });

                TestJobsList(actualJobsList, parsedJobsList);

                Console.WriteLine("Checking ParseInput with list of repetitive jobs...");

                actualJobsList = new List<char> { 'a', 'b', 'c', 'd' };
                parsedJobsList = this.solution.ParseInput(new List<string>() {
                                                                        "a => b",
                                                                        "a => a",
                                                                        "b => b",
                                                                        "c => b",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => ",
                                                                        "d => d",
                                                                    });

                TestJobsList(actualJobsList, parsedJobsList);
            }

            private void TestJobsList(List<char> actualJobsList, List<Job> parsedJobsList)
            {
                Console.WriteLine("Checking number of jobs returned....");
                Assert.AreEqual(actualJobsList.Count, parsedJobsList.Count);
                Console.WriteLine("Checking parsed job titles...");
                for (var i = 0; i < actualJobsList.Count; i++)
                    Assert.IsTrue(actualJobsList[i] == parsedJobsList[i].Title);
            }
        }
    }
}