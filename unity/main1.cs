using UnityEngine;
using UnityEngine.Video;
using System.IO.Ports;
using UnityEngine.UI;

public class main1 : MonoBehaviour
{
    private VideoPlayer video;
    public VideoClip[] woobin_animation;
    SerialPort Serial1 = new SerialPort("/dev/cu.usbmodem1421", 9600);
    public python _python;
    WebCamTexture cam;
    public RawImage display;
    public GameObject _cam, _box;

    public bool
        step1_start  = false,
        step2_detect = false,
        step3_bin    = false,
        step4_end    = false,
        step2_light  = false,
        step3_light  = false,
        step4_light  = false,
        step2_found  = false,
        step2_cam    = false,
        step2_reset  = false,
        step3_reset  = false,
        step3_out    = false,
        step3_throw  = false;

    public int
        delay           = 0,
        step1_data      = 5000,
        step1_check     = 0,
        step2_round     = 0,
        step2_data      = 0,
        step2_times     = 0,
        step3_round     = 0,
        step4_round     = 0,
        step3_data      = 11111,
        bin             = 0,
        correct         = 0,
        step4_animation = 0;



    void Awake() { video = GetComponent<VideoPlayer>(); }

    void Start()
    { 
        play(0);
        WebCamDevice[] devices = WebCamTexture.devices;
        cam = new WebCamTexture();
        cam.deviceName = devices[0].name;
        display.texture = cam;
        display.material.mainTexture = cam;
        _box.SetActive(false);
        _cam.SetActive(false);
    }

    void Update()
    {
        if (!Serial1.IsOpen) Serial1.Open();

        ////////////  Boot  ////////////

        if (!step1_start && !step2_detect && !step3_bin && !step4_end)
        {
            delay += 1;
            if (delay == 200) { step1_start = true; delay = 0; }
        }

        ////////////  Step 1 : Start  ////////////

        if (step1_start)
        {
            _box.SetActive(false);
            _cam.SetActive(false);

            video.loopPointReached -= step2_clear0;
            video.loopPointReached -= step2_clear1;
            video.loopPointReached -= step2_clear2;
            video.loopPointReached -= step2_clear3;
            video.loopPointReached -= step2_clear4;
            video.loopPointReached -= step2_clear5;
            video.loopPointReached -= step2_clear6;
            video.loopPointReached -= step2_clear7;
            video.loopPointReached -= step2_clear8;
            video.loopPointReached -= step2_f;

            video.loopPointReached -= step3_clear1;
            video.loopPointReached -= step3_clear2;
            video.loopPointReached -= step3_clear3;
            video.loopPointReached -= step3_clear4;
            video.loopPointReached -= step3_clear5;
            video.loopPointReached -= step3_clear6;
            video.loopPointReached -= step3_t;

            video.loopPointReached -= step4_clear1;
            video.loopPointReached -= step4_clear2;

            play(0);
            delay += 1;
            if (delay == 5)
            {
                Serial1.Write("1");
                step1_data = int.Parse(Serial1.ReadLine());
                if (step1_data > 0 && step1_data < 5000) step1_check += 1;
                else step1_check = 0;
                delay = 0;
            }
            if (step1_check > 30)
            { 
                step1_start = false;
                step2_detect = true;
                step1_check = 0;
            }
        }

        ////////////  Step 2 : Detection  ////////////

        if (step2_detect)
        {
            if (step2_reset && step2_round < 15)
            {
                if (_python.detection != 0 && _python.detection == step2_data) step2_times += 1;
                else step2_times = 0;

                if (step2_times > 2) { video.loopPointReached += step2_f; }

                if (step2_found)
                {
                    _box.SetActive(false);
                    _cam.SetActive(false);
                    bin = step2_data;
                    step2_detect = false;
                    step3_bin = true;
                    step2_found = false;
                }

                step2_data = _python.detection;
            }

            if (!step2_reset && step2_round == 3) { step2_data = 0; step2_times = 0; step2_reset = true; }

            switch (step2_round)
            {
                case 0:
                    video.loopPointReached += step2_clear0;
                    break;

                case 1:
                    if (!step2_cam) { cam.Play(); step2_cam = true; }
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(1);
                    step2_round = 2;
                    break;

                case 2:
                    video.loopPointReached += step2_clear1;
                    break;

                case 3:
                    _cam.SetActive(true);
                    _box.SetActive(true);
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 4;
                    break;

                case 4:
                    video.loopPointReached += step2_clear2;
                    break;

                case 5:
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 6;
                    break;

                case 6:
                    video.loopPointReached += step2_clear3;
                    break;

                case 7:
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 8;
                    break;

                case 8:
                    video.loopPointReached += step2_clear4;
                    break;

                case 9:
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 10;
                    break;

                case 10:
                    video.loopPointReached += step2_clear5;
                    break;

                case 11:
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 12;
                    break;

                case 12:
                    video.loopPointReached += step2_clear6;
                    break;

                case 13:
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(2);
                    step2_round = 14;
                    break;

                case 14:
                    video.loopPointReached += step2_clear7;
                    break;

                case 15:
                    _box.SetActive(false);
                    _cam.SetActive(false);
                    //if (!step2_light) { Serial1.Write("2"); step2_light = true; }
                    play(13);
                    step2_round = 16;
                    break;

                case 16:
                    video.loopPointReached += step2_clear8;
                    break;

            }

        }

        ////////////  Step 3 : Bin  ////////////

        if (step3_bin)
        {
            if (!step3_reset) { Serial1.Write("9"); step3_reset = true; }

            delay = delay + 1;
            if (delay == 3) { Serial1.Write("8"); step3_data = 10000 + int.Parse(Serial1.ReadLine()); delay = 0; }

            switch (bin)
            {
                case 1: correct = 10111; break;
                case 2: correct = 11011; break;
                case 3: correct = 11101; break;
                case 4: correct = 11110; break;
            }

            if (step3_data != 11111) { video.loopPointReached += step3_t; }

            if (step3_throw == true)
            {
                if (step3_data == correct && step4_animation == 0) step4_animation = Random.Range(7, 9);
                if (step3_data != correct && step4_animation == 0) step4_animation = Random.Range(9, 11);
                step3_bin = false;
                step4_end = true;
            }

            switch (step3_round)
            {
                case 0:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 1;
                    break;

                case 1:
                    video.loopPointReached += step3_clear1;
                    break;

                case 2:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 3;
                    break;

                case 3:
                    video.loopPointReached += step3_clear2;
                    break;

                case 4:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 5;
                    break;

                case 5:
                    video.loopPointReached += step3_clear3;
                    break;

                case 6:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 7;
                    break;

                case 7:
                    video.loopPointReached += step3_clear4;
                    break;

                case 8:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 9;
                    break;

                case 9:
                    video.loopPointReached += step3_clear5;
                    break;

                case 10:
                    if (!step3_light) { Serial1.Write((bin + 2).ToString()); step3_light = true; }
                    play(bin + 2);
                    step3_round = 11;
                    break;

                case 11:
                    video.loopPointReached += step3_clear6;
                    break;
            }

        }

        ////////////  Step 4 : end  ////////////

        if (step4_end)
        {
            switch (step4_round)
            {
                case 0:
                    if (!step4_light) { Serial1.Write("7"); step4_light = true; }
                    play(step4_animation);
                    step4_round = 1;
                    break;

                case 1:
                    video.loopPointReached += step4_clear1;
                    break;

                case 2:
                    if (!step4_light) { Serial1.Write("7"); step4_light = true; }
                    play(step4_animation);
                    step4_round = 3;
                    break;

                case 3:
                    video.loopPointReached += step4_clear2;
                    break;
            }
        }
    }



    ////////////  Function  ////////////

    void play(int i) { video.clip = woobin_animation[i]; video.Play(); }

    void step2_clear0(VideoPlayer video) { video.Pause(); step2_round = 1;  step2_light = false; }
    void step2_clear1(VideoPlayer video) { video.Pause(); step2_round = 3;  step2_light = false; }
    void step2_clear2(VideoPlayer video) { video.Pause(); step2_round = 5;  step2_light = false; }
    void step2_clear3(VideoPlayer video) { video.Pause(); step2_round = 7;  step2_light = false; }
    void step2_clear4(VideoPlayer video) { video.Pause(); step2_round = 9;  step2_light = false; }
    void step2_clear5(VideoPlayer video) { video.Pause(); step2_round = 11; step2_light = false; }
    void step2_clear6(VideoPlayer video) { video.Pause(); step2_round = 13; step2_light = false; }
    void step2_clear7(VideoPlayer video) { video.Pause(); step2_round = 15; step2_light = false; }
    void step2_clear8(VideoPlayer video)
    {
        video.Pause();
        step2_detect = false;
        step1_start = true;
        reset();
    }


    void step2_f(VideoPlayer video) { video.Pause(); step2_round = 100; step2_found = true;}

    void step3_clear1(VideoPlayer video) { video.Pause(); step3_round = 2;  step3_light = false; }
    void step3_clear2(VideoPlayer video) { video.Pause(); step3_round = 4;  step3_light = false; }
    void step3_clear3(VideoPlayer video) { video.Pause(); step3_round = 6;  step3_light = false; }
    void step3_clear4(VideoPlayer video) { video.Pause(); step3_round = 8;  step3_light = false; }
    void step3_clear5(VideoPlayer video) { video.Pause(); step3_round = 10; step3_light = false; }
    void step3_clear6(VideoPlayer video)
    {
        video.Pause();
        if (step4_animation==0) step4_animation = Random.Range(11, 13);
        step3_bin = false;
        step4_end = true;
    }

    void step3_t(VideoPlayer video) { video.Pause(); step3_round = 100; step3_throw = true; }

    void step4_clear1(VideoPlayer video) { video.Pause(); step4_round = 2; step4_light = false; }
    void step4_clear2(VideoPlayer video)
    {
        video.Pause();
        step4_end = false;
        step1_start = true;
        reset();
    }

    void reset()
    {
        step2_light  = false;
        step3_light  = false;
        step4_light  = false;
        step2_found  = false;
        //step2_cam    = false;
        step2_reset  = false;
        step3_reset  = false;
        step3_out    = false;
        step3_throw  = false;

        delay           = 0;
        step1_data      = 5000;
        step1_check     = 0;
        step2_round     = 0;
        step2_data      = 0;
        step2_times     = 0;
        step3_round     = 0;
        step4_round     = 0;
        step3_data      = 11111;
        bin             = 0;
        correct         = 0;
        step4_animation = 0;
    }

}


