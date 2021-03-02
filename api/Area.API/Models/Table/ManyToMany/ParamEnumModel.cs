using System.ComponentModel.DataAnnotations.Schema;

namespace Area.API.Models.Table.ManyToMany
{
    [Table("ParamsToEnums")]
    public class ParamEnumModel
    {
        public int ParamId { get; set; }

        public ParamModel Param { get; set; } = null!;

        public int EnumId { get; set; }

        public EnumModel Enum { get; set; } = null!;
    }
}