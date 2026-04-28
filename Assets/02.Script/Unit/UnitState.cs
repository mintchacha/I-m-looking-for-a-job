using UnityEditor;
using UnityEngine;

public class UnitState : MonoBehaviour
{
    public DIRECTION direction;
    public UNITSTATE state;    
    public bool spcialState = false;

    public void SetDirection(DIRECTION direction) 
    {
        this.direction = direction;
    }
    public void SetUnitState(UNITSTATE state) 
    {
        if (!spcialState || state == UNITSTATE.DIE) this.state = state;
    }
}
