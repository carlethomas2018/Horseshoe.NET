using Horseshoe.NET.Types;

namespace Horseshoe.NET.Expressions.Functions
{
    public class If : FunctionBase
    {
        public object Execute
        (
            bool arg0, 
            object arg1,
            object arg2 = null
        )
        {
            if (arg1 != null || arg2 != null)
            {
                if (arg1 != null && arg2 != null && !arg1.GetType().IsAssignableFrom(arg2.GetType()) && !arg2.GetType().IsAssignableFrom(arg1.GetType()))
                    throw new ExpressionException(string.Format(Lang.Get("Function.Mismatch.{type1}.{type2}"), arg1.GetType().ToShortName(), arg2.GetType().ToShortName()));
                ReturnType = arg1?.GetType() ?? arg2.GetType();
            }

            return arg0 
                ? arg1
                : (arg2 ?? (arg1 == null ? null : TypeUtil.GetDefaultValue(arg1.GetType())));
        }
    }
}
