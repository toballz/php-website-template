using CommunityToolkit.Maui.Views;
using Newtonsoft.Json;
using static Fodonn.aff.SettingsAccount;

namespace Fodonn.aff;

public partial class upgradeAccount : ContentPage
{
    public class Goodsapp
    {
        public string img { get; set; }
        public string textmessg { get; set; }
    }
    
    public upgradeAccount()
	{
		InitializeComponent(); 
        goodlist.ItemsSource = new List<Goodsapp> { 
            new Goodsapp {img="ok.png",textmessg="Remove ads" },
            new Goodsapp {img="ok.png",textmessg="Unlimited sendable data" }, 
            new Goodsapp {img="ok.png",textmessg="Send photos and files" },
            new Goodsapp {img="ok.png",textmessg="Send data offline" },
            new Goodsapp {img="ok.png",textmessg="Unlock all features" }
        };





    }

    private async void calcel_modalse_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private string paymentType= "o1";
    private void selectPricePlan_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            string planmeter = e.Parameter.ToString();
            o1.Stroke = Color.FromArgb("#C8C8C8");
            o2.Stroke = Color.FromArgb("#C8C8C8");
            o3.Stroke = Color.FromArgb("#C8C8C8");
            o4.Stroke = Color.FromArgb("#C8C8C8");
            if (planmeter == "o1")
            {
                o1.Stroke = Color.FromArgb("#48e45a");
                paymentType = "o1";
            }
            else if (planmeter == "o2")
            {
                o2.Stroke = Color.FromArgb("#48e45a");
                paymentType = "o2";
            }
            else if (planmeter == "o3")
            {
                o3.Stroke = Color.FromArgb("#48e45a");
                paymentType = "o3";
            }
            else if (planmeter == "o4")
            {
                o4.Stroke = Color.FromArgb("#48e45a");
                paymentType = "o4";
            }
        }catch (Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }
    }

    private async void subscribeBuy_Clicked(object sender, EventArgs e)
    {
        try{
            var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", ETop.RealUsername},
                    {"t","getinfoexceptdatas" },
                    {"api","rats" }
                });
            var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);

            if (htmlResJson.code == 200)
            {
                myuserDatas tstrk = JsonConvert.DeserializeObject<myuserDatas>(htmlResJson.message);
                string stripeURL = (paymentType == "o1") ? "https://buy.stripe.com/eVaaF54CV3s8b728wx?prefilled_email=" + tstrk.email ://weekly
                             (paymentType == "o2") ? "https://buy.stripe.com/00g6oP0mF3s87UQfZ0?prefilled_email=" + tstrk.email ://mmonthly
                             (paymentType == "o3") ? "https://buy.stripe.com/00gfZp5GZ2o48YU8wz?prefilled_email=" + tstrk.email ://YEARLY
                             (paymentType == "o4") ? "https://buy.stripe.com/9AQ9B1c5n2o4dfa9AE?prefilled_email=" + tstrk.email ://ONETIME
                             "https://buy.stripe.com/eVaaF54CV3s8b728wx?prefilled_email=" + tstrk.email;//weekly
                WebView newwebview = new WebView // 1
                {
                    Source = stripeURL, 
                    Cookies = new System.Net.CookieContainer()
                };
                newwebview.Navigated += async (sender, e) =>
                {
                    if (e.Url == "http://example.com/" || e.Url == "http://example.com" || e.Url== "https://api.myladyexpress.com/api.php?end=x")
                    {
                        var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", ETop.RealUsername},
                    {"t","update:Premium" },
                    {"pretYpe",paymentType },
                    {"api","rats" }});
                        var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                        if (htmlResJson.code == 200)
                        {
                            _ = Navigation.PopModalAsync();
                            _ = Navigation.PopModalAsync();
                            _ = Navigation.PopAsync();
                            App.Current.MainPage = new NavigationPage(new MainPage());
                        }
                    }
                };
                var ll = new Label // 0
                {
                    Text = "Cancel",
                    TextColor = Colors.White,
                    Padding = new Thickness(2),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = 24
                };
                TapGestureRecognizer labelTap = new TapGestureRecognizer();
                labelTap.Tapped += (s, e) =>
                {
                    _ = Navigation.PopModalAsync();
                };
                ll.GestureRecognizers.Add(labelTap);

                var tt = new Grid(); // 01
                tt.RowDefinitions.Add(new RowDefinition(24));
                tt.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(ll, 0);
                Grid.SetRow(newwebview, 1);
                tt.Children.Add(ll);
                tt.Children.Add(newwebview);

                await Navigation.PushModalAsync(new ContentPage
                {
                    Content = new ContentView
                    {
                        Content = tt,
                        Padding = new Thickness(25, 65, 25, 0)
                    },
                    Background = Color.FromArgb("#673ab7")


                }, true);
            }
            else
            {
                _ = DisplayAlert("Error", "There has been an error. Please contact support!", "OK");
            }
        }
        catch (Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }

    }
}