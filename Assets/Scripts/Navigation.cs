/**
 * \file Navigation.cs
 * \brief Script pour créer ou rejoindre une salle
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 29/11/2022, modification: 30/11/2022}
*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviourPunCallbacks
{
    /**
     * \cond
     */
    public GameObject texte;
    private readonly System.Random _random = new System.Random();
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

    /** \brief Fonction pour le bouton quitter.
     * 
     */
    public void Quit()
    {
        Application.Quit();
    }

    /** \brief Fonction copiée collée pour le bouton solo.
     * 
     */
    private string RandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);

        // Unicode/ASCII Letters are divided into two blocks
        // (Letters 65–90 / 97–122):
        // The first group containing the uppercase letters and
        // the second group containing the lowercase.  

        // char is a single Unicode character  
        char offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26; // A...Z or a..z: length=26  

        for (var i = 0; i < size; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }

    /** \brief Fonction pour le bouton solo.
     * 
     */
    public void Autocreate()
    {
        PhotonNetwork.CreateRoom(RandomString(20));
    }

    /** \brief Fonction en cas de victoire: fin.
     * 
     */
    public void Win_stop()
    {
        SceneManager.LoadScene("Lobby");
    }
}
