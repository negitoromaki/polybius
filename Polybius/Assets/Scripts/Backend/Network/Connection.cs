﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Core;
using Sfs2X.Requests;
using UnityEngine.UI;


public class Connection : MonoBehaviour {

	public string ip = "";
	public int port = 9933;
	public SmartFox sfs = new SmartFox();
	public string initZone = "Polybius";
	public bool logged = false;
	public GameObject userName, passWord, switcher,MainMenu;


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
		logged = false;
	}
	void onLoggin(BaseEvent e){
		sfs.Send (new JoinRoomRequest ("Lobby"));
		switcher.GetComponent<polybius.UIPanelSwitcher> ().ChangeMenu (MainMenu);
		logged = true;

	}
	void onResponse(BaseEvent e){
		
		string cmd = (string)e.Params ["cmd"];
		ISFSObject paramsa = (SFSObject)e.Params["params"];
		string message = cmd + " " + paramsa.GetUtfString("result") + " message: " + paramsa.GetUtfString ("message");
		Debug.Log (cmd + " message: " + message);
	}

	//public methods
	public void loggin(){
		if (userName.GetComponent<InputField> ().text != null && userName.GetComponent<InputField> ().text != null) {

			ISFSObject l = new SFSObject ();
			l.PutUtfString ("username",userName.GetComponent<InputField> ().text);
			l.PutUtfString ("password", userName.GetComponent<InputField> ().text);
			sfs.Send (new LoginRequest (userName.GetComponent<InputField> ().text, "", initZone));
			sfs.Send (new ExtensionRequest ("UserLogin", l));
		}
	}
	public void logout(){
		if (userName.GetComponent<InputField> ().text != null && userName.GetComponent<InputField> ().text != null) {

			ISFSObject l = new SFSObject ();
			l.PutUtfString ("username",userName.GetComponent<InputField> ().text);
			l.PutUtfString ("password", userName.GetComponent<InputField> ().text);
			sfs.Send (new ExtensionRequest ("UserLogout", l));
		}
	}

	public void create(string username, string password, string email){
		ISFSObject o = new SFSObject ();
		o.PutUtfString ("username", username);
		o.PutUtfString ("password", password);
		o.PutUtfString ("email", email);
		sfs.Send (new ExtensionRequest ("CreateUser", o));
	}

	public SmartFox getConnection(){
		return sfs;
	}
	public bool isLogged(){
		return logged;
	}

	//exit handler
	void OnApplicationQuit(){
		logout ();
		Debug.Log ("exiting");
		sfs.RemoveAllEventListeners ();
		if (sfs.IsConnected) {
			sfs.Disconnect();
		}

	}

}
