using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CognitiveXamarin.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace CognitiveXamarin
{
	public partial class MainPage : ContentPage
	{
	    private string currentImgPath;
        public MainPage()
		{
			InitializeComponent();

		    CameraButton.Clicked += CameraButton_OnClicked;
		    FileButton.Clicked += FileButton_OnClicked;
		    AnalysisButton.Clicked += AnalysisButton_OnClicked;
        }

	    private async void CameraButton_OnClicked(object sender, EventArgs e)
	    {
	        await CrossMedia.Current.Initialize();

	        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
	        {
	            await DisplayAlert("No Camera", ":( No camera available.", "OK");
	            return;
	        }


            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
	        {
	            Directory = "Sample",
	            Name = "test.jpg",
	            PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            });

	        if (file == null)
	            return;

	        currentImgPath = file.Path;
      
	        SelectedImage.Source = ImageSource.FromStream(() => file.GetStream());
        }

	    private async void FileButton_OnClicked(object sender, EventArgs e)
	    {
	        await CrossMedia.Current.Initialize();

	        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
	        {
	            await DisplayAlert("No Camera", ":( No camera available.", "OK");
	            return;
	        }


	        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
	        {
	            PhotoSize = PhotoSize.Medium,
	            CompressionQuality = 92
            });

	        
	        if (file == null)
	            return;

	        currentImgPath = file.Path;

            SelectedImage.Source = ImageSource.FromStream(() => file.GetStream());
        }

	    private async void AnalysisButton_OnClicked(object sender, EventArgs e)
	    {
	        if (currentImgPath == null)
	        {
	            await DisplayAlert("Please Select an Image:", "No image Selected", "OK");
	            return;
	        }

            var results = await CognitiveAnalysisService.CognitiveRequest(currentImgPath);

            if(!string.IsNullOrEmpty(results)) { 
	            await Navigation.PushAsync(new SummaryPage(SelectedImage.Source, results));
            }
        }
    }
}
