using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Input;

namespace Helper {
    public class DoubleClickEventTrigger : EventTrigger {
        protected override void OnEvent(EventArgs eventArgs) {
            var e = eventArgs as MouseButtonEventArgs;
            if (e == null) {
                return;
            }
            if (e.ClickCount == 2) {
                base.OnEvent(eventArgs);
            }
        }
    }
}