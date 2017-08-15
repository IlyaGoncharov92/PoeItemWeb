using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{

    public class UserConnection
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Index("IX_InnerId", IsUnique = true)]
        public string ConnectionId { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}