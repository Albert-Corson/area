using Microsoft.Graph;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    [SwaggerSchema("## This is the response model for the \"Microsoft Outlook Unread Emails\" widget")]
    public class MicrosoftEmailModel : WidgetCallResponseItemModel
    {
        public MicrosoftEmailModel(Message message)
        {
            Header = message.Subject;
            Content = message.BodyPreview;
            Link = message.WebLink;
            HasAttachment = message.HasAttachments ?? false;
            Importance = message.Importance.ToString() ?? "Normal";
            From = new MicrosoftPersonModel {
                Email = message.From.EmailAddress.Address,
                Name = message.From.EmailAddress.Name
            };
        }

        [JsonProperty("has_attachment", Required = Required.Always)]
        [SwaggerSchema("Indicates if the email has an attachments or not")]
        public bool HasAttachment { get; set; }

        [JsonProperty("importance", Required = Required.Always)]
        [SwaggerSchema("Importance of the email")]
        public string Importance { get; set; }

        [JsonProperty("from", Required = Required.Always)]
        [SwaggerSchema("The person who sent the email")]
        public MicrosoftPersonModel From { get; set; }
    }
}