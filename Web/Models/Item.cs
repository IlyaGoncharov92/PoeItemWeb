using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Item
    {
        public Item()
        {
            ItemDetails = new List<ItemDetail>();
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public string UrlParams { get; set; }
        public string Comment { get; set; }
        public int CountNewDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<ItemDetail> ItemDetails { get; set; }
        
        public string ImageWikiHtml { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}