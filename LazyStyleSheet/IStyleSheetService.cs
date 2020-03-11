using System.Collections.ObjectModel;

namespace Excubo.Blazor.LazyStyleSheet
{
    public interface IStyleSheetService
    {
        ObservableCollection<string> RequiredStyleSheets { get; }
        void Add(string url);
    }
}
