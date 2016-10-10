using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPhoton {

    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : Photon.MonoBehaviour {

        [SerializeField]
        private float receiverLerpSpeed;

        [SerializeField]
        private float sourceTranslationSpeed;

        [SerializeField]
        private float sourceRotationSpeed;

        private Vector3 sourcePosition;
        private Quaternion sourceRotation;

        private void Start() {
            SetColor();
            CheckInit();
        }

        private void SetColor() {
            GetComponent<SpriteRenderer>().color =
                CharacterConfig.CharacterColors[GetOwnerColorIndex()];
        }

        private int GetOwnerColorIndex() {
            return GetSortedPlayerList().IndexOf(photonView.owner);
        }

        private List<PhotonPlayer> GetSortedPlayerList() {
            var sortedPlayerList = new List<PhotonPlayer>(PhotonNetwork.playerList);
            sortedPlayerList.Sort((playerA, playerB) => playerA.ID.CompareTo(playerB.ID));
            return sortedPlayerList;
        }

        private void CheckInit() {
            if (IsReceiver()) {
                InitAsReceiver();
            }
            else {
                InitAsSource();
            }
        }

        private bool IsReceiver() {
            return !photonView.isMine;
        }

        private void InitAsReceiver() {
            sourcePosition = transform.position;
            sourceRotation = transform.rotation;
            StartCoroutine(LerpRoutine());
        }

        private IEnumerator LerpRoutine() {
            while (true) {
                LerpToSource(Time.deltaTime * receiverLerpSpeed);
                yield return null;
            }
        }

        private void LerpToSource(float speed) {
            transform.position = LerpedPosition(speed);
            transform.rotation = LerpedRotation(speed);
        }

        private Vector3 LerpedPosition(float speed) {
            return Vector3.Lerp(transform.position, sourcePosition, speed);
        }

        private Quaternion LerpedRotation(float speed) {
            return Quaternion.Lerp(transform.rotation, sourceRotation, speed);
        }

        private void InitAsSource() {
            
            StartCoroutine(ControlsRoutine());
        }

        private IEnumerator ControlsRoutine() {
            while (true) {
                CheckControls();
                yield return null;
            }
        }

        private void CheckControls() {
            transform.Rotate(0f, 0f, GetRotation());
            transform.Translate(0f, GetTranslation(), 0f, Space.Self);
        }

        private float GetRotation() {
            return GetInputValue(KeyCode.A, KeyCode.D, sourceRotationSpeed);
        }

        private float GetTranslation() {
            return GetInputValue(KeyCode.W, KeyCode.S, sourceTranslationSpeed);
        }

        private float GetInputValue(KeyCode positiveKey, KeyCode negativeKey, float valueModifier) {
            var inputValue = 0f;

            if (Input.GetKey(positiveKey)) {
                inputValue += valueModifier;
            }
            else if (Input.GetKey(negativeKey)) {
                inputValue -= valueModifier;
            }

            return inputValue * Time.deltaTime;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo _) {
            if (stream.isWriting) {
                WriteToStream(stream);
            }
            else {
                ReadFromStream(stream);
            }
        }

        private void WriteToStream(PhotonStream stream) {
            // We own this player; send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        private void ReadFromStream(PhotonStream stream) {
            // Network player; receive data
            sourcePosition = (Vector3)stream.ReceiveNext();
            sourceRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}