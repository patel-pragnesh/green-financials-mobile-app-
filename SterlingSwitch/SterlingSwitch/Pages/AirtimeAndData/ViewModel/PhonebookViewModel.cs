using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingSwitch.Interface;
using SterlingSwitch.Models;
using SterlingSwitch.ViewModelBase;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.AirtimeAndData.ViewModel
{
    public class PhonebookViewModel : BaseViewModel
    {
        private List<listModel> phoneBookList;
        private List<listModel> tempContact;
        private List<PhoneContact> contacts;
        private bool _isIndicatorVisible = true;
        private bool _isPickerVisible = false;
        private bool _isPickerEnable = true;

        public bool IsIndicatorVisible
        {
            get => _isIndicatorVisible;
            set => SetProperty(ref _isIndicatorVisible, value);
        }

        public bool IsPickerVisible
        {
            get => _isPickerVisible;
            set => SetProperty(ref _isPickerVisible, value);
        }

        public bool IsPickerEnable
        {
            get => _isPickerEnable;
            set => SetProperty(ref _isPickerEnable, value);
        }



        public List<listModel> TempContacts
        {
            get => tempContact;
            set => SetProperty(ref tempContact, value);
        }

        public List<PhoneContact> Contacts
        {
            get => contacts;
            set => SetProperty(ref contacts, value);
        }
        public List<listModel> PhonebookList
        {
            get => phoneBookList;
            set => SetProperty(ref phoneBookList, value);
        }
        public PhonebookViewModel(INavigation navigation) : base(navigation)
        {
            Task.Run(async () => await GetContacts());
        
        }

        public void SearchContacts(string searchText)
        {
            PhonebookList = TempContacts.Where(t => t.DisplayText.ToLower().Contains(searchText.ToLower())).ToList();
        }



        private async Task GetContacts()
        {
            try
            {
                if (Contacts == null)
                {
                   Contacts = DependencyService.Get<IContact>().GetAllContacts().ToList();
                }

                TempContacts = new List<listModel>();

                if (Contacts.Any())
                {
                    PhonebookList = new List<listModel>();
                    Contacts.ForEach(y => y.PhoneNumber = y.PhoneNumber.Replace(" ", ""));
                    Contacts.ForEach(y => y.FirstName = y.FirstName.Replace(" ", ""));
                    Contacts.ForEach(y => y.LastName = y.LastName.Replace(" ", ""));



                    Contacts.OrderBy(j => j.Name).ToList().ForEach(u =>
                    {
                        {
                            if (PhonebookList.Count == 0)
                            {
                                PhonebookList.Add(new listModel() { DisplayText = $"{u.FirstName} {u.LastName}   {u.PhoneNumber}", PhoneNumber = u.PhoneNumber });
                            }
                            else
                            {
                                var result = PhonebookList.Find(g => g.PhoneNumber.Contains(u.PhoneNumber));
                                if (result == null)
                                {
                                    PhonebookList.Add(new listModel() { DisplayText = $"{u.Name}", PhoneNumber = u.PhoneNumber });
                                }
                            }

                        }
                    });


                    PhonebookList = PhonebookList.Distinct().ToList();
                    TempContacts.AddRange(PhonebookList);
                    IsIndicatorVisible = false;
                    IsPickerVisible = true;
                }
                else
                {
                    PhonebookList = new List<listModel>() { new listModel() { DisplayText = "No Contact Found" } };
                    IsIndicatorVisible = false;
                    IsPickerVisible = true;
                    IsPickerEnable = false;
                }
            }
            catch (Exception e)
            {
                PhonebookList = new List<listModel>() { new listModel() { DisplayText = e.Message } };
                IsIndicatorVisible = false;
                IsPickerVisible = true;
                IsPickerEnable = false;
            }

        }


    }
}
