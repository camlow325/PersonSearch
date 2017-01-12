using System.Data.Entity;

namespace PersonSearch.Models
{
    public class PeopleDbContext : DbContext, IPeopleDbContext
    {
        public PeopleDbContext(
            IDatabaseInitializer<PeopleDbContext> dbInitializer) :
            base("PersonSearch")
        {
            Database.SetInitializer(dbInitializer);
        }

        public DbSet<Person> People { get; set; }
    }
}