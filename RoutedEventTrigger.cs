using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Kvfx
{
    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        public RoutedEvent RoutedEvent { get; set; }

        public RoutedEventTrigger() { }

        protected override void OnAttached()
        {
            Behavior behavior = AssociatedObject as Behavior;

            FrameworkElement associatedElement = AssociatedObject as FrameworkElement;

            if (behavior != null)
            {
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement;
            }

            if (associatedElement == null)
            {
                throw new ArgumentException("Routed Event Trigger can only be associated to framework elements");
            }

            if (RoutedEvent != null)
            {
                associatedElement.AddHandler(RoutedEvent, new RoutedEventHandler(OnRoutedEvent));
            }
        }

        void OnRoutedEvent(object sender, RoutedEventArgs e)
        {
            OnEvent(e);
        }

        protected override string GetEventName()
        {
            return RoutedEvent.Name;
        }
    }
}
