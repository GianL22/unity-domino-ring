using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertScript : MonoBehaviour
{

    public static string DecimalToBin(int n, int length)
    {
        string res = "";
        if (n == 0)
        {
            res = "0";
        }
        else
        {
            while (n != 0)
            {
                res = (n % 2).ToString() + res;
                n /= 2;
            }
        }

        while (res.Length != length)
        {
            res = '0' + res;
        }

        return res;
    }


    public static int BinToDecimal(string bin)
    {
        int res = 0;
        int multi2 = 1;
        for (int i = bin.Length - 1; i > -1; i--)
        {
            if (bin[i] == '1')
                res += multi2; 
            multi2 *= 2;
        }
        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
