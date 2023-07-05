using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Utility
{
    public class UsernameNotFoundException : AuthenticationException
    {
        public UsernameNotFoundException(string message) : base(message)
        {
        }
    }
}
