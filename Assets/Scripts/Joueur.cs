/**
 * \file Joueur.cs
 * \brief Script du joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 19/10/2022, modification: 10/01/2023}
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
    public float VITESSE;
    public float VITESSE_ROTATION;
    int direction = 0;

    Rigidbody corps;
    PhotonView view;
    public GameObject self;
    GameObject manager;
    Message script_msg;
    GameObject coffre;
    Coffre script_coffre;
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
        manager = GameObject.FindGameObjectWithTag("Manager");
        script_msg = manager.GetComponent("Message") as Message;
        coffre = GameObject.FindGameObjectWithTag("Finish");
        script_coffre = coffre.GetComponent("Coffre") as Coffre;
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par frame permettant de redresser les rotations sur les axes x et z.
    * 
    * 
    */
    public void Update()
    {
        if (view.IsMine)
        {
            if (script_msg.win)
            {
                PhotonNetwork.LoadLevel("VICTOIRE");
            }
            else if (script_msg.lose)
            {
                PhotonNetwork.LoadLevel("DEFAITE");
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
                Vector3 heading = coffre.transform.position - transform.position;
                float dist = Mathf.Sqrt(Mathf.Pow(heading.x, 2) + Mathf.Pow(heading.z, 2));
                if (dist < 2 && !script_coffre.porte)
                {
                    script_coffre.cible = self;
                    script_coffre.porte = true;
                }
            }
        }
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par intervalle de temps fixe permettant d'appliquer les entrées.
     * 
     */
    public void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (Input.GetKey("z"))
            {
                direction = 180;
                corps.velocity += new Vector3(0, 0, 1) * VITESSE * Time.deltaTime;
            }
            else if (Input.GetKey("s"))
            {
                direction = 0;
                corps.velocity += new Vector3(0, 0, -1) * VITESSE * Time.deltaTime;
            }
            else if (Input.GetKey("d"))
            {
                direction = 270;
                corps.velocity += new Vector3(1, 0, 0) * VITESSE * Time.deltaTime;
            }
            else if (Input.GetKey("q"))
            {
                direction = 90;
                corps.velocity += new Vector3(-1, 0, 0) * VITESSE * Time.deltaTime;
            }
            float angle_cible = direction;
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, angle_cible)) > 0.05f)
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle_cible, VITESSE_ROTATION * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
            }
        }
    }
}
