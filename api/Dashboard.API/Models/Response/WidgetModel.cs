using System;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class WidgetModel
    {
        public WidgetModel()
        { }

        public WidgetModel(uint id = 0, string name = "", ServiceModel? parentService = null)
        {
            Id = id;
            Name = name;
            if (parentService != null)
                ParentService = parentService;
        }

        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("parent_service")]
        public ServiceModel ParentService { get; set; } = new ServiceModel();
    }
}
