using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class PlayerJoining : MonoBehaviour
{
    // UI elements
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public ProgressBar progress;

    // Start is called before the first frame update
    public void Start()
    {
        UIEvents.active.onPlayerJoin += ToggleScreen;
        UIEvents.active.onUpdateJoinProgress += UpdateJoinPercentage;
    }

    // Update is called once per frame
    public void ToggleScreen(bool enabled)
    {
        if (enabled)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Update join percentage
    public void UpdateJoinPercentage(float percent)
    {
        progress.currentPercent = percent;
        progress.UpdateUI();
    }
}
