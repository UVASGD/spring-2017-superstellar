using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class StartButton : PunBehaviour {

        [SerializeField]
        private NetworkController networkController;

        private Button button;

        private void Start() {
            button = GetComponent<Button>();
            Init();
        }

        private void Init() {
            button.onClick.AddListener(CheckConnect);
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
            base.OnFailedToConnectToPhoton(cause);
            button.interactable = true;
        }

        private void CheckConnect() {
            button.interactable = false;
            networkController.CheckConnect();
        }
    }
}