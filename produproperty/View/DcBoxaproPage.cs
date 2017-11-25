using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace produproperty.View
{
    public sealed class DcBoxaproPage : RichEditBox
    {
        public DcBoxaproPage()
        {
            this.DefaultStyleKey = typeof(DcBoxaproPage);
            SelectionChanged += DcBoxaproPage_SelectionChanged;
            
            TextChanged += DcBoxaproPage_TextChanged;

            Key = new KeyBehavior(this);
            Key.Add(new KeyAction("ctrl+S", (e) =>
            {
                Storage();
            }));
        }



        private void Storage()
        {
            Storaged?.Invoke(this, null);
        }

        public event EventHandler Storaged;


        private void DcBoxaproPage_TextChanged(object sender, RoutedEventArgs e)
        {
            string str = "";
            Document.GetText(TextGetOptions.None, out str);
            _text = true;
            Text = str;
        }

        private bool _text;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public KeyBehavior Key
        {
            get; set;
        }

        private bool _position;
        private bool _positiontext;
        private bool _positionlength;


        private void DcBoxaproPage_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Debug.Write("进");
            _position = true;
            _positiontext = true;
            _position = true;
            _positionlength = true;
            SelectionIndex = Document.Selection.StartPosition;
            SelectionStr = Document.Selection.Text;
            SelectionLength = Document.Selection.Length;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(DcBoxaproPage), new PropertyMetadata(default(string), (d, e) =>
            {
                var dc = d as DcBoxaproPage;
                if (dc != null && dc._text)
                {
                    dc._text = false;
                    Debug.Write("tf\r\n");
                    return;
                }

                if (e.NewValue.Equals(e.OldValue))
                {
                    return;
                }

                Debug.Write("t\r\n");
                dc._text = true;
                dc?.Document.SetText(TextSetOptions.None, e.NewValue.ToString());
            }));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty SelectionIndexProperty = DependencyProperty.Register(
            "SelectionIndex", typeof(int), typeof(DcBoxaproPage), new PropertyMetadata(default(int), (d, e) =>
            {
                
                var dc = d as DcBoxaproPage;
                if (dc == null)
                {
                    return;
                }
                else if (dc._position)
                {
                    dc._position = false;
                    Debug.Write("s出\n");
                    return;
                }

                Debug.Write("s\n");

                dc.Document.Selection.StartPosition = (int)e.NewValue;
            }));

        public static readonly DependencyProperty SelectionStrProperty = DependencyProperty.Register(
            "SelectionStr", typeof(string), typeof(DcBoxaproPage), new PropertyMetadata(default(string), (d, e) =>
            {
                var dc = d as DcBoxaproPage;
                if (dc == null)
                {
                    return;
                }
                else if (dc._positiontext)
                {
                    dc._positiontext = false;
                    Debug.Write("s出\n");
                    return;
                }

                Debug.Write("s\n");
                dc.Document.Selection.Text = (string)e.NewValue;
            }));

        public static readonly DependencyProperty SelectionLengthProperty = DependencyProperty.Register(
            "SelectionLength", typeof(int), typeof(DcBoxaproPage), new PropertyMetadata(default(int), (d, e) =>
            {
                var dc = d as DcBoxaproPage;
                if (dc == null)
                {
                    return;
                }
                else if (dc._positionlength)
                {
                    dc._positionlength = false;
                    Debug.Write("g出\r");
                    return;
                }

                Debug.Write("g\r");
                var s = dc.Document.Selection.StartPosition;
                dc.Document.Selection.SetRange(s, s + (int)e.NewValue);
            }));

        public int SelectionLength
        {
            get
            {
                return (int)GetValue(SelectionLengthProperty);
            }
            set
            {
                SetValue(SelectionLengthProperty, value);
            }
        }

        public string SelectionStr
        {
            get
            {
                return (string)GetValue(SelectionStrProperty);
            }
            set
            {
                SetValue(SelectionStrProperty, value);
            }
        }

        public int SelectionIndex
        {
            get
            {
                return (int)GetValue(SelectionIndexProperty);
            }
            set
            {
                SetValue(SelectionIndexProperty, value);
            }
        }
    }



}

