using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;

namespace Fodonn.aff;

public partial class Loginsignup : ContentPage
{
	public Loginsignup()
	{
		InitializeComponent();

        ETop.RealUsername = null;
        string cvat = ETop.RealUsername;

        if (cvat != null){
            userLoggedIn111.IsVisible = false;
            userLoggedIn222.IsVisible = true;
             
            accountSettings.ItemsSource= new List<SettingsPass>{
                new SettingsPass { LeveName = "Personal Info", LeveIcon = "personal_sb.png" },
                //new SettingsPass{ LeveName="Preference",LeveIcon="preference_sb.png"},
                new SettingsPass{ LeveName="Privacy Act",LeveIcon="privacy_sb.png"},
                new SettingsPass{ LeveName="Contact",LeveIcon="about_sb.png"},
                new SettingsPass{ LeveName="Logout",LeveIcon="logout_sb.png"}
            };
            getinfoaa();
        }
    }
     
    public async void getinfoaa()
    {
        try
        {
            var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", ETop.RealUsername},
                    {"t","getinfoexceptdatas" },
                    {"api","rats" }
                });
            var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
            if (htmlResJson.code == 200)
            {
                var tstrk = JsonConvert.DeserializeObject<ETop.userEmailnPremium>(htmlResJson.message);
            if (tstrk.premiunm == 1) { xxupGradeaCC.IsVisible = false; } else { xxupGradeaCC.IsVisible = true; }
             
            }
        }catch (Exception ex) { _=ex; }
    }
    public   class SettingsPass
    {
        public   string LeveName { get; set; } 
        public  string LeveIcon { get; set; }
    }
    private async void Forgotpassword_Signup_tapped(object sender, TappedEventArgs e)
    {
        freePopup loader = new freePopup("loader");
        this.ShowPopup(loader);
        await ETop.SleepDelay(1234);


        var watB = ((TappedEventArgs)e);
        forgotPasswordPanel.IsVisible = false;
        signupPanel.IsVisible = false;
        loginPanel.IsVisible = false;
        if ((string)watB.Parameter == "signup")
        { 
            signupPanel.IsVisible = true; 
        }else if ((string)watB.Parameter == "login")
        {
            loginPanel.IsVisible = true;
        }else if ((string)watB.Parameter == "FP")
        {
            forgotPasswordPanel.IsVisible = true;
        }
        loader.Close();
    }

    private async void LoginSignupForgotBtn_ClickedAsync(object sender, EventArgs e)
    {
        freePopup loader = new freePopup("loader");
        this.ShowPopup(loader);
        await ETop.SleepDelay(1234);


        var btnClicked = (Button)sender;
        if (btnClicked.Text == "Login"){
            signupMessage.IsVisible = false;
            string err = "0";
            string uuname = loginemail.Text.Trim();
            string ppword = loginpword.Text.Trim();
             

            if ((uuname.Length < 3) || (ppword.Length < 5)) { err = "Input username/email and password greater than 6 characters!"; }
            if (err != "0"){signupMessage.IsVisible = true; signupMessage.Text = err;signupMessage.BackgroundColor = Colors.Red; }
            else{
                var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "pwod", ppword},
                    { "uname", uuname},
                    {"t","access:login" },
                    {"api","rats" }
                });
                var htmlResJson=JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                    if(htmlResJson.code == 200) {
                        ETop.userInfoFileData.WriteToAsync(htmlResJson.message);
                        ETop.RealUsername = uuname;    
                        if (ETop.RealUsername != null){
                            App.Current.MainPage = new NavigationPage(new MainPage());
                        }
                    }else{
                        signupMessage.IsVisible = true; signupMessage.Text = "Username/Email or Password is incorrect!"; signupMessage.BackgroundColor = Colors.Red;
                    _ = DisplayAlert("",htmlResJson.message,"OK");
                    } 
            }
        }else if (btnClicked.Text == "Signup"){
            signupMessage.IsVisible = false;
            string err = "0";
            string eemail = signupemail.Text.Trim();
            string uuname = signupuname.Text.Trim();
            string ppword = signuppwod.Text.Trim();

            Thread.Sleep(1470);

            if (uuname.Length < 3) { err = "Input username greater than 3 characters!"; }else
            if ((eemail.Length < 6)|| (!ETop.PregEmail.Match(eemail).Success)) { err = "Input a valid email!"; }else
            if (ppword.Length < 5) { err = "Your password should be greater than 6 characters!"; }else
            if (ppword != signuprepwod.Text){err = "Your password do not match!";}
            if (err != "0"){signupMessage.IsVisible = true; signupMessage.Text = err;signupMessage.BackgroundColor = Colors.Red;}
            else{
                var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "pwod", ppword},
                    { "uname", uuname},
                    { "email", eemail},
                    {"t","create:signup" },
                    {"api","rats" }
                }); 
                var htmlResJson=JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                if(htmlResJson.code == 200) {
                    ETop.userInfoFileData.WriteToAsync(uuname);
                         
                    ETop.RealUsername = uuname;    
                    Debug.WriteLine("Signupusername:"+ ETop.RealUsername);
                    if (ETop.RealUsername != null){
                        App.Current.MainPage = new NavigationPage(new MainPage());
                    }
                }else{
                    _ = DisplayAlert("", htmlResJson.message, "OK");
                } 
            }
        }else if(btnClicked.Text == "Recover Password"){
            signupMessage.IsVisible = false;
            string err = "0";
            string iusernamer = iusername.Text.Trim();
            string iemailr = iemail.Text.Trim();


            if (iusernamer.Length < 4) { err = "Input username a valid username greater than 6 characters!"; }
            if (!ETop.PregEmail.Match(iemailr).Success) { err = "Input username a email!"; }
            if (err != "0") { signupMessage.IsVisible = true; signupMessage.Text = err; signupMessage.BackgroundColor = Colors.Red; }
            else
            {
                var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "fname", iusernamer},
                    { "vemail", iemailr},
                    {"t","forgot:password"},
                    {"api","rats" }
                });
                var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                if (htmlResJson.code == 200)
                {
                    _=DisplayAlert("", htmlResJson.message,"OK");
                }
                else
                {
                    signupMessage.IsVisible = true; signupMessage.Text = htmlResJson.message; signupMessage.BackgroundColor = Colors.Red;                }
            }
        }
        loader.Close();
    }

    private void accountAcordion_Clicked(object sender, TappedEventArgs e)
    { string abcx = e.Parameter.ToString();
        if (abcx == "Logout") {
            ETop.LogoutUser(); return; }
        else if (abcx== "Privacy Act") {
            _ = Launcher.OpenAsync(ETop.apiHttpLinkDomain+"blogdiscuss/353491234/Privacy-Policy");return;
        } else {
            _ = Navigation.PushModalAsync(new SettingsAccount(e.Parameter.ToString())); return;
        }
    }

    private async void GotoHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void upgradeTapped_Tapped(object sender, TappedEventArgs e)
    {
        _ = Navigation.PushModalAsync(new upgradeAccount());
    }
    private void rememberMe_tapped(object sender, TappedEventArgs e)
    {
        checkRemember.IsChecked = !checkRemember.IsChecked;
    }
}
