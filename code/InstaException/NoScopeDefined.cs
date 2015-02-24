using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.InstaException
{
    class NoScopeDefined : Exception
    {
        public override String Message
        {
            get
            {
                return "No Scope has been defined for your application. For help regarding scopes, refer Instagram API documentation.";
            }
        }
    }
}
