using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WagsReader.Services.Interfaces
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }
        Page CurrentPage { get; }
        void Configure(string pageKey, Type pageType);
        Task GoBack();
        Task GoToRoot();
        Task NavigateAsync(string pageKey, bool isAnimated = true);
        Task NavigateAsync(string pageKey, object parameter, bool isAnimated = true);
        Task ReplacePage(object page, object previousPage, bool isAnimated = true);
        Task ShellNavigateAsync(string route);
        Task ShellGoBack();
        Task ShellGoToRoot();
    }
}
