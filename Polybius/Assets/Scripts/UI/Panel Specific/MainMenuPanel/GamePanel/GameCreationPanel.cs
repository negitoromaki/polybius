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
            PolybiusManager.dm.hostQuery(Random.Range(0f, 100000000000000000000000000000f).ToString(), gp.currGameType);
        }
    }
}