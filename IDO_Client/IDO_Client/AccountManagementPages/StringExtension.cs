using System;
using System.Collections.Generic;
using System.Text;

namespace IDO_Client.AccountManagementPages
{
    static class StringExtension
    {
        public static bool IsStringConsistOf(this string str, string symbols)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (symbols.IndexOf(str[i]) == -1)
                    return false;
            }
            return true;
        }
    }
}
