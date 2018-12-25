using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using FeserWard.Controls;
using Mono.Cecil;

namespace BOA.OneDesigner.WpfControls
{
    class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
    {
        public string TypesAssemblyFileName { get; set; } = "BOA.Types.CardGeneral.DebitCard.dll";

        public string RequestFullName { get; set; }


        #region Public Methods
        public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
        {
            return GetValue().Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
        }
        #endregion

        #region Methods
        protected virtual IEnumerable<string> Sort(IEnumerable<string> preResults)
        {
            return preResults.OrderByDescending(s => s.Length);
        }

        List<string> GetValue()
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly($@"D:\boa\server\bin\{TypesAssemblyFileName}");

            var items = new List<string>();

            foreach (var moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (var type in moduleDefinition.Types)
                {
                    if (type.Name.Contains("<"))
                    {
                        continue;
                    }

                    if (type.FullName == "BOA.Types.CardGeneral.DebitCard.CardStockRequest")
                    {
                        foreach (var propertyDefinition in type.Properties)
                        {
                            items.Add(propertyDefinition.Name);
                        }
                    }
                }
            }

            return items;
        }
        #endregion
    }
}