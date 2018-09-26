using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SterlingSwitch.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SterlingSwitch.Custom.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendedEntry : ContentView
    {
        public event EventHandler IconTapped;
        public ExtendedEntry()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            HeaderTxt.Text = Title;
            EmbeddedEntry.Placeholder = Placeholder;
            EmbeddedEntry.Text = Text;
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler Completed;

        public static BindableProperty TextProperty = BindableProperty.Create(
                                                            propertyName: nameof(Text),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedEntry),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TextPropertyChanged);

        public static BindableProperty UnformattedTextProperty = BindableProperty.Create(
                                                            propertyName: nameof(Text),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedEntry),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TextPropertyChanged);

        public static BindableProperty PlaceholderProperty = BindableProperty.Create(
                                                            propertyName: nameof(Placeholder),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedEntry),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: PlaceholderPropertyChanged);

        public static BindableProperty ReturnCommandProperty = BindableProperty.Create(
                                                            propertyName: nameof(ReturnCommand),
                                                            returnType: typeof(ICommand),
                                                            declaringType: typeof(ExtendedEntry),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: ReturnCommandPropertyChanged);

        public static BindableProperty TitleProperty = BindableProperty.Create(
                                                            propertyName: nameof(Title),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedEntry),
                                                            defaultValue: string.Empty,
                                                            defaultBindingMode: BindingMode.TwoWay,
                                                            propertyChanged: TitlePropertyChanged);


        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedEntry)bindable;

            if (control != null)
            {
                control.Title = (string)newValue;
                control.HeaderTxt.Text = control.Title;
            }
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedEntry)bindable;
            
            if (control != null && newValue != null)
            {
                control.EmbeddedEntry.Text = (string)newValue;
                //control.EmbeddedEnt
            }
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedEntry)bindable;

            if (control != null)
            {
                control.Placeholder = (string)newValue;
                control.EmbeddedEntry.Placeholder = control.Placeholder;
            }
        }

        private static void ReturnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedEntry)bindable;

            if (control != null)
            {
                control.ReturnCommand = (ICommand)newValue;
                control.EmbeddedEntry.ReturnCommand = control.ReturnCommand;
            }
        }

        public bool IsAmount
        {
            get => FormatValue;
            set => SetKeyboard(value);
        }

        public TextAlignment HorizontalTextAlignment
        {
            get => EmbeddedEntry.HorizontalTextAlignment;
            set => EmbeddedEntry.HorizontalTextAlignment = value;
        }

        public bool FormatValue
        {
            get;
            set;
        }

        public string Title
        {

            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
            // get => HeaderTxt.Text;
            // set => HeaderTxt.Text = value;
        }

        public ImageSource Icon
        {
            get => icon.Source;
            set => icon.Source = value;
        }

        
        public bool EnableActivityIndicator
        {
            get => IndicatirGrid.IsVisible = false;
            set => IndicatirGrid.IsVisible = value;
        }

        public bool IsCurrencyVisible
        {
            get => currencySymbol.IsVisible;
            set => currencySymbol.IsVisible = value;
        }

        public string CurrencySymbol
        {
            set => currencySymbol.Text = string.IsNullOrEmpty(value) ? Utilities.GetCurrency("NGN") : Utilities.GetCurrency(value);
        }


        public string Text
        {
            get
            {
                string txt = EmbeddedEntry.Text;

                if (IsAmount && !string.IsNullOrWhiteSpace(txt))
                    txt = txt.Replace(",", "");

                //return txt;
                if (IsAmount)
                    return ((string)GetValue(TextProperty))?.Replace(",","");
                else
                    return (string)GetValue(TextProperty);
            }
            set
            {
                var tempVal = value;
                if(tempVal.Replace(",","") != Text)
                {
                    if (IsAmount && value.Length > 3)
                    {
                        string s = value.Replace(",", "");

                        if (double.TryParse(s, out double result) && s.Length > 3)
                        {
                            var val = String.Format("{0:0,0}", result); //result.ToString("0:0,0");
                                                                        // EmbeddedEntry.Text = String.Format("{0:0,0}", result); //result.ToString("0:0,0");
                            SetValue(TextProperty, val);
                        }
                        else
                        {
                            //  EmbeddedEntry.Text = s;
                            SetValue(TextProperty, s);
                        }
                    }
                    else
                        SetValue(TextProperty, value);
                    // EmbeddedEntry.Text = value;
                }

            }
        }
        private bool _isfocused = true;

        public bool IsFocused
        {
            get => _isfocused; 
            set
            {
                _isfocused = value;
                if(!_isfocused) { 
                    EmbeddedEntry.Unfocus();
                }
            }
        }



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }

            // get => EmbeddedEntry.Placeholder;
            // set => EmbeddedEntry.Placeholder = value;
        }

        public int MaxLength
        {
            get => EmbeddedEntry.MaxLength;
            set => EmbeddedEntry.MaxLength = value;
        }

        public Keyboard Keyboard
        {
            get => EmbeddedEntry.Keyboard;
            set => EmbeddedEntry.Keyboard = value;
        }

        public TextAlignment ExtendedHorizontalTextAlignment
        {
            get => EmbeddedEntry.HorizontalTextAlignment;
            set => EmbeddedEntry.HorizontalTextAlignment = value;
        }

        public bool isUnderLineVisible
        {
            get => BoxViewLine.IsVisible;
            set => BoxViewLine.IsVisible = value;
        }

        public bool IsPassword
        {
            get => EmbeddedEntry.IsPassword;
            set => EmbeddedEntry.IsPassword = value;
        }

        public bool IsSpellCheckEnabled
        {
            get => EmbeddedEntry.IsSpellCheckEnabled;
            set => EmbeddedEntry.IsSpellCheckEnabled = value;
        }

        public bool IsTextPredictionEnabled
        {
            get => EmbeddedEntry.IsTextPredictionEnabled;
            set => EmbeddedEntry.IsTextPredictionEnabled = value;
        }

        public ICommand ReturnCommand
        {
            get => EmbeddedEntry.ReturnCommand;
            set => EmbeddedEntry.ReturnCommand = value;
        }

        private void SetKeyboard(bool value)
        {
            if (value)
            {
                EmbeddedEntry.Keyboard = Keyboard.Numeric;
                FormatValue = true;
            }
            else
                FormatValue = false;
        }

        private void EmbeddedEntry_Completed(object sender, EventArgs e)
        {
            Completed?.Invoke(sender, e);
        }

        private void EmbeddedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var args = new TextChangedEventArgs(e.OldTextValue, e.NewTextValue);
            this.Text = e?.NewTextValue ?? "";
            TextChanged?.Invoke(sender, args);
        }

        private void Icon_Tapped(object sender, EventArgs e)
        {
            IconTapped?.Invoke(this, null);
        }
    }
}