using System;
using System.Collections.Generic;
using System.Linq;
using SecretSanta.Models;

namespace SecretSanta.Business_Layer
{
    public class PairingProcessor
    {
        private const string ErrorMessage = "Processing not possible";

        /// <summary>
        /// Takes the list of Persons and generates pair of Persons dictionary which represents Secret Santa and a Receiver person
        /// Below are the criteria:
        /// It won't pair persons with the same last name
        /// A Person can't be secret santa more than once
        /// A person can't receive more than one gift
        /// A Person can't be a secret santa for himself.
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public Dictionary<Person, Person> GetPairs(List<Person> persons)
        {
            var sortedGroups = GetSortedGroupsByFamily(persons);
            var personsArray = sortedGroups.SelectMany(x => x).ToArray();
            
            var pairs = FormPairs(personsArray);

            return pairs;
        }

        public IOrderedEnumerable<IGrouping<string, Person>> GetSortedGroupsByFamily(List<Person> persons)
        {
            var sortedGroups = persons.GroupBy(x => x.LastName).OrderBy(x => x.Count());
            return sortedGroups;
        }

        private Dictionary<Person, Person> FormPairs(Person[] persons)
        {
            var pairs = new Dictionary<Person, Person>();

            //Start pairing by picking Santa from the start of the collection and Receiver from the end until Family names for both persons are not same.
            PairByFirstAndLast(persons, pairs);

            if (!pairs.Any())
                throw new Exception(ErrorMessage);

            var index = GetIndexToTrackRemainingReceivers(persons, pairs);

            //Pair again for the remaining persons who didn't get marked as receivers, by picking up santa from the existing receivers 
            PairRemainingReceiversWithAvailableSantas(persons, pairs, index);
            
            return pairs;
        }
        
        private void PairByFirstAndLast(Person[] persons, Dictionary<Person, Person> pairs)
        {
            for (int i = 0, j = persons.Length - 1; i < j; i++, j--)
            {
                if (!pairs.ContainsKey(persons[i]))
                {
                    if (!persons[i].LastName.Equals(persons[j].LastName))
                    {
                        pairs.Add(persons[i], persons[j]);
                    }
                }
                else
                {
                    throw new Exception(ErrorMessage);
                }
            }
        }

        private void PairRemainingReceiversWithAvailableSantas(Person[] persons, Dictionary<Person, Person> pairs, int index)
        {
            for (int s = persons.Length - 1, t = index; t >= 0; s--, t--)
            {
                if (!pairs.ContainsKey(persons[s]) && !persons[s].LastName.Equals(persons[t].LastName))
                {
                    pairs.Add(persons[s], persons[t]);
                }

                else
                {
                    throw new Exception(ErrorMessage);
                }
            }
        }
        private int GetIndexToTrackRemainingReceivers(Person[] persons, Dictionary<Person, Person> pairs)
        {
            int indexOfRecentReceiver = Array.IndexOf(persons, pairs.Last().Value);
            int index = indexOfRecentReceiver - 1;

            if (index < 0)
                throw new Exception(ErrorMessage);
            return index;
        }

    }
}