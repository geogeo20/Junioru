using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedCock.Gameplay;
using RedCock.Utils;
using System;
using System.Threading.Tasks;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationManager : SingletonBehaviour<NotificationManager>, IManager
{
    public string Name => "Notification Manager";

    public Action OnInitialized { get; set; }

    public Task Init()
    {
#if UNITY_ANDROID
        RegisterChanel();
#endif
        return null;
    }

#if UNITY_ANDROID

    private void RegisterChanel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void ScheduleAndroidNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "La Junioru";
        notification.Text = "Nu uita sa joci zilnic pentru a putea castiga o cafea la sfarsitul saptamanii";
        notification.FireTime = System.DateTime.Now.AddHours(24);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

#endif

#if UNITY_IOS
    // Start is called before the first frame update
    private void ScheduleiOSNotification()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(24, 0, 0),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Title = "La Junioru",
            Body = "Nu uita sa joci zilnic pentru a putea castiga o cafea la sfarsitul saptamanii",
            ShowInForeground = false,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

#endif

    private void OnDestroy()
    {
#if UNITY_IOS
        ScheduleiOSNotification();
#elif UNITY_ANDROID
        ScheduleAndroidNotification();
#endif
    }
}
