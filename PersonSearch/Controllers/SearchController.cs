using System.Linq;
using System.Threading;
using System.Web.Mvc;
using PersonSearch.Models;

namespace PersonSearch.Controllers
{
    public class SearchController : Controller
    {
        private IPeopleDbContext dbContext;

        public SearchController() : this(new PeopleDbContext(
            new PeopleDbInitializer()))
        {
        }

        public SearchController(IPeopleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get a JSON representation of information matching the specified
        /// person name parameter.
        /// </summary>
        /// <param name="name">Information is returned for users whose first
        /// and/or last name contains the characters in the 'name' parameter.
        /// If a null value is specified for this parameter, information for
        /// all available people is returned.
        /// </param>
        /// <param name="delay">Delay for the number of seconds specified by
        /// this parameter before returning to the caller. This parameter is
        /// optional. When not specified, no delay is imposed.</param>
        /// <returns> The JSON payload contains an array with a map of
        /// information for each user found.
        ///
        /// Example response (in JSON):
        ///
        /// [
        ///   {
        ///     "Address": "5678 Gumdrop Lane",
        ///     "Age": 28,
        ///     "FirstName": "Jane",
        ///     "Interests": "Boxing, Kayaking",
        ///     "LastName": "Doe",
        ///     "PersonId": 2,
        ///     "Picture": "/9j/4AAQ..."
        ///   },
        ///   {
        ///     "Address": "1234 Lollipop Drive",
        ///     "Age": 24,
        ///   ...
        ///   }
        ///   ...
        /// ]
        ///
        /// The Address, FirstName, Interests, LastName, and PersonId elements
        /// are all of type 'string'.  The Age and PersonId elements are of
        /// type 'number'.
        ///
        /// The content of the PersonId string is a base-64 encoded
        /// representation of a jpg image of the person.
        ///
        /// The array elements are sorted in ascending alphabetic order by
        /// the person's last name, then by the person's first name for
        /// matching last names.
        /// </returns>
        [HttpGet]
        // Disabling caching so that repeated searches in the browser
        // can experience the delay for testing.  Wouldn't do this in
        // a production application.
        [OutputCache(Duration = 0)]
        public JsonResult GetUsers(string name, int? delay)
        {
            if (delay != null)
            {
                Thread.Sleep((int) delay * 1000);
            }

            IQueryable<Person> people;
            if (name != null)
            {
                people = dbContext.People
                    .Where(x => (x.FirstName.Contains(name)) ||
                        (x.LastName.Contains(name)));
            }
            else
            {
                people = dbContext.People;
            }

            return Json(people
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName), JsonRequestBehavior.AllowGet);
        }
    }
}
