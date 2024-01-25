using UnityEngine;

public class StandByState<T> : IState<T>
{

    public override void OnEnter()
    {
        
    }

    public override void OnExit() 
    {

    }

    // Update is called once per frame
    public override void OnUpdate() {}
    
    void Attack(IAttack attack) 
    {
        // Switch State to Attack State
    }
}

public class AttackState<T> : IState<T>
{
    public override void OnEnter()
    {
        
    }

    public override void OnExit() 
    {

    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        
    }

    void OnAttackComplete() 
    {
        // Switch to Cooldown State or Standby state if there is no cooldown
    }
}

public class Cooldown<T> : IState<T>
{
    public override void OnEnter()
    {
        
    }

    public override void OnExit() 
    {

    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        
    }
}

