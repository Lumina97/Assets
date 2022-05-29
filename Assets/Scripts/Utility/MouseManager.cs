using UnityEngine;

public static class MouseManager
{
    public static bool Lockstate;
    public static void ToggleMouseLock(bool lockMouse)
    {
        Lockstate = lockMouse;
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
