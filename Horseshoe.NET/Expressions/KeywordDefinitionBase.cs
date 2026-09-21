using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions
{
    public abstract class KeywordDefinitionBase
    {
        public abstract List<Item> Definitions { get; }
        public abstract class Item 
        {
            public string Name { get; }
            public abstract object GetValue(params object[] args);
            public Type DataType { get; protected set; }

            public Item(string name)
            {
                Name = name;
            }
        }

        public class KeywordConstantItem : Item
        {
            private object Value { get; }

            public KeywordConstantItem(string name, object value) : base(name) 
            { 
                Value = value;
                if (value != null)
                    DataType = value.GetType();
            }

            public override object GetValue(params object[] args) => Value;
        }

        public class FunctionItem : Item
        {
            public int MinArgs { get; }
            public int MaxArgs { get; }
            public Func<object[], object> ValueCalc { get; }

            public FunctionItem(string name, int minArgs, int maxArgs, Func<object[], object> valueCalc) : base(name)
            {
                MinArgs = minArgs;
                MaxArgs = maxArgs;
                ValueCalc = valueCalc ?? throw new ExpressionException("This function item must have a value calculation");
            }

            public FunctionItem(string name, Type dataType, int minArgs, int maxArgs, Func<object[], object> valueCalc) : this(name, minArgs, maxArgs, valueCalc)
            {
                DataType = dataType;
            }

            public override object GetValue(params object[] args) => ValueCalc.Invoke(args);
        }
    }
}
