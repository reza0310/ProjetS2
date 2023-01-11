/**
 * \file LoadLevel.cs
 * \brief Script pour générer le niveau
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 11/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class LoadLevel : MonoBehaviour
{
    public GameObject Cofre;
    public GameObject Mur;
    public GameObject Map;
    public GameObject Porte;
    public GameObject Prefab;
    public float PROBA_CONTINUER = 1f;
    public float PROBA_TOURNER = 0.3f;

    public void Start()
    {
        StreamReader sr = new StreamReader("Assets/difficulty.txt");
        System.String line = sr.ReadLine();
        sr.Close();
        int difficulty = int.Parse(line);

        // ----- Gen laby -----
        int[] vals;
        switch (difficulty) {
            case 1:
                vals = new int[4] { 10, 20, 5, 15 };
                break;

            case 2:
                vals = new int[4] { 20, 30, 15, 30 };
                break;

            default:
                vals = new int[4] { 30, 50, 30, 50 };
                break;
        }

        int nbre_cases = Random.Range(vals[0], vals[1]);
        int nbre_ennemis = Random.Range(vals[2], vals[3]);

        // Valeurs:
        // 0 => vide
        // 1 => plein
        // 2 => Nord
        // 3 => Sud
        // 4 => Est
        // 5 => Ouest
        // 6 => X

        // Génération labyrinthe plein (que des murs)
        List<List<int>> laby = new List<List<int>>();
        List<int> tmp;
        for (int x = 0; x < nbre_cases; x++) {
            tmp = new List<int>();
            for (int y = 0; y < nbre_cases; y++)
            {
                tmp.Add(1);
            }
            laby.Add(tmp);
        }

        // Initialisation minage directionnel
        laby[laby.Count-2][nbre_cases / 2] = 2;
        bool chemins = true;

        // Minage des chemins
        while (chemins)
        {
            chemins = false;
            for (int x = 0; x < nbre_cases; x++)
            {
                for (int y = 0; y < nbre_cases; y++)
                {
                    // Gestion Nord
                    if (laby[x][y] == 2 && laby[x - 1][y] != 6)
                    {
                        // Avancée
                        if (x <= 2)
                        {
                            laby[x - 1][y] = 6;
                        }
                        else if (laby[x - 1][y] == 1 && Random.value < PROBA_CONTINUER)
                        {
                            laby[x - 1][y] = 2;
                        }
                        // Rotation
                        if (y - 2 > 0 && laby[x - 1][y-1] == 1 && laby[x + 1][y-1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x][y - 1] = 5;
                        }
                        if (y + 2 < nbre_cases && laby[x - 1][y+1] == 1 && laby[x + 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x][y + 1] = 4;
                        }
                    }

                    // Gestion Sud
                    else if (laby[x][y] == 3 && laby[x + 1][y] != 6)
                    {
                        // Avancée
                        if (x + 2 > nbre_cases - 2)
                        {
                            laby[x + 1][y] = 6;
                        }
                        else if (laby[x + 1][y] == 1 && Random.value < PROBA_CONTINUER)
                        {
                            laby[x + 1][y] = 3;
                        }
                        // Rotation
                        if (y - 2 > 0 && laby[x - 1][y-1] == 1 && laby[x + 1][y-1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x][y - 1] = 5;
                        }
                        if (y + 2 < nbre_cases && laby[x - 1][y+1] == 1 && laby[x + 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x][y + 1] = 4;
                        }
                    }

                    // Gestion Ouest
                    if (laby[x][y] == 5 && laby[x][y-1] != 6)
                    {
                        // Avancée
                        if (y <= 2)
                        {
                            laby[x][y - 1] = 6;
                        }
                        else if (laby[x][y-1] == 1 && Random.value < PROBA_CONTINUER)
                        {
                            laby[x][y - 1] = 5;
                        }
                        // Rotation
                        if (x - 2 > 0 && laby[x - 1][y-1] == 1 && laby[x - 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x - 1][y] = 2;
                        }
                        if (x + 2 < nbre_cases && laby[x + 1][y-1] == 1 && laby[x + 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x + 1][y] = 3;
                        }
                    }

                    // Gestion Est
                    if (laby[x][y] == 4 && laby[x][y+1] != 6)
                    {
                        // Avancée
                        if (y + 2 > nbre_cases - 2)
                        {
                            laby[x][y + 1] = 6;
                        }
                        else if (laby[x][y+1] == 1 && Random.value < PROBA_CONTINUER)
                        {
                            laby[x][y + 1] = 4;
                        }
                        // Rotation
                        if (x - 2 > 0 && laby[x - 1][y-1] == 1 && laby[x - 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x - 1][y] = 2;
                        }
                        if (x + 2 < nbre_cases && laby[x + 1][y-1] == 1 && laby[x + 1][y+1] == 1 && Random.value <= PROBA_TOURNER)
                        {
                            laby[x + 1][y] = 3;
                        }
                    }



                    // Fin de minage
                    if (laby[x][y] == 2 || laby[x][y] == 3 || laby[x][y] == 4 || laby[x][y] == 5)
                    {
                        laby[x][y] = 0;
                        chemins = true;
                    }
                }
            }
        }

        // Nettoyage
        laby[nbre_cases - 1][nbre_cases / 2] = 0;
        for (int x = 0; x < nbre_cases; x++)
        {
            for (int y = 0; y < nbre_cases; y++)
            {
                if (laby[x][y] == 6)
                {
                    laby[x][y] = 1;
                }
                else if (laby[x][y] == 2 || laby[x][y] == 3 || laby[x][y] == 4 || laby[x][y] == 5)
                {
                    laby[x][y] = 0;
                }
            }
        }

        // ----- Placer blocs -----
        GameObject temp;
        for (int x = 0; x < nbre_cases; x++)
        {
            for (int y = 0; y < nbre_cases; y++)
            {
                if (laby[x][y] == 1)
                {
                    temp = PhotonNetwork.Instantiate(Mur.name, new Vector3(x*4, 2, y*4), new Quaternion(0, 0, 0, 0));
                    temp.transform.parent = Map.transform;
                }
            }
        }

        // ----- Placer porte -----
        temp = PhotonNetwork.Instantiate(Porte.name, new Vector3(nbre_cases*4, -2, (nbre_cases/2)*4), new Quaternion(0, 0, 0, 0));
        temp.transform.parent = Map.transform;

        // ----- Placer gardes -----
        // ----- Placer joueur -----
        GameObject MyPlayer = PhotonNetwork.Instantiate(Prefab.name, new Vector3((nbre_cases-1)*4, 1, (nbre_cases/2)*4), new Quaternion(0, 0, 0, 0));
        Joueur jscript = MyPlayer.GetComponent("Joueur") as Joueur;
        jscript.self = MyPlayer;

        GameObject came = GameObject.FindWithTag("MainCamera");
        CamScript script = came.GetComponent("CamScript") as CamScript;
        script.cible = MyPlayer;

        // ----- Placer coffre -----
        bool tresor = false;
        while (!tresor)
        {
            int x_test = (int)Random.Range(0f, nbre_cases / 2);
            int y_test = (int)Random.Range(0f, nbre_cases - 1f);
            if (laby[x_test][y_test] == 0)
            {
                Cofre = GameObject.FindGameObjectWithTag("Finish");
                Vector3 pos = new Vector3(x_test*4, 1, y_test*4);
                Cofre.transform.position = pos;
                Coffre script_coffre = Cofre.GetComponent("Coffre") as Coffre;
                script_coffre.Starte();
                tresor = true;
            }
        }
    }
}
