using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Fodonn
{
    internal class ETop
    {
        public static string UsernameDEF = ".userInfoGg.";
        public static Regex PregEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public static string PregURL = "^http(|s)?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$";
        private static string _o;
        public static string RealEmail { get; set; }
        public static string RealUsername
        {
            get { return _o; }
            set { _o = userInfoFileData.ReadFromAsync(null)[0]; }
        }
        public class ApiResponse
        {
            public int code { get; set; }
            public string message { get; set; }
        }
        public class UserFilesnData
        {   //{"StrType":"","StrVal":""}
            /// <summary>
            /// string type [[ text / link / filePath ]]
            /// </summary>
            public string StrType { get; set; }
            /// <summary>
            /// string [[ echo if string or link ]]
            /// </summary>
            public string StrVal { get; set; }
            public string sha256hash { get; set; }
        }
        public static void LogoutUser()
        {
            userInfoFileData.WriteToAsync(null);
            userInfoFileData.DeleteTheAsync(); RealUsername = null;
            if (RealUsername == null) { Application.Current.MainPage = new NavigationPage(new MainPage()); }
        }
        public static class userInfoFileData
        {
            static string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, UsernameDEF);

            public static async void WriteToAsync(string afg)
            {
                using FileStream outputStream = File.OpenWrite(targetFile);
                using StreamWriter streamWriter = new StreamWriter(outputStream);

                await streamWriter.WriteAsync(afg);
            }
            public static string[] ReadFromAsync(string afg)
            {
                if (File.Exists(targetFile))
                {
                    return File.ReadAllLines(targetFile);
                }
                else
                {
                    string[] jj = new string[1];
                    jj[0] = null; return jj;
                }
            }
            public static void DeleteTheAsync()
            {
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
            }
        }
        public class userEmailnPremium
        {
            public string email { get; set; }
            public int premiunm { get; set; }
        }
        public static string apiHttpLinkDomain = "https://fodonn.com/";
        public static string apiHttpLink = apiHttpLinkDomain + "api.php";
        public static async Task<string> HttpConntAsync(Dictionary<string, string> datafae, string url = null)
        {
            string attdx;
            try{
                using HttpClient client = new HttpClient{ Timeout = TimeSpan.FromMinutes(2) };
                var content = new FormUrlEncodedContent(datafae);
                var response = await client.PostAsync(url == null ? apiHttpLink : url, content);
                attdx = await response.Content.ReadAsStringAsync();
                //Debug.WriteLine("ErrorHttpReq: " + attdx);
            }
            catch (Exception ex) {
                Debug.WriteLine("ErrorHttpReq: " + ex.Message);
                attdx = "{\"code\":600,\"message\":\"Check your internet connection.\"}";
            }
            return attdx;
        }
        public static async Task SleepDelay(int numSec)
        {
            await new TaskFactory().StartNew(() => { Thread.Sleep(numSec); });
        }
    }
}
