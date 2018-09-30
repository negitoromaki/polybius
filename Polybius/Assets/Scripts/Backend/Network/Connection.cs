using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Core;
using Sfs2X.Requests;

public class Connection : MonoBehaviour {

	public string ip = "";
	public int port = 9933;
	public static SmartFox sfs = new SmartFox();


	// Use this for initialization
	void Start () {
		sfs.AddEventListener (SFSEvent.LOGIN, onLoggin);
		sfs.AddEventListener (SFSEvent.CONNECTION, onConnect);
		sfs.AddEventListener (SFSEvent.CONNECTION_LOST, onLost);
		sfs.AddEventListener (SFSEvent.EXTENSION_RESPONSE, onResponse);
		sfs.ThreadSafeMode = true;

		sfs.Connect (ip, port);
		
	}
	
	// Update is called once per frame
	void Update () {
		sfs.ProcessEvents ();
		
	}


	//event listeners
	void onConnect(BaseEvent e){
		if ((bool)e.Params ["success"]) {
			Debug.Log ("Connected");

		} else {
			Debug.Log ("Connection failed");
		}
	}
	void onLost(BaseEvent e){

	}
	void onLoggin(BaseEvent e){

	}
	void onResponse(BaseEvent e){

	}

	//public methods
	public void loggin(){

	}

	public void create(){

	}

}
