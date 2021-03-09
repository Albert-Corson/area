using System;
using Microsoft.Graph;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    [SwaggerSchema("## This is the response model for the \"Microsoft Todo list\" widget")]
    public class MicrosoftTodoModel : WidgetCallResponseItemModel
    {
        public MicrosoftTodoModel(TodoTask task)
        {
            Header = task.Title;
            Content = task.Body.Content;
            Status = task.Status switch {
                TaskStatus.NotStarted => "Not started",
                TaskStatus.InProgress => "In progress",
                TaskStatus.WaitingOnOthers => "Waiting on others",
                _ => task.Status.ToString()!
            };
            Importance = task.Importance.ToString()!;
        }

        [JsonProperty("importance", Required = Required.Always)]
        [SwaggerSchema("Importance level of the task")]
        public string Importance { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        [SwaggerSchema("Status of the task")]
        public string Status { get; set; }
    }
}