using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

public class ManagerTester : MonoBehaviour
{


    // Use this for initialization
    SmartFox sfs;
    polybius.PolybiusManager connection;
    void Start()
    {
        connection = GetComponent<polybius.PolybiusManager>();
        sfs = connection.getConnection();
        StartCoroutine(tester());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator tester()
    {
        while (!connection.isLogged())
        {
            yield return null;
        }

        Debug.Log("Begin tests");

        //test create
        string username = "gib";
        string password = "megamind";
        string email = "tim@gove.com";
        string dob = System.DateTime.Now.ToString("MM/dd/yyyy");


        connection.create(username, password, email, dob);
        connection.login(username, password);


    }



}
