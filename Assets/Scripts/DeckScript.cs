using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{

    public List<Sprite> dominoesSprites;
    public List<GameObject> dominoes = new List<GameObject>();
    public List<GameObject> dominoesOriginal = new List<GameObject>();
    public List<bool> dominoesUsed = new List<bool>();
    public GameObject dominoPreFab;
    public PlayerScript playerScript;

    [ContextMenu("Agregar Domino")]
    public void AddToHand()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int index = Random.Range(0, dominoes.Count);
        this.dominoes.RemoveAt(index);
        GameObject domino = dominoes[index];
        domino.SetActive(true);
        playerScript.AddDominoToHand(domino);
        dominoesUsed[index] = true;
    }

    public int GetRandomDominoIndex()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int index;
        Debug.Log("Dominos en el deck " + dominoes.Count);
        index = Random.Range(0, dominoes.Count);
        DominoScript domino = this.dominoes[index].GetComponent<DominoScript>();
        this.dominoes.Remove(this.dominoes[index]);
        dominoesUsed[index] = true;
        return domino.GetId();
    }
    public GameObject GetDominoByIndex(int index)
    {
        GameObject dominoFinded = null;
        foreach (GameObject domino in dominoesOriginal)
        {
            if (domino.GetComponent<DominoScript>().GetId() == index)
            {
                dominoFinded = domino;
                break;
            }
        }
        if (!dominoFinded)
        {
            Debug.Log(" no se encontró el domino con indice " + index);
            return null;
        } 
        dominoesUsed[index] = true;
        dominoFinded.SetActive(true);
        this.dominoes.Remove(dominoFinded);
        return dominoFinded;

    }

    public GameObject AddToHandDominoByIndex(int index)
    {
        GameObject domino = dominoes[index];
        domino.SetActive(true);
        dominoesUsed[index] = true;
        playerScript.AddDominoToHand(domino);
        return domino;
    }

    public int GetNDominoesAvaibles()
    {
        return this.dominoes.Count;
    }

    public List<GameObject> GetRandomDominoes(int n)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        List<GameObject> randomDominoes = new List<GameObject>();
        int index;
        for (int i = 0; i < n; i++)
        {
            index = Random.Range(0, dominoes.Count);
            GameObject domino = dominoes[index]; 
            domino.SetActive(true);
            dominoesUsed[index] = true;
            randomDominoes.Add(domino);
            dominoes.Remove(domino);
        }
        return randomDominoes;
    }

    //public void GetDomino(int index)
    //{

    //    if (dominoesUsed[index])
    //    {
    //        GameObject domino = dominoes[index];
    //        domino.SetActive(true);
    //        playerScript.AddDominoToHand(domino);
    //        dominoesUsed[index] = true;
    //    }
        
    //}

    private void RemoveAllDominoes()
    {
        foreach (GameObject domino in dominoesOriginal)
        {
            Destroy(domino);
        }
        dominoesOriginal = new List<GameObject>();
    }

    private void LoadDominoes()
    {

        int cont = 0;
        for (int l = 0; l < 7; l++)
        {
            for (int r = l; r < 7; r++)
            {
                GameObject domino = Instantiate(dominoPreFab, new Vector3(0, 0, 0), transform.rotation);
                DominoScript dominoScript = domino.GetComponent<DominoScript>();
                dominoes.Add(domino);
                dominoesOriginal.Add(domino);
                dominoesUsed.Add(false);
                dominoScript.SetValues(l, r);
                dominoScript.SetId(cont);
                dominoScript.SetSprite(dominoesSprites[cont]);
                domino.SetActive(false);
                cont++;
            }
        }
    }

    [ContextMenu("On New Match")]
    public void OnNewMatch()
    {
        dominoes = new List<GameObject>();
        RemoveAllDominoes();
        dominoesUsed = new List<bool>();
        this.LoadDominoes();
    }


    // Start is called before the first frame update
    void Start()
    {

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        this.LoadDominoes();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
