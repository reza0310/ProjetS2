/**
 * \file Garde.cs
 * \brief Script des gardes du jeu
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 09/10/2022, modification: 16/10/2022}
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

    public Light TORCHE;
    public float DISTANCE_VUE;
    public LayerMask MASQUE_VUE;

    float angle_vue;
    Transform joueur;
    Color couleur_origine_torche;
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

        angle_vue = TORCHE.spotAngle;
        joueur = GameObject.FindGameObjectWithTag("Player").transform;  // Existe une version au pluriel
        couleur_origine_torche = TORCHE.color;

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

    /** \brief Fonction disant si le garde voit le joueur.
     * 
     * \return vu   Le booléen disant si oui ou non le joueur est vu.
     * 
     */
    public bool voit_joueur()
    {
        if (Vector3.Distance(transform.position, joueur.position) < DISTANCE_VUE)
        {
            Vector3 direction_vers_joueur = (joueur.position - transform.position).normalized;
            float angle_garde_joueur = Vector3.Angle(transform.forward, direction_vers_joueur);
            if (angle_garde_joueur < angle_vue / 2f)
            {
                if (!Physics.Linecast(transform.position, joueur.position, MASQUE_VUE))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /** \brief Fonction **prédéfinie** exécutée une fois par frame permettant de checker si on voit le joueur.
     * 
     * 
     */
    public void Update()
    {
        if (voit_joueur())
        {
            TORCHE.color = Color.red;
        }
        else
        {
            TORCHE.color = couleur_origine_torche;
        }
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

    /** \brief Fonction **prédéfinie** permettant d'afficher dans l'éditeur seulement le chemin et le vue du garde.
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

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * DISTANCE_VUE);
    }
}
