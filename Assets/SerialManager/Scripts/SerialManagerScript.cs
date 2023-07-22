using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System;

public class SerialManagerScript : MonoBehaviour
{
    public String com; //1

    public delegate void SerialEvent(string dataReceived); //3
    public static event SerialEvent WhenReceiveDataCall; //3

    private bool abort; //1
    //private static SerialPort portSend;	//1
    //private static SerialPort portReceive;	//1
    private static SerialPort port;	//1

    private Thread serialThread;  //1
    private SynchronizationContext mainThread;  //6

    //private char incomingChar; //2
    //private string incomingString;    //2

    private string dataReceived = "";

    void OnEnable()
    {
        String[] avaiblePorts = SerialPort.GetPortNames();

        //portSend = new SerialPort("COM9", 9600, Parity.None, 8, StopBits.One);
        //portReceive = new SerialPort("COM8", 9600, Parity.None, 8, StopBits.One);


        //portSend = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        //portReceive = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);


        //portSend = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);
        //portReceive = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);


        //portSend = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);
        //portReceive = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);

        port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

        port.Open();//1
        port.DiscardOutBuffer(); //1
        port.DiscardInBuffer(); //1
        port.ReadTimeout = 300; //5

        //portSend.Open();//1
        //portSend.DiscardOutBuffer(); //1
        //portSend.DiscardInBuffer(); //1
        //portSend.ReadTimeout = 300; //5

        //portReceive.Open();//1
        //portReceive.DiscardOutBuffer(); //1
        //portReceive.DiscardInBuffer(); //1
        //portReceive.ReadTimeout = 300; //5

        mainThread = SynchronizationContext.Current;  //6

        if(mainThread == null)  //6
        {
            mainThread = new SynchronizationContext();  //6
        }

        serialThread = new Thread(Receive); //1

        //if (portSend.IsOpen && portReceive.IsOpen)	//1
        if (port.IsOpen)	//1
        {
            serialThread.Start();
        }
    }

    void Receive() //1
    {
        while (true)
        {
            if (abort)	//1
            {
                serialThread.Abort();
                break;
            }

            try //2
            {
                //dataReceived = portReceive.ReadLine();

                dataReceived = port.ReadLine();

            }

            catch (Exception e) 
            {
                //Debug.Log(e);
            } //2

            //if (!incomingChar.Equals('\n')) //2
            //{
            //    incomingString += incomingChar;
            //}
            if (dataReceived != "") //3
            {

                //todo esto se ejecuta en el hilo principal pero se llama desde el secundario//
                mainThread.Send((object state) => //6
                {
                    if (WhenReceiveDataCall != null)
                    {
                        WhenReceiveDataCall(dataReceived);
                    }                    
                }, null);
                ///////////////////////////////////////////////////////////////////////////////
                dataReceived = "";
            }
        }
    }

    public static void SendInfo(string infoToSend) //4
    {
        //portSend.WriteLine(infoToSend);
        //portSend.BaseStream.Flush();
        port.WriteLine(infoToSend);
        port.BaseStream.Flush();
    }

    void OnDestroy() //5
    {
        OnApplicationQuit(); //5
    }

    private void OnApplicationQuit() //1
    {
        try
        {
            abort = true;
            //portSend.DiscardOutBuffer();
            //portSend.DiscardInBuffer();
            //portSend.Close();

            port.DiscardOutBuffer();
            port.DiscardInBuffer();
            port.Close();

            //portReceive.DiscardOutBuffer();
            //portReceive.DiscardInBuffer();
            //portReceive.Close();

        }
        catch (Exception e) 
        {
            //Debug.Log(e);
        }        
    }
}
