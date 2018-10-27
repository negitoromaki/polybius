using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine.Assertions;

namespace polybius {
    public class CreateTester : MonoBehaviour {

        // Use this for initialization
        SmartFox sfs;
        void Start() {
            
           
            StartCoroutine(tester());
        }

        // Update is called once per frame
        void Update() {

        }

        IEnumerator tester()
        {
            while (!PolybiusManager.loggedIn)
            {
                yield return null;
            }
            sfs = PolybiusManager.dm.getConnection();
            Debug.Log("Begin User Creation Test");

            //test create
            Assert.IsTrue(  PolybiusManager.player.setUsername("gib") &&
                            PolybiusManager.player.setPassword("megamind") &&
                            PolybiusManager.player.setEmail("tim@gove.com") &&
                            PolybiusManager.player.setDob(System.DateTime.Now.ToString("MM/dd/yyyy")));


            PolybiusManager.dm.create();
            Assert.IsTrue(PolybiusManager.loggedIn);
            PolybiusManager.dm.login();
            Assert.IsTrue(sfs.IsConnected);
            print("Create Test Succesful");
        }



    }
}