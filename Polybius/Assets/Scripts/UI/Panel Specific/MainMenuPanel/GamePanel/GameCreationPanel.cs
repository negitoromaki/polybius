using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class GameCreationPanel : MonoBehaviour {

        public GamePanel gp;
        public TextMeshProUGUI header;

        public void OnEnable() {
            Debug.Assert(header != null);
            header.text = "Create new " + gp.currGameType.ToString() + " game";
        }

        public void createGame() {
			PolybiusManager.dm.hostLobby("testRoom" + Random.Range(0,1000000000));
        }
    }
}