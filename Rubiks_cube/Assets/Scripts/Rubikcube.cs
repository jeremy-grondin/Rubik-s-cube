using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubikcube : MonoBehaviour
{
    [SerializeField]
    int size = 0;

    [SerializeField]
    GameObject cube = null;

    [SerializeField]
    GameObject parent;

    [SerializeField]
    List<Material> materials;

    void Start()
    {
        GenerateCubes();
    }

    void GenerateCubes()
    {
        if (cube == null)
            return;

        Vector3 pos = Vector3.zero;

        float decal = (size - 1) * -0.5f; 
        
        for(int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    Transform temp = Instantiate(cube.transform, parent.transform);
                    temp.position = new Vector3(decal + i, decal + j, decal + k);
                }
            }
        }
    }

    void Update()
    {
        
    }
}
