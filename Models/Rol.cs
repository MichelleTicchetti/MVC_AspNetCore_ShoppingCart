using Carrito_A.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A.Models
{
    public class Rol : IdentityRole<int>
    {

        public Rol() : base()
        {

        }

        //constructor parametrizado para nuevo rol
        public Rol(string rolName) : base(rolName)
        {

        }

        [Display(Name= Alias.RolName)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }

        }
    }
}
