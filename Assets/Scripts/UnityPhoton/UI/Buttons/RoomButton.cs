using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class RoomButton: PunBehaviour {
        private Button button;

        private void Start() {
            button = GetComponent<Button>();
            Init();
        }

        private void Init() {
            button.interactable = PhotonNetwork.inRoom;
        }

        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            button.interactable = true;
        }

        public override void OnLeftRoom() {
            base.OnLeftRoom();
            button.interactable = false;
        }
    }
}