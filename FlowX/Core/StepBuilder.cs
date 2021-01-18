using System;
using System.Collections.Generic;
using FlowX.Core.Steps;
using FlowX.Core.Steps.Definition;
using FlowX.Core.Steps.Value;

namespace FlowX.Core
{
    public class StepBuilder
    {
        public static If If(Func<bool> condition) => new If(condition);

        public static While While(Func<bool> condition) => new While(condition);
        
        public static Foreach<T> Foreach<T>(out Readonly<T> item, IValue<IEnumerable<T>> collection)
            => new Foreach<T>(out item, collection);
        
        public static Foreach<T> Foreach<T>(out Readonly<T> item, IEnumerable<T> collection)
            => new Foreach<T>(out item, collection);
        
        public static Try Try(Action body) => Try((Step) body);
        
        public static Try Try(IStep body) => new Try(body);

        public static DefineConst<T> Define<T>(out Const<T> name, T value) => new DefineConst<T>(out name, value);
        
        public static DefineReadonly<T> Define<T>(out Readonly<T> name, Func<T> func) => new DefineReadonly<T>(out name, func);
        
        public static DefineVariable<T> Define<T>(out Variable<T> name, Func<T> func) => new DefineVariable<T>(out name, func);

        public static IStep Section(string name) => new Section(name);
    }
}