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
    public class UserTesting : MonoBehaviour
    {
        // Use this for initialization
        SmartFox sfs;
        void Start()
        {

            StartCoroutine(tester());
        }

        // Update is called once per frame
        void Update()
        {
        }

        IEnumerator tester()
        {
            bool[] marks = new bool[] { false, false, false, false, false };
            while (!PolybiusManager.loggedIn)
            {
                yield return null;
            }
            sfs = PolybiusManager.dm.getConnection();
            Debug.Log("Begin User Creation Test");

            //test create
            PolybiusManager.player.setUsername("ree");
            PolybiusManager.player.setPassword("everyone");
            PolybiusManager.player.setEmail("eee@gen.com");
            PolybiusManager.player.setDob(System.DateTime.Now.ToString("MM/dd/yyyy"));


            PolybiusManager.dm.create();
            if (PolybiusManager.dm.result.Equals("success")) 
            marks[0] = true;
            print(PolybiusManager.dm.result);

            PolybiusManager.dm.login();
            if (PolybiusManager.dm.result.Equals("success")) 
            marks[1] = true;
            PolybiusManager.dm.logout();
            if (PolybiusManager.dm.result.Equals("success")) 
            marks[2] = !PolybiusManager.loggedIn;
            PolybiusManager.dm.login();
            if (PolybiusManager.dm.result.Equals("success")) 
            marks[3] = true;
            PolybiusManager.dm.logout();
            PolybiusManager.player.setUsername("Keaton");
            PolybiusManager.player.setPassword("everyone");
            PolybiusManager.player.setEmail("G");
            PolybiusManager.player.setDob(System.DateTime.Now.ToString("MM/dd/yyyy"));
            PolybiusManager.dm.create();
            if (!PolybiusManager.dm.result.Equals("success"))
                marks[4] = true;

            if (marks[0])
            {
                print("TestCreate Passed");
            }
            else
            {
                print("TestCreate Failed");

            }
            if (marks[1])
            {
                print("TestNewLogin Passed");

            }
            else
            {
                print("TestNewLogin Failed");

            }
            if (marks[2])
            {
                print("TestLogout Passed");

            }
            else
            {
                print("TestLogout Failed");

            }
            if (marks[3])
            {
                print("TestExistingLogin Passed");

            }
            else
            {
                print("TestExistingLogin Failed");

            }
            if (marks[4])
            {
                print("TestBadEmail Passed");

            }
            else
            {
                print("TestBadEmail Failed");

            }

        }
    }
}
