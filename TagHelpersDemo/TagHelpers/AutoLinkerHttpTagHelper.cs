using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TagHelpersDemo.TagHelpers
{
    //    [HtmlTargetElement("p")]
    //    public class AutoLinkerHttpTagHelper : TagHelper
    //    {
    //        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    //        {
    //            var childContent = await output.GetChildContentAsync();
    //            // Find Urls in the content and replace them with their anchor tag equivalent.
    //            output.Content.SetHtmlContent(Regex.Replace(
    //                 childContent.GetContent(),
    //                 @"\b(?:https?://)(\S+)\b",
    //                  "<a target=\"_blank\" href=\"$0\">$0</a>"));  // http link version}
    //        }
    //    }

    //    [HtmlTargetElement("p")]
    //    public class AutoLinkerWwwTagHelper : TagHelper
    //    {
    //        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    //        {
    //            var childContent = await output.GetChildContentAsync();
    //            // Find Urls in the content and replace them with their anchor tag equivalent.
    //            output.Content.SetHtmlContent(Regex.Replace(
    //                childContent.GetContent(),
    //                 @"\b(www\.)(\S+)\b",
    //                 "<a target=\"_blank\" href=\"http://$0\">$0</a>"));  // www version
    //        }
    //    }
    [HtmlTargetElement("p")]
    public class AutoLinkerHttpTagHelper : TagHelper
    {
        public override int Order
        {
            get { return int.MinValue; }
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.Content.IsModified ? output.Content.GetContent() :
                (await output.GetChildContentAsync()).GetContent();

            var GetChildContent = await output.GetChildContentAsync();

            // Find Urls in the content and replace them with their anchor tag equivalent.
            output.Content.SetHtmlContent(Regex.Replace(
                 childContent,
                 @"\b(?:https?://)(\S+)\b",
                  "<a target=\"_blank\" href=\"$0\">$0</a>"));  // http link version}
        }
    }

    [HtmlTargetElement("p")]
    public class AutoLinkerWwwTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.Content.IsModified ? output.Content.GetContent() :
                (await output.GetChildContentAsync()).GetContent();
            var GetChildContent = await output.GetChildContentAsync();
            // Find Urls in the content and replace them with their anchor tag equivalent.
            output.Content.SetHtmlContent(Regex.Replace(
                 childContent,
                 @"\b(www\.)(\S+)\b",
                 "<a target=\"_blank\" href=\"http://$0\">$0</a>"));  // www version
        }
    }
}
