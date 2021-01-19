using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using FlowX.Core;
using FlowX.Core.Extensions;
using FlowX.Core.Steps.Value;

namespace Demo
{
    class Program: FlowBuilder
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddStepMiddleware<CustomStepMiddleware1>()
                // .AddStepMiddleware<CustomStepMiddleware2>()
                .AddFlowMiddleware<CustomFlowMiddleware1>()
                // .AddFlowMiddleware<CustomFlowMiddleware2>()
                .BuildServiceProvider();
            
            new Program().Build(serviceProvider).Run();
        }

        public override IStep BuildStep(IServiceProvider serviceProvider)
        {
            var flow =
                Section("Test1")
                | Define(out Variable<int> x, () => 1)
                | (() =>
                {
                    Console.WriteLine(x.Value);
                })
                | If(()=>x.Value%2==1).Then(() =>
                {
                    Console.WriteLine("True");
                }).Else(() =>
                {
                    Console.WriteLine("False");
                }) 
                | While(() => x.Value < 3).Do(() =>
                {
                    Console.WriteLine(x.Value);
                    x.Value++;
                })
                | Try(() =>
                {
                    var b = 0;
                    var a = 1 / b;
                }).Catch(out Readonly<Exception> ex1).Do(() =>
                {
                    Console.WriteLine("Error");
                }).Catch(out Readonly<Exception> ex2).Do(() =>
                {
                    Console.WriteLine("Won't appear");
                }).Finally(() =>
                {
                    Console.WriteLine("Always appear");
                })
                | Foreach(out Readonly<string> item, new[] {"Hello", "World"}).Do(
                    Foreach(out Readonly<int> idx, new []{1,2}).Do(() =>
                    {
                        Console.WriteLine($"{item.Value}-{idx.Value}");
                    })
                );
            
            return flow;
        }

    }
    
    class CustomStepMiddleware1 : IStepMiddleware
    {
        public void Run(IFlow flow, IStep step, Action next)
        {
            Console.WriteLine(@$"/==============\ {step.GetType().Name} {this.GetType().Name} start");
            next();
            Console.WriteLine(@$"\==============/ {step.GetType().Name} {this.GetType().Name} end");
        }
    }
    
    class CustomStepMiddleware2 : IStepMiddleware
    {
        public void Run(IFlow flow, IStep step, Action next)
        {
            Console.WriteLine(@$"/--------------\ {step.GetType().Name} {this.GetType().Name} start");
            next();
            Console.WriteLine(@$"\--------------/ {step.GetType().Name} {this.GetType().Name} end");
        }
    }
    
    class CustomFlowMiddleware1 : IFlowMiddleware
    {
        public void Run(IFlow flow, Action next)
        {
            Console.WriteLine(@$"/##############\ {this.GetType().Name} start");
            next();
            Console.WriteLine(@$"\##############/ {this.GetType().Name} end");
        }
    }
    
    class CustomFlowMiddleware2 : IFlowMiddleware
    {
        public void Run(IFlow flow, Action next)
        {
            Console.WriteLine(@$"/@@@@@@@@@@@@@@\ {this.GetType().Name} start");
            next();
            Console.WriteLine(@$"\@@@@@@@@@@@@@@/ {this.GetType().Name} end");
        }
    }
}