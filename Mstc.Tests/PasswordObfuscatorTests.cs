using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mstc.Core.Domain;
using Mstc.Core.Providers;
using Machine.Fakes;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Mstc.Tests
{
    public class PasswordObfuscatorTests
    {
        [Subject(typeof(PasswordObfuscator))]
        public class when_obfuscating_member_json_then_ : WithSubject<PasswordObfuscator>
        {
            private static RegistrationFullDetails testContent = new RegistrationFullDetails()
            {
                RegistrationDetails = new RegistrationDetails() { Password = "SecretCode"}
            };
            private static string input = JsonConvert.SerializeObject(testContent, Formatting.Indented);

            Because of = () => output = Subject.ObfuscateString(input);

            private It has_password_replaced = () => output.ShouldNotContain("SecretCode");
            private static string output;
        } 

    }
}
