/**
 * \file Connecter.cs
 * \brief Script pour se connecter au serveur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 23/11/2022, modification: 23/11/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Connecter : MonoBehaviourPunCallbacks
{
    /** \brief Fonction prédéfinie permettant de se connecter au serveur.
     * 
     */
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /** \brief Fonction prédéfinie permettant de rejoindre un lobby.
     * 
     */
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    /** \brief Fonction prédéfinie permettant de passer sur la scène "lobby".
     * 
     */
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
