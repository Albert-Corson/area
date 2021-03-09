using System.Collections.Generic;
using Area.API.Models;
using Area.API.Models.Table;

namespace Area.API.Services.Widgets
{
    public interface IWidget
    {
        public int Id { get; }

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams);
    }
}