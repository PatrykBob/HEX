using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{

    public Vector3[,] positions = new Vector3[7, 7];
    public GameObject token;
    public GameObject pointOnBoard;
    public int[,] toIgnore = new int[,] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 }, { 1, 1 }, { 2, 0 }, { 6, 6 }, { 6, 5 }, { 6, 4 }, { 5, 6 }, { 5, 5 }, { 4, 6 } };

    // Start is called before the first frame update
    void Start()
    {
        GeneratePositions();
        PokazPozycje();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PokazPozycje()
    {
        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                bool ignore = false;
                for (int k = 0; k < toIgnore.GetLength(0); k++)
                {
                    if (i == toIgnore[k, 0] && j == toIgnore[k, 1])
                    {
                        ignore = true;
                        //z.GetComponent<PointOnBoardScript>().ignore = true;
                        break;
                    }
                }
                if (!ignore)
                {
                    GameObject z = Instantiate(pointOnBoard, positions[i, j], Quaternion.FromToRotation(Vector3.forward, transform.up)) as GameObject;
                    z.transform.parent = this.transform;
                    z.GetComponent<PointOnBoardScript>().gridI = i;
                    z.GetComponent<PointOnBoardScript>().gridJ = j;
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
}
