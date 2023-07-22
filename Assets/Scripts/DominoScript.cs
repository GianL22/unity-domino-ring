using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoScript : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Collider2D myCollider;
    private TableManagerScript tableManagerScript;

    public DominoScript domioScriptLeft = null;
    public DominoScript domioScriptRight = null;
    public DominoScript dominoCollisioned = null;
    public string dirCollisioned = null;

    private bool canMove;
    private bool dragging;
    private float originalX;
    private float originalY;

    public int id;
    public bool placed = false;
    public int rValue;
    public int lValue;
    public void SetValues(int l, int r)
    {
        this.rValue = r;
        this.lValue = l;
    }

    public int[] GetValues()
    {
        int[] values = { rValue, lValue };
        return values;
    }

    public void SetOriginalCoords(float x, float y)
    {
        this.originalX = x;
        this.originalY = y;
    }


    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return this.id;
    }

    public void SetSprite(Sprite spriteToUse)
    {
        spriteRenderer.sprite = spriteToUse;
    }

    public void SetColor(float[] rgb)
    {
        spriteRenderer.color = new Color(rgb[0], rgb[1], rgb[2]);
    }


    public bool HasValue(int value)
    {
        return (rValue == value) || (lValue == value);
    }


    public void PlaceOriginalPosition()
    {
        this.transform.position = new Vector2(this.originalX, this.originalY);
    }



    public string WhereIsInsertable(DominoScript dominoScript)
    {
        //Esto se podría mejorar con un hashmap y un enum de direcciones pero no hay tiempo xd
        if (domioScriptLeft == null)
        {
            if (dominoScript.HasValue(this.lValue))
            {
                return "L";
            }
        }

        if (domioScriptRight == null)
        {
            if (dominoScript.HasValue(this.rValue))
            {
                return "R";
            }
        }
        return null;
    }

    public void InsertInDirection(string dir, DominoScript domino)
    {
        if (dir == "L") //Igual aquí, esto es mejor hacerlo con un hashmap
        {
            this.domioScriptLeft = domino;
        }
        else if (dir == "R")
        {
            this.domioScriptRight = domino;
        }
    }

    public string Insert(DominoScript domino)
    {
        string dir = this.WhereIsInsertable(domino);
        if (dir == null) return null;
        this.InsertInDirection(dir, domino);
        string dir2 = domino.WhereIsInsertable(this);
        domino.InsertInDirection(dir2, this);
        return dir;
    }
    void Start()
    {
        this.tableManagerScript = GameObject.FindGameObjectWithTag("TableManager").GetComponent<TableManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && !placed)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
            originalX = gameObject.transform.position.x - 0.1f;
        }
        else if (Input.GetKey(KeyCode.D) && !placed)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);
            originalX = gameObject.transform.position.x + 0.1f;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (myCollider == Physics2D.OverlapPoint(mousePos))
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
            if (canMove)
            {
                dragging = true;
            }


        }

        

        if (dragging && !placed)
        {
            this.transform.position = mousePos;
        }
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            canMove = false;
            dragging = false;
            if (GameStateScript.isPlayable)
            {
                if (dominoCollisioned)
                {
                    string tablePos = "L";
                    if (mousePos.x > 0)
                    {
                        tablePos = "R";
                    }

                    tableManagerScript.DominoPlacedByMe(dominoCollisioned, this, dirCollisioned, tablePos);
                    placed = true;
                    dominoCollisioned.Insert(this);
                }
                else if (!placed)
                {

                    tableManagerScript.DominoPlacedCenter(this);
                }
            }
            else if (!placed) 
            {
                PlaceOriginalPosition();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (placed)
        {
            DominoScript dominoCollisionScript = collision.gameObject.GetComponent<DominoScript>();
            string dir = this.WhereIsInsertable(dominoCollisionScript);

            if (dir != null)
            {
                float[] rgb = { 0.32f, 1 , 0 };
                this.SetColor(rgb);
                dominoCollisionScript.dominoCollisioned = this;
                dominoCollisionScript.dirCollisioned = dir;
            }
            else
            {
                dominoCollisionScript.dominoCollisioned = null;
                dominoCollisionScript.dirCollisioned = null;
                float[] rgb = { 255f, 0, 72f };
                this.SetColor(rgb);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        float[] rgb = { 255f, 255f, 255f };
        this.SetColor(rgb);
        DominoScript dominoCollisionScript = collision.gameObject.GetComponent<DominoScript>();
        dominoCollisionScript.dominoCollisioned = null;
        dominoCollisionScript.dirCollisioned = null;

    }



}
