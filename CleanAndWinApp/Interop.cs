using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAndWinApp
{
    //public class Interop :IDisposable
    //{
    //    private readonly string _filePath;
    //    protected readonly IJSRuntime _jsRuntime;
    //    private Task<IJSObjectReference> _module;

    //    public Interop(IJSRuntime jsRuntime)
    //    {
    //        _filePath = filePath;
    //        _jsRuntime = jsRuntime;
    //    }

    //    public Task<IJSObjectReference> Module => _module ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./hello.js").AsTask();

    //    //create a method to call function from the JS file
    //    public async void SayHelloWorld(string name)
    //    {
    //        var module = await Module;
    //        await module.InvokeVoidAsync("helloWorld", name);
    //    }

    //    public void Dispose()
    //    {
    //        if (_module is not null)
    //        {
    //            ((IDisposable)_module).Dispose();
    //        }
    //    }
    //}
}
