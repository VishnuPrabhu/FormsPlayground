using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using FormsPlayground.Features.Home.Model;
using FormsPlayground.Infrastructure.Exceptions;
using FormsPlayground.Infrastructure.Mvvm;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FormsPlayground.Features.Home.ViewModels
{
    public class FeatureListViewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, IEnumerable<FeatureNode>> LoadCommand { get; }
        public ReactiveCommand<FeatureNode, Unit> OpenFeatureCommand { get; }
        
        public extern IEnumerable<FeatureNode> Tree { [ObservableAsProperty] get; }

        private static string[] _jsonEmbeddedResources;
        
        public FeatureListViewModel(string jsonFile, string path = null)
        {
            LoadCommand = ReactiveCommand.Create<Unit, IEnumerable<FeatureNode>>(_ =>
            {
                var assembly = GetType().GetTypeInfo().Assembly;
                
                _jsonEmbeddedResources = _jsonEmbeddedResources 
                    ?? assembly
                        .GetManifestResourceNames()
                        .Where(x => x.EndsWith(".json"))
                        .ToArray();

                var resourceName = _jsonEmbeddedResources
                    .FirstOrDefault(x => x.EndsWith(jsonFile));
                
                if(resourceName == null)
                    throw new ControlledException($"{jsonFile} not found in embedded resources");

                var stream = assembly.GetManifestResourceStream(resourceName);
                
                using (var streamReader = new StreamReader(stream))
                {
                    var json = streamReader.ReadToEnd();
                    var tree = JsonConvert.DeserializeObject<IEnumerable<FeatureNode>>(json);
                    return tree;
                }
            });
            
            OpenFeatureCommand = ReactiveCommand.CreateFromTask<FeatureNode, Unit>(feature =>
            {
                return Task.FromResult(Unit.Default);
            });

            LoadCommand
                .ToPropertyEx(this, x => x.Tree);
        }
    }
}