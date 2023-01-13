/**
 * \file Joueur.cs
 * \brief Script du joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 19/10/2022, modification: 13/01/2023}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class Joueur : MonoBehaviourPun
{
    /**
     * \cond
     */
    public float VITESSE;
    public float VITESSE_ROTATION;
    public int ARMEMENT;
    public GameObject self;

    AudioSource audioData;
    int direction = 0;
    Rigidbody corps;
    PhotonView view;
    GameObject manager;
    Message script_msg;
    GameObject coffre;
    Coffre script_coffre;
    bool bouge;
    Animator anim;
    double temps;
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
        audioData = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        manager = GameObject.FindGameObjectWithTag("Manager");
        script_msg = manager.GetComponent("Message") as Message;
        coffre = GameObject.FindGameObjectWithTag("Finish");
        script_coffre = coffre.GetComponent("Coffre") as Coffre;

        bouge = false;
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        temps = t.TotalSeconds;
        StreamReader sr = new StreamReader("weapon.txt");
        String line = sr.ReadLine();
        sr.Close();
        ARMEMENT = int.Parse(line);
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
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                Vector3 heading = coffre.transform.position - transform.position;
                float dist = Mathf.Sqrt(Mathf.Pow(heading.x, 2) + Mathf.Pow(heading.z, 2));
                if (dist < 5 && !script_coffre.porte)
                {
                    script_coffre.File(PhotonView.Get(this).ViewID);
                }
            }
            if (bouge)
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                if (t.TotalSeconds - temps > 1)
                {
                    anim.SetTrigger("Cours");
                    temps = t.TotalSeconds;
                }
                if (corps.velocity.AlmostEquals(new Vector3(0, 0, 0), 40f))
                {
                    audioData.Pause();
                    bouge = false;
                    anim.SetTrigger("Stop");
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
            if (!bouge && !corps.velocity.AlmostEquals(new Vector3(0, 0, 0), 40f))
            {
                bouge = true;
                audioData.Play();
                anim.SetTrigger("Cours");
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                temps = t.TotalSeconds;
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
