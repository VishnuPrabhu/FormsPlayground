using System.Collections.Generic;

namespace FormsPlayground.Features.Home.Model
{
    public class FeatureNode
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public Link[] AuthorLinks { get; set; }
        public string ViewModelClass { get; set; }
        public string PageKey { get; set; }

        public IEnumerable<FeatureNode> Children { get; set; }
    }
}