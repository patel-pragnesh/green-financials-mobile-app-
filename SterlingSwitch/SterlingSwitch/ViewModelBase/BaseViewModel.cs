using Plugin.Connectivity;
using SterlingSwitch.Pages.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SterlingSwitch.ViewModelBase
{
    public class BaseViewModel: ObservableObject
    {
        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
            MyBankCommand = new Command(()=> {
                Navigation.PushAsync(new Dashboard());
            });
        }

        public INavigation Navigation { get; set; }

      //  private ICommand backCommand;
        public ICommand BackCommand
        {
            get;
            set;
        }

        public ICommand MyBankCommand { get; set; }
        string pagetitle = string.Empty;
        public string PageTitle
        {
            get => pagetitle;
            set => SetProperty(ref pagetitle, value);
        }

        Color pagetitlecolor = Color.Default;
        public Color PageTitleColor
        {
            get => pagetitlecolor;
            set => SetProperty(ref pagetitlecolor, value);
        }

        ImageSource backImageSource = default(ImageSource);
        public ImageSource BackImageSource
        {
            get => backImageSource;
            set => SetProperty(ref backImageSource, value);
        }
        bool isbottomNavBarVisible = true;
        public bool IsBottomNavBarVisible
        {
            get => isbottomNavBarVisible;
            set => SetProperty(ref isbottomNavBarVisible, value);
        }

        bool isbackImageVisible = true;
        public bool IsBackImageVisible
        {
            get => isbackImageVisible;
            set => SetProperty(ref isbackImageVisible, value);
        }

        bool isNavBarVisible = true;
        public bool IsNavBarVisible
        {
            get => isNavBarVisible;
            set => SetProperty(ref isNavBarVisible, value);
        }

        

        bool isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not busy.
        /// </summary>
        /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        bool canLoadMore = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can load more.
        /// </summary>
        /// <value><c>true</c> if this instance can load more; otherwise, <c>false</c>.</value>
        public bool CanLoadMore
        {
            get => canLoadMore;
            set => SetProperty(ref canLoadMore, value);
        }

        #region Methods
        public virtual void CheckConnectivity()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return;
            }
        }
        #endregion
    }

}
