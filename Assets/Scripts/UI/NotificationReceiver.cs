using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationReceiver : MonoBehaviour
{
    public enum NotificationType
    {
        NewBuilding,
        NewEnemy,
        LabGoBoom,
        AutoSave
    }
    public NotificationType notificationType;
    public NotificationManager notification;
    public AudioSource notficiationSound;

    public void Start()
    {
        Events.active.onEnemyDiscovered += ShowEnemyNotification;
        Events.active.onBuildingUnlocked += ShowBuildingNotification;
        Events.active.onLabDestroyed += ShowLabBoomBoom;
        Events.active.onAutoSave += AutoSave;
    }

    public void ShowBuildingNotification(Buildable buildable)
    {
        if (notificationType == NotificationType.NewBuilding)
        {
            notification.icon = Sprites.GetSprite(buildable.building.name);
            notification.title = "NEW UNLOCK!";
            notification.description = buildable.building.name.ToUpper();
            notification.UpdateUI();
            notification.OpenNotification();

            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
    }

    public void ShowEnemyNotification(Enemy enemy)
    {
        if (notificationType == NotificationType.NewEnemy)
        {
            notification.icon = Sprites.GetSprite(enemy.name);
            notification.title = "NEW ENEMY!";
            notification.description = enemy.name.ToUpper();
            notification.OpenNotification();

            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
    }

    public void ShowLabBoomBoom(ResearchLab lab)
    {
        if (notificationType == NotificationType.LabGoBoom)
        {
            notification.OpenNotification();
            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
    }

    public void AutoSave()
    {
        if (notificationType == NotificationType.AutoSave)
        {
            notification.OpenNotification();
            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
    }
}
