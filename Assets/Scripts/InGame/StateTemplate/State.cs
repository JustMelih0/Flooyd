using UnityEngine;

public abstract class State : ScriptableObject
{

    public virtual void Initialize(StateMachine machine, Mob mob){}
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Execute();
    public abstract void HandlePhysics();
    public virtual void GizmosDraw(){}

}
