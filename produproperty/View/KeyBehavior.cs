using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace produproperty.View
{
    public class KeyBehavior : IDisposable
    {
        public KeyBehavior(UIElement e)
        {
            e.KeyDown += OnKeyDown;
            e.AddHandler(UIElement.KeyDownEvent,new KeyEventHandler(OnKeyDown),true);
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
                str.Append(KeyAction.Ctrl);
            }
            if (shift)
            {
                str.Append(KeyAction.Shift);
            }
            if (alt)
            {
                str.Append(KeyAction.Alt);
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

    public class KeyAction
    {
        public string Key
        {
            get; set;
        }

        public KeyAction(string key, Action<KeyAction> action)
        {
            Action = action;
            Key = key;
        }

        public Action<KeyAction> Action
        {
            get; set;
        }

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

        public const string Ctrl = "ctrl";
        public const string Alt = "alt";
        public const string Shift = "shift";
    }
}