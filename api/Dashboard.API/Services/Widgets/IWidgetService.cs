using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Services.Widgets
{
    public interface IWidgetService
    {
        public string Name { get; }

        public virtual bool ValidateServiceAuth(UserServiceTokensModel serviceTokens) => true;

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams);
    }
}
