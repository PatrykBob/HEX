using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FractionEnum;

public class BoardScript : MonoBehaviour
{

    public Vector3[,] positions = new Vector3[7, 7];
    public GameObject token;
    public GameObject pointOnBoard;
    public int[,] toIgnore = new int[,] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 }, { 1, 1 }, { 2, 0 }, { 6, 6 }, { 6, 5 }, { 6, 4 }, { 5, 6 }, { 5, 5 }, { 4, 6 } };

    public PointOnBoardScript[,] points = new PointOnBoardScript[7, 7];

    bool koniec = false;

    void Start()
    {
        GeneratePositions();
        PokazPozycje();
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
                        break;
                    }
                }
                if (!ignore)
                {
                    GameObject z = Instantiate(pointOnBoard, positions[i, j], Quaternion.FromToRotation(Vector3.forward, transform.up)) as GameObject;
                    z.transform.parent = this.transform;
                    z.GetComponent<PointOnBoardScript>().gridI = i;
                    z.GetComponent<PointOnBoardScript>().gridJ = j;
                    points[i, j] = z.GetComponent<PointOnBoardScript>();
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

    public void Battle()
    {
        Debug.Log("Battle started");
        CheckBuffs();

        int init = GetHighestInitiation();

        for (int a = init; a >= 0; a--)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (points[i, j])
                    {
                        if (points[i, j].token)
                        {
                            if (points[i, j].token.GetComponent<TokenScript>().initiation == a)
                            {
                                TokenScript tokenScript = points[i, j].token.GetComponent<TokenScript>();
                                if (tokenScript.tokenObject.attackMelee.Length > 0)
                                {
                                    AttackMelee(i, j, GetRotatedValues(tokenScript.tokenObject.attackMelee, tokenScript.rotation), tokenScript.meleeBuff, tokenScript.tokenObject.fraction);
                                }
                                if (tokenScript.tokenObject.attackRange.Length > 0)
                                {
                                    AttackRange(i, j, GetRotatedValues(tokenScript.tokenObject.attackRange, tokenScript.rotation), tokenScript.meleeBuff, tokenScript.tokenObject.fraction);
                                }
                                if (tokenScript.tokenObject.attackGauss.Length > 0)
                                {
                                    AttackGauss(i, j, GetRotatedValues(tokenScript.tokenObject.attackGauss, tokenScript.rotation), tokenScript.meleeBuff, tokenScript.tokenObject.fraction);
                                }
                            }
                        }
                    }
                }
            }
            DestroyTokens();
            CheckBuffs();
        }
    }

    void DestroyTokens()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (points[i, j])
                {
                    if (points[i, j].token)
                    {
                        if (points[i, j].token.GetComponent<TokenScript>().toDestroy)
                        {
                            if(points[i, j].token.GetComponent<TokenScript>().tokenObject.headquarters)
                            {
                                KoniecGry();
                            }
                            Destroy(points[i, j].token);
                        }
                    }
                }
            }
        }
    }

    void KoniecGry()
    {
        koniec = true;
    }
    int GetHighestInitiation()
    {
        int max = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (points[i, j])
                {
                    if (points[i, j].token)
                    {
                        if (points[i, j].token.GetComponent<TokenScript>().initiation > max)
                        {
                            max = points[i, j].token.GetComponent<TokenScript>().initiation;
                        }
                    }
                }
            }
        }
        return max;
    }

    public void CheckBuffs()
    {
        Debug.Log("Check board");
        ResetBuffs();
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (points[i, j])
                {
                    if (points[i, j].token)
                    {
                        if (points[i, j].token.GetComponent<TokenScript>().tokenObject.buff)
                        {
                            TokenScript tokenScript = points[i, j].token.GetComponent<TokenScript>();
                            if (tokenScript.tokenObject.meleeAttackBuff.Length > 0)
                            {
                                GiveMeleeBuff(i, j, GetRotatedValues(tokenScript.tokenObject.meleeAttackBuff, tokenScript.rotation), tokenScript.tokenObject.fraction);
                            }
                            if (tokenScript.tokenObject.rangeAttackBuff.Length > 0)
                            {
                                GiveRangeBuff(i, j, GetRotatedValues(tokenScript.tokenObject.rangeAttackBuff, tokenScript.rotation), tokenScript.tokenObject.fraction);
                            }
                            if (tokenScript.tokenObject.initiationBuff.Length > 0)
                            {
                                GiveInitiationBuff(i, j, GetRotatedValues(tokenScript.tokenObject.initiationBuff, tokenScript.rotation), tokenScript.tokenObject.fraction);
                            }
                        }
                    }
                }
            }
        }
    }

    public void ResetBuffs()
    {
        foreach (var token in points)
        {
            if (token)
            {
                if (token.token)
                {
                    token.token.GetComponent<TokenScript>().meleeBuff = 0;
                    token.token.GetComponent<TokenScript>().rangeBuff = 0;
                    token.token.GetComponent<TokenScript>().initiation = token.token.GetComponent<TokenScript>().tokenObject.baseInitiation;
                    token.token.GetComponent<TokenScript>().UpdateInitiationText();
                }
            }
        }
    }

    public void AttackMelee(int i, int j, int[] attack, int buff, Fraction fraction)
    {
        if (attack[0] > 0)
        {
            if (IsThereAToken(i + 1, j, fraction, true))
            {
                points[i + 1, j].token.GetComponent<TokenScript>().GetAttacked(attack[0] + buff);
            }
        }
        if (attack[1] > 0)
        {
            if (IsThereAToken(i + 1, j - 1, fraction, true))
            {
                points[i + 1, j - 1].token.GetComponent<TokenScript>().GetAttacked(attack[1] + buff);
            }
        }
        if (attack[2] > 0)
        {
            if (IsThereAToken(i, j - 1, fraction, true))
            {
                points[i, j - 1].token.GetComponent<TokenScript>().GetAttacked(attack[2] + buff);
            }
        }
        if (attack[3] > 0)
        {
            if (IsThereAToken(i - 1, j, fraction, true))
            {
                points[i - 1, j].token.GetComponent<TokenScript>().GetAttacked(attack[3] + buff);
            }
        }
        if (attack[4] > 0)
        {
            if (IsThereAToken(i - 1, j + 1, fraction, true))
            {
                points[i - 1, j + 1].token.GetComponent<TokenScript>().GetAttacked(attack[4] + buff);
            }
        }
        if (attack[5] > 0)
        {
            if (IsThereAToken(i, j + 1, fraction, true))
            {
                points[i, j + 1].token.GetComponent<TokenScript>().GetAttacked(attack[5] + buff);
            }
        }
    }

    public void AttackRange(int i, int j, int[] attack, int buff, Fraction fraction)
    {
        if (attack[0] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i + r, j, fraction, true))
                {
                    if (!points[i + r, j].token.GetComponent<TokenScript>().tokenObject.armor[3])
                    {
                        points[i + r, j].token.GetComponent<TokenScript>().GetAttacked(attack[0] + buff);
                    }
                    break;
                }

            }
        }
        if (attack[1] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i + 1, j - 1, fraction, true))
                {
                    if (!points[i + 1, j - 1].token.GetComponent<TokenScript>().tokenObject.armor[4])
                    {
                        points[i + 1, j - 1].token.GetComponent<TokenScript>().GetAttacked(attack[1] + buff);
                    }
                    break;
                }
            }
        }
        if (attack[2] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i, j - r, fraction, true))
                {
                    if (!points[i, j - r].token.GetComponent<TokenScript>().tokenObject.armor[5])
                    {
                        points[i, j - r].token.GetComponent<TokenScript>().GetAttacked(attack[2] + buff);
                    }
                    break;
                }
            }
        }
        if (attack[3] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i - r, j, fraction, true))
                {
                    if (!points[i - r, j].token.GetComponent<TokenScript>().tokenObject.armor[0])
                    {
                        points[i - r, j].token.GetComponent<TokenScript>().GetAttacked(attack[3] + buff);
                    }
                    break;
                }
            }
        }
        if (attack[4] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i - r, j + r, fraction, true))
                {
                    if (!points[i - r, j + r].token.GetComponent<TokenScript>().tokenObject.armor[1])
                    {
                        points[i - r, j + r].token.GetComponent<TokenScript>().GetAttacked(attack[4] + buff);
                    }
                    break;
                }
            }
        }
        if (attack[5] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i, j + r, fraction, true))
                {
                    if (!points[i, j + r].token.GetComponent<TokenScript>().tokenObject.armor[2])
                    {
                        points[i, j + r].token.GetComponent<TokenScript>().GetAttacked(attack[5] + buff);
                    }
                    break;
                }
            }
        }
    }

    public void AttackGauss(int i, int j, int[] attack, int buff, Fraction fraction)
    {
        if (attack[0] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i + r, j, fraction, true))
                {
                    points[i + r, j].token.GetComponent<TokenScript>().GetAttacked(attack[0] + buff);
                }

            }
        }
        if (attack[1] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i + r, j - r, fraction, true))
                {
                    points[i + r, j - r].token.GetComponent<TokenScript>().GetAttacked(attack[1] + buff);
                }
            }
        }
        if (attack[2] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i, j - r, fraction, true))
                {
                    points[i, j - r].token.GetComponent<TokenScript>().GetAttacked(attack[2] + buff);
                }
            }
        }
        if (attack[3] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i - r, j, fraction, true))
                {
                    points[i - r, j].token.GetComponent<TokenScript>().GetAttacked(attack[3] + buff);
                }
            }
        }
        if (attack[4] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i - r, j + r, fraction, true))
                {
                    points[i - r, j + r].token.GetComponent<TokenScript>().GetAttacked(attack[4] + buff);
                }
            }
        }
        if (attack[5] > 0)
        {
            for (int r = 0; r < 6; r++)
            {
                if (IsThereAToken(i, j + r, fraction, true))
                {
                    points[i, j + r].token.GetComponent<TokenScript>().GetAttacked(attack[5] + buff);
                }
            }
        }
    }

    public void GiveMeleeBuff(int i, int j, int[] buff, Fraction fraction)
    {
        if (buff[0] > 0)
        {
            if (IsThereAToken(i + 1, j, fraction, false))
            {
                points[i + 1, j].token.GetComponent<TokenScript>().meleeBuff += buff[0];
            }
        }
        if (buff[1] > 0)
        {
            if (IsThereAToken(i + 1, j - 1, fraction, false))
            {
                points[i + 1, j - 1].token.GetComponent<TokenScript>().meleeBuff += buff[1];
            }
        }
        if (buff[2] > 0)
        {
            if (IsThereAToken(i, j - 1, fraction, false))
            {
                points[i, j - 1].token.GetComponent<TokenScript>().meleeBuff += buff[2];
            }
        }
        if (buff[3] > 0)
        {
            if (IsThereAToken(i - 1, j, fraction, false))
            {
                points[i - 1, j].token.GetComponent<TokenScript>().meleeBuff += buff[3];
            }
        }
        if (buff[4] > 0)
        {
            if (IsThereAToken(i - 1, j + 1, fraction, false))
            {
                points[i - 1, j + 1].token.GetComponent<TokenScript>().meleeBuff += buff[4];
            }
        }
        if (buff[5] > 0)
        {
            if (IsThereAToken(i, j + 1, fraction, false))
            {
                points[i, j + 1].token.GetComponent<TokenScript>().meleeBuff += buff[5];
            }
        }
    }

    public void GiveRangeBuff(int i, int j, int[] buff, Fraction fraction)
    {
        if (buff[0] > 0)
        {
            if (IsThereAToken(i + 1, j, fraction, false))
            {
                points[i + 1, j].token.GetComponent<TokenScript>().rangeBuff += buff[0];
            }
        }
        if (buff[1] > 0)
        {
            if (IsThereAToken(i + 1, j - 1, fraction, false))
            {
                points[i + 1, j - 1].token.GetComponent<TokenScript>().rangeBuff += buff[1];
            }
        }
        if (buff[2] > 0)
        {
            if (IsThereAToken(i, j - 1, fraction, false))
            {
                points[i, j - 1].token.GetComponent<TokenScript>().rangeBuff += buff[2];
            }
        }
        if (buff[3] > 0)
        {
            if (IsThereAToken(i - 1, j, fraction, false))
            {
                points[i - 1, j].token.GetComponent<TokenScript>().rangeBuff += buff[3];
            }
        }
        if (buff[4] > 0)
        {
            if (IsThereAToken(i - 1, j + 1, fraction, false))
            {
                points[i - 1, j + 1].token.GetComponent<TokenScript>().rangeBuff += buff[4];
            }
        }
        if (buff[5] > 0)
        {
            if (IsThereAToken(i, j + 1, fraction, false))
            {
                points[i, j + 1].token.GetComponent<TokenScript>().rangeBuff += buff[5];
            }
        }
    }

    public void GiveInitiationBuff(int i, int j, int[] buff, Fraction fraction)
    {
        if (buff[0] > 0)
        {
            if (IsThereAToken(i + 1, j, fraction, false))
            {
                points[i + 1, j].token.GetComponent<TokenScript>().initiation += buff[0];
                points[i + 1, j].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
        if (buff[1] > 0)
        {
            if (IsThereAToken(i + 1, j - 1, fraction, false))
            {
                points[i + 1, j - 1].token.GetComponent<TokenScript>().initiation += buff[1];
                points[i + 1, j - 1].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
        if (buff[2] > 0)
        {
            if (IsThereAToken(i, j - 1, fraction, false))
            {
                points[i, j - 1].token.GetComponent<TokenScript>().initiation += buff[2];
                points[i, j - 1].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
        if (buff[3] > 0)
        {
            if (IsThereAToken(i - 1, j, fraction, false))
            {
                points[i - 1, j].token.GetComponent<TokenScript>().initiation += buff[3];
                points[i - 1, j].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
        if (buff[4] > 0)
        {
            if (IsThereAToken(i - 1, j + 1, fraction, false))
            {
                points[i - 1, j + 1].token.GetComponent<TokenScript>().initiation += buff[4];
                points[i - 1, j + 1].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
        if (buff[5] > 0)
        {
            if (IsThereAToken(i, j + 1, fraction, false))
            {
                points[i, j + 1].token.GetComponent<TokenScript>().initiation += buff[5];
                points[i, j + 1].token.GetComponent<TokenScript>().UpdateInitiationText();
            }
        }
    }


    public bool IsThereAToken(int i, int j, Fraction fraction, bool reverse)
    {
        if (i < 0 || i > 6 || j < 0 || j > 6)
        {
            return false;
        }
        else
        {
            Debug.Log(i + " " + j);
            if (points[i, j])
            {
                if (points[i, j].token)
                {
                    if (reverse)
                    {
                        if (points[i, j].token.GetComponent<TokenScript>().tokenObject.fraction != fraction)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        if (points[i, j].token.GetComponent<TokenScript>().tokenObject.fraction == fraction)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public int[] GetRotatedValues(int[] array, int value)
    {
        int[] newArray = new int[6];
        value = 6 - value;
        for (int i = 0; i < 6; i++)
        {
            int index = (i + value) % 6;
            newArray[i] = array[index];
        }
        return newArray;
    }

    private void OnGUI()
    {
        if (koniec)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 100), "Koniec gry");
        }
    }
}
