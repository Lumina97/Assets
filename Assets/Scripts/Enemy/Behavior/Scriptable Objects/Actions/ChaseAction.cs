using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        if (controller.ChaseTarget == null)
        {
            GameObject target = GameManager.Instance.GetRandomPlayerInGame();
            if (target != null)
            {
                controller.ChaseTarget = target.transform;
            }
            else return;
        }

        if (controller.AiActiveState == true)
        {
            controller.NavMeshAgent.destination = controller.ChaseTarget.position;
            controller.NavMeshAgent.isStopped = false;
        }
    }
}