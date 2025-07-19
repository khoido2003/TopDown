using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField]
    private Transform ragDollRootbone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, ragDollRootbone);
        ApplyExplosionToRagDoll(ragDollRootbone, 200f, transform.position, 10f);
    }

    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransform(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagDoll(
        Transform root,
        float explosionForce,
        Vector3 explosionPosition,
        float explosionRange
    )
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagDoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
