using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDataScript : MonoBehaviour
{

    public GameStateScript gameStateScript;
    public TableManagerScript tableManagerScript;
    public MatchManager matchManager;

    void ReceiveAssignPlayerAndTeam(string payload)
    {
        Debug.Log("[Player] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);

        //gameStateScript.DebugTextUpdate(payload);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            Debug.Log("Fin de las vueltas");
            gameStateScript.DistributeMyDominos();
            //gameStateScript.TurnOn(0, 0);
        }
        else
        {
            string playerString = payload.Substring(5, 2);
            int player = ConvertScript.BinToDecimal(playerString);
            Debug.Log("El player es " + player);
            string teamString = payload.Substring(7, 1);
            int team = ConvertScript.BinToDecimal(teamString);
            Debug.Log("El team es  " + team);
            gameStateScript.AssignPlayerAndTeam(counter, player, team);

        }

    }
    void ReceiveDominoToPlayer(string payload)
    {
        Debug.Log("[ToPlayer] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);
        string playerString = payload.Substring(5, 2);
        int player = ConvertScript.BinToDecimal(playerString);
        string indexString = payload.Substring(7, 5);
        int index = ConvertScript.BinToDecimal(indexString);

        Debug.Log("[ToPlayer] Recibido : player " + player + " index: " + index + " contador " + counter);
        gameStateScript.DistributeDomino(counter, player, index);
        

    }

    void ReceiveTurn(string payload)
    {
        Debug.Log("[Turn] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);
        string lastTurnWasPassString = payload.Substring(7, 1);
        int lastTurnWasPass = ConvertScript.BinToDecimal(lastTurnWasPassString);

        if (counter >= gameStateScript.GetNPlayers() - 1)
        {
            //Se tranco
            Debug.Log("Se tranco");
            gameStateScript.CountMyPoints(-1, 0, 0);
        }
        else
        {
            string playerString = payload.Substring(5, 2);
            int player = ConvertScript.BinToDecimal(playerString);
            if (gameStateScript.GetIdPlayer() == player)
            {
                gameStateScript.TurnOn(counter, lastTurnWasPass);
            }
            else
            {
                gameStateScript.EmitTurn(counter, player);

            }
        }
    }

    void ReceiveDominoToPlace(string payload)
    {
        Debug.Log("[ToPlace] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            if (gameStateScript.HasEmptyHand())
            {
                gameStateScript.CountMyPoints(-1, 0, 0);

            }
            else
            {
                gameStateScript.currentPasts = 0;
                gameStateScript.NextTurn(false);
            }
        }
        else
        {
 

            string playerString = payload.Substring(5, 2);
            int player = ConvertScript.BinToDecimal(playerString);
            string tablePosString = payload.Substring(7, 1);
            int tablePos = ConvertScript.BinToDecimal(tablePosString);
            string indexString = payload.Substring(8, 5);
            int index = ConvertScript.BinToDecimal(indexString);

            if (matchManager.idPlayerLastTurn == -1)
            {
                matchManager.idPlayerLastTurn = player;
            }


            Debug.Log("Recibido : player " + player + " contador " + counter + " tablePos " + tablePos + " index: " + index);
            tableManagerScript.DominoPlacedByOtherPlayer(counter, player, "" + tablePos, index);
        }



    }

    void ReceiveCountTeamPoints(string payload)
    {
        Debug.Log("[CountTeamPoints] Recibido : payload " + payload);
        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);
        string team1PtsString = payload.Substring(5, 7);
        int team1Pts = ConvertScript.BinToDecimal(team1PtsString);
        string team2PtsString = payload.Substring(12, 7);
        int team2Pts = ConvertScript.BinToDecimal(team2PtsString);
        Debug.Log("Recibido Convertido : player " + counter + " team1Pts " + team1Pts + " team2Pts " + team2Pts);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            gameStateScript.DecideWinnerTeam(team1Pts, team2Pts);
        }
        else
        {
            Debug.Log("Recibido : player " + counter + " team1Pts " + team1Pts + " team2Pts " + team2Pts);
            gameStateScript.CountMyPoints(counter, team1Pts, team2Pts);
        }
    }

    void ReceiveMatchWinner(string payload)
    {
        Debug.Log("[MatchWinner] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);
        string teamString = payload.Substring(5, 1);
        int team = ConvertScript.BinToDecimal(teamString);
        string teamPtsString = payload.Substring(6, 7);
        int teamPts = ConvertScript.BinToDecimal(teamPtsString);

        Debug.Log("[MatchWinner] Recibido Convertido : player " + counter + " teamPts " + teamPts);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            matchManager.AddPointsToWinnerTeam(team, teamPts);
            //gameStateScript.ShowWinner(team, teamPts);
            if (matchManager.IsGameFinished())
            {
                Debug.Log("[MatchWinner] Listo se ha ganado la partida, no hay más vueltas");
                matchManager.ShowWinnerGame(-1, team);
            }
            else
            {
                Debug.Log("[MatchWinner] Nuevo match");
                matchManager.OnNewMatch(-1);
            }
        }
        else
        {
            gameStateScript.ShowWinner(counter, team, teamPts);
            matchManager.AddPointsToWinnerTeam(team, teamPts);


        }
    }

    void ReceiveWinnerGame(string payload)
    {
        Debug.Log("[WinnerGame] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);
        string teamString = payload.Substring(5, 1);
        int team = ConvertScript.BinToDecimal(teamString);

        Debug.Log("[WinnerGame] Recibido Convertido : counter " + counter + " team " + team);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            Debug.Log("[WinnerGame] STOP");
        }
        else
        {
            matchManager.ShowWinnerGame(counter, team);
        }
    }

    void ReceiveReset(string payload)
    {
        Debug.Log("[Reset] Recibido : payload " + payload);

        string counterString = payload.Substring(3, 2);
        int counter = ConvertScript.BinToDecimal(counterString);

        Debug.Log("[Reset] Recibido Convertido : counter " + counter);

        if (counter == gameStateScript.GetNPlayers() - 1)
        {
            //gameStateScript.ShowWinner(team, teamPts);
            Debug.Log("[Reset] Se acabo el reset");
            gameStateScript.DistributeMyDominos();

            //if (gameStateScript.GetIdPlayer() == matchManager.idPlayerLastTurn)
            //{
            //    gameStateScript.DistributeMyDominos();
            //    //gameStateScript.TurnOn(0, 0);

            //}
            //else
            //{
            //    //gameStateScript.EmitTurn(-1, matchManager.idPlayerLastTurn);
            //}
        }
        else
        {
            matchManager.NewMatch(counter);
        }
    }

   



    void ReceiveData(string dataReceived)
    {

        string header = dataReceived.Substring(0, 3);

        switch (header)
        {
            case "000":
                ReceiveAssignPlayerAndTeam(dataReceived);
                break;
            case "001":
                ReceiveDominoToPlayer(dataReceived);
                break;
            case "010":
                ReceiveTurn(dataReceived);
                break;
            case "011":
                ReceiveDominoToPlace(dataReceived);
                break;
            case "100":
                ReceiveCountTeamPoints(dataReceived);
                break;
            case "101":
                ReceiveMatchWinner(dataReceived);
                break;
            case "110":
                ReceiveWinnerGame(dataReceived);
                break;
            case "111":
                ReceiveReset(dataReceived);
                break;
            default:
                break;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        SerialManagerScript.WhenReceiveDataCall += ReceiveData;
        gameStateScript = GameObject.FindGameObjectWithTag("State").GetComponent<GameStateScript>();
        tableManagerScript = GameObject.FindGameObjectWithTag("TableManager").GetComponent<TableManagerScript>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();


    }





}
