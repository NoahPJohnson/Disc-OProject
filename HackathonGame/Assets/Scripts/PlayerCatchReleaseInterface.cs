using System.Collections;
using UnityEngine;

public interface PlayerCatchReleaseInterface
{
    void Catch(bool buttonDown);
    void Throw(bool buttonUp);
    void IdentifyCatchBox(Transform catchBox);
}
