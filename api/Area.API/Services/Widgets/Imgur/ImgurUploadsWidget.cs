using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Services.Services;
using Imgur.API.Endpoints.Impl;

namespace Area.API.Services.Widgets.Imgur
{
    public class ImgurUploadsWidget : IWidget
    {
        public ImgurUploadsWidget(ImgurService imgur)
        {
            Imgur = imgur;
        }

        private ImgurService Imgur { get; }

        public int Id { get; } = 3;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(IEnumerable<ParamModel> _)
        {
            var accountEndpoint = new AccountEndpoint(Imgur.Client);
            var result = await accountEndpoint.GetAlbumsAsync();

            var items = new List<WidgetCallResponseItemModel>();
            foreach (var album in result) {
                var imageTask = new AlbumEndpoint(Imgur.Client).GetAlbumImagesAsync(album.Id);
                imageTask.Wait();
                if (!imageTask.IsCompletedSuccessfully)
                    continue;
                var imageLink = imageTask.Result.FirstOrDefault()?.Link;
                if (imageLink == null)
                    continue;
                items.Add(new WidgetCallResponseItemModel {
                    Image = imageLink,
                    Header = album.Title,
                    Content = album.Description,
                    Link = album.Link
                });
            }

            return items;
        }
    }
}