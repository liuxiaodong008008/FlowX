# FlowX

Light-weight C# workflow library, which focuses on ease of use.

## Features
- Variables
  - Const
  - Readonly
  - Variable
- Controls
  - Sequence
  - If-ElseIf-Else
  - While
  - Foreach
- Exception Handling
  - Try-Catch-Finally
- Middlewares
  - StepMiddleware
  - FlowMiddleware

## Demo
```CSharp
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
```

Output:

```
/##############\ CustomFlowMiddleware1 start
/==============\ Sequence CustomStepMiddleware1 start
/==============\ Section CustomStepMiddleware1 start
\==============/ Section CustomStepMiddleware1 end
/==============\ DefineVariable`1 CustomStepMiddleware1 start
\==============/ DefineVariable`1 CustomStepMiddleware1 end
/==============\ Step CustomStepMiddleware1 start
1
\==============/ Step CustomStepMiddleware1 end
/==============\ IfThenElse CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
True
\==============/ Step CustomStepMiddleware1 end
\==============/ IfThenElse CustomStepMiddleware1 end
/==============\ WhileDo CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
1
\==============/ Step CustomStepMiddleware1 end
/==============\ Step CustomStepMiddleware1 start
2
\==============/ Step CustomStepMiddleware1 end
\==============/ WhileDo CustomStepMiddleware1 end
/==============\ Try CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
Error
\==============/ Step CustomStepMiddleware1 end
/==============\ Step CustomStepMiddleware1 start
Always appear
\==============/ Step CustomStepMiddleware1 end
\==============/ Try CustomStepMiddleware1 end
/==============\ ForeachDo`1 CustomStepMiddleware1 start
/==============\ ForeachDo`1 CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
Hello-1
\==============/ Step CustomStepMiddleware1 end
/==============\ Step CustomStepMiddleware1 start
Hello-2
\==============/ Step CustomStepMiddleware1 end
\==============/ ForeachDo`1 CustomStepMiddleware1 end
/==============\ ForeachDo`1 CustomStepMiddleware1 start
/==============\ Step CustomStepMiddleware1 start
World-1
\==============/ Step CustomStepMiddleware1 end
/==============\ Step CustomStepMiddleware1 start
World-2
\==============/ Step CustomStepMiddleware1 end
\==============/ ForeachDo`1 CustomStepMiddleware1 end
\==============/ ForeachDo`1 CustomStepMiddleware1 end
\==============/ Sequence CustomStepMiddleware1 end
\##############/ CustomFlowMiddleware1 end


```
