using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Area.API.Models.Table.Owned;

namespace Area.API.Models.Table
{
    [Table("Enums")]
    public class EnumModel
    {
        [ForeignKey("EnumId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<EnumValueModel> Values { get; set; } = null!;
    }
}