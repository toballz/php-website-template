﻿using CommunityToolkit.Maui.Views;
using Fodonn.aff;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Fodonn;

public partial class MainPage : ContentPage
{
    public string mainDomain = ETop.apiHttpLink;
    public MainPage()
    {
        InitializeComponent();
        ETop.RealUsername = null;
        var username = ETop.RealUsername;
        Loaded += (s, e) => {
            Updatelist_Async(s, e);
            if (username == null){
                var y = new TappedEventArgs(e);
                GotoLogin_Clicked(s,y);
            }
        };
    }

    //FilesNdata 111111111111111111
    private async void Updatelist_Async(object sender, EventArgs e)
    {
        freePopup loader = new freePopup("loader");this.ShowPopup(loader);await ETop.SleepDelay(1234);
        try
        {
            string uuname = ETop.RealUsername;
            var gfh = new List<ETop.UserFilesnData>();
            if (uuname != null)
            {
                var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", uuname},
                    {"t","get" },
                    {"api","rats" }
                });
                //Debug.WriteLine("jakajaka:-0 "+httpResponse.ToString());return;
                ETop.ApiResponse htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                if (htmlResJson.code == 200)
                {
                    List<ETop.UserFilesnData> jsonString = JsonConvert.DeserializeObject<List<ETop.UserFilesnData>>(htmlResJson.message);
                    FilesndataSource.ItemsSource = jsonString;
                }
                else
                {
                    gfh.Add(new ETop.UserFilesnData { StrType = "", StrVal = htmlResJson.message });
                    FilesndataSource.ItemsSource = gfh;
                }
            }
            else
            {
                gfh.Add(new ETop.UserFilesnData { StrType = "", StrVal = "You have no data. Login / Signup to start." });
                FilesndataSource.ItemsSource = gfh;
            }
        }catch(Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }
            loader.Close();
    }
    private async void OnTap_imageNlabelTaped(object sender, TappedEventArgs e)
    {
        try
        {
            var LabelSender = (VerticalStackLayout)sender;
            var thisClickChildrensGridGetTypeInfo = LabelSender.Children.OfType<Grid>().ElementAt(0);
            var thisClickChildrensGridGetTypeInfoLabel = thisClickChildrensGridGetTypeInfo.Children.OfType<Label>();
            var infoLabelStrType=thisClickChildrensGridGetTypeInfoLabel.ElementAt(0);
            var infoLabelStrVal= thisClickChildrensGridGetTypeInfoLabel.ElementAt(1);
            if (infoLabelStrType.Text == "Link"){
                _ = Launcher.OpenAsync(infoLabelStrVal.Text);
            }
            else if (infoLabelStrType.Text == "Text")
            {
                if (ETop.RealUsername != null)
                {
                    await Clipboard.SetTextAsync(infoLabelStrVal.Text);
                    _ = DisplayAlert("Alert", "Your text has been copied!", "OK");
                }
            }
            else if (infoLabelStrType.Text == "File")
            {
                var ddato = await DisplayAlert("Alert", "Download your file?", "Download", "Cancel");
                if (ddato == true)
                {
#if ANDROID
                    var FileLocationDownloads = Path.Combine( Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                        Android.OS.Environment.DirectoryDownloads,
                        infoLabelStrVal.Text); 
#else
                    var FileLocationDownloads = Path.Combine(FileSystem.Current.AppDataDirectory, infoLabelStrVal.Text);
#endif
                    if (!File.Exists(FileLocationDownloads))
                    {
                        var httpClient = new HttpClient();
                        using (var stream = await httpClient.GetStreamAsync(ETop.apiHttpLinkDomain + "catch/" +infoLabelStrVal.Text))
                        {
                            using (var fileStream = new FileStream(FileLocationDownloads, FileMode.CreateNew))
                            {
                                await stream.CopyToAsync(fileStream);
                                var saye = await DisplayAlert("Alert", "Your file has been downloaded", "Open File", "Cancel");
                                if (saye == true)
                                {
                                    _ = Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(FileLocationDownloads), });
                                }
                            }
                        }
                    }else { _ = Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(FileLocationDownloads) }); }
                }
            }
        }catch (Exception ex) {
            freePopup errPopup = new freePopup("erroralert",ex.Message);this.ShowPopup(errPopup);
        }
    }
    private async Task ShareFile(string khj)
    {
        string fn = khj;
        string file = Path.Combine(FileSystem.CacheDirectory, fn);

        File.WriteAllText(file, "Hello World");

        await Share.Default.RequestAsync(new ShareFileRequest {
            Title = "Share text file",
            File = new ShareFile(file)
        });
    }

    private async void TapDeleteCopy_Tapped(object sender, TappedEventArgs e)
    {
        freePopup loader = new freePopup("loader");this.ShowPopup(loader);await ETop.SleepDelay(450);
        try
        {
            //var asg = e.Parameter.ToString().Split('-');

            var LabelSender = (VerticalStackLayout)sender;
            var thisClickChildrensGridGetTypeInfo = LabelSender.Children.OfType<Grid>().ElementAt(0);
            var thisClickChildrensGridGetTypeInfoLabel = thisClickChildrensGridGetTypeInfo.Children.OfType<Label>();
            
            var infoLabelStrTypeCopyTrash = thisClickChildrensGridGetTypeInfoLabel.ElementAt(0);
            var infoLabelStrVal = thisClickChildrensGridGetTypeInfoLabel.ElementAt(1);
            var infoLabelStrHash = thisClickChildrensGridGetTypeInfoLabel.ElementAt(2);
            if (infoLabelStrTypeCopyTrash.Text == "trash")
            {
                if (infoLabelStrHash.Text.Length > 3)
                {
                    string uuname = ETop.RealUsername;
                    var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", uuname},
                    {"deleteHash",infoLabelStrHash.Text},
                    {"api","rats" },
                    {"t","deleteData" }
                });
                    ETop.ApiResponse htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                    if (htmlResJson.code == 200){
                        Updatelist_Async(sender, e);
                    }
                }
            }else if (infoLabelStrTypeCopyTrash.Text == "copy"){
                await Clipboard.SetTextAsync(infoLabelStrVal.Text);
                _ = DisplayAlert("", "Your text has been copied", "OK");
            }
        }catch (Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }
        loader.Close();
    }

    //FilesNdata 000000000000000000



    //Upload Files n Datas 1111111111111111111111
    private async void UploadBtn_Clicked_Async(object sender, EventArgs e)
    {
        freePopup loader = new freePopup("loader");this.ShowPopup(loader);await ETop.SleepDelay(1234);
        try
        {
            string uname = ETop.RealUsername;
            string entrytext = @entryText.Text.Trim();

            if (uname != null){
                if (entrytext.Length > 0){
                    Regex reg = new Regex(ETop.PregURL, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    string strtype = (reg.IsMatch(entrytext) ? "Link" : "Text");

                    var httpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "jasonData", JsonConvert.SerializeObject(new ETop.UserFilesnData {StrType= strtype,StrVal= entrytext })},
                    { "uname", uname},
                    {"t","insert" },
                    {"api","rats" }
                    });
                    //_ = DisplayAlert("h", httpResponse, "g");return;
                    ETop.ApiResponse htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(httpResponse);
                    if (htmlResJson.code == 200)
                    {
                        entryText.Text = "";
                        Updatelist_Async(sender, e);
                    }else if (htmlResJson.code == 404){
                        Updatelist_Async(sender, e);
                        bool tt = await DisplayAlert("Alert", htmlResJson.message, "Upgrade", "Later");
                        if (tt == true){
                            _ = Navigation.PushModalAsync(new upgradeAccount());
                        }
                    }else{
                        _ = DisplayAlert("", htmlResJson.message,"OK");
                    }
                }
            } else {
                _ = DisplayAlert("Alert", "Login to start uploading data.", "OK");
            }
        }catch (Exception ex){
            freePopup errPopup = new freePopup("erroralert", ex.Message); this.ShowPopup(errPopup);
        }
        loader.Close();
    }
    private async void UploadFilesJust_Clicked(object sender, EventArgs e)
    {
        try
        {
            var getinfoexceptdatashttpResponse = await ETop.HttpConntAsync(new Dictionary<string, string> {
                    { "uname", ETop.RealUsername},
                    {"t","getinfoexceptdatas" },
                    {"api","rats" }
                });
            var getinfoexceptdatashtmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(getinfoexceptdatashttpResponse);
            if (getinfoexceptdatashtmlResJson.code == 200){
                var tstrk = JsonConvert.DeserializeObject<ETop.userEmailnPremium>(getinfoexceptdatashtmlResJson.message);
                if (tstrk.premiunm == 1) {   

                    var result = await FilePicker.Default.PickAsync();
                    if (result != null) {
                freePopup loader = new freePopup("loader");this.ShowPopup(loader);await ETop.SleepDelay(1234);

                var imageContent = new ByteArrayContent(File.ReadAllBytes(result.FullPath));
                var assa = new ByteArrayContent(new Byte[0]);
                var requestContent = new MultipartFormDataContent{
                    { assa, "t", "upload:filE;;"+ETop.RealUsername },
                    {imageContent, "flieContent", result.FileName}
                };

                using HttpClient client = new();
                var htmlRes = await client.PostAsync(ETop.apiHttpLink, requestContent);

                var htmlResJson = JsonConvert.DeserializeObject<ETop.ApiResponse>(htmlRes.Content.ReadAsStringAsync().Result);
                client.Dispose();
                entryText.Text = "";
                if (htmlResJson.code == 200)
                {
                    await DisplayAlert("Alert", htmlResJson.message, "OK");
                    Updatelist_Async(sender, e);
                }
                else
                {
                    await DisplayAlert("", htmlResJson.message, "OK");
                }
                loader.Close();
            }

                }else{var tf =await DisplayAlert("","Please upgrade to premium to send files.","Upgrade","Cancel");
                    if (tf == true){
                        _ = Navigation.PushModalAsync(new upgradeAccount());
                    }
                }

            }

        }catch (Exception ex){
            freePopup errPopup = new freePopup("erroralert",ex.Message);this.ShowPopup(errPopup);
        }

    }
    //Upload Files n Datas 0000000000000000000000


    //goto ologin 1111111111111111111111 
    public async void GotoLogin_Clicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Loginsignup());
    }
    //goto ologin 0000000000000000000000
}

