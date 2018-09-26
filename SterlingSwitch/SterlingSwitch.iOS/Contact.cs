using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contacts;
using Foundation;
using SterlingSwitch.Interface;
using SterlingSwitch.Models;
using switch_mobile.iOS;
using switch_mobile.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Contact))]
namespace switch_mobile.iOS 
{
    public class Contact : IContact
    {
        public IEnumerable<PhoneContact> GetAllContacts()
        {
            var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers };
            NSError error;
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers
            var contactList = new List<CNContact>();
            //test
            using (var store = new CNContactStore())
            {
                var request = new CNContactFetchRequest(keysTOFetch);
                store.EnumerateContacts(request, out error, new CNContactStoreListContactsHandler((CNContact contact, ref bool stop) => contactList.Add(contact)));

            }
            var contacts = new List<PhoneContact>();

            foreach (var item in contactList)
            {
               
                var numbers = item.PhoneNumbers;
                if (numbers != null)
                {
                    if (item.GivenName == "SPAM"){

                    foreach (var item2 in numbers)
                        {
                            var givenName = item2.Label.Split('-');
                            contacts.Add(new PhoneContact
                            {
                                FirstName = givenName.Length > 1 ? givenName[1] : "",
                                LastName = item.FamilyName,
                                PhoneNumber = item2.Value.StringValue

                            });
                        }
                    }else{


                    foreach (var item2 in numbers)
                    {
                       
                        contacts.Add(new PhoneContact
                        {
                            FirstName = item.GivenName ,
                            LastName = item.FamilyName,
                            PhoneNumber = item2.Value.StringValue

                        });
                    }

                    }

                }
            }
            return contacts;
        }
    }
}