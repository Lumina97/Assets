using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State CurrentState;
    public EnemyStats EnemyStats;
    public Transform Eyes;
    [Tooltip("The state created to server as 'Remain in state' function. Should be the same you use in you decisions")]
    public State RemainState;

    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public ShipWeapons Weapons;
    [HideInInspector] public Transform ChaseTarget;
    [HideInInspector] public float StateTimeElapsed;

    private bool _aiActive = true;
    public bool AiActiveState
    {
        get { return _aiActive; }
    }
    
    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Weapons = GetComponent<ShipWeapons>();
    }

    public void SetAiActiveState(bool _activeState)
    {
        _aiActive = _activeState;
        if (_aiActive)
        {
            NavMeshAgent.enabled = true;
        }
        else
        {
            NavMeshAgent.enabled = false;
        }
    }

    private void Update()
    {
        if (_aiActive == false)
            return;
        CurrentState.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
        if (CurrentState != null && Eyes != null && EnemyStats != null)
        {
            Gizmos.color = CurrentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(Eyes.position, EnemyStats.LookSphereCastRadius);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
            CurrentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        StateTimeElapsed += Time.deltaTime;
        return (StateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        StateTimeElapsed = 0;
    }        
}

