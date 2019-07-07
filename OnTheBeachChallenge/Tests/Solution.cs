using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;

namespace OnTheBeachChallenge
{
    public partial class Solution
    {
        /// <summary>
        /// Contains unit tests for methods of class solution. Uses NUnit.
        /// </summary>
        [TestFixture]
        public class TestClass
        {
            private Solution solution = new Solution();

            /// <summary>
            /// Tests ParseInput and sees that valid input strings are converted to jobs correctly. 
            /// </summary> 
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

            /// <summary>
            /// Tests ParsedInput and sees that dependencies are parsed and assigned correctly.
            /// </summary>
            [Test]
            public void UT_ParseInput_Dependencies()
            {
                var jobsList = new List<char> { 'a', 'b' };
                var dependenciesList = new List<List<char>> { new List<char>(), new List<char>() { 'b' } };
                var parsedJobsList = this.solution.ParseInput(new List<string>() { "a =>", "b => b" });

                TestDependenciesList(jobsList, dependenciesList, parsedJobsList);

                jobsList = new List<char> { 'a', 'b', 'c', 'd' };
                dependenciesList = new List<List<char>> {
                                                new List<char>() { 'a' },
                                                new List<char>() { 'b', 'a' },
                                                new List<char>(),
                                                new List<char>() { 'd' },
                                            };
                parsedJobsList = this.solution.ParseInput(new List<string>() { "a => a", "b => b", "b => a", "c", "d => d", "d => " });

                TestDependenciesList(jobsList, dependenciesList, parsedJobsList);
            }

            /// <summary>
            /// Tests ParseInput() and sees that exception is thrown on receiving incorrectly formatted job string.
            /// </summary>
            [Test]
            public void UT_ParseInput_InvalidFormat()
            {
                string titleException = "Each job must be represented by a single character"; ;
                string formatException = "Invalid Format of Jobs";
                TestInvalidInputFormats(new List<string>() { "=>" }, titleException);
                TestInvalidInputFormats(new List<string>() { "=> a" }, titleException);
                TestInvalidInputFormats(new List<string>() { "=> abc" }, titleException);
                TestInvalidInputFormats(new List<string>() { "abc" }, titleException);
                TestInvalidInputFormats(new List<string>() { "abc => a" }, titleException);
                TestInvalidInputFormats(new List<string>() { "abc=>abc" }, titleException);
                TestInvalidInputFormats(new List<string>() { "a => a => " }, formatException);
                TestInvalidInputFormats(new List<string>() { "a => a => abc" }, formatException);
                TestInvalidInputFormats(new List<string>() { "a => a => a" }, formatException);
            }

            /// <summary>
            /// Basic tests for output from GetJobSequence().
            /// </summary>
            [Test]
            public void UT_GetJobSequence_Basic()
            {
                TestJobSequence(new List<string>() { "a" }, "a");
                TestJobSequence(new List<string>() { "a", "b", "c" }, "abc");
                TestJobSequence(new List<string>() { "a => ", "b => c", "c =>" }, "acb");
                var result = this.solution.GetJobSequence(new List<string>() { "a", "b => c", "c => f", "d => a", "e => b", "f" });
                Assert.IsTrue(result.Length == 6 &&
                              IsBefore(result, 'c', 'b') &&
                              IsBefore(result, 'f', 'c') &&
                              IsBefore(result, 'a', 'd') &&
                              IsBefore(result, 'b', 'e') &&
                              result.Contains('a'));
            }

            /// <summary>
            /// Basic tests for output from GetJobSequence().
            /// </summary>
            public void UT_GetJobSequence_circularInput()
            {
                TestcircularDependency(new List<string>() { "a => a" });
                TestcircularDependency(new List<string>() { "a => ", "b => ", "c => c" });
                TestcircularDependency(new List<string>() { "a =>", "b =>c", "c => f", "d => a", " e =>", "f=>b" });
            }

            #region HelperFunctions

            private void TestJobsList(List<char> actualJobsList, List<Job<char>> parsedJobsList)
            {
                Console.WriteLine("Checking number of jobs returned....");
                Assert.AreEqual(actualJobsList.Count, parsedJobsList.Count);
                Console.WriteLine("Checking parsed job titles...");
                for (var i = 0; i < actualJobsList.Count; i++)
                    Assert.IsTrue(actualJobsList[i] == parsedJobsList[i].Title);
            }

            private void TestDependenciesList(List<char> jobsList, List<List<char>> dependenciesList, List<Job<char>> parsedJobsList)
            {
                Assert.AreEqual(jobsList.Count, parsedJobsList.Count);
                for (var i = 0; i < jobsList.Count; i++)
                    Assert.AreEqual(jobsList[i], parsedJobsList[i].Title);

                for (var i = 0; i < dependenciesList.Count; i++)
                {
                    Assert.AreEqual(dependenciesList[i].Count, parsedJobsList[i].Dependencies.Count);
                    for (var j = 0; j < dependenciesList[i].Count; j++)
                        Assert.IsTrue(dependenciesList[i][j] == parsedJobsList[i].Dependencies[j].Title, "");
                }
            }

            private void TestInvalidInputFormats(List<string> inputs, string exceptionMessage)
            {
                var result = Assert.Throws<ArgumentException>(() => this.solution.ParseInput(inputs));
                Assert.IsTrue(result.Message == exceptionMessage);
            }

            private void TestJobSequence(List<string> inputs, string output)
            {
                var result = this.solution.GetJobSequence(inputs);
                Assert.IsTrue(result == output);
            }

            private bool IsBefore(string str, char first, char second)
            {
                return str.Contains(first) && str.Contains(second) && str.IndexOf(first) < str.IndexOf(second);
            }

            private void TestcircularDependency(List<string> inputs)
            {
                var result = Assert.Throws<ArgumentException>(() => this.solution.GetJobSequence(inputs));
                Assert.IsTrue(result.Message == "Input contains circular dependency");
            }

            #endregion
        }
    }
}