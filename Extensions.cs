using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using ExtensionMethods;

namespace ƒx.UnityUtils
{
    public static class ListExtensions
    {
        public static string ToArrayString<T>(this List<T> values)
        {
            string format = "";
            for (int i = 1; i <= values.Count; i *= 10)
                format += "0";
            
            string msg = "";
            for (int i = 0; i < values.Count; i++)
            {
                msg += "\n  [" + i.ToString(format) + "]: " + values[i].ToString() + "";
            }
            return msg;
        }
    }
}