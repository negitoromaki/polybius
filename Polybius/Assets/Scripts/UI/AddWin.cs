using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polybius {
    public class AddWin : MonoBehaviour {

        public string gameType;

        public void OnEnable() {
            Debug.Assert(gameType != null);

            Stat s = new Stat(PolybiusManager.player.getUserID(), 0, 0, 0);

            if (gameType == "pong") {
                s = new Stat(PolybiusManager.player.getUserID(), 1, 0, 0);
            } else if (gameType == "ticTacToe") {
                s = new Stat(PolybiusManager.player.getUserID(), 0, 1, 0);
            } else if (gameType == "connect4") {
                s = new Stat(PolybiusManager.player.getUserID(), 0, 0, 1);
            }

            PolybiusManager.dm.setStat(s);
            PolybiusManager.currGame = null;
        }
    }
}
