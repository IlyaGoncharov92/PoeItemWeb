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
        public bool IsVerified { get; set; }
        public int DeleteNumber { get; set; }
        public string Message { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}