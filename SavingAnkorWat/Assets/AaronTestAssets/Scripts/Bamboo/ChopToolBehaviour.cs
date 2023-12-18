using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopToolBehaviour : MonoBehaviour
{
    [SerializeField] private Transform bladeTransform;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);

        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (contactPoints[i].thisCollider.transform != bladeTransform) continue;
            {
                if (collision.gameObject.TryGetComponent(out IChoppable choppable))
                {
                    if (collision.relativeVelocity.magnitude < choppable.MinimumChopVelocity) return;

                    choppable.Chop(collision);
                }
            }
        }
    }
}
