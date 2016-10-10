using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace UnityPhoton {
    public class LobbyController : PunBehaviour {
        private bool isJoiningRoom;

        private void Start() {
            isJoiningRoom = false;
        }

        public void FlagAsJoiningRoom() {
            isJoiningRoom = true;
        }

        public override void OnReceivedRoomListUpdate() {
            base.OnReceivedRoomListUpdate();
            Debug.Log("LobbyController.OnReceivedRoomListUpdate()");
        }

        public override void OnLobbyStatisticsUpdate() {
            base.OnLobbyStatisticsUpdate();
            Debug.Log("LobbyController.OnLobbyStatisticsUpdate()");
        }

        public override void OnLeftLobby() {
            base.OnLeftLobby();
            Debug.Log("LobbyController.OnLeftLobby()");
            CheckReturnToMainMenu();
        }

        private void CheckReturnToMainMenu() {
            if (!isJoiningRoom) {
                SceneManager.LoadScene("MainMenu");
            }
        }

        public override void OnCreatedRoom() {
            base.OnCreatedRoom();
            Debug.Log("LobbyController.OnCreatedRoom()");
        }

        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            Debug.Log("LobbyController.OnJoinedRoom()");
            SceneManager.LoadScene("Room");
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
            base.OnPhotonJoinRoomFailed(codeAndMsg);
            Debug.Log("LobbyController.OnPhotonJoinRoomFailed()");
            isJoiningRoom = false;
        }

        public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged) {
            base.OnPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
            Debug.Log("LobbyController.OnPhotonCustomRoomPropertiesChanged()");
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
            base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
            Debug.Log("LobbyController.OnPhotonPlayerPropertiesChanged()");
        }

        #region Unhandled events
        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            throw new NotImplementedException(GetType().ToString() + ".OnConnectedToMaster()");
        }

        public override void OnConnectedToPhoton() {
            base.OnConnectedToPhoton();
            throw new NotImplementedException(GetType().ToString() + ".OnConnectedToPhoton()");
        }

        public override void OnConnectionFail(DisconnectCause cause) {
            base.OnConnectionFail(cause);
            throw new NotImplementedException(GetType().ToString() + ".OnConnectionFail()");
        }

        //public override void OnCreatedRoom() {
        //    base.OnCreatedRoom();
        //    throw new NotImplementedException(GetType().ToString() + ".OnCreatedRoom()");
        //}

        public override void OnCustomAuthenticationFailed(string debugMessage) {
            base.OnCustomAuthenticationFailed(debugMessage);
            throw new NotImplementedException(GetType().ToString() + ".OnCustomAuthenticationFailed()");
        }

        //public override void OnCustomAuthenticationResponse(Dictionary<string, object> data) {
        //    base.OnCustomAuthenticationResponse(data);
        //    throw new NotImplementedException(GetType().ToString() + ".OnCustomAuthenticationResponse()");
        //}

        public override void OnDisconnectedFromPhoton() {
            base.OnDisconnectedFromPhoton();
            throw new NotImplementedException(GetType().ToString() + ".OnDisconnectedFromPhoton()");
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
            base.OnFailedToConnectToPhoton(cause);
            throw new NotImplementedException(GetType().ToString() + ".OnFailedToConnectToPhoton()");
        }

        //public override void OnJoinedRoom() {
        //    base.OnJoinedRoom();
        //    throw new NotImplementedException(GetType().ToString() + ".OnJoinedRoom()");
        //}

        public override void OnLeftRoom() {
            base.OnLeftRoom();
            throw new NotImplementedException(GetType().ToString() + ".OnLeftRoom()");
        }

        public override void OnJoinedLobby() {
            base.OnJoinedLobby();
            throw new NotImplementedException(GetType().ToString() + ".OnJoinedLobby()");
        }

        //public override void OnLeftLobby() {
        //    base.OnLeftLobby();
        //    throw new NotImplementedException(GetType().ToString() + ".OnLeftLobby()");
        //}

        //public override void OnLobbyStatisticsUpdate() {
        //    base.OnLobbyStatisticsUpdate();
        //    throw new NotImplementedException(GetType().ToString() + ".OnLobbyStatisticsUpdate()");
        //}

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
            base.OnMasterClientSwitched(newMasterClient);
            throw new NotImplementedException(GetType().ToString() + ".OnMasterClientSwitched()");
        }

        public override void OnOwnershipRequest(object[] viewAndPlayer) {
            base.OnOwnershipRequest(viewAndPlayer);
            throw new NotImplementedException(GetType().ToString() + ".OnOwnershipRequest()");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
            base.OnPhotonCreateRoomFailed(codeAndMsg);
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonCreateRoomFailed()");
        }

        //public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged) {
        //    base.OnPhotonCustomRoomPropertiesChanged(propertiesThatChanged);
        //    throw new NotImplementedException(GetType().ToString() + ".OnPhotonCustomRoomPropertiesChanged()");
        //}

        public override void OnPhotonInstantiate(PhotonMessageInfo info) {
            base.OnPhotonInstantiate(info);
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonInstantiate()");
        }

        //public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        //    base.OnPhotonJoinRoomFailed(codeAndMsg);
        //    throw new NotImplementedException(GetType().ToString() + ".OnPhotonJoinRoomFailed()");
        //}

        public override void OnPhotonMaxCccuReached() {
            base.OnPhotonMaxCccuReached();
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonMaxCccuReached()");
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
            base.OnPhotonPlayerConnected(newPlayer);
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonPlayerConnected()");
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
            base.OnPhotonPlayerDisconnected(otherPlayer);
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonPlayerDisconnected()");
        }

        //public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
        //    base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        //    throw new NotImplementedException(GetType().ToString() + ".OnPhotonPlayerPropertiesChanged()");
        //}

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
            base.OnPhotonRandomJoinFailed(codeAndMsg);
            throw new NotImplementedException(GetType().ToString() + ".OnPhotonRandomJoinFailed()");
        }

        //public override void OnReceivedRoomListUpdate() {
        //    base.OnReceivedRoomListUpdate();
        //    throw new NotImplementedException(GetType().ToString() + ".OnReceivedRoomListUpdate()");
        //}

        public override void OnUpdatedFriendList() {
            base.OnUpdatedFriendList();
            throw new NotImplementedException(GetType().ToString() + ".OnUpdatedFriendList()");
        }

        public override void OnWebRpcResponse(OperationResponse response) {
            base.OnWebRpcResponse(response);
            throw new NotImplementedException(GetType().ToString() + ".OnWebRpcResponse()");
        }
        #endregion Unhandled events
    }
}