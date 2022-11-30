/**
 * \file SpawnPlayer.cs
 * \brief Script pour faire apparaître les joueurs
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 30/11/2022, modification: 30/11/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject Prefab;

    private void Start()
    {
        PhotonNetwork.Instantiate(Prefab.name, new Vector3(-12, 1, 0), new Quaternion(0, 0, 0, 0));
    }
}
