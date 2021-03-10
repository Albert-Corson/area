using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using Microsoft.Graph;

namespace Area.API.Services.Widgets.Microsoft
{
    public class MicrosoftUnreadEmailsWidget : IWidget
    {
        public MicrosoftUnreadEmailsWidget(MicrosoftService microsoft)
        {
            Microsoft = microsoft;
        }

        private MicrosoftService Microsoft { get; }

        public int Id { get; } = 14;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(IEnumerable<ParamModel> _)
        {
            var messages = await Microsoft.Client!.Me.Messages
                .Request()
                .Filter("isRead eq false")
                .Select("subject,bodyPreview,hasAttachments,importance,webLink,from,isRead")
                .Top(50)
                .GetAsync();

            return from it in messages where !(it is EventMessageResponse) select new MicrosoftEmailModel(it);
        }
    }
}