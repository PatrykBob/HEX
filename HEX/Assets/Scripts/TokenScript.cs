using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenScript : MonoBehaviour
{
    public int i;
    public int j;

    public TokenScriptableObject tokenObject;
    // Start is called before the first frame update
    void Start()
    {
        if (tokenObject.initiation > 0)
        {
            GameObject initiationText = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
            initiationText.transform.SetParent(transform.FindChild("Canvas").transform);
            initiationText.transform.localPosition = new Vector3(tokenObject.initiation1X, tokenObject.initiation1Y, -1.05f);
            initiationText.transform.localRotation = new Quaternion(0, 0, 0, 0);
            initiationText.transform.localScale = new Vector3(1, 1, 1);

            if (tokenObject.doubleAttack)
            {
                GameObject initiationText2 = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
                initiationText2.transform.SetParent(transform.FindChild("Canvas").transform);
                initiationText2.transform.localPosition = new Vector3(tokenObject.initiation2X, tokenObject.initiation2Y, -1.05f);
                initiationText2.transform.localRotation = new Quaternion(0, 0, 0, 0);
                initiationText2.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[1] = Resources.Load<Material>("Materials/" + tokenObject.name);
        GetComponent<MeshRenderer>().materials = materials;
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}
