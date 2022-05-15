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
        // 같은 Popup Window 여러개 띄우는 경우가 있을 수 있기 때문에 서비스 수명주기를 AddTransient 사용
        services.AddTransient<IDialogService, DialogService>();
        // 같은 Popup Window 여러개 띄우는 경우가 있을 수 있기 때문에 서비스 수명주기를 AddTransient 사용
        services.AddTransient<IDialog, PopWindow>();
        services.AddSingleton<IUserProfileViewDialog, UserProfileViewWindow>();

        services.AddSingleton<ILoginService, LoginService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<ISettingService, SettingService>();
        services.AddSingleton<IUserProfileViewerDialogService, UserProfileViewerDialogService>();

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
