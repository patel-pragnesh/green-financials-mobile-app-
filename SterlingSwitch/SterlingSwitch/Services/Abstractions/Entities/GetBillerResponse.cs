namespace SterlingSwitch.Services.Abstractions.Entities
{

    public class GetBillerResponse
    {
        public string message { get; set; }
        public string response { get; set; }
        public object responsedata { get; set; }
        public GetBillerData data { get; set; }
    }

    public class GetBillerData
    {
        public string billers { get; set; }
        public string status { get; set; }
    }


    public class GetBillersXMl
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Response
        {

            private uint responseCodeField;

            private ResponseBillerList billerListField;

            /// <remarks/>
            public uint ResponseCode
            {
                get
                {
                    return this.responseCodeField;
                }
                set
                {
                    this.responseCodeField = value;
                }
            }

            /// <remarks/>
            public ResponseBillerList BillerList
            {
                get
                {
                    return this.billerListField;
                }
                set
                {
                    this.billerListField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ResponseBillerList
        {

            private ResponseBillerListCategory[] categoryField;

            private ushort countField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Category")]
            public ResponseBillerListCategory[] Category
            {
                get
                {
                    return this.categoryField;
                }
                set
                {
                    this.categoryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort count
            {
                get
                {
                    return this.countField;
                }
                set
                {
                    this.countField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ResponseBillerListCategory
        {

            private ResponseBillerListCategoryBiller[] billerField;

            private byte idField;

            private string nameField;

            private string descriptionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Biller")]
            public ResponseBillerListCategoryBiller[] Biller
            {
                get
                {
                    return this.billerField;
                }
                set
                {
                    this.billerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ResponseBillerListCategoryBiller
        {

            private ushort idField;

            private ushort pAYDirectProductIdField;

            private string nameField;

            private string shortNameField;

            private string narrationField;

            private string customerField1Field;

            private string customerField2Field;

            private string logoUrlField;

            private object urlField;

            private ushort surchargeField;

            private object customSectionUrlField;

            private ushort currencyCodeField;

            private string currencySymbolField;

            private string quickTellerSiteUrlNameField;

            private string supportEmailField;

            private string customMessageField;

            private string riskCategoryIdField;

            private string typeField;

            private ulong networkIdField;

            private bool networkIdFieldSpecified;

            private string productCodeField;

            private byte customerIdLengthField;

            private bool customerIdLengthFieldSpecified;

            /// <remarks/>
            public ushort Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public ushort PAYDirectProductId
            {
                get
                {
                    return this.pAYDirectProductIdField;
                }
                set
                {
                    this.pAYDirectProductIdField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string ShortName
            {
                get
                {
                    return this.shortNameField;
                }
                set
                {
                    this.shortNameField = value;
                }
            }

            /// <remarks/>
            public string Narration
            {
                get
                {
                    return this.narrationField;
                }
                set
                {
                    this.narrationField = value;
                }
            }

            /// <remarks/>
            public string CustomerField1
            {
                get
                {
                    return this.customerField1Field;
                }
                set
                {
                    this.customerField1Field = value;
                }
            }

            /// <remarks/>
            public string CustomerField2
            {
                get
                {
                    return this.customerField2Field;
                }
                set
                {
                    this.customerField2Field = value;
                }
            }

            /// <remarks/>
            public string LogoUrl
            {
                get
                {
                    return this.logoUrlField;
                }
                set
                {
                    this.logoUrlField = value;
                }
            }

            /// <remarks/>
            public object Url
            {
                get
                {
                    return this.urlField;
                }
                set
                {
                    this.urlField = value;
                }
            }

            /// <remarks/>
            public ushort Surcharge
            {
                get
                {
                    return this.surchargeField;
                }
                set
                {
                    this.surchargeField = value;
                }
            }

            /// <remarks/>
            public object CustomSectionUrl
            {
                get
                {
                    return this.customSectionUrlField;
                }
                set
                {
                    this.customSectionUrlField = value;
                }
            }

            /// <remarks/>
            public ushort CurrencyCode
            {
                get
                {
                    return this.currencyCodeField;
                }
                set
                {
                    this.currencyCodeField = value;
                }
            }

            /// <remarks/>
            public string CurrencySymbol
            {
                get
                {
                    return this.currencySymbolField;
                }
                set
                {
                    this.currencySymbolField = value;
                }
            }

            /// <remarks/>
            public string QuickTellerSiteUrlName
            {
                get
                {
                    return this.quickTellerSiteUrlNameField;
                }
                set
                {
                    this.quickTellerSiteUrlNameField = value;
                }
            }

            /// <remarks/>
            public string SupportEmail
            {
                get
                {
                    return this.supportEmailField;
                }
                set
                {
                    this.supportEmailField = value;
                }
            }

            /// <remarks/>
            public string CustomMessage
            {
                get
                {
                    return this.customMessageField;
                }
                set
                {
                    this.customMessageField = value;
                }
            }

            /// <remarks/>
            public string RiskCategoryId
            {
                get
                {
                    return this.riskCategoryIdField;
                }
                set
                {
                    this.riskCategoryIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ulong NetworkId
            {
                get
                {
                    return this.networkIdField;
                }
                set
                {
                    this.networkIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool NetworkIdSpecified
            {
                get
                {
                    return this.networkIdFieldSpecified;
                }
                set
                {
                    this.networkIdFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ProductCode
            {
                get
                {
                    return this.productCodeField;
                }
                set
                {
                    this.productCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte CustomerIdLength
            {
                get
                {
                    return this.customerIdLengthField;
                }
                set
                {
                    this.customerIdLengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool CustomerIdLengthSpecified
            {
                get
                {
                    return this.customerIdLengthFieldSpecified;
                }
                set
                {
                    this.customerIdLengthFieldSpecified = value;
                }
            }
        }
    }

}
