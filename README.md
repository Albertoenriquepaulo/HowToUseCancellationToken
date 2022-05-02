# How to use CancellationToken in your .NET API requests

## Give your clients the ability to cancel their requests instead of always completing it

The ability to stop processing a request if your client decides that it is not necessary anymore is very important.

Some scenario that might happen is if the client sends a request through a web browser but then reloads or close this page, most of the time you don’t want to carry on those changes, as it might lead to data inconsistency.

For such scenarios where you need to stop processing a request when your client requests it, .NET provides us `CancellationToken`. A very useful tool that enables us to cancel an asynchronous request.

# Requirements

If you want to follow along, you need:

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Your favorite IDE/Text Editor (I’ll be using VS Code)
- [Postman](https://www.postman.com/) — To easily make HTTP requests

# Here we go

To demonstrate it, let’s start by creating a new project with:

```shell
dotnet new sln HowToUseCancellationToken
dotnet new webapi -minimal -n HowToUseCancellationToken -o src
dotnet sln add .\HowToUseCancellationToken\HowToUseCancellationToken.csproj
```

Now that we have our project, we can go to the `Program.cs` and replace the `GetWeatherForecast` endpoint for this:

<iframe src="https://medium.com/media/4784d1e832fb54bbeabf2453ca044bf6" allowfullscreen="" frameborder="0" height="193" width="692" title="Endpoint.cs" class="fq aq as ag cf" scrolling="auto" style="box-sizing: inherit; height: 193px; top: 0px; left: 0px; width: 692px; position: absolute;"></iframe>

It just logs that it received a request and then returns `Hello World`.

Now to run it use:

```
dotnet run -p .\src\CancellationToken.csproj
```

And then with Postman call the endpoint. You should get the response:

![img](https://miro.medium.com/max/510/1*aiyu0mEuux405urklcudSA.png)

Response from “/endpoint”

Now let’s add a `Task.Wait` code to mock an asynchronous request inside our method.

<iframe src="https://medium.com/media/1815959fa631fadc91e9ae5c755d5953" allowfullscreen="" frameborder="0" height="303" width="692" title="EndpointWithTaskWait.cs" class="fq aq as ag cf" scrolling="auto" style="box-sizing: inherit; height: 303px; top: 0px; left: 0px; width: 692px; position: absolute;"></iframe>

Now if you run Postman you should see that the request takes around 5 seconds to finish:

![img](https://miro.medium.com/max/505/1*soSHj-zQC9KiPZfMAo_Hzg.gif)

And see the logs stating our request flow:

![img](https://miro.medium.com/max/218/1*nPf4CjnTaF-s_S3M93ll4Q.png)

Also, note the `Cancel` button when you send the request. When you click on this button, Postman cancels the request. But if you do it right now, nothing happens. That is because our API doesn’t handle request cancellation yet.

To do that, we are going to add the `CancellationToken` as a parameter for our endpoint.

<iframe src="https://medium.com/media/6ab64a11a7a74594f11031ff430ed57c" allowfullscreen="" frameborder="0" height="303" width="692" title="AsyncEndpointWithCancellationToken.cs" class="fq aq as ag cf" scrolling="auto" style="box-sizing: inherit; height: 303px; top: 0px; left: 0px; width: 692px; position: absolute;"></iframe>

See how we pass the `CancellationToken` by just adding it as a parameter to our request. That is because .NET will implicitly add it to our request.

Now we can run again and hit the `Cancel` button.

![img](https://miro.medium.com/max/505/1*M54hRzvi95mK9h0ckX093w.gif)

But it looks like nothing happened. But if you take a look at your logs you’ll see something different:

![img](https://miro.medium.com/max/700/1*u1EnWNpYqaTfzqlYAVpKjg.png)

An exception was thrown.

This happens because when a task gets canceled, the code throws a `TaskCanceledException` stating that that task was canceled.

So, to improve our code, we should add a `try-catch` block between our asynchronous code and handle this exception properly.

<iframe src="https://medium.com/media/dd9c187a1b36ec14bac9064a3b67158f" allowfullscreen="" frameborder="0" height="567" width="692" title="CompleteAsyncEndpointWithCancellationToken.cs" class="fq aq as ag cf" scrolling="auto" style="box-sizing: inherit; height: 567px; top: 0px; left: 0px; width: 692px; position: absolute;"></iframe>

Now if we run again, hit the `Cancel` button, and check the logs, we should see the following:

![img](https://miro.medium.com/max/700/1*pAyofAaqD0AQ06OZDddCOQ.png)

Note that our `LogError` was called with our message before the exception, which means that we caught it and “properly” handled it.

# Conclusion

You could see how powerful and convenient it is to use `CancellationToken` in your API. It gives you the ability to stop the execution of an asynchronous code when requested and it helps you prevent any undesired outcomes if your client doesn’t want a request to continue executing.

It also gives you the ability to cancel long-running and/or stuck tasks, thus freeing CPU and memory for other processes.

Happy coding! 💻



The original code for this article can be found [here](https://github.com/alopes2/Medium-CancellationToken).