using System.Windows.Input;
using RealEstateApp.Models;

namespace RealEstateApp.ViewModels;

public class LoginPageViewModel : BaseViewModel
{
    public string Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public LoginResult Result
    {
        get => _result;
        set
        {
            _result = value;
            ((Command)LogoutCommand).ChangeCanExecute();
        }
    }

    private Command _loginCommand;

    public ICommand LoginCommand => _loginCommand ??= new Command(async () =>
    {
        if (Username != "username" || Password != "password") return;
        
        Result = new LoginResult()
        {
            Succeed = true,
            AccessToken = Guid.NewGuid().ToString(),
            RefreshToken = Guid.NewGuid().ToString()
        };

        await SecureStorage.SetAsync(nameof(Result.AccessToken), Result.AccessToken);
        await SecureStorage.SetAsync(nameof(Result.RefreshToken), Result.RefreshToken);
    });

    private Command _logoutCommand;
    private string _username;
    private string _password;
    private LoginResult _result;

    public ICommand LogoutCommand => _logoutCommand ??= new Command(() =>
    {
        SecureStorage.Remove(nameof(Result.AccessToken));
        SecureStorage.Remove(nameof(Result.RefreshToken));
        Result = new LoginResult();
    }, () => !string.IsNullOrWhiteSpace(Result?.AccessToken));

    public async Task LoadTokens()
    {
        string accessToken = await SecureStorage.GetAsync(nameof(Result.AccessToken));
        string refreshToken = await SecureStorage.GetAsync(nameof(Result.RefreshToken));

        if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            Result = new LoginResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Succeed = true
            };
        }
    }
}