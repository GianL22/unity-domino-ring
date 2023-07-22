using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public HandScript handScript;

    public int id;
    public int team;

    public void SetIdAndTeam( int id, int team )
    {
        this.id = id;
        this.team = team;
    }

    public int GetId()
    {
        return this.id;
    }

    public int GetHandNDominoes()
    {
        return this.handScript.GetNDominoes();
    }

    public void AddDominosToHand(List<GameObject> dominosToAdd)
    {
        foreach (GameObject dominoToAdd in dominosToAdd)
        {
            handScript.AddDomino(dominoToAdd);
        }
    }

    public void AddDominoToHand(GameObject domino)
    {
        handScript.AddDomino(domino);
    }

    public bool CanPlay()
    {
        return this.handScript.CanPlay();
    }

    public bool HasEmptyHand()
    {
        return handScript.IsEmpty();
    }

    public bool HasDoubleSix()
    {
        return handScript.HasDoubleSix();
    }

    public int GetPoints()
    {
        return this.handScript.GetPoints();
    }

    public void OnNewMatch()
    {
        this.handScript.OnNewMatch();
    }

    public int GetSumDoubleMax()
    {
        return handScript.GetSumDoubleMax();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handScript = GameObject.FindGameObjectWithTag("Hand").GetComponent<HandScript>();

    }
}
