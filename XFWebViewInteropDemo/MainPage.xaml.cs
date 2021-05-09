﻿using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XFWebViewInteropDemo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void GoToDefaultWebViewButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DefaultWebViewDemoPage());
        }

        private async void GoToHybridWebViewButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HybridWebViewDemoPage());
        }


    }
}
