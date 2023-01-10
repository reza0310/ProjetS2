/**
 * \file MoneyManager.cs
 * \brief Script pour gérer la monnaie
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 10/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI MoneyText;

    void Start()
    {
        StreamReader sr = new StreamReader("Assets/money.txt");
        System.String line = sr.ReadLine();
        sr.Close();
        money = int.Parse(line);
        MoneyText.SetText(line);
    }

    public bool AddMoney(int x)
    {
        if (money+x < 0)
        {
            return false;
        }
        money += x;
        StreamWriter sw = new StreamWriter("Assets/money.txt");
        Debug.Log("Ajout de " + money.ToString() + " monnaie.");
        sw.Write(money.ToString());
        sw.Close();
        MoneyText.SetText(money.ToString());
        return true;
    }
}
