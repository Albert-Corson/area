using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Graph;
using Newtonsoft.Json;
using Swan;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    [SwaggerSchema("## This is the response model for the \"Microsoft Calendar\" widget")]
    public class MicrosoftEventModel : WidgetCallResponseItemModel
    {
        [SwaggerSchema("## This is an internal model of the `MicrosoftEvent` model")]
        public class AttendeeModel : MicrosoftPersonModel
        {
            [JsonProperty("response_status", Required = Newtonsoft.Json.Required.Always)]
            [SwaggerSchema("The response received from the attendee")]
            public string ResponseStatus { get; set; } = null!;

            [JsonProperty("required", Required = Newtonsoft.Json.Required.Always)]
            [SwaggerSchema("Indicates if the attendee's presence is required or not")]
            public bool Required { get; set; }
        }

        public MicrosoftEventModel(Event ev)
        {
            Header = ev.Subject;
            Content = ev.BodyPreview;
            Link = ev.WebLink;
            Start = DateTime.Parse(ev.Start.DateTime, null, DateTimeStyles.RoundtripKind).ToUnixEpochDate();
            End = DateTime.Parse(ev.End.DateTime, null, DateTimeStyles.RoundtripKind).ToUnixEpochDate();
            Organizer = new MicrosoftPersonModel {
                Email = ev.Organizer.EmailAddress.Address,
                Name = ev.Organizer.EmailAddress.Name
            };
            Location = ev.Location.DisplayName;
            Attendees = ev.Attendees.Select(attendee => new AttendeeModel {
                Email = attendee.EmailAddress.Address,
                Name = attendee.EmailAddress.Name,
                Required = attendee.Type == AttendeeType.Required,
                ResponseStatus = attendee.Status.Response switch {
                    ResponseType.NotResponded => "Not responded",
                    ResponseType.TentativelyAccepted => "Tentatively Accepted",
                    _ => attendee.Status.Response.ToString() ?? "None"
                }
            });
        }

        [JsonProperty("start", Required = Required.Always)]
        [SwaggerSchema("The start time of the event in UTC Linux EPOCH")]
        public long Start { get; set; }

        [JsonProperty("end", Required = Required.Always)]
        [SwaggerSchema("The end time of the event in UTC Linux EPOCH")]
        public long End { get; set; }

        [JsonProperty("organizer", Required = Required.Always)]
        [SwaggerSchema("Organizer of the event")]
        public MicrosoftPersonModel Organizer { get; set; }

        [JsonProperty("location", Required = Required.Always)]
        [SwaggerSchema("Location of the event")]
        public string Location { get; set; }

        [JsonProperty("attendees", Required = Required.Always)]
        [SwaggerSchema("List of attendees")]
        public IEnumerable<AttendeeModel> Attendees { get; set; }
    }
}