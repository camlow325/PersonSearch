using System.Collections.Generic;
using System.Data.Entity;
using PersonSearch.Models;

namespace PersonSearch.Tests.Controllers
{
    public class TestPeopleDbInitializer :
        DropCreateDatabaseAlways<PeopleDbContext>
    {
        private IList<Person> people;

        public TestPeopleDbInitializer(IList<Person> people)
        {
            this.people = people;
        }

        protected override void Seed(PeopleDbContext context)
        {
            foreach (Person person in people)
                context.People.Add(person);

            base.Seed(context);
        }
    }
}
