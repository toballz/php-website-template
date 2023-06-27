using CommunityToolkit.Maui.Views;

namespace Fodonn.aff;

public partial class freePopup:Popup
{
	public freePopup(string xg )
	{
		InitializeComponent();

		if (xg== "loader")
		{
			loaderIndicator.IsVisible = true;
		}

	}
}