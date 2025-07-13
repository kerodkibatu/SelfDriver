using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] int row = 10;
    [SerializeField] int col = 10;
    [SerializeField] GameObject envPrefab;

    //Spacing
    [SerializeField] float spacing = 40f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Instantiate(envPrefab, new Vector3(i * spacing, 0, j * spacing), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
