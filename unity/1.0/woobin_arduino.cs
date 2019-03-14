using UnityEngine;
using System.IO.Ports;


public class woobin_arduino : MonoBehaviour
{
    SerialPort Serial = new SerialPort("/dev/cu.usbmodem1411", 9600);
    public woobin_main _woobin_main;
    public int step1_data = 1000, step1_check, step3_data;
    public int delay = 0;
    public woobin_python _woobin_python;

    void Update()
    {
        if (!Serial.IsOpen) Serial.Open();
        //step1_start
        if (_woobin_main.step1_start)
        {
            delay = delay + 1;
            if (delay == 8)
            {
                Serial.Write("1");
                step1_data = int.Parse(Serial.ReadLine());
                if (step1_data < 5000) step1_check = step1_check + 1;
                else
                {
                    step1_check = 0;
                }
                delay = 0;
            }
        }
        if (step1_check > 25)
        {
            _woobin_main.step1_start = false;
            _woobin_main.step2_detect = true;
            _woobin_main.step3_check = false;
            _woobin_main.step4_end = false;
            _woobin_python.times = 0;
            step1_check = 0;
            step1_data = 1000;
        }
    }
}
