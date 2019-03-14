using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class woobin_bg : MonoBehaviour
{
    public GameObject bg2;
    public GameObject bg3;
    public main1 _main;
    public python _python;

    void Start()
    {
        bg2.SetActive(false);
        bg3.SetActive(false);
    }

    void Update()
    {
        if (_main.step2_round == 2) { bg2.SetActive(true); bg3.SetActive(false); }
        if (_main.step2_round == 16) { bg2.SetActive(false); bg3.SetActive(false); }

        if (_main.step2_times > 2) { bg2.SetActive(false); bg3.SetActive(true); }
        if (_main.step4_round==3) { bg2.SetActive(false); bg3.SetActive(false); }
    }
}
