using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TurnDirection = ShipMotor.ETurnDirection;

public class ShipInput : MonoBehaviour
{
    [SerializeField] private SO_ControlScheme _controls;
    public void SetInputScheme(SO_ControlScheme _newControls)
    {
        if (_newControls != null)
            _controls = _newControls;
    }

    private bool _receiveInput;
    /// <summary>
    /// Enables/Disabled input for the ship
    /// Used to disable ships during menu
    /// or when the ships are being spawned and the game is being prepped 
    /// so the player can't control his ship during a loadingscreen
    /// </summary>
    public bool ReceiveInput
    {
        get { return _receiveInput; }
        set
        {
            _receiveInput = value;
            if (value == false)
            {
                if (_motor)
                {
                    _motor.SetMovement(0, TurnDirection.none, true);
                }
            }
        }
    }

    private ShipMotor _motor;
    public ShipMotor Motor
    {
        get { return _motor; }
        set { _motor = value; }
    }

    private ShipWeapons _weapons;
    public ShipWeapons Weapons
    {
        get { return _weapons; }
        set
        {
            _weapons = value;
        }
    }

    private bool isMobile = false;

    private void Awake()
    {
        //Check if we are running on windows or mobile
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isMobile = true;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            isMobile = false;
        }

    }

    private void Update()
    {
        //exit if we don't want to control the ship

        if (ReceiveInput == false)
            return;

        //check for a motor
        if (Motor)
        {
            //check movement input each frame
            CheckMovementInput();
        }
        //check for Shipweapons
        if (Weapons)
        {
            //check Weapons input each frame
            CheckWeaponsInput();
        }
    }

    private void CheckWeaponsInput()
    {
        if (_weapons)
        {
            //only run on windows
            if (isMobile == false)
            {
                if (_controls && Input.GetKey(_controls.FireWeapon))
                {
                    _weapons.FireWeapon();
                }
                else if (_controls == null && Input.GetKey(KeyCode.Space))
                {
                    _weapons.FireWeapon();
                }
            }

            //only run on Android
            if (isMobile == true)
            {
                if (CrossPlatformInputManager.GetButton("Fire"))
                {
                    _weapons.FireWeapon();
                }
            }
        }
    }

    private void CheckMovementInput()
    {
        //check for a motor
        if (_motor)
        {
            //get forward input
            float forwardMovement = 0;
            //save turn direction
            TurnDirection turnDir = TurnDirection.none;

            //only on Windows
            if (isMobile == false)
            {
                if (_controls)
                {
                    forwardMovement = Input.GetKey(_controls.MoveForward) ? 1 : 0;
                }
                else
                {
                    forwardMovement = Input.GetAxisRaw("Vertical");
                }

                //get turn input..
                float turn;
                if (_controls)
                {
                    turn = Input.GetKey(_controls.TurnLeft) ? -1 : Input.GetKey(_controls.TurnRight) ? 1 : 0;
                }
                else
                {
                    turn = Input.GetAxisRaw("Horizontal");
                }

                //.. convert to turndirection
                turnDir = turn < 0 ? TurnDirection.left : turn > 0 ? TurnDirection.right : TurnDirection.none;
            }

            //only on mobile
            if (isMobile == true)
            {
                //check for turn input
                if (CrossPlatformInputManager.GetButton("Turn Left"))
                {
                    turnDir = TurnDirection.left;
                }
                else if (CrossPlatformInputManager.GetButton("Turn Right"))
                {
                    turnDir = TurnDirection.right;
                }
                else
                {
                    turnDir = TurnDirection.none;
                }

                //check for movement input
                if (CrossPlatformInputManager.GetButton("Move"))
                {
                    forwardMovement = 1;
                }
                else
                {
                    forwardMovement = 0;
                }
            }

            //.. pass all data to the motor to perform movement
            _motor.SetMovement(forwardMovement, turnDir);
        }
    }
}