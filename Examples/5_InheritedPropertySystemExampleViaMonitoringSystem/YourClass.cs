using System.Windows.Media;
using System.Windows.Shapes;
using Atesh.BindableProperties;

namespace InheritedPropertySystemExampleViaMonitoringSystem;

// IMPORTANT: Please read the first example before this.
public class YourClass
{
    // Purpose of this example is to show how to use the monitoring system for implementing an inherited property system.

    // Monitoring system helps you unbind a bindable property and rebind it to the correct target automatically when one of the monitored targets change.
    // Monitoring system works in the order described below.
    // 1) You pass a callback method "Binder" parameter when you first create the bindable property instance. Binder callback you implement must perform the steps 2 and 3 below.
    // 2) You register the target properties one by one to the monitoring list of a bindable property by calling Monitor method.
    // 3) You bind the bindable property to a target (which isn't monitored). You can't add to the monitoring list after the binding.
    // 4) Monitoring system will detect a change on one of the monitored targets and it will unbind the bindable property automatically.
    // 5) Monitoring system flushes out the old monitoring list of the bindable property.
    // 6) Monitoring system calls the Binder callback which performs the steps number 2 and 3 so the automated binding/unbinding cycle continues from the step number 4.

    // This class gets a visual rectangle object and controls its color according to BackgroundColor property.
    public Rectangle Rectangle { get; }

    // We want the background property to be set publicly and it needs to support the empty value for inherited property system so we use PrivatelyBindablePropertyWithEmptyValue type.
    // No need for a backing field because we will consume the new background color value in BackgroundColor_Changed handler by assigning it to the rectangle object.
    public PrivatelyBindablePropertyWithEmptyValue<Color> BackgroundColor { get; }

    // Parent property for inherited properties system.
    public PrivatelyBindableProperty<YourClass> Parent { get; }

    // Our example inherited properties system also has skinning support.
    public PrivatelyBindableProperty<Skin> Skin { get; }

    // Method containers to control the bindable property.
    readonly PrivatelyBindableProperty<Color>.BindMethods BackgroundColor_BindMethods;

    readonly Color DefaultBackgroundColor = Colors.Gray;

    YourClass Parent_;
    Skin Skin_;
    bool UnboundBackgroundColorIsEmpty;
    bool ExecutingBindBackgroundColor;

    public YourClass(Rectangle Rectangle)
    {
        this.Rectangle = Rectangle;

        // Create the bindable property and receive the methods via the containers.
        BackgroundColor = new(this, out BackgroundColor_BindMethods, true, BindBackgroundColor);

        Parent = new(this, out _);
        Parent.Changed += Parent_Changed;

        Skin = new(this, out _);
        Skin.Changed += Skin_Changed;

        // Subscribe to the Changed event of the bindable property.
        BackgroundColor.Changed += BackgroundColor_Changed;
    }

    void Skin_Changed(PrivatelySettablePrivatelyBindableProperty<Skin> Sender, ChangedEventArgs<Skin> Args) => Skin_ = Args.Value;
    void Parent_Changed(PrivatelySettablePrivatelyBindableProperty<YourClass> Sender, ChangedEventArgs<YourClass> Args) => Parent_ = Args.Value;

    void BackgroundColor_Changed(PrivatelySettablePrivatelyBindableProperty<Color> Sender, ChangedEventArgs<Color> Args)
    {
        if (BackgroundColor.BoundProperty == null) UnboundBackgroundColorIsEmpty = Args.IsEmpty;

        var BoundPropertyIsBoundToo = BackgroundColor.BoundProperty is PrivatelySettablePrivatelyBindableProperty<Color> { BoundProperty: { } };
        if (BoundPropertyIsBoundToo) BackgroundColor_BindMethods.Unbind();

        if (Args.IsEmpty)
        {
            if (!ExecutingBindBackgroundColor)
            {
                if (BackgroundColor.BoundProperty is { }) BackgroundColor_BindMethods.Unbind();

                BindBackgroundColor();

                // If got bound above
                if (BackgroundColor.BoundProperty is { }) return;
            }

            Rectangle.Fill = new SolidColorBrush(DefaultBackgroundColor);
        }
        else
        {
            if (BoundPropertyIsBoundToo) BindBackgroundColor();

            Rectangle.Fill = new SolidColorBrush(Args.Value);
        }
    }

    // This is where all the binding magic happens.
    // We check the entire parent chain of this object for any existing BackgroundColor value along with skins of every parent step until we find a suitable binding target.
    // We add all parents, BackgroundColor properties, skins and skin BackgroundColor to the monitoring list before binding the BackgroundColor property of this object to the suitable target.
    // The monitoring system will let us repeat this operation after unbinding the BackgroundColor property and flushing the monitoring list when anything in the chain changes.
    void BindBackgroundColor()
    {
        // If the property have a value and coming from monitoring system(Binder callback) 
        if (!UnboundBackgroundColorIsEmpty) return;

        if (BackgroundColor.IsMonitoringWithoutBinding) BackgroundColor_BindMethods.StopMonitoring();

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
                        BackgroundColor_BindMethods.Bind(P.BackgroundColor);

                        return;
                    }

                    BackgroundColor_BindMethods.Monitor(P.BackgroundColor);
                }

                BackgroundColor_BindMethods.Monitor(P.Skin);

                var P_Skin = P.Skin_;

                if (P_Skin is { })
                {
                    // Second, we check the BackgroundColor of the skin of current parent step.
                    if (!P_Skin.BackgroundColorIsEmpty)
                    {
                        BackgroundColor_BindMethods.Bind(P_Skin.BackgroundColor);

                        return;
                    }

                    BackgroundColor_BindMethods.Monitor(P_Skin.BackgroundColor);
                }

                BackgroundColor_BindMethods.Monitor(P.Parent);

                P = P.Parent_;
            }
        }
        finally
        {
            if (BackgroundColor.BoundProperty == null)
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

                BackgroundColor_BindMethods.StartMonitoring();
            }
        }
    }
}