using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.ViewModels
{
    public class PersonsListViewModel
    {
        public List<Person> Persons { get; set; }

        public Person Person { get; set; }
    }
}