using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChoppableBehaviour : MonoBehaviour, IChoppable
{
    [SerializeField] private float minimumChopVelocity;
    [SerializeField] private UnityEvent<Collision> OnGetChopped;

    public float MinimumChopVelocity => minimumChopVelocity;

    public void Chop(Collision collison)
    {
        OnGetChopped?.Invoke(collison);
    }
}
