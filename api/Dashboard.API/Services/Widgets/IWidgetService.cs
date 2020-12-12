using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Widgets
{
    public interface IWidgetService
    {
        public string Name { get; }

        public virtual bool ValidateServiceAuth(UserServiceTokensModel serviceTokens) => true;

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response);
    }
}
