/**
 * \file Joueur.cs
 * \brief Script du joueur
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 19/10/2022, modification: 19/10/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur : MonoBehaviour
{
    /**
     * \cond
     */
    public float VITESSE = 7;
    public float LISSEUR_MOUV = .1f;
    public float VITESSE_ROTATION = 8;

    float angle;
    float magnitude_lisse;
    float acceleration_lisse;
    Vector3 acceleration;

    Rigidbody corps;
    /**
     * \endcond
     */

    /** \brief Fonction **prédéfinie** exécutée à la première frame pour initialiser le corps de collision avec les obstacles.
     * 
     */
    public void Start()
    {
        corps = GetComponent<Rigidbody>();
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par frame permettant de lire les entrées et de les lisser.
     * 
     */
    public void Update()
    {
        Vector3 entree_direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float magnitude = entree_direction.magnitude;
        magnitude_lisse = Mathf.SmoothDamp(magnitude_lisse, magnitude, ref acceleration_lisse, LISSEUR_MOUV);

        float angle_cible = Mathf.Atan2(entree_direction.x, entree_direction.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, angle_cible, Time.deltaTime * VITESSE_ROTATION * magnitude);

        acceleration =  transform.forward * VITESSE * magnitude_lisse;
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par intervalle de temps fixe permettant d'appliquer les entrées lissées.
     * 
     */
    public void FixedUpdate()
    {
        corps.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        corps.MovePosition(corps.position + acceleration * Time.deltaTime);
    }
}
