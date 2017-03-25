using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace produproperty.View
{
    public class Foobar : TextBox
    {
        public Foobar()
        {
            DefaultStyleKey = typeof(Foobar);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }

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
                e.Execute = false; Storage();
                e.Execute = true;
            }));
        }

        private void Storage()
        {

        }

        private void DcBoxaproPage_TextChanged(object sender, RoutedEventArgs e)
        {
            string str = "";
            Document.GetText(TextGetOptions.None, out str);
            _text = true;
            Text = str;
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var controlKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            var ctrl = (controlKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var shiftKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);
            var shift = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var altKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu);
            var alt = (altKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

            if (!ctrl && !shift && !alt)
            {
                return;
            }

            var key = e.Key.ToString();

            if (key == "Control" || key == "Shift" || key == "Menu")
            {
                return;
            }

            StringBuilder str = new StringBuilder();
            if (ctrl)
            {
                str.Append("ctrl+");
            }
            if (shift)
            {
                str.Append("shift+");
            }
            if (alt)
            {
                str.Append("alt+");
            }
            str.Append(e.Key.ToString());

            base.OnKeyDown(e);
        }

        private bool _text;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public KeyBehavior Key { get; set; }


        private void DcBoxaproPage_SelectionChanged(object sender, RoutedEventArgs e)
        {
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
                    return;
                }
                if (e.NewValue.Equals(e.OldValue))
                {
                    return;
                }
                dc._text = true;
                dc?.Document.SetText(TextSetOptions.None, e.NewValue.ToString());
            }));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SelectionIndexProperty = DependencyProperty.Register(
            "SelectionIndex", typeof(int), typeof(DcBoxaproPage), new PropertyMetadata(default(int), (d, e) =>
            {
                var dc = d as DcBoxaproPage;
                if (dc == null)
                {
                    return;
                }

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
                var s = dc.Document.Selection.StartPosition;
                dc.Document.Selection.SetRange(s, s + (int)e.NewValue);
            }));

        public int SelectionLength
        {
            get { return (int)GetValue(SelectionLengthProperty); }
            set { SetValue(SelectionLengthProperty, value); }
        }

        public string SelectionStr
        {
            get { return (string)GetValue(SelectionStrProperty); }
            set { SetValue(SelectionStrProperty, value); }
        }

        public int SelectionIndex
        {
            get { return (int)GetValue(SelectionIndexProperty); }
            set { SetValue(SelectionIndexProperty, value); }
        }
    }


    public class KeyAction
    {
        public string Key { get; set; }

        public KeyAction(string key, Action<KeyAction> action)
        {
            Action = action;
            Key = key;
        }

        public Action<KeyAction> Action { get; set; }

        public void Run()
        {
            if (!Execute || !_action)
            {
                return;
            }
            _action = false;
            Action?.Invoke(this);
            _action = true;
        }

        public bool Execute { get; set; } = true;
        private bool _action = true;
    }



    public class KeyBehavior : IDisposable
    {
        public KeyBehavior(UIElement e)
        {
            e.KeyDown += OnKeyDown;
            _element = e;
        }

        private UIElement _element;

        public Dictionary<string, KeyAction> Action { get; set; } = new Dictionary<string, KeyAction>();

        public void Add(KeyAction action)
        {
            Action.Add(action.Key, action);
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var controlKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            var ctrl = (controlKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var shiftKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);
            var shift = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var altKeyState = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu);
            var alt = (altKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

            if (!ctrl && !shift && !alt)
            {
                return;
            }

            var key = e.Key.ToString();

            if (key == "Control" || key == "Shift" || key == "Menu")
            {
                return;
            }

            StringBuilder str = new StringBuilder();
            if (ctrl)
            {
                str.Append("ctrl+");
            }
            if (shift)
            {
                str.Append("shift+");
            }
            if (alt)
            {
                str.Append("alt+");
            }
            str.Append(key);

            key = str.ToString();
            if (Action.ContainsKey(key))
            {
                Action[key].Run();
            }
        }

        public void Dispose()
        {
            _element.KeyDown -= OnKeyDown;
            Action.Clear();
        }
    }
}

