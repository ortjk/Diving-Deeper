using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class StaticFunctions
{
    public static void EnableControllable(PlayerInput input, Camera camera, bool enable=true)
    {
        input.enabled = enable;
        camera.enabled = enable;
    }
}

