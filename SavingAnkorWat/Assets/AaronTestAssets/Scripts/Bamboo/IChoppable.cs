using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChoppable
{
    public void Chop(Collision collison);
    public float MinimumChopVelocity { get; }
}
