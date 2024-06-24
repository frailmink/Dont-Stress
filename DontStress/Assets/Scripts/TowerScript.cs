using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public virtual void EnableScript()
    {
        this.enabled = true;
    }

    public virtual void DisableScript()
    {
        this.enabled = false;
    }
}
