# devmark-dotnet

This is a simple benchmark tool that measures how fast your computer can run builds.

Currently, it includes two benchmarks: one for Angular applications, one for .net applications.

This tool measures the overall time taken to build an application.

# usage

Build devmark-dotnet:


dotnet publish --self-contained --runtime win-x64 --configuration release
The .exe will be published to devmark-dotnet\bin\Release\netcoreapp2.2\win-x64\publish\devmark-dotnet.exe

Run the angular benchmark:

devmark-dotnet angular.json

Run the .net benchmark:

devmark-dotnet dotnet.json

Override the temp path used to execute builds:

devmark-dotnet new\path\to\use benchmark.json

You should manually delete the temp folder after running the benchmark, they can become quite big (> 1 GB for each run).


# current limitations

- The tool does not yet recognise failed test steps.
- In the dotnet benchmark, xUnit sometimes shows error dialog boxes which have to be closed manually.
- There are no unit tests yet.
- The benchmark probably only runs on Window.


