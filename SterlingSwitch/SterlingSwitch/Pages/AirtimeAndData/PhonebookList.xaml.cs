using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Interface;
using SterlingSwitch.Models;
using SterlingSwitch.Pages.AirtimeAndData.ViewModel;
using SterlingSwitch.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Pages.AirtimeAndData
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhonebookList : SwitchMasterPage
    {
        

	    private PhonebookViewModel _vm;
        
        public List<PhoneContact> TempContact { get; set; }
        public event EventHandler<string> SelectedPhone;
        public PhonebookList()
        {
            InitializeComponent();
            this.BindingContext = _vm = new PhonebookViewModel(Navigation);
        }

	    protected override  void OnAppearing()
	    {
	        base.OnAppearing();
	        
           
            //await GetContacts();
	    }

	    

	    private void Cross_OnTapped(object sender, EventArgs e)
	    {
	        Navigation.PopAsync(true);
	    }

	    

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            
            var selectedContact = e.Item as listModel;
            ((ListView)sender).SelectedItem = null;
            SelectedPhone?.Invoke(this, selectedContact?.PhoneNumber);
            Navigation.PopAsync();
        }

	    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
	    {
	       _vm.SearchContacts(searchEntry.Text);
	    }
	}

    public class listModel
    {
        public string DisplayText { get; set; }
        public string PhoneNumber { get; set; }
    }
}
