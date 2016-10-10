using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class DialogControllerButton : MonoBehaviour {

        [SerializeField]
        private GameObject dialog;

        [SerializeField]
        private DialogAction dialogAction;

        private void Start() {
            GetComponent<Button>().onClick.AddListener(SetDialogActiveState);
        }

        private void SetDialogActiveState() {
            dialog.SetActive(dialogAction == DialogAction.Open);
        }

        private enum DialogAction {
            Open,
            Close,
        }
    }
}