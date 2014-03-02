using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {

	public string serverAddress;
	public int port = 9090;
	public int messageTimeout = 5;

	// OnGUI is called for rendering and handling
	// GUI events.
	void OnGUI () {
		GUILayout.BeginArea(new Rect(Screen.width - 130, 10, 120, Screen.height));

		GUILayout.Label("LAN Multiplayer");
		if (Network.isServer) {
			GUILayout.Box("Server Running");
			GUILayout.Label("Address: " + Network.player.ipAddress);
			GUILayout.Label("Port: " + port);

			GUILayout.Box("Connected Players");
			GUILayout.BeginHorizontal();
			GUILayout.Label(Network.player.ipAddress);
			GUILayout.Label("" + Network.GetAveragePing(Network.player));
			GUILayout.EndHorizontal();
			foreach (NetworkPlayer player in Network.connections) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(player.ipAddress);
				GUILayout.Label("" + Network.GetAveragePing(player));
				GUILayout.EndHorizontal();
			}
		} else if (Network.isClient) {
			GUILayout.Box("Connected");
			foreach (NetworkPlayer player in Network.connections) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(player.ipAddress);
				GUILayout.Label("" + Network.GetAveragePing(player));
				GUILayout.EndHorizontal();
		      }
		} else {
			if (GUILayout.Button("Host Game"))
				Network.InitializeServer(4, port, false);
			
			if (GUILayout.Button("Join Game"))
				Network.Connect(serverAddress, port);
		}

		GUILayout.EndArea();
	}
	
	// Called on the server whenever a new
	// player has successfully connected.
	void OnPlayerConnected (NetworkPlayer player) {
		
	}
	
	// Called on the server whenever a Network.InitializeServer
	// was invoked and has completed.
	void OnServerInitialized () {
		
	}
	
	// Called on the client when you have successfully
	// connected to a server.
	void OnConnectedToServer () {
		
	}
	
	// Called on the server whenever a player
	// disconnected from the server.
	void OnPlayerDisconnected (NetworkPlayer player) {
		
	}
	
	// Called on the client when the connection
	// was lost or you disconnected from the
	// server.
	void OnDisconnectedFromServer (NetworkDisconnection info) {
		
	}
	
	// Called on the client when a connection
	// attempt fails for some reason.
	void OnFailedToConnect (NetworkConnectionError error) {
		
	}
	
	// Called on clients or servers when there
	// is a problem connecting to the MasterServer.
	void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		
	}
	
	// Called on clients or servers when reporting
	// events from the MasterServer.
	void OnMasterServerEvent (MasterServerEvent msEvent) {
		
	}
	
	// Called on objects which have been network
	// instantiated with Network.Instantiate
	void OnNetworkInstantiate (NetworkMessageInfo info) {
		
	}
	
	// Used to customize synchronization of
	// variables in a script watched by a network
	// view.
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
		
	}
	
}
