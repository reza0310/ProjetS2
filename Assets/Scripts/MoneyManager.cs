/**
 * \file MoneyManager.cs
 * \brief Script pour gérer la monnaie
 * \author LabyStudio
 * \version 1.0
 * \date {creation: 10/01/2023, modification: 13/01/2023}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI MoneyText;
    public GameObject menu1;
    public GameObject menu2;
    public GameObject menu3;
    public GameObject menu4;
    public Sprite fin;
    public bool should;

    void Start()
    {
        StreamReader sr = new StreamReader("money.txt");
        System.String line = sr.ReadLine();
        sr.Close();
        money = int.Parse(line);
        MoneyText.SetText(line+" pièces");
    }

    public bool AddMoney(int x)
    {
        if (money + x < 0)
        {
            return false;
        }
        money += x;
        StreamWriter sw = new StreamWriter("money.txt");
        Debug.Log("Ajout de " + money.ToString() + " monnaie.");
        sw.Write(money.ToString());
        sw.Close();
        MoneyText.SetText(money.ToString() + " pièces");
        return true;
    }

    public void PayEas()
    {
        StreamWriter sw = new StreamWriter("difficulty.txt");
        sw.Write(1);
        sw.Close();
    }

    public void PayMoy()
    {
        if (AddMoney(-75))
        {
            StreamWriter sw = new StreamWriter("difficulty.txt");
            sw.Write(2);
            sw.Close();
        }
    }

    public void PayHar()
    {
        if (AddMoney(-2400))
        {
            StreamWriter sw = new StreamWriter("difficulty.txt");
            sw.Write(3);
            sw.Close();
        }
    }

    public void PayMV()
    {
        StreamWriter sw = new StreamWriter("weapon.txt");
        sw.Write(1);
        sw.Close();
    }

    public void PayD()
    {
        if (AddMoney(-3375))
        {
            StreamWriter sw = new StreamWriter("weapon.txt");
            sw.Write(2);
            sw.Close();
        }
    }

    public void PayR()
    {
        if (AddMoney(-33750))
        {
            StreamWriter sw = new StreamWriter("weapon.txt");
            sw.Write(3);
            sw.Close();
        }
    }

    public void PayMAX()
    {
        if (should && AddMoney(-100000))
        {
            menu1.GetComponent<Image>().sprite = fin;
            menu2.GetComponent<Image>().sprite = fin;
            menu3.GetComponent<Image>().sprite = fin;
            menu4.GetComponent<Image>().sprite = fin;
        }
    }
}
