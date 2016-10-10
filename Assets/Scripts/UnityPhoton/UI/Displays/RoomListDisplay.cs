using UnityEngine;

namespace UnityPhoton {
    public class RoomListDisplay : MonoBehaviour {

        [SerializeField]
        private RectTransform roomDisplayContainer;

        [SerializeField]
        private Object roomDisplayPrefab;

        [SerializeField]
        private LobbyController lobbyController;

        private ListDisplay<RoomInfo, RoomDisplay> listDisplay;

        public ListDisplay<RoomInfo, RoomDisplay> ListDisplay {
            get {
                return listDisplay ?? LoadListDisplay();
            }
        }

        private ListDisplay<RoomInfo, RoomDisplay> LoadListDisplay() {
            listDisplay = new ListDisplay<RoomInfo, RoomDisplay>(
                roomDisplayContainer,
                roomDisplayPrefab,
                RoomInfoComparison);

            listDisplay.LoadElementDisplayEvent.Subscribe(OnLoadRoomDisplay);
            return listDisplay;
        }

        private int RoomInfoComparison(RoomInfo roomInfoA, RoomInfo roomInfoB) {
            return roomInfoA.name.CompareTo(roomInfoB.name);
        }

        private void OnLoadRoomDisplay(RoomDisplay roomDisplay) {
            roomDisplay.Init(lobbyController);
        }
    }
}