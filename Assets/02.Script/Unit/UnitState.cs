using UnityEditor;
using UnityEngine;

public class UnitState : MonoBehaviour
{
    public DIRECTION direction;
    public UNITSTATE state;    

    public void SetDirection(DIRECTION direction) 
    {
        this.direction = direction;
    }
    public void SetUnitState(UNITSTATE state) 
    {
        this.state = state;
    }
}
