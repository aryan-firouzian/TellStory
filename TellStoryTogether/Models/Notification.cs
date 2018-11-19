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

        // All or Comment or Favorite
        public string State { get; set; }

        public bool Seen { get; set; }

        public int Starred { get; set; }

        public int Favorited { get; set; }

        public int Commented { get; set; }

        public int Forked { get; set; }
    }
}