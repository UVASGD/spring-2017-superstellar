using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {
    public class RoomNameDisplay : PunBehaviour {

        [SerializeField]
        private Text roomNameText;

        private void Start() {
            roomNameText.text = PhotonNetwork.room.name;
        }

        public override void OnLeftRoom() {
            base.OnLeftRoom();
            roomNameText.text = "";
        }
    }
}