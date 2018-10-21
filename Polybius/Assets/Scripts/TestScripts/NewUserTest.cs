using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine.Assertions;

namespace polybius
{
    public class NewUserTest : MonoBehaviour
    {
        // Use this for initialization
        SmartFox sfs;
        void Start()
        {
            Assert.raiseExceptions = true;
            StartCoroutine(tester());
        }

        // Update is called once per frame
        void Update()
        {
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
            PolybiusManager.player.username = "gib";
            PolybiusManager.player.password = "megamind";
            PolybiusManager.player.email = "tim@gove.com";
            PolybiusManager.player.dob = System.DateTime.Now.ToString("MM/dd/yyyy");


            PolybiusManager.dm.create();
            Assert.IsTrue(PolybiusManager.loggedIn);
            PolybiusManager.dm.login();
            Assert.IsTrue(sfs.IsConnected);
            print("Create Test Succesful");
        }
    }
}
