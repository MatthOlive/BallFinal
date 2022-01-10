using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTex : MonoBehaviour
{

    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(256, 256, 32);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        computeShader.SetTexture(0,"Result", renderTexture);
        computeShader.SetFloat("resolution", renderTexture.width);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.width / 8, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
