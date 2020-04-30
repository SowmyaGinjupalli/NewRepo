using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace SecretSanta.Models
{
    public class Person 
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public override int GetHashCode()
        {
            return FirstName.GetHashCode() ^ LastName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                Person p = (Person)obj;

                return this.FirstName.Equals(p.FirstName) && this.LastName.Equals(p.LastName);
            }

            return false;
        }
    }
}