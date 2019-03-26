using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{

    public Vector3[,] positions = new Vector3[7, 7];
    public GameObject token;
    public GameObject pointOnBoard;
    public int[,] ignore = new int[,] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 }, { 1, 1 }, { 2, 0 }, { 6, 6 }, { 6, 5 }, { 6, 4 }, { 5, 6 }, { 5, 5 }, { 4, 6 } };

    // Start is called before the first frame update
    void Start()
    {
        GeneratePositions();
        //UsunNiepotrzebne();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                for (int j = 0; j < positions.GetLength(1); j++)
                {
                    GameObject z = Instantiate(pointOnBoard, positions[i,j], Quaternion.FromToRotation(Vector3.forward, transform.up)) as GameObject;

                    for (int k = 0; k < ignore.GetLength(0); k++)
                    {
                        if (i == ignore[k, 0] && j == ignore[k, 1])
                        {
                            z.GetComponent<PointOnBoardScript>().ignore = true;
                            Debug.Log(i + " " + j + " " + k);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void GeneratePositions()
    {
        Transform t = gameObject.transform;
        float t1 = token.GetComponent<Renderer>().bounds.size.x;

        float displacementX = t1 + 0.1f;
        float displacementZ = t1 - 0.1f;

        int length = positions.GetLength(0);

        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                positions[i, j] = new Vector3(
                    t.position.x + displacementX * (i - length / 2) + (displacementX / 2) * (j - length / 2),
                    t.localScale.y / 2 + t.position.y,
                    t.position.z + displacementZ * (j - length / 2));
            }
        }
    }

    public void UsunNiepotrzebne()
    {
        for(int i = 0; i < ignore.GetLength(0); i++)
        {
            positions[ignore[i, 0], ignore[i, 1]] = Vector3.zero;
        }
    }
}
