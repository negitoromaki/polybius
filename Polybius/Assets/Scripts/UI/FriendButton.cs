using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendButton : MonoBehaviour {

    public Sprite heartEmpty, heartFull;

    private bool friend = false;
    
	public void toggleFriendIcon () {
        friend = !friend;
        if (friend) {
            GetComponent<Image>().sprite = heartFull;
        } else {
            GetComponent<Image>().sprite = heartEmpty;
        }
	}
}
