using System.Collections.Generic;
using Area.API.Models;
using Area.API.Models.Table;

namespace Area.API.Services.Widgets
{
    public interface IWidget
    {
        public int Id { get; }

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams, ref WidgetCallResponseModel response);
    }
}