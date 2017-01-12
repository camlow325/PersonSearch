using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PersonSearch.Controllers;
using PersonSearch.Models;

namespace PersonSearch.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        private static Person CreatePerson(int id, string firstName,
            string lastName)
        {
            return new Person()
            {
                PersonId = id,
                FirstName = firstName,
                LastName = lastName,
                Age = id + 20,
                Address = String.Format("{0}'s Address", firstName),
                Interests = String.Format("{0}'s Interests", firstName),
                Picture = String.Format("{0}'s Picture", firstName)
            };
        }

        private static IList<Person> FindUsers(IList<Person> usersToFind,
            IList<Person> possibleUsers)
        {
            return usersToFind.Select(x =>
                possibleUsers.FirstOrDefault(y =>
                y.FirstName == x.FirstName &&
                y.LastName == x.LastName))
                .ToList();
        }

        private static void ValidateUsers(IList<Person> expectedUsers,
            IList<Person> actualUsers)
        {
            Assert.AreEqual(expectedUsers.Count, actualUsers.Count,
                "Number of users does not match");

            for (int i = 1; i < expectedUsers.Count + 1; i++)
            {
                Person expectedUser = expectedUsers[i - 1];
                Person actualUser = actualUsers[i - 1];

                Assert.AreEqual(expectedUser.PersonId, actualUser.PersonId,
                    String.Format("Id for user {0} does not match", i));
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName,
                    String.Format("First name for user {0} does not match", i));
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName,
                    String.Format("Last name for user {0} does not match", i));
                Assert.AreEqual(expectedUser.Age, actualUser.Age,
                    String.Format("Age for user {0} does not match", i));
                Assert.AreEqual(expectedUser.Address, actualUser.Address,
                    String.Format("Address for user {0} does not match", i));
                Assert.AreEqual(expectedUser.Interests, actualUser.Interests,
                    String.Format("Interests for user {0} do not match", i));
                Assert.AreEqual(expectedUser.Picture, actualUser.Picture,
                    String.Format("Picture for user {0} does not match", i));
            }
        }

        public static IList<Person> UsersToSeedInDb()
        {
            return new List<Person>()
            {
                CreatePerson(1, "William", "Douglas"),
                CreatePerson(2, "Gail", "Adams"),
                CreatePerson(3, "Fred", "Johnson"),
                CreatePerson(4, "Bill", "Howard"),
                CreatePerson(5, "Jane", "Smith"),
                CreatePerson(6, "Bob", "Jones"),
            };
        }

        private static IList<Person> GetUsersFromDb(IList<Person> seedUsers,
            string match)
        {
            SearchController controller = new SearchController(
                new PeopleDbContext(new TestPeopleDbInitializer(seedUsers)));
            JsonResult json = controller.GetUsers(match, 0) as JsonResult;
            return ((IEnumerable<Person>)json.Data).ToList<Person>();
        }

        [TestMethod]
        public void GetUsersWhereOnlySomeFirstNamesMatch()
        {
            IList<Person> seedUsers = UsersToSeedInDb();
            IList<Person> expectedUsers = FindUsers(
                new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "William",
                        LastName = "Douglas"
                    },
                    new Person()
                    {
                        FirstName = "Bill",
                        LastName = "Howard"
                    },
                }, seedUsers);

            IList<Person> usersFound = GetUsersFromDb(seedUsers, "ill");

            ValidateUsers(expectedUsers, usersFound);
        }

        [TestMethod]
        public void GetUsersWhereOnlySomeLastNamesMatch()
        {
            IList<Person> seedUsers = UsersToSeedInDb();
            IList<Person> expectedUsers = FindUsers(
                new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Fred",
                        LastName = "Johnson"
                    },
                    new Person()
                    {
                        FirstName = "Bob",
                        LastName = "Jones"
                    },
                }, seedUsers);

            IList<Person> usersFound = GetUsersFromDb(seedUsers, "jo");

            ValidateUsers(expectedUsers, usersFound);
        }

        [TestMethod]
        public void GetUsersWhereOnlySomeFirstAndLastNamesMatch()
        {
            IList<Person> seedUsers = UsersToSeedInDb();
            IList<Person> expectedUsers = FindUsers(
                new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "Gail",
                        LastName = "Adams"
                    },
                    new Person()
                    {
                        FirstName = "William",
                        LastName = "Douglas"
                    },
                    new Person()
                    {
                        FirstName = "Bill",
                        LastName = "Howard"
                    },
                    new Person()
                    {
                        FirstName = "Jane",
                        LastName = "Smith"
                    },
                }, seedUsers);

            IList<Person> usersFound = GetUsersFromDb(seedUsers, "a");

            ValidateUsers(expectedUsers, usersFound);
        }

        [TestMethod]
        public void GetUsersWhereNoneMatch()
        {
            IList<Person> seedUsers = UsersToSeedInDb();
            IList<Person> expectedUsers = new List<Person>();

            IList<Person> usersFound = GetUsersFromDb(seedUsers, "ZzZ");

            ValidateUsers(expectedUsers, usersFound);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            IList<Person> seedUsers = UsersToSeedInDb();
            IList<Person> expectedUsers = seedUsers.OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName).ToList();

            IList<Person> usersFound = GetUsersFromDb(seedUsers, null);

            ValidateUsers(expectedUsers, usersFound);
        }
    }
}