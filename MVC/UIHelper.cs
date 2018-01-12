public static class UIHelpers
    {
         public static string AssignIfTrue(this bool condition, string val)
         {
             if (condition)
             {
                 return val;
             }

             return "";
         }

         public static string AssignIfFalse(this bool condition, string val)
         {
             if (!condition)
             {
                 return val;
             }

             return "";
         }

        public static string AssignIfValue<T>(this T condition, T val, string assign)
        {
            if (condition != null)
            {
                if (condition.Equals(val))
                {
                    return assign;
                }
            }
            

            return string.Empty;
        }
    }