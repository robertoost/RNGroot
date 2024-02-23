using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int hoe_vaak_zeg_ik_hallo = 10;

        for (int i = 0; i < hoe_vaak_zeg_ik_hallo; i++)
        {
            if (i == 0)
            {
                Debug.Log("Hallo wereld!");
            }
            else
            {
                Debug.Log($"Hallo wereld (voor de {i + 1}e keer)");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
