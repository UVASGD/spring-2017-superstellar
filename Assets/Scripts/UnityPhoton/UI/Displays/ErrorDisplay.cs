using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Extensions;
using UObject = UnityEngine.Object;

namespace UnityPhoton {

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class ErrorDisplay : MonoBehaviour {

        [SerializeField]
        private UObject errorMessagePrefab;

        private RectTransform rectTransform;
        private Image background;
        private List<string> currentErrors;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            background = GetComponent<Image>();
            currentErrors = new List<string>();
            SetVisibility();
        }

        public void SetErrors(IEnumerable<string> errors) {
            currentErrors.Clear();
            currentErrors.AddRange(errors);
            UpdateErrorMessages();
        }

        private void UpdateErrorMessages() {
            DestroyOldErrorMessages();
            currentErrors.ForEach(LoadErrorMessage);
            SetVisibility();
        }

        private void DestroyOldErrorMessages() {
            foreach (Transform errorMessage in rectTransform) {
                Destroy(errorMessage.gameObject);
            }
        }

        private void LoadErrorMessage(string error) {
            rectTransform.InstantiateChild<Text>(errorMessagePrefab).text = error;
        }

        private void SetVisibility() {
            background.enabled = currentErrors.Count > 0;
        }

        public void Clear() {
            currentErrors.Clear();
            UpdateErrorMessages();
        }
    }
}