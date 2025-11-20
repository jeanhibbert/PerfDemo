
Tips and Tricks for C# code performance optimization




Tips and tricks into optimising performance

- sharplab.io
- zlinq
- benchmark dotnet
- ilspy
- visual studio profiler
- stephen taub blog
- otimizing dictionary usage

Diagnosing Performance issues

	- finding the hotpath using visual studio profiler
	- understanding the garbage collector
	- using dotnet memory collector
		- generating a csv
		- using AI to analyse the output
		
Advice : General do's and dont's

	- logging in dot net core 7+
	- never throw exceptions
		- use the result pattern
	- working with strings & SPAN
	- keep your code self documenting by default
	- Know your enemy : I/O & database access
	- Peek under the hood: Read Stephen Taub's blog posts
		- demo frozen collections
		- Demo using span to view memory in dictionary (NC)
	
Emergency optimisations
	- Preparing for war
		- isolate code using dependency injection
		- unit test the code to protect the behaviour of the classes
		- Create a benchmark project to benchmark the code
		- evaluate code by lowering it 
			- performance predictions
			- ilspy
		- using SPAN
		- optimising dictionary usage
		