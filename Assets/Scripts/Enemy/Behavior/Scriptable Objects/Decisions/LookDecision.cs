using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public LayerMask VisibilityMask;

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        RaycastHit2D hit;

        Debug.DrawRay(controller.Eyes.position, controller.Eyes.up.normalized * controller.EnemyStats.LookRange, Color.green);

        hit = Physics2D.Raycast(controller.Eyes.position, controller.Eyes.up, controller.EnemyStats.LookRange, VisibilityMask);
        if (hit && hit.collider.CompareTag("PlayerShip"))
        {
            controller.ChaseTarget = hit.transform;
            return true;
        }
        else
        {
            return false;
        }
    }
}