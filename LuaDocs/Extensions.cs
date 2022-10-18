using System;

namespace LuaDocs
{
    public static class FlagExtensions
    {
        #region Flag helpers
        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            var a = Convert.ToUInt64(variable);
            var b = Convert.ToUInt64(value);
            return (a & b) != 0;
        }
        //Definitely not optimized
        public static bool HasAllFlags(this Enum value, Enum flags)
        {
            if (value is null)
                return false;

            if (flags is null)
                throw new ArgumentNullException("value");

            var a = Convert.ToUInt64(value);
            var b = Convert.ToUInt64(flags);

            return (a & b) == b;
        }
        public static bool HasAnyFlag(this Enum value, Enum flags)
        {
            if (value is null)
                return false;

            if (flags is null)
                throw new ArgumentNullException("value");

            var a = Convert.ToUInt64(value);
            var b = Convert.ToUInt64(flags);

            return (a & b) != 0;
        } 
        #endregion
    }
}
