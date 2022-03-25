using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.User
{
    public class UserRoleModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] RoleNames { get; set; }
    }
}
