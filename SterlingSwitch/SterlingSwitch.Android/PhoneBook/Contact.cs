using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SterlingSwitch.Droid.PhoneBook;
using switch_mobile.Services;
using SterlingSwitch.Interface;
using SterlingSwitch.Models;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

[assembly: Dependency(typeof(Contact))]

namespace SterlingSwitch.Droid.PhoneBook
{
    public class Contact : IContact
    {
        List<PhoneContact> phoneContacts = new List<PhoneContact>();
        public IEnumerable<PhoneContact> GetAllContacts()
        {
            // var phoneContacts = new List<PhoneContact>();
            var currentActivity = CrossCurrentActivity.Current.Activity;

            //Check for permission
            if (ContextCompat.CheckSelfPermission(currentActivity, Manifest.Permission.ReadContacts) == (int)Permission.Granted)
            {
                phoneContacts = GetContactList();
            }
            else
            {
                var requiredPermission = new string[] { Manifest.Permission.ReadContacts };
                ActivityCompat.RequestPermissions(currentActivity, requiredPermission, 0);
               
                // Snackbar.Make(currentActivity,"Permission is required to access your contact",Snackbar.LengthIndefinite).SetAction()
            }
            return phoneContacts;

        }

        List<PhoneContact> GetContactList()
        {
            using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null))
            {
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                            string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));

                            string[] words = name.Split(' ');
                            var contact = new PhoneContact();
                            contact.FirstName = words[0];
                            if (words.Length > 1)
                                contact.LastName = words[1];
                            else
                                contact.LastName = ""; //no last name
                            contact.PhoneNumber = phoneNumber;
                            phoneContacts.Add(contact);
                            phoneContacts.ForEach(h => h.PhoneNumber = h.PhoneNumber.Replace(" ", ""));
                            phoneContacts = phoneContacts.Distinct().ToList();
                        }
                        catch (Exception ex)
                        {

                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    phones.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
            }
            return phoneContacts;
        }
    }
}