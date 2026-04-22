using System;

public enum DIRECTION
{
    LEFT,
    RIGHT,
}

public enum UNITSTATE
{
    IDLE,
    MOVE,
    JUMP,
    ATTACK,
    DAMAGED,
    DIE
}

public enum GAMESTATE
{ 
    PLAYING,
    REWARD
}

[Serializable]
public struct UnitStatData
{
    public string name;
    public float maxHealth;
    public float atk;
    public float atkSpeed;
    //public UnitStatData(string name, float maxHealth, float atk, float atkSpeed)
    //{
    //    this.name = name;
    //    this.maxHealth = maxHealth;
    //    this.atk = atk;
    //    this.atkSpeed = atkSpeed;
    //}
}
