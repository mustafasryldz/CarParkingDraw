using DG.Tweening;
using UnityEngine;
public class Car : MonoBehaviour
{
    public Route route;
    public Transform bottomTransform;
    public Transform bodyTransform;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem smokeFX;
    [SerializeField] Rigidbody rb;
    [SerializeField] float danceValue;
    [SerializeField] float durationMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (bodyTransform != null)
        {
            bodyTransform.DOLocalMoveY(danceValue, 0.1f)
    .SetLoops(-1, LoopType.Yoyo)
    .SetEase(Ease.Linear);
        }



    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Car otherCar))
        {
            StopDancingAnim();
            rb.DOKill(false);

            // add explosion:
            Vector3 hitPoint = collision.contacts[0].point;
            AddExplosionForce(hitPoint);
            smokeFX.Play();

            Game.Instance.OnCarCollision?.Invoke();
        }
    }

    private void AddExplosionForce(Vector3 point)
    {
        rb.AddExplosionForce(400f, point, 3f);
        rb.AddForceAtPosition(Vector3.up * 2f, point, ForceMode.Impulse);
        rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
    }
    private float GetRandomAngle()
    {
        float angle = 10f;

        float rand = UnityEngine.Random.value;
        return rand > .5f ? angle : -angle;
    }
    public void CarMove(Vector3[] path)
    {

        rb.DOLocalPath(path, 2f * durationMultiplier * path.Length)
        .SetLookAt(0.01f, false)
        .SetEase(Ease.Linear);

    }
    public void StopDancingAnim()
    {
        bodyTransform.DOKill(true);
    }
    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterials[0].color = color;
    }
}
