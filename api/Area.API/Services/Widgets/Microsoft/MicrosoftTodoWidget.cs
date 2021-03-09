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
    public class MicrosoftTodoWidget : IWidget
    {
        public MicrosoftTodoWidget(MicrosoftService microsoft)
        {
            Microsoft = microsoft;
        }

        private MicrosoftService Microsoft { get; }

        public int Id { get; } = 15;

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams, ref WidgetCallResponseModel response)
        {
            var taskLists = Microsoft.Client!.Me.Todo.Lists.Request().GetAsync().Await();

            var tasks = from it in taskLists
                select Microsoft.Client!.Me.Todo.Lists[it.Id]
                    .Tasks
                    .Request()
                    .GetAsync();

            List<MicrosoftTodoModel> list = new List<MicrosoftTodoModel>();
            foreach (var task in tasks) {
                var todos = task.Await();
                if (todos == null)
                    continue;
                list.AddRange(from it in todos
                    where it.Status != TaskStatus.Completed
                    select new MicrosoftTodoModel(it));
            }

            response.Items = list;
        }
    }
}