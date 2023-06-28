using CommunityToolkit.Maui.Views;

namespace Fodonn.aff;

public partial class freePopup:Popup
{
	public freePopup(string xg,string errText=null )
	{
		InitializeComponent();

		if (xg== "loader")
		{
			loaderIndicator.IsVisible = true;
		}else if (xg=="erroralert")
		{
            CanBeDismissedByTappingOutsideOfPopup=true;
                errorText.IsVisible=true;
				errorText.Text=errText;
            gridParent.Padding=new Thickness(0,0,0,0);


        }

	}
}