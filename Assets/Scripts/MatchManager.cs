using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{

    public SendDataScript sendDataScript;
    public TableManagerScript tableManagerScript;
    public GameStateScript gameStateScript;

    public Text winnerText;

    public int team1Pts;
    public int team2Pts;
    public int idPlayerLastTurn = -1;
    private int pointsToWin = 100;
    public bool finished = false;
    public float delay = 7f;
    public float timer;

    private int counterReceived;
    public void AddPointsToWinnerTeam(int team, int points)
    {
        if (team == 0)
        {
            team1Pts += points;
        }
        else
        {
            team2Pts += points;
        }
        if (team1Pts >= pointsToWin || team2Pts >= pointsToWin)
        {
            winnerText.text = "Ha ganado el equipo " + team;
        }
    }

    public bool IsGameFinished()
    {
        return (team1Pts >= pointsToWin || team2Pts >= pointsToWin);
    }

    public void ShowWinnerGame(int counter, int team)
    {
        winnerText.text = "HA GANADO EL EQUIPO " + team;
        sendDataScript.SendWinnerGame(counter + 1, team);
    }

    [ContextMenu("NewMatch")]
    public void OnNewMatch(int counter)
    {
        finished = true;
        this.counterReceived = counter;
    }


    public void NewMatch(int counter)
    {
        Debug.Log("[MatchManager] vamos a reiniciar");
        tableManagerScript.OnNewMatch();
        gameStateScript.OnNewMatch();
        sendDataScript.SendReset(counter + 1);
        
        idPlayerLastTurn++;
        if (idPlayerLastTurn > gameStateScript.GetNPlayers() - 1)
        {
            idPlayerLastTurn = 0;
        }

        if (gameStateScript.GetIdPlayer() == idPlayerLastTurn)
        {
            gameStateScript.TurnOn(0,0);
        } 

        
    }

    // Start is called before the first frame update
    void Start()
    {
        sendDataScript = GameObject.FindGameObjectWithTag("Sender").GetComponent<SendDataScript>();
        gameStateScript = GameObject.FindGameObjectWithTag("State").GetComponent<GameStateScript>();
        tableManagerScript = GameObject.FindGameObjectWithTag("TableManager").GetComponent<TableManagerScript>();
    }

    void Update()
    {
        if (finished)
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                NewMatch(counterReceived);
                timer = 0f;
                finished = false;
            }
        }

        winnerText.text = "equipo 0 : " + team1Pts + " puntos"  + "\nequipo 1 : " + team2Pts + " puntos";

    }

}
