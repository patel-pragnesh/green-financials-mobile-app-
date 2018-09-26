using SterlingSwitch.PopUps;
using SterlingSwitch.Services;
using SterlingSwitch.Services.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardsLanding : ContentView
	{
		public CardsLanding ()
		{
			InitializeComponent ();
        }

        int Position { get; set; }

       

        private void Setup()
        {
            var Cards = GlobalStaticFields.Customer.MyCards;

            if (Cards?.Count > 0)
            {
                NoCardTxt.IsVisible = false;
                CardsCarousel.ItemsSource = Cards;
            }
            else
            {
                NoCardTxt.IsVisible = true;
            }
        }

        private void CardsCarousel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Position = CardsCarousel.Position;
            var MyCardList = GlobalStaticFields.Customer.MyCards;

            //if (MyCardList?.Count > 1)
            //{
            //    try
            //    {
            //        if (MyCardList.ElementAt(Position) == MyCardList.LastOrDefault())
            //            MoreCardsImg.IsVisible = false;
            //        else
            //            MoreCardsImg.IsVisible = true;

            //        if (MyCardList.ElementAt(Position) != MyCardList.FirstOrDefault())
            //            LeftMoreCardsImg.IsVisible = true;
            //        else
            //            LeftMoreCardsImg.IsVisible = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        string log = ex.ToString();
            //    }
            //}
        }

        private void FundCard_Tapped(object sender, EventArgs e)
        {
            if (GlobalStaticFields.Customer.MyCards?.Count == 0)
            {
                MessageDialog.Show("No Card", "No card found to perform this operation", DialogType.Info, "OK", null);
                return;
            }

            App.Current.MainPage.Navigation.PushAsync(new FundCardPage());
        }

        private void FreezeCard_Tapped(object sender, EventArgs e)
        {



            if(GlobalStaticFields.Customer.MyCards?.Count == 0)
            {
                MessageDialog.Show("No Card", "No card found to perform this operation", DialogType.Info, "OK", null);
                return;
            }

            App.Current.MainPage.Navigation.PushAsync(new FreezeCardPage(GlobalStaticFields.Customer.MyCards[Position]));
        }

        private void ManageCard_Tapped(object sender, EventArgs e)
        {
            if (GlobalStaticFields.Customer.MyCards?.Count == 0)
            {
                MessageDialog.Show("No Card", "No card found to perform this operation", DialogType.Info, "OK", null);
                return;
            }

            App.Current.MainPage.Navigation.PushAsync(new ManageCardPage(GlobalStaticFields.Customer.MyCards[Position]));
        }

        private void RequestBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new RequestCardPage());
        }
    }
}