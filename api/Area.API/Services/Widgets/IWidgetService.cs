using Area.API.Models;
using Area.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Widgets
{
    public interface IWidgetService
    {
        public string Name { get; }

        public virtual bool ValidateServiceAuth(UserServiceTokensModel serviceTokens) => true;

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response);
    }
}
