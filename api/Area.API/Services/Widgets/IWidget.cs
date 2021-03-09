using System.Collections.Generic;
using System.Threading.Tasks;
using Area.API.Models;
using Area.API.Models.Table;

namespace Area.API.Services.Widgets
{
    public interface IWidget
    {
        public int Id { get; }

        public Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(IEnumerable<ParamModel> widgetCallParams);
    }
}