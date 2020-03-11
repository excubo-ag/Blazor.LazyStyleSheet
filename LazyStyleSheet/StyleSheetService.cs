using System.Collections.ObjectModel;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Excubo.Blazor.Tests_LazyStyleSheet")]
namespace Excubo.Blazor.LazyStyleSheet
{
    internal class StyleSheetService : IStyleSheetService
    {
        public ObservableCollection<string> RequiredStyleSheets { get; } = new ObservableCollection<string>();
        public void Add(string url)
        {
            lock (RequiredStyleSheets)
            {
                if (!RequiredStyleSheets.Contains(url))
                {
                    RequiredStyleSheets.Add(url);
                }
            }
        }
    }
}
