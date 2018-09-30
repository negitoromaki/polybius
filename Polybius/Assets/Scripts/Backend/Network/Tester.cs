using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

[RequireComponent(typeof(Connection))]
public class Tester : MonoBehaviour {


	// Use this for initialization
	SmartFox sfs;
	Connection connection;
	void Start () {
		connection = GetComponent<Connection> ();
		sfs = connection.getConnection ();
		StartCoroutine (tester ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator tester(){
		while (!connection.isLogged()) {
			yield return null;
		}

		Debug.Log ("Begin tests");

		//test create
		string username = "tim";
		string password = "badpass";
		string email = "tim@tim.com";

		connection.create (username, password, email);
		connection.loggin (username, password);


	}



}
