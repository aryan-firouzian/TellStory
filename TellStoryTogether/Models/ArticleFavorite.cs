using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TellStoryTogether.Models
{
    [Table("ArticleFavorite")]
    public class ArticleFavorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleFavoriteId { get; set; }

        public Article Article { get; set; }

        public UserProfile User { get; set; }
    }
}