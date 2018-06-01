var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");

Task("Default")
    .Does(() =>
    {
        Information("Hello world");
    });

RunTarget(target);