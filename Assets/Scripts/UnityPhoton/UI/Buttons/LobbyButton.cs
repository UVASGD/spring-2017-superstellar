using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class LobbyButton : PunBehaviour {
        private Button button;

        private void Start() {
            button = GetComponent<Button>();
            Init();
        }

        private void Init() {
            button.interactable = PhotonNetwork.insideLobby;
        }

        public override void OnJoinedLobby() {
            base.OnJoinedLobby();
            button.interactable = true;
        }

        public override void OnLeftLobby() {
            base.OnLeftLobby();
            button.interactable = false;
        }
    }
}