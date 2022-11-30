/**
 * \file CreerEtRejoindre.cs
 * \brief Script pour créer ou rejoindre une salle
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 30/11/2022, modification: 30/11/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreerEtRejoindre : MonoBehaviourPunCallbacks
{
    /**
     * \cond
     */
    public GameObject texte;
    PhotonView view;
    /**
     * \endcond
     */

    /** \brief Fonction prédéfinie permettant de créer une salle de jeu.
     * 
     * \param[in] texte Le nom de la salle.
     * 
     */
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(texte.GetComponent<TMP_InputField>().text);
        view = GetComponent<PhotonView>();
    }

    /** \brief Fonction prédéfinie permettant de rejoindre une salle de jeu.
     * 
     * \param[in] texte Le nom de la salle.
     * 
     */
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(texte.GetComponent<TMP_InputField>().text);
    }

    /** \brief Callback prédéfinie appelé quand on rejoint une salle. Permet de charger la map.
     * 
     */
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Projet");
    }
}
