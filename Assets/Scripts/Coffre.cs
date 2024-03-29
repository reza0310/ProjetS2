/**
 * \file Coffre.cs
 * \brief Script pour permettre au coffre de suivre le joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 13/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coffre : MonoBehaviour
{
    /**
     * \cond
     */
    public GameObject cible;
    public bool porte;
    GameObject Porte;
    public int offset;
    GameObject manager;
    Message script_msg;
    /**
     * \endcond
     */

    public void Starte()
    {
        cible = null;
        porte = false;
        Porte = GameObject.FindGameObjectWithTag("Porte");
        manager = GameObject.FindGameObjectWithTag("Manager");
        script_msg = manager.GetComponent("Message") as Message;
    }

    // Update is called once per frame
    void Update()
    {
        if (cible != null)
        {
            transform.position = cible.transform.position + new Vector3(0, offset, 0);
        }
        Vector3 heading = Porte.transform.position - transform.position;
        float dist = Mathf.Sqrt(Mathf.Pow(heading.x, 2) + Mathf.Pow(heading.z, 2));
        if (dist < 4)
        {
            script_msg.sendWin();
        }
    }

    public void File(int id)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Pecho", RpcTarget.All, id);
    }

    [PunRPC]
    void Pecho(int invincible)
    {
        cible = PhotonView.Find(invincible).gameObject;
        porte = true;
    }
}
