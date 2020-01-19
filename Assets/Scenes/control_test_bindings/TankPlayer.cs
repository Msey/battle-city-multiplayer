using InControl;
using UnityEngine;

public class TankPlayer : MonoBehaviour
{
    public TankPlayerActions Actions { get; set; }

    void OnEnable()
    {
        LoadBindings();
    }

    void OnDisable()
    {
        if (Actions != null)
        {
            Actions.Destroy();
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Actions != null)        
        {
            print("X: " + Actions.Direction.X + "| Y: " + Actions.Direction.Y + "| Fire: " + (Actions.Fire.IsPressed ? "Y" : "N"));
        }
    }


    string GetActionName()
    {
        if (Actions.Up)
            return Actions.Up.Name;

        if (Actions.Down)
            return Actions.Down.Name;

        if (Actions.Left)
            return Actions.Left.Name;

        if (Actions.Right)
            return Actions.Right.Name;

        if (Actions.Fire)
            return Actions.Fire.Name;

        return string.Empty;
    }

    string saveData;

    void SaveBindings()
    {
        saveData = Actions.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }


    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            Actions.Load(saveData);
        }
    }    

    void OnGUI()
    {
        const float h = 22.0f;
        var y = 10.0f;

        GUI.Label(new Rect(10, y, 300, y + h), "Last Input Type: " + Actions.LastInputType);
        y += h;

        //GUI.Label( new Rect( 10, y, 300, y + h ), "Active Device: " + playerActions.ActiveDevice.Name );
        //y += h;

        GUI.Label(new Rect(10, y, 300, y + h), "Last Device Class: " + Actions.LastDeviceClass);
        y += h;

        GUI.Label(new Rect(10, y, 300, y + h), "Last Device Style: " + Actions.LastDeviceStyle);
        y += h;

        var actionCount = Actions.Actions.Count;
        for (var i = 0; i < actionCount; i++)
        {
            var action = Actions.Actions[i];

            var name = action.Name;
            if (action.IsListeningForBinding)
            {
                name += " (Listening)";
            }
            name += " = " + action.Value;
            //name += " via " + action.ActiveDevice.Name;
            //name += ", class: " + action.LastDeviceClass;
            //name += ", style: " + action.LastDeviceStyle;
            GUI.Label(new Rect(10, y, 500, y + h), name);
            y += h;

            var bindingCount = action.Bindings.Count;
            for (var j = 0; j < bindingCount; j++)
            {
                var binding = action.Bindings[j];

                GUI.Label(new Rect(75, y, 300, y + h), binding.DeviceName + ": " + binding.Name);

                if (GUI.Button(new Rect(20, y + 3.0f, 20, h - 5.0f), "-"))
                {
                    action.RemoveBinding(binding);
                }

                if (GUI.Button(new Rect(45, y + 3.0f, 20, h - 5.0f), "+"))
                {
                    action.ListenForBindingReplacing(binding);
                }

                y += h;
            }

            if (GUI.Button(new Rect(20, y + 3.0f, 20, h - 5.0f), "+"))
            {
                action.ListenForBinding();
            }

            if (GUI.Button(new Rect(50, y + 3.0f, 50, h - 5.0f), "Reset"))
            {
                action.ResetBindings();
            }

            y += 25.0f;
        }

        if (GUI.Button(new Rect(20, y + 3.0f, 50, h), "Load"))
        {
            LoadBindings();
        }

        if (GUI.Button(new Rect(80, y + 3.0f, 50, h), "Save"))
        {
            SaveBindings();
        }

        if (GUI.Button(new Rect(140, y + 3.0f, 50, h), "Reset"))
        {
            Actions.Reset();
        }
    }
}
