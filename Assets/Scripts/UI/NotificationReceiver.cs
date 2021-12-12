using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NotificationReceiver : MonoBehaviour
{
    public enum NotificationType
    {
        NewBuilding,
        NewEnemy,
        LabGoBoom,
        AutoSave,
        EnemyGroup,
        Disconnected
    }
    public NotificationType notificationType;
    public NotificationManager notification;
    public AudioSource notficiationSound;

    public void Start()
    {
        if (notificationType == NotificationType.EnemyGroup) Events.active.onEnemyGroupSpawned += ShowEnemyGroupNotification;
        else if (notificationType == NotificationType.NewEnemy) Events.active.onEnemyDiscovered += ShowEnemyNotification;
        else if (notificationType == NotificationType.NewBuilding) Events.active.onBuildingUnlocked += ShowBuildingNotification;
        else if (notificationType == NotificationType.LabGoBoom) Events.active.onLabDestroyed += ShowLabBoomBoom;
        else if (notificationType == NotificationType.AutoSave) Events.active.onAutoSave += AutoSave;
        else if (notificationType == NotificationType.Disconnected) Events.active.onClientDisconnect += ShowDisconnectNotif;
    }
    
    public void ShowDisconnectNotif()
    {
        if (notificationType == NotificationType.Disconnected)
        {
            notification.OpenNotification();
            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
    }

    public void ShowEnemyGroupNotification(string msg)
    {
        if (notificationType == NotificationType.EnemyGroup)
        {
            notification.description = msg;
            notification.UpdateUI();
            notification.OpenNotification();

            notficiationSound.volume = Settings.sound;
            notficiationSound.Play();
        }
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
