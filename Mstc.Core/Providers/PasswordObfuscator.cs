using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mstc.Core.Providers
{
    using System.Text.RegularExpressions;

    public class PasswordObfuscator
    {
        public string ObfuscateString(string input)
        {
            var firstPass = Regex.Replace(input, "\"Password\":\"(.*?)\"", "\"Password\":\"xxxxxxxx\"");
            var secondPass = Regex.Replace(input, "\"Password\": \"(.*?)\"", "\"Password\": \"xxxxxxxx\"");
            return secondPass;
        }
    }
}
