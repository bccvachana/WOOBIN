using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woobin_cam_box : MonoBehaviour
{
    public GUISkin skin;
    public python _python;

    void OnGUI()
    {
        if (_python.data[0] == 0)
            GUI.Box(new Rect(0, 0, 0, 0), "", skin.box);
        else
            GUI.Box(new Rect(Mathf.Abs(_python.data[3]- 382)+449, _python.data[2]+467, _python.data[3] - _python.data[1], _python.data[4] - _python.data[2] ), "", skin.box);
    }
}
