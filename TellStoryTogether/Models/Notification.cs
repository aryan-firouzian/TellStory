using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TellStoryTogether.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public Article Article { get; set; }

        public UserProfile User { get; set; }

        public DateTime Time { get; set; }

        public string Content { get; set; }

        // State to keep notification record Create or Comment or Favorite
        public bool CreateState { get; set; }

        public bool CommentState { get; set; }

        public bool FavoriteState { get; set; }

        // Visibility of notification
        public bool Seen { get; set; }

        public bool Visited { get; set; }

        // Action when subscribing
        public int Liked { get; set; }

        public int Favorited { get; set; }

        public int Commented { get; set; }

        public int Forked { get; set; }
        
        // Splited by '|'
        public string ForkedArticleIds { get; set; }

        public string Identifier { get; set; }
    }
}