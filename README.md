PersonSearch
===========

PersonSearch is a simple ASP.NET MVC application which allows users to search
for people by name.  PersonSearch has a single web page which has two
prompts, a `Name` and a `Delay`, and a `Search` button.

When the user presses the `Search` button, the application finds all people
whose first name and/or last name matches an entry in the database and
displays the results in a scrollable window. The value in the `Delay`
field allows the user to request that the server delay for an
artificial amount of time, for testing, before providing search results.
The `Delay` field is expressed in seconds.

Information for people in the database is seeded automatically at
database startup.  The information can currently only be changed by
editing [this C# source file](./PersonSearch/Models/PeopleDbInitializer.cs)
before the application is started.  Note that the value in the `Picture`
field represents the user's picture as a base-64 encoded jpg image.