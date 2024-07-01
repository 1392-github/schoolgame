using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuInput : MonoBehaviour
{
    public int row;
    public int column;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void change(string s)
    {
        if (int.TryParse(s, out int n))
        {
            if (n < 1 || n > 6)
            {
                return;
            }
            player.sudoku[row, column] = n;
        }
    }
}
