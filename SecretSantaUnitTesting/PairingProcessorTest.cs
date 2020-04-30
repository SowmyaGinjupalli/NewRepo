using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Business_Layer;
using SecretSanta.Models;

namespace SecretSantaUnitTesting
{
    [TestClass]
    public class PairingProcessorTest
    {
        private PairingProcessor _pairingProcessor;

        [TestInitialize]
        public void SetUp()
        {
            _pairingProcessor = new PairingProcessor();
        }

        [TestMethod]
        public void GetPairs_HappyPath_Test()
        {
            var persons = new List<Person>
            {
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "B", LastName = "1"},
                new Person() {FirstName = "C", LastName = "2"},
                new Person() {FirstName = "D", LastName = "2"},
                new Person() {FirstName = "E", LastName = "3"},
            };

            var pairs = _pairingProcessor.GetPairs(persons);
            
            Assert.AreEqual(persons.Count, pairs.Count);
            AssertReceivers(persons, pairs);
        }

        [TestMethod]
        public void GetPairs_SameFamily_Failure_Test()
        {
            var persons = new List<Person>
            {
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "B", LastName = "1"},
                new Person() {FirstName = "C", LastName = "1"}
            };

            Assert.ThrowsException<Exception>(() => _pairingProcessor.GetPairs(persons));
        }


        [TestMethod]
        public void GetPairs_NotEnough_Santas_Failure_Test()
        {
            var persons = new List<Person>
            {
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "B", LastName = "1"},
                new Person() {FirstName = "C", LastName = "1"},
                new Person() {FirstName = "D", LastName = "2"},
                new Person() {FirstName = "E", LastName = "2"}
            };

            Assert.ThrowsException<Exception>(() => _pairingProcessor.GetPairs(persons));
        }

        [TestMethod]
        public void GetPairs_Duplicate_Names_Failure_Test()
        {
            var persons = new List<Person>
            {
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "C", LastName = "2"},
                new Person() {FirstName = "D", LastName = "2"},
            };

            Assert.ThrowsException<Exception>(() => _pairingProcessor.GetPairs(persons));
        }

        [TestMethod]
        public void GetPairs_MoreMembers_HappyPath_Test()
        {
            List<Person> persons = new List<Person>
            {
                new Person() {FirstName = "E", LastName = "2"},
                new Person() {FirstName = "I", LastName = "6"},
                new Person() {FirstName = "G", LastName = "4"},
                new Person() {FirstName = "F", LastName = "3"},
                new Person() {FirstName = "H", LastName = "5"},
                new Person() {FirstName = "D", LastName = "2"},
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "B", LastName = "1"},
                new Person() {FirstName = "C", LastName = "1"},
                new Person() {FirstName = "J", LastName = "6"},
                new Person() {FirstName = "K", LastName = "6"},
                new Person() {FirstName = "L", LastName = "6"}
            };


            var pairs = _pairingProcessor.GetPairs(persons);

            Assert.AreEqual(persons.Count, pairs.Count);
            AssertReceivers(persons, pairs);
        }

        [TestMethod]
        public void GetSortedGroupsByFamily_Test()
        {

            var persons = new List<Person>
            {
                new Person() {FirstName = "E", LastName = "2"},
                new Person() {FirstName = "I", LastName = "6"},
                new Person() {FirstName = "F", LastName = "3"},
                new Person() {FirstName = "K", LastName = "6"},
                new Person() {FirstName = "D", LastName = "2"},
                new Person() {FirstName = "A", LastName = "1"},
                new Person() {FirstName = "C", LastName = "1"},
                new Person() {FirstName = "J", LastName = "6"},
                new Person() {FirstName = "L", LastName = "6"},
                new Person() {FirstName = "B", LastName = "1"}
            };

            var groups = _pairingProcessor.GetSortedGroupsByFamily(persons);
            var result = groups.Select(x => new { lastName = x.Key, membersCount = x.Count() }).ToList();

            Assert.AreEqual("3", result[0].lastName);
            Assert.AreEqual("2", result[1].lastName);
            Assert.AreEqual("1", result[2].lastName);
            Assert.AreEqual("6", result[3].lastName);
        }

        private static void AssertReceivers(List<Person> persons, Dictionary<Person, Person> pairs)
        {
            var sortedPersons = persons.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            var resultReceivers = pairs.Values.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            for (int i = 0; i < persons.Count; i++)
            {
                Assert.AreEqual(sortedPersons[i].FirstName, resultReceivers[i].FirstName);
                Assert.AreEqual(sortedPersons[i].LastName, resultReceivers[i].LastName);
            }
        }
    }
}
