using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using FormsPlayground.Features.Home.Model;
using FormsPlayground.Infrastructure.Exceptions;
using FormsPlayground.Infrastructure.Mvvm;
using FormsPlayground.Resources;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;

namespace FormsPlayground.Features.Home.ViewModels
{
    public class FeatureListViewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, IEnumerable<FeatureNode>> LoadCommand { get; }
        public ReactiveCommand<FeatureNode, Unit> OpenFeatureCommand { get; }
        
        public extern IEnumerable<FeatureNode> Tree { [ObservableAsProperty] get; }

        private string _currentPath;
        
        public FeatureListViewModel(string resourceKey, string path = null)
        {
            LoadCommand = ReactiveCommand.Create<Unit, IEnumerable<FeatureNode>>(_ =>
            {
                var tree = ((IEnumerable<FeatureNode>) Application.Current
                    .Resources[resourceKey])
                    .ToList();

                if (path != null)
                {
                    _currentPath = path;

                    var pathParts = path.Split(new []{"."}, StringSplitOptions.RemoveEmptyEntries);
                    string title = null;
                    foreach (var part in pathParts)
                    {
                        var node = tree.First(x => x.Id == part);
                        title = node.Title;
                        tree = node.Children.ToList();
                    }

                    // set title after the loop to avoid unnecessary property changed events
                    Title = title;
                }

                // nodes with children lookup
                var parents = tree
                    .Where(x => x.Children != null)
                    .ToList();

                // throw when id is missing in parent nodes
                var firstFailure = parents.FirstOrDefault(x => string.IsNullOrEmpty(x.Id));
                if (firstFailure != null)
                    throw new ControlledException(
                        string.Format(Strings.FeatureListViewModel_Node_id_error, firstFailure.Title), 
                        Strings.FeatureListViewModel_Data_error);
                
                // throw with duplicated ids
                var duplicated = parents
                    .GroupBy(c => c.Id)
                    .Where(g => g.Skip(1).Any())
                    .SelectMany(c => c);
                
                if (duplicated.Count() > 1)
                    throw new ControlledException(
                        Strings.FeatureListViewModel_Feature_list_has_multiple_parent_nodes_with_the_same_id, 
                        Strings.FeatureListViewModel_Data_error);

                return tree;
            });
            
            OpenFeatureCommand = ReactiveCommand.CreateFromTask<FeatureNode, Unit>(async feature =>
            {
                if (feature.Children == null)
                {
                    if (feature.ViewModelType == null)
                    {
                        throw new ControlledException(
                            string.Format(Strings.FeatureListViewModel_ViewModelType_error, 
                                feature.Title, 
                                nameof(feature.ViewModelType)),
                            Strings.FeatureListViewModel_Data_error);
                    }
                    
                    await Navigator.Push(feature.ViewModelType, new {title = feature.Title});
                    return Unit.Default;
                }

                var nestedPath = _currentPath != null 
                    ? $"{_currentPath}.{feature.Id}"
                    : feature.Id;
                
                await Navigator.Push<FeatureListViewModel>(new {resourceKey, path=nestedPath});
                return Unit.Default;
            });

            LoadCommand
                .ToPropertyEx(this, x => x.Tree);
        }
    }
}