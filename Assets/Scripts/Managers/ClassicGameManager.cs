using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : Singleton<ClassicGameManager>
{
    override protected void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForFixedUpdate();
        EventManager.TriggerEvent(new LevelStartedEvent());
    }
}
