using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryEmissionSample : MonoBehaviour
{
    // Start is called before the first frame update
    new Renderer renderer;
    float currentTIme;
    float maxTime=5;
    void Start()
    {
        renderer=GetComponent<Renderer>();
        currentTIme=Random.Range(0,5);
    }

    // Update is called once per frame
    void Update()
    {
        currentTIme+=Time.deltaTime;
        var newColor=Color.Lerp(Color.red,Color.green,currentTIme/maxTime);
        //Sets the special instance variable
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        props.SetColor("_EColor",newColor);
        renderer.SetPropertyBlock(props);
        //----------------------------------------------------------

        if(currentTIme>maxTime)
            currentTIme=0f;

    }
}
