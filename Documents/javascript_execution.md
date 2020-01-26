
# JavaScript Execution

Chromely allows JavaScript execution in C# code via implementaion of [IChromelyJavaScriptExecutor](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.Core/IChromelyJavaScriptExecutor.cs). A [default implementation]() is provided but this is can be changed via registration of a custom executor.

To register a custom exeuctor:

````csharp
    var config = DefaultConfiguration.CreateForRuntimePlatform();
    config.JavaScriptExecutor  = new CustomJavaScriptExecutor();

    public class CustomJavaScriptExecutor : IChromelyJavaScriptExecutor
    {
    }
````

To "Execute" a script on the main frame, a "frameName" is not required. To "Execute" on an **iframe** a "frameName" is required. To get the "frameName", the developer will have to declare that in the  **iframe** object.

````javascript
  <iframe id="demoframe" name="alldemoframe" .. />
  </div>
````
#### Executing the script on main frame
````csharp
    var javaScriptExecutor  = new CustomJavaScriptExecutor()
    javaScriptExecutor.Execute(script);
````
#### Evaluating the script using a frame name
````csharp
  var javaScriptExecutor  = new CustomJavaScriptExecutor()
    javaScriptExecutor.Execute("alldemoframe", script);
````
