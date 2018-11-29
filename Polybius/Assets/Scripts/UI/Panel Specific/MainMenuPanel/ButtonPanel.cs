using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour {

    public GameObject initialButton;
    public Color selectedColor;
    public Color unselectedColor;

    private GameObject activeButton;

	public void OnEnable() {
        // Set all inactive except active one
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.GetComponent<Image>().color = unselectedColor;

        // Activate
        Debug.Assert(initialButton != null);
        PressButton(initialButton);
    }

    // method to switch buttons
    public void PressButton(GameObject button) {
        if (activeButton)
            activeButton.GetComponent<Image>().color = unselectedColor;
        button.GetComponent<Image>().color = selectedColor;

        activeButton = button;
    }
}
