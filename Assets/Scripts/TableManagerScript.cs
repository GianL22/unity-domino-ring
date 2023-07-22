using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManagerScript : MonoBehaviour
{

    public SendDataScript sendDataScript;
    public PlayerScript playerScript;
    public DeckScript deckScript;
    public  DominoScript lastDominoScriptLeft = null;
    public DominoScript lastDominoScriptRight = null;
    public DominoScript dominoScriptCenter = null;

    private bool isCenterOccupied = false; 
    private readonly Vector2[] centerArea = { new Vector2(-3f, -3f), new Vector2(3f, 3f) };

    public bool IsPlayable(DominoScript dominoToPlay)
    {
        if (lastDominoScriptLeft == null && lastDominoScriptRight == null)
        {
            return true;
        } 
        return ((lastDominoScriptLeft.WhereIsInsertable(dominoToPlay) != null) || (lastDominoScriptRight.WhereIsInsertable(dominoToPlay) != null));
    }


    private void CorrectRotation(DominoScript dominoScript)
    {
        int[] values = dominoScript.GetValues();
        if (values[0] == values[1])
        {
            dominoScript.transform.Rotate(0, 0, -90f, Space.Self);
        }
    }


    public void DominoPlacedByOtherPlayer(int counter, int player, string tablePos, int index)
    {
        GameObject dominoToPlaceObject = deckScript.GetDominoByIndex(index);

        DominoScript dominoToPlace = dominoToPlaceObject.GetComponent<DominoScript>();
        
        if (!lastDominoScriptLeft && !lastDominoScriptRight)
        {
            dominoToPlace.transform.position = new Vector2(0, 0);
            isCenterOccupied = true;
            CorrectRotation(dominoToPlace);
            dominoToPlace.placed = true;
            lastDominoScriptRight = dominoToPlace;
            lastDominoScriptLeft = dominoToPlace;
            dominoScriptCenter = dominoToPlace;
            sendDataScript.SendDominoToPlace(counter + 1, player, tablePos, index);
            return; 
        } 

        DominoScript dominoOnTable;
        if (tablePos == "0")
        {
            tablePos = "L";
            dominoOnTable = lastDominoScriptLeft;
        }
        else
        {
            tablePos = "R";
            dominoOnTable = lastDominoScriptRight;
        }
        string dirDominoOnTable = dominoOnTable.WhereIsInsertable(dominoToPlace); //Esto puede dar error ojo
        this.DominoPlacedWihtOther(dominoOnTable, dominoToPlace, dirDominoOnTable, tablePos);
        dominoToPlace.placed = true;
        dominoOnTable.Insert(dominoToPlace);
        sendDataScript.SendDominoToPlace(counter + 1, player, tablePos, index);

    }

    public void DominoPlacedByMe(DominoScript dominoOnTable, DominoScript dominoToPlace, string dirDominoOnTable, string tablePos)
    {

        if ((dominoOnTable == dominoScriptCenter) && (dominoScriptCenter == lastDominoScriptLeft) && (dominoScriptCenter != lastDominoScriptRight))
        {
            tablePos = "L";
        }
        else if ((dominoOnTable == dominoScriptCenter) && (dominoScriptCenter != lastDominoScriptLeft) && (dominoScriptCenter == lastDominoScriptRight))
        {
            tablePos = "R";
        }
        this.DominoPlacedWihtOther(dominoOnTable, dominoToPlace, dirDominoOnTable, tablePos);
        sendDataScript.SendDominoToPlace(0, playerScript.GetId(), tablePos, dominoToPlace.GetId());
    }




    public void DominoPlacedWihtOther(DominoScript dominoOnTable, DominoScript dominoToPlace, string dirDominoOnTable, string tablePos)
    {
        string dirDominoToPlace = dominoToPlace.WhereIsInsertable(dominoOnTable);
        float offSetX = 1.6f;
        bool flipped = dominoOnTable.spriteRenderer.flipX;
        bool isDoble = (dominoOnTable.GetValues()[0] == dominoOnTable.GetValues()[1]);
        if ((lastDominoScriptLeft == lastDominoScriptRight) && isDoble)
        {
            dirDominoOnTable = tablePos;
        }
        if ((lastDominoScriptLeft == dominoScriptCenter) && (lastDominoScriptRight != dominoScriptCenter) && isDoble) //validación para un caso muy específico
        {
            dominoToPlace.spriteRenderer.flipX = dirDominoOnTable != dirDominoToPlace;
        }
        else if ((lastDominoScriptLeft != dominoScriptCenter) && (lastDominoScriptRight == dominoScriptCenter) && isDoble)
        {
            dominoToPlace.spriteRenderer.flipX = dirDominoOnTable == dirDominoToPlace;

        }
        else
        {
            dominoToPlace.spriteRenderer.flipX = !((flipped && (dirDominoOnTable == dirDominoToPlace)) ||
                                             (!flipped && (dirDominoOnTable != dirDominoToPlace)));
        }
        Debug.Log(dominoOnTable.GetId() + " " + dominoToPlace.GetId() + " " + dirDominoToPlace + " On table " + dirDominoOnTable+ " table pos " + tablePos);
        Debug.Log(dominoToPlace.spriteRenderer.flipX);
        CorrectRotation(dominoToPlace);
        
        if (tablePos == "R")
        {
            dominoToPlace.transform.position = new Vector3(dominoOnTable.transform.position.x + offSetX, dominoOnTable.transform.position.y, 0);
            lastDominoScriptRight = dominoToPlace;
        }
        else if (tablePos == "L") 
        {
            dominoToPlace.transform.position = new Vector3(dominoOnTable.transform.position.x - offSetX, dominoOnTable.transform.position.y, 0);
            lastDominoScriptLeft = dominoToPlace;
        }
    }

    public void DominoPlacedCenter(DominoScript dominoScript)
    {

        if (!isCenterOccupied && dominoScript.myCollider == Physics2D.OverlapArea(centerArea[0], centerArea[1]))
        {
            Debug.Log(dominoScript.GetId() + " se colocó en su pos inicial ");
            dominoScript.transform.position = new Vector2(0, 0);
            isCenterOccupied = true;
            CorrectRotation(dominoScript);
            dominoScript.placed = true;
            lastDominoScriptRight = dominoScript;
            lastDominoScriptLeft = dominoScript;
            dominoScriptCenter = dominoScript;
            sendDataScript.SendDominoToPlace(0, playerScript.GetId(), "L", dominoScript.GetId());
        }
        else
        {
            Debug.Log(dominoScript.GetId() + " se colocó en su pos inicial ");
            dominoScript.PlaceOriginalPosition();
        }
    }
    [ContextMenu("NewMatch")]
    public void OnNewMatch()
    {
        lastDominoScriptLeft = null;
        lastDominoScriptRight = null;
        dominoScriptCenter = null;
        isCenterOccupied = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        sendDataScript = GameObject.FindGameObjectWithTag("Sender").GetComponent<SendDataScript>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        deckScript = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();

    }


}
