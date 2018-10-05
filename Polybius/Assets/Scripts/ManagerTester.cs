using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

namespace polybius {
    public class ManagerTester : MonoBehaviour {

        // Use this for initialization
        SmartFox sfs;
        void Start() {
            sfs = PolybiusManager.dm.getConnection();
            StartCoroutine(tester());
        }

        // Update is called once per frame
        void Update() {

        }

        IEnumerator tester() {
            while (!PolybiusManager.loggedIn) {
                yield return null;
            }

            Debug.Log("Begin tests");

            //test create
            PolybiusManager.player.username = "gib";
            PolybiusManager.player.password = "megamind";
            PolybiusManager.player.email = "tim@gove.com";
            PolybiusManager.player.dob = System.DateTime.Now.ToString("MM/dd/yyyy");


            PolybiusManager.dm.create();
            PolybiusManager.dm.login();


        }



    }
}