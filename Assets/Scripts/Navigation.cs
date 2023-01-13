/**
 * \file Navigation.cs
 * \brief Script pour les boutons des menus
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 29/11/2022, modification: 10/01/2023}
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
using System.IO;

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

    /** \brief Fonction en cas de victoire.
     * 
     */
    public void Gain()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        StreamReader sr = new StreamReader("difficulty.txt");
        System.String line = sr.ReadLine();
        sr.Close();
        int difficulty = int.Parse(line);
        MoneyManager script_money = manager.GetComponent("MoneyManager") as MoneyManager;
        script_money.AddMoney((int)Mathf.Pow(15, difficulty));
    }

    /** \brief Fonction en cas de reprise.
        * 
        */
    public void Next(bool vic)
    {
        if (vic)
        {
            Gain();
        }

        PhotonNetwork.LoadLevel("Projet");
    }

    /** \brief Fonction en cas de fin.
     * 
     */
    public void Stop(bool vic)
    {
        if (vic)
        {
            Gain();
        }

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}
