using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CognitiveXamarin
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private void Analysis_OnClicked(object sender, EventArgs e)
	    {
	        Navigation.PushAsync(new SummaryPage());
	    }
	}
}
