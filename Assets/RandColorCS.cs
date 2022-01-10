using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandColorCS : MonoBehaviour
{
    
    struct Cube
    {
        public Vector3 position;
        public Color color;    
    }
  
    public ComputeShader computeShader;
    public int iteraction = 50;
    public int count = 100;
    GameObject[] gameObjects;
    Cube[] data;
    public GameObject modelPref;
   




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, -5, 0) * 10);
        }
    }

    void OnGUI() { 

        if (data == null){
          if (GUI.Button(new Rect(0,0,100,50), "Create"))
            {
             createCube();
            }
        }

        if (data != null)
        {
            if (GUI.Button(new Rect(110,0, 100, 50), "Random CPU"))
            { 
                for (int k = 0; k < iteraction; k++)
                {
                
                    for (int i = 0; i < gameObjects.Length; i++)
                    {
                        gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
                        //gameObjects[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * 10);
                    }
                }
            }
        }


        if (data != null)
        {
            if (GUI.Button(new Rect(220, 0, 100, 50), "Random GPU"))
            {
                int totalSize = 4*sizeof(float) +3 *sizeof(float) ;
                ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, totalSize);
                computeBuffer.SetData(data);

                computeShader.SetBuffer(0,"cubes", computeBuffer);
                computeShader.SetInt("iteraction", iteraction);

                computeShader.Dispatch(0,data.Length/10,1,1);

                computeBuffer.GetData(data);

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", data[i].color);
                }

                computeBuffer.Dispose();
            }
        }
    }



    private void createCube() {
        data = new Cube[count * count];
        gameObjects = new GameObject[count* count];

        for (int i = 0; i < count; i++)
        {
            float offsetX = (-count/ 2 +i);

            for (int j = 0; j < count; j++) {
                float offsetY = (-count / 2 + j);

                Color _color = Random.ColorHSV();

                GameObject go = GameObject.Instantiate(modelPref, new Vector3(offsetX*0.7f, 0, offsetY*0.7f), Quaternion.identity);
                go.GetComponent<MeshRenderer>().material.SetColor("_Color",_color);

                gameObjects[i * count + j] = go;

                data[i * count + j] = new Cube();

                data[i * count + j].position = go.transform.position;
                data[i * count + j].color = _color;

            }
        }
    }
}
