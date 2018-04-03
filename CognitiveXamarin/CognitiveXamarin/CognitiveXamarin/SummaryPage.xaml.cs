using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CognitiveXamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SummaryPage: ContentPage
	{

	    private string analysisResults;

        public SummaryPage(ImageSource img, string analysisResults)
		{
            InitializeComponent();

		    SelectedImage.Source = img;

		    this.analysisResults = analysisResults;
		}

	    protected override void OnAppearing()
	    {
            ResultPrompt();
	    }

	    public async void ResultPrompt()
	    {
	        await DisplayAlert("Results", analysisResults, "Ok");
	    }
	}
}