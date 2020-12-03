using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Services
{
    public interface IWidgetService
    {
        string GetWidgetName();

        JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallCallParams, UserServiceTokensModel? serviceTokens = null);
    }
}
