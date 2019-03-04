using System;

namespace FormsPlayground.Features.Home.Model
{
    public class FeatureNode
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public Link[] Authors { get; set; }
        public Link[] References { get; set; }
        public Type ViewModelType { get; set; }
        public string PageKey { get; set; }
        public FeatureNode[] Children { get; set; }
    }
}