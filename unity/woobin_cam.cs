using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class woobin_cam : MonoBehaviour
{
    WebCamTexture cam;
    public RawImage display;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        cam = new WebCamTexture();
        cam.deviceName = devices[1].name;
        display.texture = cam;
        display.material.mainTexture = cam;
    }

    void Update()
    {
        cam.Play();
    }
}
