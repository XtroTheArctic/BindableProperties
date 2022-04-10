namespace Atesh.BindableProperties;

public static class Strings
{
    public const string PropertyCanNotBindToItself = "Property can not bind to itself.";
    public const string PropertyCanNotBindTwoWayWithoutSecondaryConverter = "Property can not bind as two way without a valid SecondaryConverter method.";
    public const string PropertyCanNotMonitorItself = "Property can not monitor itself.";
    public const string PropertyCanNotMonitorAfterMonitoringStarted = "Property can not monitor a new target after the monitoring started. Please call Monitor method before Bind and StartMonitoring.";
    public const string MonitoringAlreadyStarted = "Monitoring has already started.";
    public const string MonitoringNotStartedYet = "Monitoring has not started yet.";
    public const string PropertyNotBoundYet = "Property is not bound yet.";

    public static string GetValueMethodIsNotSupposedToBeCalledFrequently => $"{nameof(PrivatelySettablePrivatelyBindableProperty<object>.GetValueVeryExpensively)} method is not supposed to be called frequently. Subscribe to {nameof(PrivatelySettablePrivatelyBindableProperty<object>.Changed)} event if you need to retrieve the value frequently. You can call {nameof(PrivatelySettablePrivatelyBindableProperty<object>.DisableGetValueTimeCheckAsALastResort)} method as a LAST RESORT to bypass this informational error message.";
}