﻿using System;
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
		public SummaryPage(ImageSource img)
		{
            InitializeComponent();

		    SelectedImage.Source = img;
		}
	}
}