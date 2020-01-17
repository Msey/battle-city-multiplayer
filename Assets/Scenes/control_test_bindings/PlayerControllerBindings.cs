using UnityEngine;

public class PlayerControllerBindings : MonoBehaviour
{
    PlayerTankControllerActions playerActions;
    string saveData;

    void OnEnable()
    {
        playerActions = PlayerTankControllerActions.CreateWithDefaultBindings();
    }


    void Update()
    {
        if (playerActions.Fire.IsPressed) Debug.Log(playerActions.Fire.Name);

        if (playerActions.Up.IsPressed) Debug.Log(playerActions.Up.Name);
        if (playerActions.Down.IsPressed) Debug.Log(playerActions.Down.Name);

        if (playerActions.Left.IsPressed) Debug.Log(playerActions.Left.Name);
        if (playerActions.Right.IsPressed) Debug.Log(playerActions.Right.Name);

        if (Input.GetKeyDown(KeyCode.Z))
            playerActions.BindInput(InputType.Fire);

    }
}
