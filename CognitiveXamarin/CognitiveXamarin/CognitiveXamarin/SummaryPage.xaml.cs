using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
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
		    AddTagButton.Clicked += AddTagButton_OnClicked;
            TrainButton.Clicked += TrainButton_OnClicked;
            //ProbabilitySlider.ValueChanged += FilterPrediction;

            SelectedImage.Source = img;
		    currentImgPath = filename;
            this.predictions = JObject.Parse(analysisResults).SelectToken("$.Predictions").ToObject<Prediction[]>();
		    
            //TODO: Find a better way to do this? Is there a better way to repeat using xaml?
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
		        //toggle.Toggled += ToggleSwitched;

                sl.Children.Add(toggle);
                //ad togle to the switch dictionary
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
	        if (currentImgPath == null)
	        {
	            await DisplayAlert("Please Select an Image:", "No image Selected", "OK");
	            return;
	        }

	        List<string> tags = new List<string>();

            foreach (var set in switchDictionary)
	        {
	            if (set.Key.IsToggled)
	            {
	                tags.Add(set.Value);
	            }
	        }
          
	        var results = await CognitiveTrainingService.CognitiveTraining(currentImgPath, tags);

	        if (!string.IsNullOrEmpty(results))
	        {
	            await DisplayAlert("Results", results, "OK");
            }
        }

	    //private static void ToggleSwitched(object sender, ToggledEventArgs e)
	    //{
     //       //if toogle is on...
	    //    if (e.Value)
	    //    {
     //           //add the corresponding value to the tag to the list of tags
     //       }
	    //    else
	    //    {
     //           //remove the corresponding value from the list of tags.
	    //    }
	    //}

        //private async void FilterPrediction(object sender, EventArgs e)
        //{
        //       //TODO: Add a filter based on the current value of the slider.
        //}



    }
}