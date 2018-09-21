using System;
using Discord;

namespace Crow
{
    public static class IdHelper
    {
        public static ulong UlongConvert(string input)
        {
            try
            {
                return ulong.Parse(input);
            }
            catch (Exception e)
            {
                Crow.Log(new LogMessage(LogSeverity.Critical, "StringToUlong", "Critical!", e));
                return 0;
            }
        }
        public static ulong UlongConvert(int input)
        {
            try
            {
                return Convert.ToUInt64(input);
            }
            catch (Exception e)
            {
                Crow.Log(new LogMessage(LogSeverity.Critical, "StringToUlong", "Critical!", e));
                return 0;
            }
        }
    }
}