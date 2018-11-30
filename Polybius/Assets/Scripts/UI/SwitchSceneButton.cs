using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour {

	public void goBackToUI() {
        SceneManager.LoadScene("UI");
    }
}
