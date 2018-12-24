using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TellStoryTogether.Models
{
    [Table("Language")]
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageId { get; set; }

        public string LanguageInEnglish { get; set; }

        public string LanguageInNative { get; set; }

        public string TextDirection { get; set; }

    }
}