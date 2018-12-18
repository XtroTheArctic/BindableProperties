using System.Windows.Media;
using System.Windows.Shapes;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem
{
    // IMPORTANT: Please read the first example before this.
    class YourClass
    {
        // Purpose of this example is to show how to use the monitoring system for implementing an inherited property system.

        // Monitoring system helps you unbind a bindable property and bind it again to the correct target automatically when one of the monitored targets change.
        // Monitoring system works in the order described below.
        // 1) You provide a callback "BinderCallback" when you fist create the bindable property instance. BinderCallback you implement must perform the steps 2 and 3 below.
        // 2) You register the target properties one by one to the monitoring list of a bindable property by calling Monitor method.
        // 3) You bind the bindable property to a target (which isn't monitored). You can't add to the monitoring list after the binding.
        // 4) Monitoring system will detect a change on one of the monitored targets and it will unbind the bindable property automatically.
        // 5) Monitoring system flushes out the old monitoring list of the bindable property.
        // 6) Monitoring system calls the BinderCallback which performs the steps number 2 and 3 so the automated binding/unbinding cycle continues from the step number 4.

        // This class gets a visual rectangle object and controls its color according to BackgroundColor property.
        readonly Rectangle Rectangle;

        // We want the background property to be set publicly and it needs to support the empty value for inherited property system so we use PrivatelyBindablePropertyWithEmptyValue type.
        // No need for a backing field because we will consume the new background color value in BackgroundColor_Changed handler by assigning it to the rectangle object.
        public readonly PrivatelyBindablePropertyWithEmptyValue<Color> BackgroundColor;

        // Parent property for inherited properties system.
        //todo: Parent property doesn't need to be a bindable property but we need it as a monitoring target until regular property targeting support gets implemented.
        public readonly PrivatelyBindableProperty<YourClass> Parent;

        // Our example inherited properties system also has skinning support.
        //todo: Skin property doesn't need to be a bindable property but we need it as a monitoring target until regular property targeting support gets implemented.
        public readonly PrivatelyBindableProperty<Skin> Skin;

        // Delegates to control the bindable property.
        PrivatelyBindableProperty<Color>.BindDelegates BindBackgroundColorDelegates;

        readonly Color DefaultBackgroundColor = Colors.Gray;

        YourClass _Parent;
        Skin _Skin;
        bool UnboundBackgroundColorIsEmpty;
        bool ExecutingBindBackgroundColor;

        public YourClass(Rectangle Rectangle)
        {
            this.Rectangle = Rectangle;

            // Create the bindable property and get the delegates back.
            BackgroundColor = new PrivatelyBindablePropertyWithEmptyValue<Color>(this, out BindBackgroundColorDelegates, true, BindBackgroundColor);

            Parent = new PrivatelyBindableProperty<YourClass>(this, out _);
            Parent.Changed += Parent_Changed;

            Skin = new PrivatelyBindableProperty<Skin>(this, out _);
            Skin.Changed += Skin_Changed;

            // Subscribe to the Changed event of the bindable property.
            BackgroundColor.Changed += BackgroundColor_Changed;
        }

        void Skin_Changed(PrivatelySettablePrivatelyBindableProperty<Skin> Sender, ChangedEventArgs<Skin> Args) => _Skin = Args.Value;
        void Parent_Changed(PrivatelySettablePrivatelyBindableProperty<YourClass> Sender, ChangedEventArgs<YourClass> Args) => _Parent = Args.Value;

        void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args)
        {
            if (!BackgroundColor.IsBound) UnboundBackgroundColorIsEmpty = Args.IsEmpty;

            if (Args.IsEmpty)
            {
                if (!ExecutingBindBackgroundColor)
                {
                    if (!BackgroundColor.IsBound)
                    {
                        BindBackgroundColor();

                        // If got bound above
                        if (BackgroundColor.IsBound) return;
                    }
                }

                Rectangle.Fill = new SolidColorBrush(DefaultBackgroundColor);
            }
            else Rectangle.Fill = new SolidColorBrush(Args.Value);
        }

        // This is where all the binding magic happens.
        // We check the entire parent chain of this object for any existing BackgroundColor value along with skins of every parent step until we find a suitable binding target.
        // We add all parents, BackgroundColor properties, skins and skin BackgroundColor to the monitoring list before binding the BackgroundColor property of this object to the suitable target.
        // The monitoring system will let us repeat this operation after unbinding the BackgroundColor property and flushing the monitoring list when anything in the chain changes.
        void BindBackgroundColor()
        {
            // If the property have a value and coming from monitoring system(BinderCallback) 
            if (!UnboundBackgroundColorIsEmpty) return;

            if (BackgroundColor.IsMonitoringWithoutBinding) BindBackgroundColorDelegates.StopMonitoring();

            var P = this;

            try
            {
                // Starting from "this", we traverse the parent chain upwards.
                while (true)
                {
                    if (P == null) return;

                    // First, we check the BackgroundColor of the current parent step (Except "this" step).
                    if (P != this)
                    {
                        if (!P.UnboundBackgroundColorIsEmpty)
                        {
                            BindBackgroundColorDelegates.Bind(P.BackgroundColor);

                            return;
                        }

                        BindBackgroundColorDelegates.Monitor(P.BackgroundColor);
                    }

                    BindBackgroundColorDelegates.Monitor(P.Skin);

                    if (P._Skin != null)
                    {
                        // Second, we check the BackgroundColor of the skin of current parent step.
                        if (!P._Skin.BackgroundColorIsEmpty)
                        {
                            BindBackgroundColorDelegates.Bind(P._Skin.BackgroundColor);

                            return;
                        }

                        BindBackgroundColorDelegates.Monitor(P._Skin.BackgroundColor);
                    }

                    BindBackgroundColorDelegates.Monitor(P.Parent);

                    P = P._Parent;
                }
            }
            finally
            {
                if (!BackgroundColor.IsBound)
                {
                    if (UnboundBackgroundColorIsEmpty)
                    {
                        // Prevent recursion.
                        ExecutingBindBackgroundColor = true;

                        try
                        {
                            BackgroundColor.ClearValue();
                        }
                        finally
                        {
                            ExecutingBindBackgroundColor = false;
                        }
                    }

                    BindBackgroundColorDelegates.StartMonitoring();
                }
            }
        }
    }
}