using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using ViewModels;
using ViewModels.MainSettingViewModels;
using Views.Windows;

namespace MainEntry;

public class Bootstrapper
{
    public Bootstrapper(ref IServiceProvider services)
    {
        services = ConfigureServices();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Services
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<IDialog, PopWindow>();

        services.AddSingleton<ILoginService, LoginService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<ISettingService, SettingService>();

        // Viewmodels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<GeneralViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<FriendListViewModel>();
        services.AddTransient<ChattingListViewModel>();

        return services.BuildServiceProvider();
    }
}
