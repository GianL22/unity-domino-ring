using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{

    public SendDataScript sendDataScript;
    public DeckScript deckScript;
    public PlayerScript playerScript;
    public MatchManager matchManager;
    private List<GameObject> dominoesToAdd = new List<GameObject>();

    public Text status;
    public Text turnText;
    public Text winnerText;
    public Button buttonToPass;
    public GameObject buttonToPlay;


    static public bool isPlayable = false;
    public int nPlayers = 4; //public int nPlayers = 2;
    private int maxNDominoes = 7;
    public int id;
    public int team;
    public int currentPasts;
    public int lastTurnWasPass;
    public bool startPlay = false;

    public int GetNPlayers()
    {
        return nPlayers;
    }

    public int GetIdPlayer()
    {
        return this.playerScript.GetId();
    }


    public void DebugTextUpdate(string statusText)
    {
        status.text = statusText;
    }

    public void initPlay()
    {
        buttonToPlay.SetActive(false);
        playerScript.SetIdAndTeam(0, 0);
        sendDataScript.SendAssignPlayerAndTeam(0, 1, 1);
        DebugTextUpdate("Jugador: " + 0 + " Equipo: " + 0);
    }

    public void AssignPlayerAndTeam(int counter, int idToAssign, int teamToAssign)
    {
        buttonToPlay.SetActive(false);
        playerScript.SetIdAndTeam(idToAssign, teamToAssign);
        DebugTextUpdate("Jugador: " + idToAssign + " Equipo: " + teamToAssign);
        if (teamToAssign == 0) teamToAssign = 1;
        else teamToAssign = 0;
        int nextIdToAssign = idToAssign + 1;
        if (nextIdToAssign == nPlayers) nextIdToAssign = 0;
        sendDataScript.SendAssignPlayerAndTeam(counter + 1, nextIdToAssign, teamToAssign);
    }
    [ContextMenu("Crear mano")]

    public void DistributeMyDominos()
    {
        List<GameObject> dominoesToAdd = deckScript.GetRandomDominoes(maxNDominoes);
        playerScript.AddDominosToHand(dominoesToAdd);
        int id = deckScript.GetRandomDominoIndex();
        int nextPlayer = this.GetIdPlayer() + 1;
        if (nextPlayer > GetNPlayers() - 1) nextPlayer = 0;
        sendDataScript.SendDominoToPlayer(0, nextPlayer, id);
    }

    public void TurnValidation()
    {

        if (!startPlay && playerScript.GetHandNDominoes() == maxNDominoes)
        {
            Debug.Log("entro aqui");
            startPlay = true;
            if (playerScript.HasDoubleSix())
            {
                matchManager.idPlayerLastTurn = playerScript.GetId();
                Debug.Log("entro aqui 2");
                TurnOn(0, 0);
            }
        }


        //if (!startPlay && (otherHandScript.GetNDominoes() == maxNDominoes && playerScript.GetHandNDominoes() == maxNDominoes))
        //{
        //    startPlay = true;
        //    if (playerScript.HasDouble())
        //    {
        //        //Hacer que se juegue x ficha unicamente
        //        if (playerScript.GetSumDoubleMax() > otherHandScript.GetSumDoubleMax())
        //        {
        //            matchManager.idPlayerLastTurn = playerScript.GetId();
        //            TurnOn(0, 0);
        //        }
        //    }
        //    else if (!otherHandScript.HasDouble() && playerScript.id == 0)
        //    {
        //        TurnOn(0, 0);
        //    }
        //}
    }

    public void TurnOn(int currentPast, int wasPass)
    {
        this.lastTurnWasPass = wasPass;
        isPlayable = true;
        this.currentPasts = currentPast;
        buttonToPass.interactable = isPlayable && !playerScript.CanPlay();
        turnText.text = "Es tu turno";
        Debug.Log("[TurnOn] Se llamó");
    }

    public void TurnOff()
    {
        isPlayable = false;
        turnText.text = "No es tu turno";
        buttonToPass.interactable = false;
    }

    public void EmitTurn(int counter, int player)
    {
        sendDataScript.SendTurn(counter + 1, player, 0);
    }

    public void NextTurn(bool type)
    {
        if (type)
        {
            
            if (lastTurnWasPass == 1)
            {
                currentPasts++;
            }
            else
            {
                currentPasts = 0;
            }
            lastTurnWasPass = 1;
        }
        else
        {
            currentPasts = 0;
            lastTurnWasPass = 0;
        }

        TurnOff();
        int nextPlayer = playerScript.GetId() + 1;
        if (nextPlayer == nPlayers)
        {
            nextPlayer = 0;
        }

        sendDataScript.SendTurn(this.currentPasts, nextPlayer, this.lastTurnWasPass);
    }

    public void DistributeDomino(int counter, int idPlayer, int index)
    {

        Debug.Log("Hay disponibles " + deckScript.GetNDominoesAvaibles());

        if (counter < nPlayers - 1)
        {
            if (playerScript.id == idPlayer)
            {
                if (playerScript.GetHandNDominoes() < maxNDominoes)
                {
                    GameObject domino = deckScript.AddToHandDominoByIndex(index);
                    Debug.Log("He recibido  " + index);
                    //DebugTextUpdate("Recibido " + index);
                    sendDataScript.SendDominoToPlayer(counter + 1, idPlayer, index);
                    dominoesToAdd.Add(domino);
                }
                else
                {
                    Debug.Log("Estoy full" + index);
                    int nextPlayer = this.GetIdPlayer() + 1;
                    if (nextPlayer > GetNPlayers() - 1) nextPlayer = 0;
                    sendDataScript.SendDominoToPlayer(counter + 1, nextPlayer, index);
                    //playerScript.AddDominosToHand(dominoesToAdd);

                }

            }
            else
            {
                sendDataScript.SendDominoToPlayer(counter + 1, idPlayer, index);
            }
        }
        //else if (deckScript.GetNDominoesAvaibles() > 13)
        else if (deckScript.GetNDominoesAvaibles() > 0)
        {
            int newIndex = deckScript.GetRandomDominoIndex();

            Debug.Log("Voy a mandar " + newIndex);
            int nextPlayer = this.GetIdPlayer() + 1;
            if (nextPlayer > GetNPlayers() - 1) nextPlayer = 0;
            sendDataScript.SendDominoToPlayer(0, nextPlayer, newIndex);
        }

        //DebugTextUpdate(" counter: " + counter + " idPlayer: " + idPlayer + " index: " + index);
    }

    public void CountMyPoints(int counter, int team1pts, int team2pts)
    {
        int myPoints = this.playerScript.GetPoints();
        if (playerScript.team == 0)
        {
            sendDataScript.SendCountTeamPoints(counter + 1, team1pts + myPoints, team2pts);
        }
        else
        {
            sendDataScript.SendCountTeamPoints(counter + 1, team1pts, team2pts + myPoints);
        }

    }


    public void DecideWinnerTeam(int team1Pts, int team2Pts)
    {
        int winnerTeam;
        int winnerTeamPts;
        if (this.playerScript.HasEmptyHand())
        {
            winnerTeam = playerScript.team;
            if (winnerTeam == 0)
            {
                winnerTeamPts = team2Pts;
            }
            else
            {
                winnerTeamPts = team1Pts;
            }
        }
        else if (team1Pts > team2Pts)
        {
            winnerTeam = 1;
            winnerTeamPts = team1Pts;

        }
        else if (team1Pts < team2Pts)
        {
            winnerTeam = 0;
            winnerTeamPts = team2Pts;
        }
        else
        {
            winnerText.text = "Empate con " + team2Pts;
            return;
        }

        ShowWinner(-1, winnerTeam, winnerTeamPts);

    }

    public void ShowWinner(int counter, int team, int teamPoints)
    {
        winnerText.text = "Ganador equipo "+ team + " se le suma " + teamPoints;
        sendDataScript.SendMatchWinner(counter + 1, team, teamPoints);

    }

    public bool HasEmptyHand()
    {
        return this.playerScript.HasEmptyHand();
    }
    
    [ContextMenu("On New Match")]
    public void OnNewMatch()
    {
        //DebugTextUpdate("");
        winnerText.text = "";
        dominoesToAdd = new List<GameObject>();
        isPlayable = false;
        currentPasts = 0;
        lastTurnWasPass = 0;
        this.playerScript.OnNewMatch();
        this.deckScript.OnNewMatch();
    }

// Start is called before the first frame update
    void Start()
    {
        sendDataScript = GameObject.FindGameObjectWithTag("Sender").GetComponent<SendDataScript>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        deckScript = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        winnerText.text = "";

    }

    void Update()
    {

        TurnValidation();

        if (GameStateScript.isPlayable)
        {
            buttonToPass.interactable = !playerScript.CanPlay();
            turnText.text = "Es tu turno";
        }
        else
        {
            buttonToPass.interactable = false;
            turnText.text = "No es tu turno";
        }
    }


}
