using System;
using System.Collections.Generic;
using System.Text;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Investments.ViewModel
{
    public class LandingPageViewModel:BaseViewModel
    {
        private ObservableRangeCollection<WalkThroughContent> walkThroughList;
        public ObservableRangeCollection<WalkThroughContent> WalkThroughList
        {
            get => walkThroughList;
            set => SetProperty(ref walkThroughList, value);
        }
        public LandingPageViewModel(INavigation navigation) : base(navigation)
        {
            SetWalkThroughContent();
        }

        void SetWalkThroughContent()
        {
            WalkThroughList = new ObservableRangeCollection<WalkThroughContent>();
            WalkThroughList.AddRange(new List<WalkThroughContent>{
                new WalkThroughContent{ Title = "Fixed Deposits", Description = $"Save towards a goal \n or make returns on your extra cash.", ImageUrl = "investmentAdd1.png" },
                new WalkThroughContent{ Title = "Treasury Bills", Description = $"Re-invest your initial deposit or the deposit \n and interest for another round of profit", ImageUrl = "" },
                new WalkThroughContent{ Title = "Fixed Deposits", Description = $"Save towards a goal \n or make returns on your extra cash.", ImageUrl = "" }
            });
        }
    }
}
