using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models.DTO
{
    public class UserTokenDTO
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
       
    }
}
