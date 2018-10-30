using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TellStoryTogether.Models;

namespace TellStoryTogether.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    // Build the project
    // Add-Migration MyFavouriteName in the NuGet Command
    // Update-Database –Verbose

    // if freeze then:
    // Enable-Migrations -ContextTypeName UsersContext -ProjectName TellStoryTogether
    internal sealed class Configuration : DbMigrationsConfiguration<TellStoryTogether.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TellStoryTogether.Models.UsersContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            List<string> languageList = new List<string>()
            {
                "English",
                "Finnish",
                "French"
            };

            List<string> genreList = new List<string>()
            {
                "SciFic",
                "Drama",
                "Adventur",
                "Romance",
                "Mystery",
                "Horror",
                "Travel",
                "Children",
                "Religion",
                "Spirituality",
                "Science",
                "History",
                "Poetry",
                "Comics",
                "Art",
                "Diaries",
                "Fantasy"
            };

            foreach (string s in languageList)
            {
                context.Languages.AddOrUpdate(x=>x.LanguageId, new Language(){LanguageInEnglish = s});
            }

            foreach (string s in genreList)
            {
                context.Genres.AddOrUpdate(x => x.Name, new Genre() {Name = s, Detail = "Best in " + s});
            }

            /*foreach (int source in Enumerable.Range(0, 20).ToList())
            {
                Random random = new Random(source);
                int randomValue = random.Next(0, 7000);
                string randomGenre = genreList[source%4];
                //throw new Exception("here");
                context.Articles.AddOrUpdate(x => x.Title, new Article()
                {
                    ArticleInitId = -1,
                    Serial = 1,
                    Parallel = 1,
                    Title = "Light Dark "+ source,
                    Text =
                        "Private First Class Greg Sully O’Sullivan was before a screen in the cool communications room, cordoned off with a woman on the screen half a world away. They weren’t strangers but the closest two people could be. Wetness pooled in his eyes. They weren’t talking as if thousands of miles were between them.",
                    PictureUrl = "Images/StoryImage/89391248-176-k423531.jpg",
                    Owner = context.UserProfiles.First(),
                    Point = randomValue+source*10,
                    Seen = randomValue,
                    Favorite = 0,
                    Selected = true,
                    Time = DateTime.Now,
                    Genre = context.Genres.First(p => p.Name == randomGenre)
                });
            }*/


            /*int articleId = 222;
            string articleTitle = context.Articles.First().Title;
            Genre articleGenre = context.Articles.First().Genre;

            foreach (int source in Enumerable.Range(0, 1).ToList())
            {
                context.Articles.AddOrUpdate(x => x.ArticleId, new Article()
                {
                    ArticleInitId = articleId,
                    Serial = source%7,
                    Parallel = source%5,
                    Title = articleTitle,
                    Text =
                        "Private First Class Greg Sully O’Sullivan was before a screen in the cool communications room, cordoned off with a woman on the screen half a world away. They weren’t strangers but the closest two people could be. Wetness pooled in his eyes. They weren’t talking as if thousands of miles were between them.",
                    PictureUrl = "Images/StoryImage/89391248-176-k423531.jpg",
                    Owner = context.UserProfiles.First(),
                    Point = source*10,
                    Seen = source*12,
                    Favorite = 0,
                    Selected = false,
                    Time = DateTime.Now,
                    Genre = articleGenre
                });
            }*/
        }
    }
}
