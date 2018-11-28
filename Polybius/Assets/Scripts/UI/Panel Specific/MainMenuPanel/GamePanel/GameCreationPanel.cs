using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace polybius {
    public class GameCreationPanel : MonoBehaviour {

        public GamePanel gp;
        public TextMeshProUGUI header;
        public TMP_InputField name;

        public void OnEnable() {
            Debug.Assert(header != null && name != null);
            header.text = "Create new " + gp.currGameType.ToString() + " game";
        }

        public void Update() {
            if (PolybiusManager.currGame != null) {
                // TODO: Open correct game scene
            }
        }

        public void createGame() {
			PolybiusManager.dm.host(name.text, gp.currGameType);
            PolybiusManager.dm.hostLobby(name.text);
        }
    }
}