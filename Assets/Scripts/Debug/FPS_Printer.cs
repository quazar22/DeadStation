using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Printer : MonoBehaviour
{
    float deltaTime = 0.0f;
    float fps = 0.0f;

    Text t;

    Stopwatch st;
    long count = 0;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        st = new Stopwatch();
        st.Start();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        deltaTime /= 2.0f;
        fps += 1.0f / deltaTime;
        ++count;


        if((st.ElapsedMilliseconds / 1000) > 1)
        {
            t.text = ((int)(fps / count)).ToString();
            fps = 0.0f;
            count = 0;
            st.Restart();
        }
    }
}
