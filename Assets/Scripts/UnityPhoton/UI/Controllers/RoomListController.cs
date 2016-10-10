using Photon;
using UnityEngine;

namespace UnityPhoton {
    public class RoomListController : PunBehaviour {

        [SerializeField]
        private GameObject emptyRoomListDisplay;

        [SerializeField]
        private RoomListDisplay roomListDisplay;

        private void Start() {
            UpdateRoomList();
        }

        private void UpdateRoomList() {
            ShowRoomListDisplay(PhotonNetwork.GetRoomList());
        }

        private void ShowRoomListDisplay(RoomInfo[] roomList) {
            roomListDisplay.gameObject.SetActive(roomList.Length > 0);
            emptyRoomListDisplay.SetActive(!roomListDisplay.gameObject.activeSelf);
            roomListDisplay.ListDisplay.Load(roomList);
        }

        public override void OnReceivedRoomListUpdate() {
            base.OnReceivedRoomListUpdate();
            UpdateRoomList();
        }
    }
}