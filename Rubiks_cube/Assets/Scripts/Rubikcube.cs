using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubikcube : MonoBehaviour
{
    [SerializeField]
    int profondeur = 0;

    [SerializeField]
    GameObject cube = null;

    [SerializeField]
    int decalage = 5;

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

        for(int i = 0; i < profondeur; i++)
        {
            for (int j = 0; j < profondeur; j++)
            {
                for (int k = 0; k < profondeur; k++)
                {
                    Transform temp = Instantiate(cube.transform, pos, Quaternion.identity);
                    temp.parent = parent.transform;
                    pos.z += decalage;
                }
                pos.z = 0;
                pos.y += decalage;
            }
            pos.x += decalage;
            pos.y = 0;
        }
    }

    void Update()
    {
        
    }
}
