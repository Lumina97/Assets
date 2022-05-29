using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Shoot")]
public class ShootAction : Action
{
    public override void Act(StateController controller)
    {
        Shoot(controller);
    }

    private void Shoot(StateController controller)
    {
        controller.Weapons.FireWeapon();
        if (controller.AiActiveState == true)
        {
            controller.NavMeshAgent.isStopped = true;
        }
    }
}