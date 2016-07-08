﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Client : MonoBehaviour 
{
	public Canvas connectWindow;
	public Button connectButton;
	public Keyboard keyboard;
	public InputField inputIP;

	public DebugInfo debugInfo;
	private string serverIP = "";
	private int port = 9973;

	// Use this for initialization
	void Start() 
	{
		connectWindow.enabled = false;
		connectButton.onClick.AddListener(StartConnect);
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			connectWindow.enabled ^= true;
	}

	void OnGUI()
	{
		serverIP = inputIP.text;
	}

	public void StartConnect()
	{
		NetworkConnectionError error = Network.Connect(serverIP, port);
		switch (error) 
		{
			case NetworkConnectionError.NoError:
				break;
			default:
				Debug.Log("client:" + error);
				debugInfo.Log("Client", error.ToString());
				break;
		}
	}
	
	public void Send(string tag, string message)
	{
		Send(tag + ":" + message);
	}

	public void Send(string message)
	{
        GetComponent<NetworkView>().RPC("ReceiveMessage", RPCMode.All, message);
    }
	
	[RPC]
	void ReceiveMessage(string message, NetworkMessageInfo info)
	{
		Debug.Log(message);
		string tag = message.Split(':')[0];
		string msg = message.Split(':')[1];
		switch (tag)
		{
			case "TouchScreen Keyboard Size":
				if (msg == "+")
					keyboard.ZoomIn();
				else if (msg == "-")
					keyboard.ZoomOut();
				break;
			default:
				break;
		}
	}
}
