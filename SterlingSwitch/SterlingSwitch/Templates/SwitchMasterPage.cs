using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FormsControls.Base;
using SterlingSwitch.Pages.Dashboard;
using xam = Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using SterlingSwitch.Pages.BankAccounts;
using SterlingSwitch.PopUps;
using System.Threading.Tasks;
using SterlingSwitch.Pages.Pagelanding;
using SterlingSwitch.Pages.Onboarding.Login;
using SterlingSwitch.Services;
using System.Runtime.CompilerServices;

namespace SterlingSwitch.Templates
{
    [xam.ContentProperty("PageContent")]
    public class SwitchMasterPage : xam.ContentPage,IAnimationPage
    {

       
        public SwitchMasterPage()
        {
            xam.NavigationPage.SetHasNavigationBar(this, false);

            GoBackCommand = new xam.Command(()=>GoBack());
            
            var controlTemplate = (xam.ControlTemplate)xam.Application.Current.Resources["masterPage"];
           
           
            this.Effects.Add(xam.Effect.Resolve("Sterling.SafeAreaPaddingEffect"));
         
            this.Content = PageContent;
           
        }

        private Action GoBack()
        {
           // if (Navigation.NavigationStack.Count == 0) return null;
            this.Navigation.PopAsync();
            return null;
        }



        #region Bindable Properties


        public static readonly xam.BindableProperty PageContentProperty = xam.BindableProperty.Create(nameof(PageContent), typeof(xam.ContentView), typeof(SwitchMasterPage),
           defaultValue: new xam.ContentView(),
           defaultBindingMode: xam.BindingMode.TwoWay,propertyChanged:OnPageContentPropertyChanged);

        private static void OnPageContentPropertyChanged(xam.BindableObject bindable, object oldValue, object newValue)
        {
            if(newValue != null){
                var page = (SwitchMasterPage)bindable;
                var  contentView =(xam.ContentView)newValue;
                page.Content = contentView; //  page.PageContent;
               
            }
        }

        public xam.ContentView PageContent
        {
            get { return (xam.ContentView)GetValue(PageContentProperty); }
            set { SetValue(PageContentProperty, value); }
        }


        public static readonly xam.BindableProperty HeaderMarginProperty = xam.BindableProperty.Create(nameof(HeaderMargin), typeof(xam.Thickness), typeof(SwitchMasterPage), defaultValue: new xam.Thickness(0, 0, 0, 0), defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Thickness HeaderMargin
        {
            get { return (xam.Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }



        public static readonly xam.BindableProperty PageTitleProperty = xam.BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(SwitchMasterPage),
           defaultValue: "",
           defaultBindingMode: xam.BindingMode.TwoWay,
           propertyChanged: OnHeaderTextChanged);

        private static void OnHeaderTextChanged(xam.BindableObject bindable, object oldValue, object newValue)
        {

        }

        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        public static readonly xam.BindableProperty SubPageTitleProperty = xam.BindableProperty.Create(nameof(SubPageTitle), typeof(string), typeof(SwitchMasterPage),
          defaultValue: "",
          defaultBindingMode: xam.BindingMode.TwoWay,propertyChanged:OnSubHeaderTextChanged);


        public string SubPageTitle
        {
            get { return (string)GetValue(SubPageTitleProperty); }
            set { SetValue(SubPageTitleProperty, value); }
        }
        private static void OnSubHeaderTextChanged(xam.BindableObject bindable, object oldValue, object newValue)
        {
           
        }
        public static readonly xam.BindableProperty IsSubPageTitleVisibleProperty = xam.BindableProperty.Create(nameof(IsSubPageTitleVisible), typeof(bool), typeof(SwitchMasterPage),
          defaultValue: false,
          defaultBindingMode: xam.BindingMode.TwoWay);
        

        public bool IsSubPageTitleVisible
        {
            get { return (bool)GetValue(IsSubPageTitleVisibleProperty); }
            set { SetValue(IsSubPageTitleVisibleProperty, value); }
        }


        public static readonly xam.BindableProperty BackCommandProperty = xam.BindableProperty.Create(nameof(BackCommand), typeof(ICommand), typeof(SwitchMasterPage), defaultValue: default(ICommand), defaultBindingMode: xam.BindingMode.TwoWay);

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }


        public static readonly xam.BindableProperty BackImageSourceProperty = xam.BindableProperty.Create(nameof(BackImageSource), typeof(xam.ImageSource), typeof(SwitchMasterPage),
            defaultValue: xam.ImageSource.FromFile("BlackBackArrow.png"),
            defaultBindingMode: xam.BindingMode.TwoWay,
            propertyChanged: OnImageSourceChanged);

        private static void OnImageSourceChanged(xam.BindableObject bindable, object oldValue, object newValue)
        {


        }

        public xam.ImageSource BackImageSource
        {
            get { return (xam.ImageSource)GetValue(BackImageSourceProperty); }
            set { SetValue(BackImageSourceProperty, value); }
        }


        public static readonly xam.BindableProperty PageTitleColorProperty = xam.BindableProperty.Create(nameof(PageTitleColor), typeof(xam.Color), typeof(SwitchMasterPage), defaultValue: xam.Color.White, defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Color PageTitleColor
        {
            get { return (xam.Color)GetValue(PageTitleColorProperty); }
            set { SetValue(PageTitleColorProperty, value); }
        }


        public static readonly xam.BindableProperty HeaderPaddingProperty = xam.BindableProperty.Create(nameof(HeaderPadding), typeof(xam.Thickness), typeof(SwitchMasterPage), defaultValue: new xam.Thickness(10, 10, 10, 0), defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Thickness HeaderPadding
        {
            get { return (xam.Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }


        public static readonly xam.BindableProperty IsBottomNavBarVisibleProperty = xam.BindableProperty.Create(nameof(IsBottomNavBarVisible), typeof(bool), typeof(SwitchMasterPage), defaultValue: true, defaultBindingMode: xam.BindingMode.TwoWay);

        public bool IsBottomNavBarVisible
        {
            get { return (bool)GetValue(IsBottomNavBarVisibleProperty); }
            set { SetValue(IsBottomNavBarVisibleProperty, value); }
        }


        public static readonly xam.BindableProperty IsBackImageVisibleProperty = xam.BindableProperty.Create(nameof(IsBackImageVisible), typeof(bool), typeof(SwitchMasterPage), defaultValue: true, defaultBindingMode: xam.BindingMode.TwoWay);

        public bool IsBackImageVisible
        {
            get { return (bool)GetValue(IsBackImageVisibleProperty); }
            set { SetValue(IsBackImageVisibleProperty, value); }
        }

        public static readonly xam.BindableProperty IsNavBarVisibleProperty = xam.BindableProperty.Create(nameof(IsNavBarVisible), typeof(bool), typeof(SwitchMasterPage), defaultValue: true, defaultBindingMode: xam.BindingMode.TwoWay);

        public bool IsNavBarVisible
        {
            get { return (bool)GetValue(IsNavBarVisibleProperty); }
            set { SetValue(IsNavBarVisibleProperty, value); }
        }
        public static readonly xam.BindableProperty ContentPaddingProperty = xam.BindableProperty.Create(nameof(ContentPadding), typeof(xam.Thickness), typeof(SwitchMasterPage), defaultValue: new xam.Thickness(20, 0, 20, 0), defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Thickness ContentPadding
        {
            get { return (xam.Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }


        public static readonly xam.BindableProperty CurrentPageProperty = xam.BindableProperty.Create(nameof(CurrentPage), typeof(BottomNavBar), typeof(SwitchMasterPage), 
            defaultValue: BottomNavBar.None,
            defaultBindingMode: xam.BindingMode.TwoWay, propertyChanged: OnCurrentPageChanged);

        public BottomNavBar CurrentPage
        {
            get { return (BottomNavBar)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        private static void OnCurrentPageChanged(xam.BindableObject bindable, object oldValue, object newValue)
        {
            var obj = (SwitchMasterPage)bindable;
            SetActiveBottomNavBar(obj);

        }

        public static readonly xam.BindableProperty TopNavBarBackgroundColorProperty = xam.BindableProperty.Create(nameof(TopNavBarBackgroundColor), typeof(xam.Color), typeof(SwitchMasterPage),
           defaultValue: xam.Color.FromHex("#EDEDED"),
           defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Color TopNavBarBackgroundColor
        {
            get { return (xam.Color)GetValue(TopNavBarBackgroundColorProperty); }
            set { SetValue(TopNavBarBackgroundColorProperty, value); }
        }

        public static readonly xam.BindableProperty IsTopNavBarSeparatorVisibleProperty = xam.BindableProperty.Create(nameof(IsTopNavBarSeparatorVisible), typeof(bool), typeof(SwitchMasterPage),
          defaultValue: true,
          defaultBindingMode: xam.BindingMode.TwoWay);

        public bool IsTopNavBarSeparatorVisible
        {
            get { return (bool)GetValue(IsTopNavBarSeparatorVisibleProperty); }
            set { SetValue(IsTopNavBarSeparatorVisibleProperty, value); }
        }


        public static readonly xam.BindableProperty ContentBackgroundColorProperty = xam.BindableProperty.Create(nameof(ContentBackgroundColor), typeof(xam.Color), typeof(SwitchMasterPage), defaultValue:xam.Color.White, 
            defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Color ContentBackgroundColor
        {
            get { return (xam.Color)GetValue(ContentBackgroundColorProperty); }
            set { SetValue(ContentBackgroundColorProperty, value); }
        }

        public static readonly xam.BindableProperty PageContentBackgroundColorProperty = xam.BindableProperty.Create(nameof(PageContentBackgroundColor), typeof(xam.Color), typeof(SwitchMasterPage), defaultValue: xam.Color.White,
           defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Color PageContentBackgroundColor
        {
            get { return (xam.Color)GetValue(PageContentBackgroundColorProperty); }
            set { SetValue(PageContentBackgroundColorProperty, value); }
        }


        public static readonly xam.BindableProperty SubPageTitleColorProperty = xam.BindableProperty.Create(nameof(SubPageTitleColor), typeof(xam.Color), typeof(SwitchMasterPage), defaultValue: xam.Color.White,
           defaultBindingMode: xam.BindingMode.TwoWay);

        public xam.Color SubPageTitleColor
        {
            get { return (xam.Color)GetValue(SubPageTitleColorProperty); }
            set { SetValue(SubPageTitleColorProperty, value); }
        }
        #endregion

        #region Navigation Commands

        public ICommand GoBackCommand { get; set; }

        public static readonly xam.BindableProperty MyBankCommandProperty = xam.BindableProperty.Create(nameof(MyBankCommand), typeof(ICommand), typeof(SwitchMasterPage), defaultValue: default(ICommand), defaultBindingMode: xam.BindingMode.TwoWay);

        public ICommand MyBankCommand
        {
            get { return (ICommand)GetValue(MyBankCommandProperty); }
            set { SetValue(MyBankCommandProperty, value); }
        }

        public static readonly xam.BindableProperty HomeCommandProperty = xam.BindableProperty.Create(nameof(HomeCommand), typeof(ICommand), typeof(SwitchMasterPage), defaultValue: default(ICommand), defaultBindingMode: xam.BindingMode.TwoWay);

        public ICommand HomeCommand
        {
            get { return (ICommand)GetValue(HomeCommandProperty); }
            set { SetValue(HomeCommandProperty, value); }
        }

      
       
        #endregion

        #region Methods
        static void SetActiveBottomNavBar(SwitchMasterPage page)
        {
            ResetBottomNavBar();
            switch (page.CurrentPage)
            {
                case BottomNavBar.Home:
                    xam.Application.Current.Resources["homeNavBar"] = xam.Application.Current.Resources["EnabledShortcutStyle"];
                    xam.Application.Current.Resources["homeImage"] = xam.Application.Current.Resources["selectedHome"];

                    //
                    break;
                case BottomNavBar.Actions:
                    xam.Application.Current.Resources["actionsNavBar"] = xam.Application.Current.Resources["EnabledShortcutStyle"];
                    xam.Application.Current.Resources["actionImage"] = xam.Application.Current.Resources["selectedAction"];
                    
                    break;
                case BottomNavBar.MyBank:
                    xam.Application.Current.Resources["myBankNavBar"] = xam.Application.Current.Resources["EnabledShortcutStyle"];
                    xam.Application.Current.Resources["actionMyBank"] = xam.Application.Current.Resources["selectedMyBank"];
                    
                    break;
                case BottomNavBar.Payments:
                    xam.Application.Current.Resources["paymentsNavBar"] = xam.Application.Current.Resources["EnabledShortcutStyle"];
                    xam.Application.Current.Resources["paymentsImage"] = xam.Application.Current.Resources["selectedPayments"];

                    
                    break;
                case BottomNavBar.More:
                    xam.Application.Current.Resources["moreNavBar"] = xam.Application.Current.Resources["EnabledShortcutStyle"];

                    break;
                default:
                    break;
            }
        }
       static void ResetBottomNavBar()
        {
            xam.Application.Current.Resources["moreNavBar"] = xam.Application.Current.Resources["DisabledShortcutStyle"];
            xam.Application.Current.Resources["paymentsNavBar"] = xam.Application.Current.Resources["DisabledShortcutStyle"];
            xam.Application.Current.Resources["myBankNavBar"] = xam.Application.Current.Resources["DisabledShortcutStyle"];
            xam.Application.Current.Resources["actionsNavBar"] = xam.Application.Current.Resources["DisabledShortcutStyle"];
            xam.Application.Current.Resources["homeNavBar"] = xam.Application.Current.Resources["DisabledShortcutStyle"];
            //Images
            xam.Application.Current.Resources["homeImage"] = xam.Application.Current.Resources["unselectedHome"];
            xam.Application.Current.Resources["actionImage"] = xam.Application.Current.Resources["unselectedAction"];
            xam.Application.Current.Resources["paymentsImage"] = xam.Application.Current.Resources["unselectedPayments"];
            xam.Application.Current.Resources["actionMyBank"] = xam.Application.Current.Resources["unselectedMyBank"];

        }

        void ReturnBack()
        {
            Navigation.PopAsync();
        }
        #endregion
        protected override bool OnBackButtonPressed()
        {

            bool result = true;
            var currentpage = (Xamarin.Forms.NavigationPage)xam.Application.Current.MainPage;
            if (currentpage.CurrentPage is Dashboard || currentpage.CurrentPage is BankAccountsView || currentpage.CurrentPage is Pages.QuickActions.QuickActions || currentpage.CurrentPage is Pages.Pagelanding.PaymentsLanding || currentpage.CurrentPage is Pages.More.MorePage)
            {

                MessageDialog.Show("Quit Switch", "Are you Sure you want to quit Switch?", DialogType.Question, "Yes", ()=> { xam.Device.BeginInvokeOnMainThread(Close); }, "Cancel", null);


                return result;
            }
            if (currentpage.CurrentPage is CategoryTagLanding)
            {
                return false;
            }

            return base.OnBackButtonPressed();

        }

        void Close()
        {
            GlobalStaticFields.Customer.IsLoggedOn = false;
            xam.Application.Current.MainPage = new AnimationNavigationPage(new UnProfiledLoginPage());

        }

        public void OnAnimationStarted(bool isPopAnimation)
        {
            
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            
        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var current = this;
            //var safearea = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            //var currentPadding = this.Padding;
            //if(safearea.Top > 20)
            //{
            //    PageContent.Margin = new xam.Thickness(0, 54, 0, 0);
            //  //  HeaderPadding = new xam.Thickness(currentPadding.Left, safearea.Top, currentPadding.Right, currentPadding.Bottom);
            //}
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
           

            if(propertyName == PageContentProperty.PropertyName)
            {
                
               PageContent.Content.BackgroundColor = this.ContentBackgroundColor;
               PageContent.BackgroundColor = this.TopNavBarBackgroundColor;
            }
        }

        public IPageAnimation PageAnimation => new SlidePageAnimation { Duration = AnimationDuration.Long, Subtype = AnimationSubtype.FromLeft, BounceEffect = false };
    }

    public enum BottomNavBar
    {
        Home,
        Actions,
        MyBank,
        Payments,
        More,
        None
    }
}
