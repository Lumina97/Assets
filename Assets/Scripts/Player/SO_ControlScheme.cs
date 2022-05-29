using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Ship/Controls")]
public class SO_ControlScheme : ScriptableObject
{
	public KeyCode MoveForward;
	public KeyCode TurnLeft;
	public KeyCode TurnRight;
	public KeyCode FireWeapon;
	public KeyCode PauseGame;
}