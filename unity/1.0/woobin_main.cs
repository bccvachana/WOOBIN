using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO.Ports;

public class woobin_main : MonoBehaviour
{
    private VideoPlayer video;
    public VideoClip[] woobin_video;
    SerialPort Serial = new SerialPort("/dev/cu.usbmodem1411", 9600);
    public bool step1_start = false, step2_detect = false, step3_check = false,
    step4_end = false;
    public float start_timer = 3.0f;
    public string round = "0000000000";
    public int pause = 0, arduino_light = 0;
    public woobin_python _woobin_python;
    public int result = 0, delay = 0, step3_round = 0;
    public int step3_data = 11111, correct = 11111;

    void Awake() { video = GetComponent<VideoPlayer>(); }

    void Start() { play(0); }

    void Update()
    {
        if (start_timer > 0)
        {
            start_timer -= Time.deltaTime;
            if (start_timer < 0) step1_start = true;
        }

        //step1_start
        if (step1_start) { video.loopPointReached += play0; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //step2_detect//

        if (step2_detect)
        {
            if (!Serial.IsOpen) Serial.Open();

            else if (round == "0000000000" & pause == 0) { video.loopPointReached += round0; }

            else if (round == "0000000000" & pause == 1)
            {
                if (arduino_light == 0) { Serial.Write("2"); arduino_light = 1; }
                play(1);
                video.loopPointReached += round1;
            }

            else if (round == "1000000000" & pause == 0) { round_pause(); }

            else if (round == "1000000000" & pause == 1)
            {
                if (arduino_light == 1) { Serial.Write("2"); arduino_light = 2; }
                play(1);
                video.loopPointReached += round2;
            }

            else if (round == "1100000000" & pause == 0) { round_pause(); }

            else if (round == "1100000000" & pause == 1)
            {
                if (arduino_light == 2) { Serial.Write("2"); arduino_light = 3; }
                play(1);
                video.loopPointReached += round3;
            }

            else if (round == "1110000000" & pause == 0) { round_pause(); }

            else if (round == "1110000000" & pause == 1)
            {
                if (arduino_light == 3) { Serial.Write("2"); arduino_light = 4; }
                play(1);
                video.loopPointReached += round4;
            }

            else if (round == "1111000000" & pause == 0) { round_pause(); }

            else if (round == "1111000000" & pause == 1)
            {
                if (arduino_light == 4) { Serial.Write("2"); arduino_light = 5; }
                play(1);
                video.loopPointReached += round5;
            }

            else if (round == "1111100000" & pause == 0) { round_pause(); }

            else if (round == "1111100000" & pause == 1)
            {
                if (arduino_light == 5) { Serial.Write("2"); arduino_light = 6; }
                play(1);
                video.loopPointReached += round6;
            }

            else if (round == "1111110000" & pause == 0) { round_pause(); }

            else if (round == "1111110000" & pause == 1)
            {
                if (arduino_light == 6) { Serial.Write("2"); arduino_light = 7; }
                play(1);
                video.loopPointReached += round7;
            }

            else if (round == "1111111000" & pause == 0) { round_pause(); }

            else if (round == "1111111000" & pause == 1)
            {
                if (arduino_light == 7) { Serial.Write("2"); arduino_light = 8; }
                play(1);
                video.loopPointReached += round8;
            }

            else if (round == "1111111100" & pause == 0)
            {
                step1_start = true;
                step2_detect = false;
                step3_check = false;
                step4_end = false; ;
                arduino_light = 0;
                video.loopPointReached += round_reset;
            }

            if (_woobin_python.times > 3)
            {
                step1_start = false;
                step2_detect = false;
                step3_check = true;
                step4_end = false; ;
                result = _woobin_python.detection;
                switch (result)
                {
                    case 1: video.loopPointReached += play3; break;
                    case 2: video.loopPointReached += play4; break;
                    case 3: video.loopPointReached += play5; break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //step3_check//

        if (step3_check)
        {
            delay = delay + 1;
            if (delay % 8 == 0) { Serial.Write("3"); step3_data = 10000 + int.Parse(Serial.ReadLine()); }
            if (delay == 120 && step3_round < 8) { Serial.Write((result + 4).ToString()); delay = 0; step3_round = step3_round + 1; }

            switch (result)
            {
                case 1: correct = 11011; break;
                case 2: correct = 11101; break;
                case 3: correct = 11110; break;
            }

            if (step3_data == 11111) step3_check = true;
            else if (step3_data == correct)
            {
                Serial.Write("4");
                step1_start = false;
                step2_detect = false;
                step3_check = false;
                step4_end = true; ;
                step3_round = 0; delay = 0;
                video.loopPointReached += play6;
            }
            else if (step3_data != 11111 && step3_data != correct)
            {
                Serial.Write("4");
                step1_start = false;
                step2_detect = false;
                step3_check = false;
                step4_end = true;
                step3_round = 0; delay = 0;
                video.loopPointReached += play7;
            }
            if (step3_round >= 8) video.loopPointReached += play8;
        }


        if (step4_end)
        {
            delay = delay + 1;
            if (delay == 120 && step3_round < 8) { Serial.Write("2"); delay = 0; step3_round = step3_round + 1; }
            if (step3_round >= 8)
            {
                step1_start = true;
                step2_detect = false;
                step3_check = false;
                step4_end = false;
                round = "0000000000";
                pause = 0;
                step3_round = 0; delay = 0;
            }
        }
    }

    void play(int i) { video.clip = woobin_video[i]; video.Play(); }

    //Round
    void round0 (VideoPlayer video)  { video.Pause(); pause = 1;}
    void round1 (VideoPlayer video)  { round = "1000000000"; pause = 0; }
    void round2 (VideoPlayer video)  { round = "1100000000"; pause = 0; }
    void round3 (VideoPlayer video)  { round = "1110000000"; pause = 0; }
    void round4 (VideoPlayer video)  { round = "1111000000"; pause = 0; }
    void round5 (VideoPlayer video)  { round = "1111100000"; pause = 0; }
    void round6 (VideoPlayer video)  { round = "1111110000"; pause = 0; }
    void round7 (VideoPlayer video)  { round = "1111111000"; pause = 0; }
    void round8 (VideoPlayer video)  { round = "1111111100"; pause = 0; }
    void round9 (VideoPlayer video)  { round = "1111111110"; pause = 0; }
    void round10(VideoPlayer video)  { round = "1111111111"; pause = 0; }
    void round_reset(VideoPlayer video) { round = "0000000000"; pause = 0;}
    void round_pause() { video.Pause(); pause = 1; }

    void play2(VideoPlayer video) { arduino_light = 0; step3_data = 11111; play(2); }
    void play3(VideoPlayer video) { arduino_light = 0; step3_data = 11111; play(3); }
    void play4(VideoPlayer video) { arduino_light = 0; step3_data = 11111; play(4); }
    void play5(VideoPlayer video) { arduino_light = 0; step3_data = 11111; play(5); }
    void play6(VideoPlayer video) { play(6); }
    void play7(VideoPlayer video) { play(7); }
    void play8(VideoPlayer video) { play(8); }
    void play0(VideoPlayer video) { play(0); }
}

