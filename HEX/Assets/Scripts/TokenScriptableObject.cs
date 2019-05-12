﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenScriptableObject : ScriptableObject
{
    public string tokenName;

    public FractionEnum.Fraction fraction;

    public int initiation = 10;
    public int health = 1;

    public bool canGetBuff = false;

    public bool[] armor = null;

    public int[] attackRange = null;
    public int[] attackMelee = null;
    public int[] attackGauss = null;

    public bool[] net = null;

    public bool netted = false;

    public bool doubleAttack = false;
    public bool tripleAttack = false;
    public bool mobility = false;

    public int[] rangeAttackBuff = null;
    public int[] meleeAttackBuff = null;
    public int[] initiationBuff = null;
    public int[] doubleAttackBuff = null;
    public int[] healthBuff = null;
    public int[] mobilityBuff = null;

    public bool globalMobilityBuff = false;

    public bool[] scooper = null;

    public bool[] quartermaster = null;

    public int[] enemyInitiationNerf = null;

    public bool canExplode = false;
}