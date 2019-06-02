using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Token/Create Token")]
public class TokenScriptableObject : ScriptableObject
{
    public string tokenName;

    public FractionEnum.Fraction fraction;

    public int baseInitiation;
    public int baseArmor = 0;

    public bool headquarters;

    public float initiation1X;
    public float initiation1Y;
    public float initiation2X;
    public float initiation2Y;

    public float armor1X;
    public float armor1Y;
    public float armor2X;
    public float armor2Y;

    public bool buff = false;

    public bool[] armor = null;

    public int[] attackRange = null;
    public int[] attackMelee = null;
    public int[] attackGauss = null;

    public bool[] net = null;

    public bool doubleAttack = false;
    public bool mobility = false;

    public int[] rangeAttackBuff = null;
    public int[] meleeAttackBuff = null;
    public int[] initiationBuff = null;
    public bool[] doubleAttackBuff = null;
    public int[] healthBuff = null;
    public bool[] mobilityBuff = null;

    public bool globalMobilityBuff = false;

    public bool[] scooper = null;

    public bool[] quartermaster = null;

    public int[] enemyInitiationNerf = null;

    public bool canExplode = false;
}
