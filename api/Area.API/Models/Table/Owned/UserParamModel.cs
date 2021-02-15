using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasParams")]
    public class UserParamModel
    {
        public int ParamId { get; set; }

        public ParamModel Param { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}