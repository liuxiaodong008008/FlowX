using System;
using System.Collections;
using System.Collections.Generic;
using FlowX.Core.Steps.Value;

namespace FlowX.Core
{
    public class Foreach<T>
    {
        internal readonly IValue<IEnumerable<T>> _collection;
        internal T _itemValue;
        internal readonly Readonly<T> _item;

        public Foreach(out Readonly<T> item, IValue<IEnumerable<T>> collection)
        {
            _collection = collection;
            item = new Readonly<T>(()=>this._itemValue);
            _item = item;
        }
        
        public Foreach(out Readonly<T> item, IEnumerable<T> collection) : 
            this(out item, new Const<IEnumerable<T>>(collection))
        {
        }

        public ForeachDo<T> Do(IStep body)
        {
            return new ForeachDo<T>(this, body);
        }
        
        public ForeachDo<T> Do(Action body)
        {
            return new ForeachDo<T>(this, (Step) body);
        }
    }

    public class ForeachDo<T>: IStep
    {
        private readonly Foreach<T> _foreach;
        private readonly IStep _body;

        public ForeachDo(Foreach<T> @foreach, IStep body)
        {
            _foreach = @foreach;
            _body = body;
        }

        public void RunStep(IFlow flow)
        {
            foreach (var item in _foreach._collection.Value)
            {
                _foreach._itemValue = item;
                _foreach._item.Refresh();
                
                _body.Run(flow);
            }
        }
    }
}