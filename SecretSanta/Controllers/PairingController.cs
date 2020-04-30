using SecretSanta.Models;
using SecretSanta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SecretSanta.Business_Layer;

namespace SecretSanta.Controllers
{
    public class PairingController : Controller
    {
        private readonly PairingProcessor _pairingProcessor;
        public PairingController()
        {
            _pairingProcessor = new PairingProcessor();
        }

        // GET: Pairing
        [HttpGet]
        public ActionResult Index()
        {
            return View(new PersonsListViewModel() { Persons = new List<Person>(), Person = new Person() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SecretSantaList(PersonsListViewModel personListViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", personListViewModel);
            }

            if (personListViewModel.Persons == null)
            {
                return View("Index", new PersonsListViewModel() { Persons = new List<Person>(), Person = new Person() });
            }

            var pairs = _pairingProcessor.GetPairs(personListViewModel.Persons);
            return View(pairs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddName(PersonsListViewModel personListViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new PersonsListViewModel() { Persons = personListViewModel.Persons ?? new List<Person>(), Person = personListViewModel.Person });
            }

            var newList = new List<Person>();
            if (personListViewModel.Persons != null) newList.AddRange(personListViewModel.Persons);
            newList.Add(personListViewModel.Person);
            ModelState.Clear();
            return View("Index", new PersonsListViewModel() { Persons = newList, Person = new Person() });
        }
    }
}