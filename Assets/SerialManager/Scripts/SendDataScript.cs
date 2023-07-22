using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDataScript : MonoBehaviour
{

    private int cont = 0;


    public void SendData()
    {
        SerialManagerScript.SendInfo("Recibiste " + cont++);
    }

    public void SendAssignPlayerAndTeam(int counter, int player, int team)
    {
        string header = "000";
        string counterString = ConvertScript.DecimalToBin(counter,2);
        string playerString  = ConvertScript.DecimalToBin(player,2);
        string teamString    = ConvertScript.DecimalToBin(team,1);

        string infoToSend = header + counterString + playerString + teamString;
        Debug.Log("enviado " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }

    public void SendDominoToPlayer(int counter, int player, int index)
    {
        string header = "001";
        Debug.Log("index original " + index);
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string playerString = ConvertScript.DecimalToBin(player, 2);
        string indexString = ConvertScript.DecimalToBin(index, 5);
        Debug.Log("index string " + indexString);

        string infoToSend = header + counterString + playerString + indexString;
        Debug.Log("enviado " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }

    public void SendTurn(int counter, int player, int lastTurnWasPass)
    {
        string header = "010";
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string playerString  = ConvertScript.DecimalToBin(player, 2);
        string lastTurnWasPassString = ConvertScript.DecimalToBin(lastTurnWasPass, 1);
        Debug.Log("lastTurnWasPassString: " + lastTurnWasPassString);
        string infoToSend    = header + counterString + playerString + lastTurnWasPassString;
        Debug.Log("[SendTurn] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);

    }

    public void SendDominoToPlace(int counter, int player, string tablePos, int index)
    {
        string header = "011";
        string tablePosBin = "0";
        if (tablePos == "R")
        {
            tablePosBin = "1";
        }
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string playerString = ConvertScript.DecimalToBin(player, 2);
        string indexString = ConvertScript.DecimalToBin(index, 5);
        string infoToSend = header + counterString + playerString + tablePosBin + indexString;
        Debug.Log("[SendToPlace] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);

    }


    public void SendCountTeamPoints(int counter, int team1Pts, int team2Pts)
    {
        string header = "100";
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string team1PtsString = ConvertScript.DecimalToBin(team1Pts, 7);
        string team2PtsString = ConvertScript.DecimalToBin(team2Pts, 7);

        string infoToSend = header + counterString + team1PtsString + team2PtsString;
        Debug.Log("[SendCountTeamPts] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }

    public void SendMatchWinner(int counter, int winnerTeam, int teamPts)
    {
        string header = "101";
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string winnerTeamString = ConvertScript.DecimalToBin(winnerTeam, 1);
        string teamPtsString = ConvertScript.DecimalToBin(teamPts, 7);

        string infoToSend = header + counterString + winnerTeamString + teamPtsString;
        Debug.Log("[SendMatchWinner] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }


    public void SendWinnerGame(int counter, int team)
    {
        string header = "110";
        string counterString = ConvertScript.DecimalToBin(counter, 2);
        string teamString = ConvertScript.DecimalToBin(team, 1);

        string infoToSend = header + counterString + teamString;
        Debug.Log("[SendWinnerGame] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }

    public void SendReset(int counter)
    {
        string header = "111";
        string counterString = ConvertScript.DecimalToBin(counter, 2);
    
        string infoToSend = header + counterString;
        Debug.Log("[SendReset] se envio la informacion " + infoToSend);
        SerialManagerScript.SendInfo(infoToSend);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


}
