using InControl;
using UnityEngine;

public class InputPlayer
{ 
    public InputPlayerActions PlayerActionSet { get; set; }
    public bool EnabledController { get; set; }

    string saveData;

    void SaveBindings()
    {
        saveData = PlayerActionSet.Save();
        PlayerPrefs.SetString("Bindings", saveData);
    }
    
    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveData = PlayerPrefs.GetString("Bindings");
            PlayerActionSet.Load(saveData);
        }
    }    
}
