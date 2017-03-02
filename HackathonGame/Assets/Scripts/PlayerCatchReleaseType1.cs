using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchReleaseType1 : PlayerCatchReleaseInterface
{
    Transform playerCatchBox;
    CatchScript catchScript;

    public void Catch(bool buttonDown)
    {
        if (buttonDown == true)
        {
            catchScript.AttemptCatch();
        }
    }

    public void Throw(bool buttonUp)
    {
        if (buttonUp == true)
        {
            catchScript.AttemptThrow();
        }
    }

    public void IdentifyCatchBox(Transform catchBox)
    {
        playerCatchBox = catchBox;
        catchScript = catchBox.GetComponent<CatchScript>();
    } 
}
