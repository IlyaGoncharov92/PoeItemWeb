using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ItemDetail
    {
        public int Id { get; set; }
        public string TradeId { get; set; }
        public string UserName { get; set; }
        public string Price { get; set; }
        public string Message { get; set; }
        public string ItemHtml { get; set; }
        public string ItemName { get; set; }
        public string TimeAgo { get; set; }
        public string WikiLink { get; set; }

        public bool IsVerified { get; set; }
        public int DeleteNumber { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}