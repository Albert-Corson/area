using System;
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
    public class MicrosoftCalendarWidget : IWidget
    {
        public MicrosoftCalendarWidget(MicrosoftService microsoft)
        {
            Microsoft = microsoft;
        }

        private MicrosoftService Microsoft { get; }

        public int Id { get; } = 13;

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams, ref WidgetCallResponseModel response)
        {
            var today = DateTime.Today.ToUniversalTime();
            var options = new List<Option> {
                new QueryOption("startdatetime", today.ToString("s")),
                new QueryOption("enddatetime", today.AddDays(7).ToString("s"))
            };

            var events = Microsoft.Client!.Me.CalendarView
                .Request(options)
                .Select("subject,bodyPreview,organizer,attendees,start,end,location,webLink")
                .GetAsync()
                .Await();

            response.Items = events.Select(ev => new MicrosoftEventModel(ev));
        }
    }
}