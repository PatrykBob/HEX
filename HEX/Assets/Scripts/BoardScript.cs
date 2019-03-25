using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{

    public Vector3[,] positions = new Vector3[7, 7];
    public GameObject zeton;
    public int[,] doUsunieca = new int[,] { { 0, 0 }, { 0, 1 }, { 6, 6 }, { 6, 5 }, { 5, 6 }, { 6, 4 }, { 0, 6 }, { 0, 5 }, { 6, 0 }, { 6, 1 }, { 6, 2 }, { 5, 0 } };

    // Start is called before the first frame update
    void Start()
    {
        GeneratePositions();
        UsunNiepotrzebne();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var pos in positions)
            {
                Instantiate(zeton, pos, Quaternion.FromToRotation(Vector3.forward, transform.up));
            }

        }
    }

    public void GeneratePositions()
    {
        Transform t = gameObject.transform;
        float t1 = zeton.GetComponent<Renderer>().bounds.size.x;

        float displacementX = t1 + 0.1f;
        float displacementZ = t1 - 0.1f;

        int length = positions.GetLength(0);

        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                if (j % 2 == 0)
                    positions[i, j] = new Vector3(t.position.x + displacementX / 2 + displacementX * (i - length / 2), t.localScale.y / 2 + t.position.y, t.position.z + displacementZ * (j - length / 2));
                else
                    positions[i, j] = new Vector3(t.position.x + displacementX * (i - length / 2), t.localScale.y / 2 + t.position.y, t.position.z + displacementZ * (j - length / 2));
            }
        }
    }

    public void UsunNiepotrzebne()
    {
        for(int i = 0; i < doUsunieca.GetLength(0); i++)
        {
            positions[doUsunieca[i, 0], doUsunieca[i, 1]] = Vector3.zero;
        }
    }
}
