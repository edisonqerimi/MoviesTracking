using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Utilities
{
    public class ValidEmailDomainAttribute:ValidationAttribute
    {
        public ValidEmailDomainAttribute(string allowedDomain)
        {
            AllowedDomain = allowedDomain;
        }

        public string AllowedDomain { get; }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToLower() == AllowedDomain.ToLower();
        }
    }
}
