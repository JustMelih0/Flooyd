using UnityEngine;

public abstract class Enemies_StateMachine : StateMachine
{
    [SerializeField]protected Mob mob;
    protected bool stopProcess;
    protected void InitFromSO<T>(State stateTemplate, out T state) where T : Enemies_State
    {
        state = (T)Instantiate(stateTemplate);
        state.Initialize(this, mob);
    }

    public abstract void MachineProcess();
}
