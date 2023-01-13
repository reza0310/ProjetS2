using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Message : MonoBehaviourPun
{
    public bool win;
    public bool lose;  // Facile: 1, moyen: 2, difficile: 3
    public int cases;
    public Vector3 spawn;
    public GameObject MurPavu;
    public GameObject Map;
    public GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        win = false;
        lose = false;
    }

    public void sendWin()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Win", RpcTarget.All);
    }

    public void sendLose()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Lose", RpcTarget.All);
    }

    [PunRPC]
    void Set(int a, Vector3 b)
    {
        // Extérieur
        for (int x = -10; x < a + 11; x++)
        {
            for (int y = -10; y < a + 11; y++)
            {
                if ((x < 0 || x >= a || y < 0 || y >= a) && (y != a / 2 || x < 0))
                {
                    GameObject temp = Instantiate(MurPavu, new Vector3(x * 4, 2, y * 4), Quaternion.Euler(-90, 0, 0));
                    temp.transform.parent = Map.transform;
                }
            }
        }
        GameObject MyPlayer = PhotonNetwork.Instantiate(Prefab.name, b, new Quaternion(0, 0, 0, 0));
        Joueur jscript = MyPlayer.GetComponent("Joueur") as Joueur;
        jscript.self = MyPlayer;

        GameObject came = GameObject.FindWithTag("MainCamera");
        CamScript script = came.GetComponent("CamScript") as CamScript;
        script.cible = MyPlayer;
    }

    [PunRPC]
    void Win()
    {
        win = true;
    }

    [PunRPC]
    void Lose()
    {
        lose = true;
    }
}
