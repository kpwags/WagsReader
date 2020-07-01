using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WagsReader.Services.Interfaces;
using Xamarin.Forms;

namespace WagsReader.Services
{
    public class NavigationService : INavigationService
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly Stack<NavigationPage> _navigationPageStack = new Stack<NavigationPage>();
        private NavigationPage CurrentNavigationPage => _navigationPageStack.Peek();
        const bool DefaultAnimate = false;

        public Page CurrentPage
        {
            get
            {
                lock (_sync)
                {
                    if (CurrentNavigationPage?.CurrentPage == null)
                    {
                        return null;
                    }

                    return CurrentNavigationPage.CurrentPage;
                }
            }
        }

        public string CurrentPageKey
        {
            get
            {
                lock (_sync)
                {
                    if (CurrentNavigationPage?.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = CurrentNavigationPage.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (_sync)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public Page SetRootPage(string rootPageKey)
        {
            var rootPage = GetPage(rootPageKey);
            _navigationPageStack.Clear();

            var mainPage = new NavigationPage(rootPage);
            _navigationPageStack.Push(mainPage);

            return mainPage;
        }

        private Page GetPage(string pageKey, object parameter = null)
        {
            lock (_sync)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"No such page ({pageKey}). Did you forget to call NavigationService.Configure?");
                }

                var type = _pagesByKey[pageKey];
                ConstructorInfo constructor;
                object[] parameters;

                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                    {

                    };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                var p = c.GetParameters();
                                return p.Length == 1 && p[0].ParameterType == parameter.GetType();
                            });

                    parameters = new object[]
                    {
                        parameter
                    };
                }

                if (constructor == null)
                {
                    throw new InvalidOperationException($"No suitable constructor found for {pageKey}");
                }

                var page = constructor.Invoke(parameters) as Page;
                return page;
            }
        }

        public async Task GoBack()
        {
            var navigationStack = CurrentNavigationPage.Navigation;
            if (navigationStack.NavigationStack.Count > 1)
            {
                await CurrentNavigationPage.PopAsync();
                return;
            }

            if (_navigationPageStack.Count > 1)
            {
                _navigationPageStack.Pop();
                await CurrentNavigationPage.Navigation.PopModalAsync();
                return;
            }

            await CurrentNavigationPage.PopAsync();
        }

        public async Task GoToRoot()
        {
            var navigationStack = CurrentNavigationPage.Navigation;
            if (navigationStack.NavigationStack.Count > 1)
            {
                await CurrentNavigationPage.Navigation.PopToRootAsync();
                return;
            }

            if (_navigationPageStack.Count > 1)
            {
                _navigationPageStack.Clear();
                await CurrentNavigationPage.Navigation.PopToRootAsync();
                return;
            }
        }

        public async Task NavigateAsync(string pageKey, bool isAnimated = DefaultAnimate)
        {
            await NavigateAsync(pageKey, null, isAnimated);
        }

        public async Task NavigateAsync(string pageKey, object parameter, bool isAnimated = DefaultAnimate)
        {
            var page = GetPage(pageKey, parameter);
            await CurrentNavigationPage.Navigation.PushAsync(page, isAnimated);
        }

        public async Task ReplacePage(object page, object pageBefore, bool isAnimated = DefaultAnimate)
        {
            var navigationStack = CurrentNavigationPage.Navigation;
            navigationStack.InsertPageBefore((Page)page, (Page)pageBefore);

            await navigationStack.PopAsync();
        }

        public async Task ShellNavigateAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task ShellGoBack()
        {
            if (Shell.Current.Navigation.ModalStack.Count() > 0)
            {
                await Shell.Current.Navigation.PopModalAsync(DefaultAnimate);
            }
            else if (Shell.Current.Navigation.NavigationStack.Count() > 0)
            {
                await Shell.Current.Navigation.PopAsync(DefaultAnimate);
            }
        }

        public async Task ShellGoToRoot()
        {
            var shellNavigation = Shell.Current.Navigation;
            while (shellNavigation.ModalStack.Count > 0)
            {
                await Shell.Current.Navigation.PopModalAsync(DefaultAnimate);
            }

            while (shellNavigation.NavigationStack.Count > 1)
            {
                await Shell.Current.Navigation.PopAsync(DefaultAnimate);
            }
        }
    }
}
