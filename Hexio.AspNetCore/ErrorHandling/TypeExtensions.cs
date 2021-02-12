using System;
using System.Collections.Generic;

namespace Hexio.AspNetCore.ErrorHandling
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            while (type != typeof(object))
            {
                yield return type;

                type = type.BaseType;
            }
        }
    }
}
