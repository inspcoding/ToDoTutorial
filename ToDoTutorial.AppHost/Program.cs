using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<ToDoTutorial>("todoTutorial");
builder.Build().Run();
