using Photon;
using System;
using UnityEngine.SceneManagement;

namespace UnityPhoton {
    public class LobbyConnector : PunBehaviour {
        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            TryToJoinLobby();
        }

        public void TryToJoinLobby() {
            if (!PhotonNetwork.JoinLobby()) {
                throw new Exception("Failed to join lobby");
            }
        }

        public override void OnJoinedLobby() {
            base.OnJoinedLobby();
            SceneManager.LoadScene("Lobby");
        }
    }
}