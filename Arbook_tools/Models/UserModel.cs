using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbook_tools.Model
{
    internal class UserModel
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string isForm { get; set; }

        public string? Token { get; set; }
    }
}
