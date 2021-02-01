using System;
using System.Text.RegularExpressions;

namespace Area.API.Utilities
{
    public static class UsernameUtilities
    {
        public static bool IsUsernameValid(string username)
        {
            try {
                return Regex.IsMatch(username, "^[A-Za-z][A-za-z0-9._-]{3,}$");
            } catch {
                return false;
            }
        }
    }
}