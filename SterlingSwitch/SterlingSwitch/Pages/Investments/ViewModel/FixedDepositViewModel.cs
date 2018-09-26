using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.Investments.ViewModel
{
    public class FixedDepositViewModel : BaseViewModel
    {
        private ObservableRangeCollection<WalkThroughContent> walkThroughList;
        private decimal _amountToInvest;
        private decimal _displayedValue = decimal.Zero;
        private bool _isToggled;
        

        public ICommand MinusCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand PerformFixedDepositCommand { get; set; }
        public decimal AmountToInvest
        {
            get => _amountToInvest;
            set => SetProperty(ref _amountToInvest, value);
        }

        public bool IsToggled
        {
            get => _isToggled;
            set => SetProperty(ref _isToggled, value);
        }

        public ObservableRangeCollection<WalkThroughContent> WalkThroughList
        {
            get => walkThroughList;
            set => SetProperty(ref walkThroughList, value);
        }
        public FixedDepositViewModel(INavigation navigation) : base(navigation)
        {
            SetWalkThroughContent();
            AmountToInvest = 100000;
            MinusCommand = new Command<string>(ManipulateInvestmentAmout);
            PlusCommand = new Command<string>(ManipulateInvestmentAmout);
            PerformFixedDepositCommand = new Command(FixDeposit);
        }

        private void ManipulateInvestmentAmout(string amount)
        {

            if (string.IsNullOrEmpty(amount))
            {
                return;
            }
            var convAmount = decimal.Parse(amount);
            if (convAmount < 0 && _displayedValue == decimal.Zero )
            {
                return;
            }
            _displayedValue = _displayedValue + convAmount;
            AmountToInvest += convAmount;
        } 
        void SetWalkThroughContent()
        {
            WalkThroughList = new ObservableRangeCollection<WalkThroughContent>();
            WalkThroughList.AddRange(new List<WalkThroughContent>{
                new WalkThroughContent{ Title = "Fixed Deposits", Description = $"Save towards a goal \n or make returns on your extra cash.", ImageUrl = "investmentAdd1.png" },
                new WalkThroughContent{ Title = "Fixed Deposits", Description = $"Re-invest your initial deposit or the deposit \n and interest for another round of profit", ImageUrl = "" },
                new WalkThroughContent{ Title = "Fixed Deposits", Description = $"Save towards a goal \n or make returns on your extra cash.", ImageUrl = "" }
            });
        }

        private void FixDeposit()
        {
            // perform operations here 
            // display popup and proceed if response is proceed, else dismiss dialog
            
        }

    }

    public class WalkThroughContent
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

