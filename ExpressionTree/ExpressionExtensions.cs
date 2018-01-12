using System;
using System.Linq.Expressions;

namespace Expressions
{
    public static class ExpressionExtensions
{
    // Methods
    public static MemberExpression GetMemberExpression<T, TValue>(this Expression<Func<T, TValue>> expression) where T: class
    {
        if (expression != null)
        {
            if (expression.Body is MemberExpression)
            {
                return (MemberExpression) expression.Body;
            }
            if (expression.Body is UnaryExpression)
            {
                Expression operand = ((UnaryExpression) expression.Body).Operand;
                if (operand is MemberExpression)
                {
                    return (MemberExpression) operand;
                }
                if (operand is MethodCallExpression)
                {
                    return (((MethodCallExpression) operand).Object as MemberExpression);
                }
            }
        }
        return null;
    }

    public static string GetNameFor<T, TValue>(this T obj, Expression<Func<T, TValue>> expression) where T: class
    {
        return expression.GetNameFor();
    }

    public static string GetNameFor<T, TValue>(this Expression<Func<T, TValue>> expression) where T: class
    {
        return GetNameFor(expression, null);
    }

    public static string GetNameFor<T, TValue>(this Expression<Func<T, TValue>> expression, string prefix) where T: class
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            return new ExpressionNameVisitor().Visit(expression.Body);
        }else
        {
            return string.Format("{0}.{1}", prefix, new ExpressionNameVisitor().Visit(expression.Body));
        }
    }

    public static TValue GetValueFrom<T, TValue>(this Expression<Func<T, TValue>> expression, T viewModel) where T: class
    {
        try
        {
            return ((viewModel == null) ? default(TValue) : expression.Compile().Invoke(viewModel));
        }
        catch (Exception)
        {
            return default(TValue);
        }
    }
	
	
	 public static string GetMethodName<T>(Expression<Action<T>> action) where T : class
        {
            var expression = (MethodCallExpression)action.Body;

            return expression.Method.Name;
        }

        public static string GetMemberName<T, P>(Expression<Func<T, P>> action) where T : class
        {
            var expression = (MemberExpression)action.Body;

            return expression.Member.Name;
        }
}

 
 

}