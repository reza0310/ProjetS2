/**
 * \file LoadLevel.cs
 * \brief Script pour générer le niveau
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 13/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class LoadLevel : MonoBehaviourPun
{
    public GameObject Cofre;
    public GameObject Mur;
    public GameObject MurPavu;
    public GameObject Map;
    public GameObject Porte;
    public GameObject Prefab;
    public GameObject Garde;
    public GameObject Chemin;
    public float PROBA_CONTINUER = 1f;
    public float PROBA_TOURNER = 0.3f;
    Message messager;

    public void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        messager = manager.GetComponent("Message") as Message;
        if (PhotonNetwork.IsMasterClient)
        {
            StreamReader sr = new StreamReader("difficulty.txt");
            System.String line = sr.ReadLine();
            sr.Close();
            int difficulty = int.Parse(line);

            // ----- Gen laby -----
            int[] vals;
            switch (difficulty)
            {
                case 1:
                    vals = new int[4] { 20, 40, 5, 10 };
                    break;

                case 2:
                    vals = new int[4] { 40, 80, 10, 20 };
                    break;

                default:
                    vals = new int[4] { 80, 200, 20, 40 };
                    break;
            }

            int nbre_cases = Random.Range(vals[0], vals[1]);
            int nbre_ennemis = Random.Range(vals[2], vals[3]);
            messager.cases = nbre_cases;


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
            for (int x = 0; x < nbre_cases; x++)
            {
                tmp = new List<int>();
                for (int y = 0; y < nbre_cases; y++)
                {
                    tmp.Add(1);
                }
                laby.Add(tmp);
            }

            // Initialisation minage directionnel
            laby[laby.Count - 2][nbre_cases / 2] = 2;
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
                            if (y - 2 > 0 && laby[x - 1][y - 1] == 1 && laby[x + 1][y - 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x][y - 1] = 5;
                            }
                            if (y + 2 < nbre_cases && laby[x - 1][y + 1] == 1 && laby[x + 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
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
                            if (y - 2 > 0 && laby[x - 1][y - 1] == 1 && laby[x + 1][y - 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x][y - 1] = 5;
                            }
                            if (y + 2 < nbre_cases && laby[x - 1][y + 1] == 1 && laby[x + 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x][y + 1] = 4;
                            }
                        }

                        // Gestion Ouest
                        if (laby[x][y] == 5 && laby[x][y - 1] != 6)
                        {
                            // Avancée
                            if (y <= 2)
                            {
                                laby[x][y - 1] = 6;
                            }
                            else if (laby[x][y - 1] == 1 && Random.value < PROBA_CONTINUER)
                            {
                                laby[x][y - 1] = 5;
                            }
                            // Rotation
                            if (x - 2 > 0 && laby[x - 1][y - 1] == 1 && laby[x - 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x - 1][y] = 2;
                            }
                            if (x + 2 < nbre_cases && laby[x + 1][y - 1] == 1 && laby[x + 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x + 1][y] = 3;
                            }
                        }

                        // Gestion Est
                        if (laby[x][y] == 4 && laby[x][y + 1] != 6)
                        {
                            // Avancée
                            if (y + 2 > nbre_cases - 2)
                            {
                                laby[x][y + 1] = 6;
                            }
                            else if (laby[x][y + 1] == 1 && Random.value < PROBA_CONTINUER)
                            {
                                laby[x][y + 1] = 4;
                            }
                            // Rotation
                            if (x - 2 > 0 && laby[x - 1][y - 1] == 1 && laby[x - 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
                            {
                                laby[x - 1][y] = 2;
                            }
                            if (x + 2 < nbre_cases && laby[x + 1][y - 1] == 1 && laby[x + 1][y + 1] == 1 && Random.value <= PROBA_TOURNER)
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
            // Labyrinthe
            for (int x = 0; x < nbre_cases; x++)
            {
                for (int y = 0; y < nbre_cases; y++)
                {
                    if (laby[x][y] == 1)
                    {
                        temp = PhotonNetwork.Instantiate(Mur.name, new Vector3(x * 4, 2, y * 4), Quaternion.Euler(-90, 0, 0));
                        temp.transform.parent = Map.transform;
                    }
                }
            }
            // Extérieur
            for (int x = -10; x < nbre_cases + 11; x++)
            {
                for (int y = -10; y < nbre_cases + 11; y++)
                {
                    if ((x < 0 || x >= nbre_cases || y < 0 || y >= nbre_cases) && (y != nbre_cases / 2 || x < 0))
                    {
                        temp = Instantiate(MurPavu, new Vector3(x * 4, 2, y * 4), Quaternion.Euler(-90, 0, 0));
                        temp.transform.parent = Map.transform;
                    }
                }
            }

            // ----- Placer porte -----
            PhotonNetwork.Instantiate(Porte.name, new Vector3(nbre_cases * 4, 0, (nbre_cases / 2) * 4), Quaternion.Euler(0, 90, 0));

            // ----- Placer gardes -----
            int x_test;
            int y_test;
            int nopx;
            int nopy;
            int noppx;
            int noppy;
            bool place;
            GameObject MasterPath;
            for (int i = 0; i < nbre_ennemis; i++)
            {
                place = false;
                while (!place)
                {
                    x_test = Random.Range(0, nbre_cases - 1);
                    y_test = Random.Range(0, nbre_cases - 1);
                    if (laby[x_test][y_test] == 0)
                    {
                        place = true;
                        MasterPath = Instantiate(Chemin, new Vector3(0, 0, 0), Quaternion.identity);
                        // Point de départ
                        temp = Instantiate(Chemin, new Vector3(x_test * 4, 0, y_test * 4), Quaternion.identity);
                        temp.transform.parent = MasterPath.transform;
                        nopx = x_test;
                        nopy = y_test;
                        noppx = x_test;
                        noppy = y_test;
                        int longueur_ronde = Random.Range(vals[0], vals[1]) * 2;
                        for (int j = 0; j < longueur_ronde; j++)
                        {
                            int dir = Random.Range(1, 5);
                            bool good = false;
                            if (dir == 1 && x_test + 1 < laby.Count && laby[x_test + 1][y_test] == 0 && (x_test + 1 != nopx || y_test != nopy) && (x_test + 1 != noppx || y_test != noppy))
                            {
                                noppx = nopx;
                                nopx = x_test;
                                x_test += 1;
                                good = true;
                            }
                            else if (dir == 2 && y_test + 1 < laby[0].Count && laby[x_test][y_test + 1] == 0 && (x_test != nopx || y_test + 1 != nopy) && (x_test != noppx || y_test + 1 != noppy))
                            {
                                noppy = nopy;
                                nopy = y_test;
                                y_test += 1;
                                good = true;
                            }
                            else if (dir == 3 && x_test >= 1 && laby[x_test - 1][y_test] == 0 && (x_test - 1 != nopx || y_test != nopy) && (x_test - 1 != noppx || y_test != noppy))
                            {
                                noppx = nopx;
                                nopx = x_test;
                                x_test -= 1;
                                good = true;
                            }
                            else if (y_test >= 1 && laby[x_test][y_test - 1] == 0 && (x_test != nopx || y_test != nopy - 1) && (x_test != noppx || y_test != noppy - 1))
                            {
                                noppy = nopy;
                                nopy = y_test;
                                y_test -= 1;
                                good = true;
                            }
                            if (good)
                            {
                                // Placer point
                                temp = Instantiate(Chemin, new Vector3(x_test * 4, 0, y_test * 4), Quaternion.identity);
                                temp.transform.parent = MasterPath.transform;
                            }
                        }
                        // On accroche le garde
                        temp = PhotonNetwork.Instantiate(Garde.name, new Vector3(0, 0, 0), Quaternion.identity);
                        Garde script_garde = temp.GetComponent("Garde") as Garde;
                        script_garde.CONTIENT_CHEMIN = MasterPath.transform;
                    }
                }
            }

            // ----- Placer joueur -----
            Vector3 VEC = new Vector3((nbre_cases - 1) * 4, 1, (nbre_cases / 2) * 4);
            messager.spawn = VEC + new Vector3(0, 5, 0);
            GameObject MyPlayer = PhotonNetwork.Instantiate(Prefab.name, VEC, new Quaternion(0, 0, 0, 0));
            Joueur jscript = MyPlayer.GetComponent("Joueur") as Joueur;
            jscript.self = MyPlayer;

            GameObject came = GameObject.FindWithTag("MainCamera");
            CamScript script = came.GetComponent("CamScript") as CamScript;
            script.cible = MyPlayer;

            // ----- Placer coffre -----
            bool tresor = false;
            while (!tresor)
            {
                x_test = Random.Range(0, nbre_cases / 2);
                y_test = Random.Range(0, nbre_cases);
                if (laby[x_test][y_test] == 0)
                {
                    Cofre = GameObject.FindGameObjectWithTag("Finish");
                    Vector3 pos = new Vector3(x_test * 4, 1, y_test * 4);
                    Cofre.transform.position = pos;
                    Coffre script_coffre = Cofre.GetComponent("Coffre") as Coffre;
                    script_coffre.Starte();
                    tresor = true;
                }
            }
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Set", RpcTarget.OthersBuffered, nbre_cases, VEC);
        }
    }
}
