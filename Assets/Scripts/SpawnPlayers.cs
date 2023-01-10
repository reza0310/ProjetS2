/**
 * \file SpawnPlayer.cs
 * \brief Script pour faire apparaître les joueurs et leur lier leur caméra
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 30/11/2022, modification: 10/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    /**
     * \cond
     */
    public GameObject Prefab;
    /**
     * \endcond
     */

    private void Start()
    {
        GameObject MyPlayer = PhotonNetwork.Instantiate(Prefab.name, new Vector3(-12, 1, 0), new Quaternion(0, 0, 0, 0));
        Joueur jscript = MyPlayer.GetComponent("Joueur") as Joueur;
        jscript.self = MyPlayer;

        GameObject came = GameObject.FindWithTag("MainCamera");
        CamScript script = came.GetComponent("CamScript") as CamScript;
        script.cible = MyPlayer;
    }
}
