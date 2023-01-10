/**
 * \file LoadLevel.cs
 * \brief Script pour générer le niveau
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 10/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    public GameObject coffre;

    public void Load()
    {
        StreamReader sr = new StreamReader("Assets/difficulty.txt");
        System.String line = sr.ReadLine();
        sr.Close();
        int difficulty = int.Parse(line);
        // Gen laby
        // Placer coffre
        // Placer porte
        // Placer gardes
        // Placer joueur
        coffre = GameObject.FindGameObjectWithTag("Finish");
        Vector3 pos = new Vector3(20, 1, -12);
        coffre.transform.position = pos;
    }
}
