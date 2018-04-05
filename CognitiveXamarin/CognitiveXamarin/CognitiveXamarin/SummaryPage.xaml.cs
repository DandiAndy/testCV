using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CognitiveXamarin.Objects;
using CognitiveXamarin.Services;
using Newtonsoft.Json.Linq;

namespace CognitiveXamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SummaryPage: ContentPage
	{
	    private Dictionary<Switch, string> switchDictionary = new Dictionary<Switch, string>();
	    private Prediction[] predictions;
	    private string currentImgPath;
        public SummaryPage(ImageSource img, string filename, string analysisResults)
		{
            InitializeComponent();
            //add the events and triggers
		    AddTagButton.Clicked += AddTagButton_OnClicked;
            TrainButton.Clicked += TrainButton_OnClicked;
            //set the img source to the img from previous page
            SelectedImage.Source = img;
            //set the img path to the img from previous page
		    currentImgPath = filename;
            //parse the json results to an object.
            this.predictions = JObject.Parse(analysisResults).SelectToken("$.Predictions").ToObject<Prediction[]>();
		    
            //TODO: Find a better way to do this? Is there a better way to repeat using xaml?
            /*foreach prediction from the results, create a horizontal stack layout with a label for the tag,
             a label for the prediction, and a toggle so that a user can select the tags that they want to 
             train with.*/
            foreach (var prediction in predictions)
		    {
		        var sl = new StackLayout { Orientation = StackOrientation.Horizontal };
                sl.Children.Add(new Label
                {
                    FontSize = 18,
                    Text = prediction.Tag,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });
		        sl.Children.Add(new Label
		        {
		            FontSize = 18,
                    Text = (prediction.Probability *100).ToString("N2") + "%",
		            HorizontalOptions = LayoutOptions.CenterAndExpand
		        });
		        Switch toggle = new Switch
		        {
		            HorizontalOptions = LayoutOptions.EndAndExpand
		        };
                sl.Children.Add(toggle);

                //also add toggle to the switch dictionary with the tag name so we know what the user selected.
                switchDictionary.Add(toggle, prediction.Tag);

                ListContainer.Children.Add(sl);
		    }

        }

        private void AddTagButton_OnClicked(object sender, EventArgs e)
        {
            //can't add empty strings or null...
            if (NewTagEntry.Text == null || NewTagEntry.Text.Length < 0)
            {
                return;
            }
            
            //add stack layout
            var sl = new StackLayout { Orientation = StackOrientation.Horizontal };

            //add tag name to stack layout
            sl.Children.Add(new Label
            {
                FontSize = 18,
                Text = NewTagEntry.Text,
                HorizontalOptions = LayoutOptions.StartAndExpand
            });

            //add toggle to stack layout
            Switch toggle = new Switch
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            //add toggle event
            //toggle.Toggled += ToggleSwitched;
            sl.Children.Add(toggle);
            switchDictionary.Add(toggle, NewTagEntry.Text);
            //add to the list container
            ListContainer.Children.Add(sl);
        }

	    private async void TrainButton_OnClicked(object sender, EventArgs e)
	    {
            //if no path, then no image is selected. I don't know how you got here...
	        if (currentImgPath == null)
	        {
	            await DisplayAlert("Please Select an Image:", "No image Selected", "OK");
	            return;
	        }

            //TODO: There must be a better way to do this. Big lists of tags will be slow.
	        List<string> tags = new List<string>();

            foreach (var set in switchDictionary)
	        {
	            if (set.Key.IsToggled)
	            {
	                tags.Add(set.Value);
	            }
	        }
            
            //make a call to the training service
	        var results = await CognitiveTrainingService.CognitiveTrainingRequest(currentImgPath, tags);

            //Prompt the user with the results of the request.
	        if (!string.IsNullOrEmpty(results))
	        {
	            await DisplayAlert("Results", results, "OK");
            }
        }

        //private async void FilterPrediction(object sender, EventArgs e)
        //{
        //       //TODO: Add a filter based on the current value of the slider.
        //}



    }
}