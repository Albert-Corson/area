using System.Collections.Generic;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Table.Owned;

namespace Area.API.Services.Widgets
{
    public interface IWidgetService
    {
        public virtual bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            return true;
        }

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams, ref WidgetCallResponseModel response);
    }
}