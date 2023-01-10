using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public bool win;
    public bool lose;  // Facile: 1, moyen: 2, difficile: 3

    // Start is called before the first frame update
    void Start()
    {
        win = false;
        lose = false;
    }
}
