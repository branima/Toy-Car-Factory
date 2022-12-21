using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsInteractivityEvent : MonoBehaviour
{
    public Transform buttonsParent;
    List<Button> buttons;

    void Start()
    {
        buttons = new List<Button>();
        foreach (Transform button in buttonsParent)
            buttons.Add(button.GetComponent<Button>());
    }

    public void DisableButtons()
    {
        //foreach (Button button in buttons)
        //    button.interactable = false;
    }

    public void EnableButtons()
    {
        //foreach (Button button in buttons)
        //    button.interactable = true;
    }
}
