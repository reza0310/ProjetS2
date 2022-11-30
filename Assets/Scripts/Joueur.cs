/**
 * \file Joueur.cs
 * \brief Script du joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 19/10/2022, modification: 30/11/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Joueur : MonoBehaviour
{
    /**
     * \cond
     */
    public float VITESSE = 7;
    public float VITESSE_ROTATION = 8;
    public Camera camera;

    Rigidbody corps;
    PhotonView view;
    /**
     * \endcond
     */

    /** \brief Fonction **prédéfinie** exécutée à la première frame pour initialiser le corps de collision avec les obstacles.
     * 
     */
    public void Start()
    {
        corps = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        camera.enabled = true;
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par frame permettant de redresser les rotations sur les axes x et z.
    * 
    * 
    */
    public void Update()
    {
        if (view.IsMine)
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par intervalle de temps fixe permettant d'appliquer les entrées.
     * 
     */
    public void FixedUpdate()
    {
        if (view.IsMine)
        {
            transform.Rotate(transform.up * Input.GetAxis("Mouse X") * VITESSE_ROTATION * Time.deltaTime);
            corps.velocity = transform.right * Input.GetAxis("Horizontal") * VITESSE * Time.deltaTime;
            corps.velocity += transform.forward * Input.GetAxis("Vertical") * VITESSE * Time.deltaTime;
        }
    }
}
