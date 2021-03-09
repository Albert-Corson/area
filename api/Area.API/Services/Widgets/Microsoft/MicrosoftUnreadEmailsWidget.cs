using System.Collections.Generic;
using System.Linq;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using Microsoft.Graph;
using Swan;

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

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> _)
        {
            var messages = Microsoft.Client!.Me.Messages
                .Request()
                .Filter("isRead eq false")
                .Select("subject,bodyPreview,hasAttachments,importance,webLink,from,isRead")
                .Top(50)
                .GetAsync()
                .Await();

            return from it in messages where !(it is EventMessageResponse) select new MicrosoftEmailModel(it);
        }
    }
}