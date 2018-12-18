using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace TellStoryTogether.Models
{
    [Table("Article")]
    public class Article
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        
        public int ArticleInitId { get; set; }

        public string Identifier { get; set; }

        public string Title { get; set; }
        
        public int Serial { get; set; }

        public int Parallel { get; set; }

        public string Text { get; set; }

        public UserProfile Owner { get; set; }

        public int Point { get; set; }

        public int Seen { get; set; }

        public int Favorite { get; set; }

        public int Comment { get; set; }

        public string PictureUrl { get; set; }

        public DateTime Time { get; set; }

        public Genre Genre { get; set; }

        public Language Language { get; set; }

        public int MinChar { get; set; }

        public int MaxChar { get; set; }

        public bool IsLast { get; set; }
    }

}