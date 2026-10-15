using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SfmlControls.Controls;

namespace SfmlControls.Events
{
    public struct SelectionChangedEventArgs
    {
        public readonly string ItemName;

        public SelectionChangedEventArgs(string itemName)
        {
            ItemName = itemName;
        }
    }

    public struct ScrollValueChangedEventArgs
    {
        public readonly string ItemName;
        public readonly ScrollAxis ScrollAxis;
        public readonly float ScrollValue;

        public ScrollValueChangedEventArgs(string itemName,  ScrollAxis scrollAxis, float scrollValue)
        {
            ItemName = itemName;
            ScrollAxis = scrollAxis;
            ScrollValue = scrollValue;
        }
    }

    public struct CheckedChangedEventsArgs
    {
        public readonly bool IsChecked;

        public CheckedChangedEventsArgs(bool isChecked)
        {
            IsChecked = isChecked;
        }
    }

    public struct TextChangedEventsArgs
    {
        public readonly string TextStr;

        public TextChangedEventsArgs(string textStr)
        {
            TextStr = textStr;
        }
    }
}
