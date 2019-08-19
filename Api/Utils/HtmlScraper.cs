using AngleSharp;
using AngleSharp.Dom;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Utils
{
    public class HtmlScraper
    {
        readonly IBrowsingContext _browsingContext;

        public HtmlScraper(IBrowsingContext browsingContext)
        {
            _browsingContext = browsingContext;
        }

        public async Task<ISet<EntryDto>> Run()
        {
            var htmlDoc = await DownloadHtmlDocument();
            var entrys = ParseHtmlDocument(htmlDoc);
            return entrys;
        }

        Task<IDocument> DownloadHtmlDocument() => _browsingContext.OpenAsync("http://www.informepastran.com/");

        ISet<EntryDto> ParseHtmlDocument(IDocument doc)
        {
            return ParseElements(doc.QuerySelectorAll(".post_box").ToArray());
        }

        ISet<EntryDto> ParseElements(IElement[] elems)
        {
            return elems.Aggregate(new HashSet<EntryDto>(), (list, entry) => 
            {
                var href = entry.QuerySelector("a").GetAttribute("href");
                var title = entry.QuerySelector("a").GetAttribute("title");
                var guid = Guid.NewGuid();
                var imageUrl = entry.QuerySelector("img").GetAttribute("src");
                var previewHistory = entry.QuerySelector(".entry-content p").TextContent;

                var entryDto = new EntryDto() {
                    Id = guid,
                    Title = title,
                    Link = href,
                    ImageUrl = imageUrl,
                    PreviewHistory = previewHistory
                };

                list.Add(entryDto);

                return list;
            });
        }
    }
}
