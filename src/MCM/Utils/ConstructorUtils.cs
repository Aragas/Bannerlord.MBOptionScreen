using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MCM.Utils
{
    internal static class ConstructorUtils
    {
        public static TDelegate? Delegate<TDelegate>(ConstructorInfo constructorInfo) where TDelegate : Delegate
        {
            var newExpression = Expression.New(constructorInfo);
            try
            {
                return Expression.Lambda<TDelegate>(newExpression).Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }

        public static Delegate? Delegate(ConstructorInfo constructorInfo)
        {
            var newExpression = Expression.New(constructorInfo);
            try
            {
                return Expression.Lambda(newExpression).Compile();
            }
            catch (Exception e) when (e is ArgumentException)
            {
                return null;
            }
        }
    }
}