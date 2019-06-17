using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public Canvas mainCanvas;

    private Animator mainMenuAnimator;

    private void Awake()
    {
        Assert.IsNotNull(mainCanvas);
        mainMenuAnimator = mainCanvas.GetComponent<Animator>();
        Assert.IsNotNull(mainMenuAnimator);
    }

    public void OpenSelectLevelCanvas()
    {
        mainMenuAnimator.SetTrigger("OnOpenSelectLevelCanvas");
    }

    public void Back()
    {
        mainMenuAnimator.SetTrigger("OnBackClicked");
    }

    public void OpenSettingsCanvas()
    {
        mainMenuAnimator.SetTrigger("OnOpenSettingsCanvas");
    }

    public void OpenInputCanvas()
    {
        mainMenuAnimator.SetTrigger("OnOpenInputCanvas");
    }
}
