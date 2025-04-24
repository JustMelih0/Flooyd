using UnityEngine;

public abstract class State : ScriptableObject
{

    public virtual void Initialize(StateMachine machine, Mob mob){}
    public virtual void EnabledObject(){}
    public virtual void DisabledObject(){}
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Execute();
    public abstract void HandlePhysics();
    public virtual void GizmosDraw(){}
    public virtual void OnAnimationStarted(){}
    public virtual void OnAnimation(){}
    public virtual void OnAnimationEnded(){}


}
