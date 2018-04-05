using System;
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
            //add the events and triggers
		    CameraButton.Clicked += CameraButton_OnClicked;
		    FileButton.Clicked += FileButton_OnClicked;
		    AnalysisButton.Clicked += AnalysisButton_OnClicked;
        }

	    private async void CameraButton_OnClicked(object sender, EventArgs e)
	    {
            //initialize cross media
	        await CrossMedia.Current.Initialize();
            //if camera doesn't exist return
	        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
	        {
	            await DisplayAlert("No Camera", ":( No camera available.", "OK");
	            return;
	        }
            //otherwise start the camera and set the quality of image
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
	        {
	            Directory = "Sample",
	            Name = "test.jpg",
	            PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            });
            //if file wasn't created return
	        if (file == null)
	            return;
            
            //otherwise set the img path to the files location and set the img to display on screen
	        currentImgPath = file.Path;
	        SelectedImage.Source = ImageSource.FromStream(() => file.GetStream());
        }

	    private async void FileButton_OnClicked(object sender, EventArgs e)
	    {
            //initialize cross media
	        await CrossMedia.Current.Initialize();
            
            //attempt to open the file directory view and covert image quality
	        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
	        {
	            PhotoSize = PhotoSize.Medium,
	            CompressionQuality = 92
            });
	        //if file wasn't created return
            if (file == null)
	            return;

	        //otherwise set the img path to the files location and set the img to display on screen
            currentImgPath = file.Path;
            SelectedImage.Source = ImageSource.FromStream(() => file.GetStream());
        }

	    private async void AnalysisButton_OnClicked(object sender, EventArgs e)
	    {
            //check if we have a selected img
	        if (currentImgPath == null)
	        {
                //if not return and prompt
	            await DisplayAlert("Please Select an Image:", "No image Selected", "OK");
	            return;
	        }
            //otherwise call the analysis request
            var results = await CognitiveAnalysisService.CognitiveAnalysisRequest(currentImgPath);

            //given success, pass the results forward to the next page with the img source and img path
            if(!string.IsNullOrEmpty(results)) { 
	            await Navigation.PushAsync(new SummaryPage(SelectedImage.Source, currentImgPath, results));
            }
        }
    }
}
