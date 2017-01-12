using System.Data.Entity;

namespace PersonSearch.Models
{
    public interface IPeopleDbContext
    {
        DbSet<Person> People { get; set; }
    }
}
