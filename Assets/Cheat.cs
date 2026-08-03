using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cheat : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void OnSubmit(string cmd)
    {
        if (cmd.StartsWith("setdate"))
        {
            GameData.time = DateTime.Parse(cmd.Substring(8));
        }
        if (cmd.StartsWith("settimespeed"))
        {
            GameData.timeSpeed = TimeSpan.Parse(cmd.Substring(13));
        }
        if (cmd.StartsWith("test"))
        {
            player.Test();
        }
        if (cmd.StartsWith("test2"))
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    GameData.studyExp[j] = (int)(GameData.studyExp[j] * 0.8f);
                }
                for (int j = 0; j < 6; j++)
                {
                    GameData.studyExp[Random.Range(0, 5)] += Random.Range(1, 501);
                }
            }
        }
        if (cmd.StartsWith("giveexp"))
        {
            player.GiveExp(int.Parse(cmd.Substring(8)));
        }
        string[] para = cmd.Split(' ');
        if (para[0] == "setsujexp")
        {
            GameData.studyExp[int.Parse(para[1])] = int.Parse(para[2]);
        }
        if (para[0] == "getach")
        {
            player.GiveAch(int.Parse(para[1]));
        }
        if (para[0] == "reloadinventory")
        {
            player.updateInventory();
        }
        gameObject.SetActive(false);
    }
}
