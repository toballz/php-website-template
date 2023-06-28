using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;

namespace Fodonn.aff;

public partial class SettingsAccount : ContentPage
{
	public SettingsAccount(string arg)
	{
		InitializeComponent();
		if (arg == "Preference") {
            preferenceAcgh.IsVisible = true;
        }
		if (arg == "Contact") {
            ContactAcgh.IsVisible = true;
        }
		if (arg == "Personal Info") {
            PersonalInfoAcgh.IsVisible = true; 
            xxusername.Text = "Username: "+ ETop.RealUsername;
            getinfoaa();
        }
    }
    public class myuserDatas{public string email{get;set;}
        public int premiunm { get; set; }}
    public async void getinfoaa()
    {
        try { 
        var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", ETop.RealUsername},
                    {"t","getinfoexceptdatas" },
                    {"api","rats" }
                }); 
        var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
        if (htmlResJson.code == 200)
        {
            var tstrk = JsonConvert.DeserializeObject<myuserDatas>(htmlResJson.message);
            if (tstrk.premiunm == 1) { xxaccpremium.IsVisible = false; } else { xxaccpremium.IsVisible = true; }
            xxemail.Text = tstrk.email;
        }
        }catch(Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }
    }
    private async void logoutForgotPassword_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        ETop.LogoutUser();
    }
    private async void calcel_modalse(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private void upgradetopremium_Tapped(object sender, TappedEventArgs e)
    {
        _ = Navigation.PushModalAsync(new upgradeAccount());
    }

    private async void UpdateEmailNInfo_clicked(object sender, EventArgs e)
    {
        if(ETop.PregEmail.Match(xxemail.Text).Success){
            var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "email",xxemail.Text},
                    {"t","update:Email" },
                    {"api","rats" },
                    {"uname",ETop.RealUsername }
                });
            var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
            if (htmlResJson.code == 200)
            {
                _ = DisplayAlert("", htmlResJson.message, "OK");
            }
        }
        else
        {
            _ = DisplayAlert("", "Input a valid email address.", "OK");
        }
    }

    private async void ContactR_Us_2ruEmailClicked(object sender, EventArgs e)
    {
        //DeviceInfo.Platform.Equals(DevicePlatform.Android);
        var fname = contactv_fname.Text.Trim();
        var email = contactv_email.Text.Trim();
        var phone = contactv_phone.Text.Trim();
        var message = contactv_message.Text.Trim();
        if (fname.Length > 5 && ETop.PregEmail.Match(email).Success && phone.Length > 5 && message.Length > 5)
        {
            var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    {"email",xxemail.Text},
                    {"t","contact:businessEmail"},
                    {"api","rats"},
                    {"uname",ETop.RealUsername}
                });
            var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
            if (htmlResJson.code == 200)
            {
                _ = DisplayAlert("", htmlResJson.message, "OK");
                contactv_fname.Text = "";
                 contactv_email.Text = "";
                 contactv_phone.Text = "";
                contactv_message.Text="";
            }
        }
        else
        {
            _ = DisplayAlert("", "Input a valid contact information and message.", "OK");
        }
    }
}