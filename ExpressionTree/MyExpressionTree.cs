using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;

namespace Common
{
    public class MyExpressionTree
    {
        public Func<T, T, T> AddFunc<T>()
        {
            var paramExp1 = Expression.Parameter(typeof(T));
            var paramExp2 = Expression.Parameter(typeof(T));

            var expBody = Expression.AddAssign(paramExp1, paramExp2);

            var lambdExp = Expression.Lambda<Func<T, T, T>>(expBody, new[] { paramExp1, paramExp2 });
            return lambdExp.Compile();

        }

        public Func<int,int> Factorial ()
        {
            // Creating a parameter expression.
            var value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable. 
            var result = Expression.Variable(typeof(int), "result");

            // Creating a label to jump to from a loop.
            var label = Expression.Label(typeof(int));

            // Creating a method body.
            var block = Expression.Block(
                new[] { result },
                Expression.Assign(result, Expression.Constant(1)),
                    Expression.Loop(
                       Expression.IfThenElse(
                           Expression.GreaterThan(value, Expression.Constant(1)),
                           Expression.MultiplyAssign(result,
                               Expression.PostDecrementAssign(value)),
                           Expression.Break(label, result)
                       ),
                   label
                )
            );
            return Expression.Lambda<Func<int, int>>(block, value).Compile();
        }

        #region Build property value compare expression

        public class Student
        {
            public string Id { get; set; }
            public int Age { get; set; }
            public double Score { get; set; }
            public DateTime JoinDate { get; set; }
        }

        private object GetPropertyByT<T>(string prop,T obj) where T : new()
        {

            return typeof(T).InvokeMember(prop,
                                                  BindingFlags.GetProperty,
                                                  null,
                                                  obj,
                                                  null);
        }

        private Func<T, bool> BuildFuncFor<T>(string prop,object propValue,Func<Expression,Expression,Expression> exp) where T : new()
        {
            Func<T, bool> ret = t =>
                {
                    var e1 = Expression.Constant(GetPropertyByT(prop,t));
                    var e2 = Expression.Constant(propValue);

                    var ep = Expression.Lambda<Func<T, bool>>(
                        exp(e1,e2),
                        Expression.Parameter(typeof(T)));
                    return ep.Compile().Invoke(t);
                };

            return ret;
        }

        public Func<T, bool> BuildEqFuncFor<T>(string prop, object propValue) where T: new ()
        {
            return BuildFuncFor<T>(prop,propValue,Expression.Equal);
        }
        public Func<T, bool> BuildNotEqFuncFor<T>(string prop, object propValue, T obj) where T : new()
        {
            return BuildFuncFor<T>(prop,propValue,Expression.NotEqual);
        }

        public Func<T, bool> BuildGreaterThanFuncFor<T>(string prop, object propValue, T obj) where T : new()
        {
            return BuildFuncFor<T>(prop,propValue,Expression.GreaterThan);
        }

        public Func<T, bool> BuildLessThanFuncFor<T>(string prop, object propValue, T obj) where T : new()
        {
            return BuildFuncFor<T>(prop,propValue,Expression.LessThan);
        }

        #endregion
    }
}
