using Newtonsoft.Json;
using SterlingSwitch.PopUps;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using SterlingSwitch.Services.RestServices;
using SterlingSwitch.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SterlingSwitch.Pages.More.FAQ
{
    public class FAQViewModel : BaseViewModel
    {
        public FAQViewModel(INavigation navigation):base(navigation)
        {
            apirequest = new ApiRequest();
            FAQs = new ObservableRangeCollection<FAQ>();
        }
       public ApiRequest apirequest { set; get; }
       public ObservableRangeCollection<FAQ> FAQs { get; set; }
        FAQ _oldFAQ;
      

        public void ShowHiddenFAQS(FAQ fAQItem)
        {
           if(_oldFAQ == fAQItem)
            {
                fAQItem.IsVisible = !fAQItem.IsVisible;
                UpdateProduct(_oldFAQ);
            }
            else
            {
                if (_oldFAQ != null)
                {
                    // hide previous selected item
                    _oldFAQ.IsVisible = false;
                    UpdateProduct(_oldFAQ);
                }
                // show selected item
                fAQItem.IsVisible = true;
                UpdateProduct(fAQItem);
            }

            _oldFAQ = fAQItem;
        }

        private void UpdateProduct(FAQ oldFAQ)
        {
            var index = FAQs.IndexOf(oldFAQ);
            FAQs.Remove(oldFAQ);
            FAQs.Insert(index, oldFAQ);
        }

        public async Task LoadFAQs()
        {


            var pd = await ProgressDialog.Show("Please wait.");
            try
            {
                var response = await apirequest.Get("", URLConstants.SwitchApiBaseUrl, "switch/GetFAQ", "FAQVIewModel");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var Faqlist = JsonConvert.DeserializeObject<List<FAQModel>>(result);

                    var faqs = new List<FAQ>();

                    foreach (var item in Faqlist)
                    {
                        var singleFAQ = new FAQ();
                        singleFAQ.Answer = item.Ans;
                        singleFAQ.Question = item.Qus;
                        singleFAQ.IsVisible = false;
                        faqs.Add(singleFAQ);
                    }

                    FAQs.ReplaceRange(faqs);

                }
            }
            catch (Exception ex)
            {

               
            }
            finally
            {
                await pd.Dismiss();
            }
        }


    }

    public class FAQ
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsVisible { get; set; }

    }
}
