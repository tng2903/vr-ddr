
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System;

public class SerialPad : MonoBehaviour
{
    public const int UpPort = 5;
    public const int DownPort = 4;
    public const int LeftPort = 3;
    public const int RightPort = 2;

    [NonSerialized]
    private SerialPort port;
    public string PortName = "COM4";
    public int BuadRate = 115200;

    // Use this for initialization
    void Awake()
    {
        print("Hi");
        port = new SerialPort(PortName, BuadRate);
        port.ReadTimeout = 1;
        port.Open();
    }

    const int NumPads = 6;
    [NonSerialized]
    public bool[] down = new bool[NumPads];
    [NonSerialized]
    public bool[] justPressedDown = new bool[NumPads];


    // Update is called once per frame
    // need to do this event based, but whatever.
    void Update()
    {
        #region Port Error States
        if (port == null)
        {
            print("null");
            return;
        }
        if (!port.IsOpen)
        {
            print("closed");
            return;
        }
        #endregion

        string newState = null;
        try
        {
            while (true)
            {
                newState = port.ReadLine();
            }
        }
        catch (TimeoutException e)
        {
            //meh
        }

        if (newState == null)
            return;

        for (int i = 1; i < newState.Length; i++)
        {
            var old = down[i - 1];
            var cur = newState[i] == '1';
            down[i - 1] = cur;
            justPressedDown[i - 1] = (cur != old) && cur;
        }
        print(newState);

    }

    private void OnDestroy()
    {
        print("cleanup port.");
        port.Close();
        port.Dispose();

    }
}
