/**
 * \file CamScript.cs
 * \brief Script pour permettre à la caméra de suivre le joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 09/01/2023, modification: 11/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    /**
     * \cond
     */
    public GameObject cible;
    public int offset;
    /**
     * \endcond
     */

    // Update is called once per frame
    void Update()
    {
        if (cible != null)
        {
            transform.position = cible.transform.position + new Vector3(0, offset, 0);
        }
    }
}
