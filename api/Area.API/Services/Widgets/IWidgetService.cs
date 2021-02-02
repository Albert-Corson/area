using Area.API.Models;
using Area.API.Models.Table.Owned;

namespace Area.API.Services.Widgets
{
    public interface IWidgetService
    {
        public virtual bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            return true;
        }

        public void CallWidgetApi(WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response);
    }
}