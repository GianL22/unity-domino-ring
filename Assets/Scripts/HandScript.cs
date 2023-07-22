using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public GameObject dominoPrefab;
    public TableManagerScript tableManagerScript;
    public List<GameObject> dominoes = new List<GameObject>();
    private int nDominoes = 0;

    public void AddDominos(List<GameObject> dominoesToAdd)
    {
        foreach (GameObject dominoToAdd  in dominoesToAdd)
        {
            AddDomino(dominoToAdd);
        }
    }

    public void AddDomino(GameObject domino)
    {
        float offsetX = 2f;
        float offsetY = 1f;
        Vector3 dominoPosition;
        if (nDominoes != 7 && nDominoes != 0)
        {
            GameObject lastDomino = this.dominoes[dominoes.Count - 1];
            if (nDominoes < 7)
                dominoPosition = new Vector3(lastDomino.transform.position.x + offsetX, lastDomino.transform.position.y, 0);
            else
            {
                dominoPosition = new Vector3(lastDomino.transform.position.x + offsetX, transform.position.y - offsetY, 0);
            }

        }
        else
        {
            if (nDominoes < 7) dominoPosition = new Vector3(transform.position.x, transform.position.y, 0);
            else dominoPosition = new Vector3(transform.position.x, transform.position.y - offsetY, 0);
        }

        domino.GetComponent<DominoScript>().SetOriginalCoords(dominoPosition.x, dominoPosition.y);
        //Debug.Log(dominoPosition.x + " " + dominoPosition.y);
        domino.transform.position = dominoPosition;

        this.dominoes.Add(domino);
        this.nDominoes =  dominoes.Count;
    }
    [ContextMenu("Validar Trancada")]

    public bool CanPlay()
    {
        bool res = false;
        foreach (GameObject domino in dominoes)
        {
            DominoScript dominoScript = domino.GetComponent<DominoScript>();
            if (dominoScript.placed) continue;

            res = tableManagerScript.IsPlayable(dominoScript);
            if (res)
            {
                return true;
            }
        }
        return false;
    }

    public int GetNDominoes()
    {
        return this.nDominoes;
    }

    public int GetPoints()
    {
        int sum = 0;
        foreach (GameObject domino in dominoes)
        {
            DominoScript dominoScript = domino.GetComponent<DominoScript>();

            if (!dominoScript.placed)
            {
                sum += dominoScript.GetValues()[0] + dominoScript.GetValues()[1];
            }
        }
        return sum;
    }

    public bool IsEmpty()
    {
        foreach (GameObject domino in dominoes)
        {
            DominoScript dominoScript = domino.GetComponent<DominoScript>();

            if (!dominoScript.placed)
            {
                return false;
            }
        }
        return true;
    }
    public int GetSumDoubleMax()
    {
        int maxSum = 0;
        foreach (GameObject domino in dominoes)
        {
            int[] values = domino.GetComponent<DominoScript>().GetValues();
            if ((values[0] == values[1]) && (maxSum < values[0] + values[1]))
            {
                maxSum = values[0] + values[1];
            }
        }
        return maxSum;
    }

    public bool HasDoubleSix()
    {
        foreach (GameObject domino in dominoes)
        {
            int[] values = domino.GetComponent<DominoScript>().GetValues();
            if ((values[0] == 6) && (values[1] == 6))
            {
                return true;
            }
        }
        return false;
    }


    private void RemoveMyHand()
    {
        this.dominoes = new List<GameObject>();
        this.nDominoes = 0;
    }
    [ContextMenu("NewMatch")]

    public void OnNewMatch()
    {
        this.RemoveMyHand();
    }

    // Start is called before the first frame update
    void Start()
    {
        tableManagerScript = GameObject.FindGameObjectWithTag("TableManager").GetComponent<TableManagerScript>();

    }

}
