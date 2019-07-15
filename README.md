# On The Beach Code Challenge
This repository contains solution to On The Beach code challenge. Please read on to understand more about the code.

## Language
The challenge has been done in C# as a windows console application.

## Summary of the Solution
Solution sequences jobs by applying topological sorting algorithm using DFS.

## Complexity of the Solution
The solution runs in linear time. Each input job when passed is marked along with it's dependencies and grand-dependencies. So edges of each job are passed only once during a run. This results in linear time complexity.

## Repository Structure
 - JobSequencer.cs contains an abstract generic class which contains code for calculating the sequence and an abstract public interface for input and output. 
 - Solution.cs contains a derived class of JobSequencer which implements it's abstract method, converts input into jobs and outputs the result.

## Unit Tests
 - Tests directory contains unit tests for both of above classes. Further tests are planned to be added here. We use NUnit for writing unit tests.
 - Tests/SolutionTests.cs contains unit tests for methods of Solution class. Our Solution class is divided into two partial classes for functionality and tests respectively.
 - Tests/JobSequencer.cs contains UTs for methods of JobSequence class. This file contains a derived class which tests concrete methods of JobSequencer.
 
## Running and Testing
- Tested on Visual Studio 2015 but should work with most other versions of Visual Studio.
- Main function contains some basic tests. Running the project provides output of these tests.
- Tests directory contains extensive unit tests which can be run by pressing Ctrl+R then A.

## To Do
[JobSequencer](OnTheBeachChallenge/Src/JobSequencer.cs) should only work correctly with struct or string as generic parameter. To make it work with other types of generic parameter, we need a way to check for job title by value in `VisitJob()`. 

## Feedback
Please feel free to provide feedback and suggestions.
