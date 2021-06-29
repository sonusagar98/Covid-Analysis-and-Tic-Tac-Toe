using CovidAnalysisAssignment.Interfaces;
using CovidAnalysisAssignment.Models;
using CovidAnalysisAssignment.Services;
using CovidAnalysisAssignment.ViewModels;
using CovidAnalysisAssignment.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace CovidAnalysisAssignment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow>();
            containerRegistry.RegisterSingleton<IGetStates,GetStates>();
            containerRegistry.RegisterSingleton<IAllocateArea, AllocateArea>();
            containerRegistry.RegisterSingleton<IGetMonths, GetMonths>();
            containerRegistry.RegisterSingleton<ICheckHeader, CheckHeader>();
        }
    }
}
