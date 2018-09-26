using SterlingSwitch.Pages.Cards;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.TabView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyBank : SwitchMasterPage
    { 
        public MyBank ()
		{
			InitializeComponent ();
            SetupTabView();

        }

        private void SetupTabView()
        {
            var tabView = new TabViewControl(new List<TabItem>()
            {
                new TabItem("ACCOUNTS", new Grid{VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand}),
                new TabItem("CARDS", new CardsLanding{VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand}),
                new TabItem("WALLET", new Grid{VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand })
            });


            //tabView.HeaderTabTextFontFamily = Device.OnPlatform<string>("SF Pro Text", "Roboto-Regular.ttf#Roboto", "");
            tabView.HeaderTabTextFontSize = tabView.HeaderTabTextFontSize + 1.5;
            tabView.HeaderBackgroundColor = (Color)App.Current.Resources["BackgroundColor"];
            tabView.HeaderSelectionUnderlineColor = (Color)App.Current.Resources["PrimaryColor"];
            tabView.HeaderTabTextColor = Color.Black;
            tabView.VerticalOptions = LayoutOptions.FillAndExpand;
            tabView.HorizontalOptions = LayoutOptions.FillAndExpand;
            
            Grid.SetRow(tabView, 1);

            MainView.Children.Add(tabView);
        }
    }
}