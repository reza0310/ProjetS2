/**
 * \file Garde.cs
 * \brief Script des gardes du jeu
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 09/10/2022, modification: 09/10/2022}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garde : MonoBehaviour
{
    /**
     * \cond
     */
    public Transform CONTIENT_CHEMIN;
    public GameObject CORPS_GARDE;
    public float TEMPS_ATTENTE = .3f;
    public float VITESSE = 10;
    public float VITESSE_ROTATION = 90;
    /**
     * \endcond
     */

    /** \brief Fonction **prédéfinie** exécutée à la première frame pour initialiser et charger le chemin.
     * 
     */
    public void Start()
    {
        // Initialisation des variables
        int nbre_enfants = CONTIENT_CHEMIN.childCount;
        Vector3[] points = new Vector3[nbre_enfants * 2 - 2];

        // Remplissage du chemin (aller retour)
        for (int i = 0; i < nbre_enfants; i++)
        {
            points[i] = CONTIENT_CHEMIN.GetChild(i).position;
        }
        for (int i = 0; i < nbre_enfants - 2; i++)
        {
            points[nbre_enfants+i] = CONTIENT_CHEMIN.GetChild(nbre_enfants - 2 - i).position;
        }

        // Correction de la hauteur
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(points[i].x, transform.position.y, points[i].z);
        }

        // Mise en place du garde à son point de départ
        transform.position = points[0];
        transform.LookAt(points[1]);

        StartCoroutine(SuivreChemin(points));
    }

    /** \brief Fonction exécutée en permanence qui permet de suivre le chemin préchargé.
     * 
     * \param[in] points   Le chemin préchargé sous forme de liste de coordonnées tridimensionelles.
     * 
     */
    public IEnumerator SuivreChemin(Vector3[] points)
    {
        // Initialisation des variables
        int actuel = 1;
        Vector3 cible_actuelle = points[actuel];

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, cible_actuelle, VITESSE * Time.deltaTime);
            if (transform.position == cible_actuelle)
            {
                actuel = (actuel + 1) % points.Length;
                cible_actuelle = points[actuel];
                yield return new WaitForSeconds(TEMPS_ATTENTE);
                yield return StartCoroutine(RegarderVers(cible_actuelle));
            }
            yield return null;
        }
    }

    /** \brief Fonction exécutée quand un garde arrive sur un point pour pointer vers le suivant.
     * 
     * \param[in] cible   La cible vers laquelle regarder.
     * 
     */
    public IEnumerator RegarderVers(Vector3 cible)
    {
        Vector3 direction = (cible - transform.position).normalized;
        float angle_cible = 90 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, angle_cible)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle_cible, VITESSE_ROTATION * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    /** \brief Fonction **prédéfinie** permettant d'afficher dans l'éditeur le chemin du garde.
     * 
     */
    public void OnDrawGizmos()
    {
        Vector3 position_precedente = CONTIENT_CHEMIN.GetChild(0).position;
        foreach (Transform point in CONTIENT_CHEMIN)
        {
            Gizmos.DrawSphere(point.position, .3f);
            Gizmos.DrawLine(position_precedente, point.position);
            position_precedente = point.position;
        }
    }
}
