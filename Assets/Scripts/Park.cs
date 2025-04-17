using UnityEngine;

public class Park : MonoBehaviour
{

    public Route route;

    [SerializeField] SpriteRenderer parkSpriteRenderer;

    [SerializeField] ParticleSystem fx;

    private ParticleSystem.MainModule fxMainModule;


    private void Start()
    {
        fxMainModule = fx.main;
    }
    public void SetColor(Color color)
    {
        parkSpriteRenderer.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car car))
        {
            if (car.route == route)
            {
                Game.Instance.OnCarEntersPark?.Invoke(route);
                StartFX();
            }
        }

    }
    private void StartFX()
    {
        fxMainModule.startColor = route.carColor;
        fx.Play();
    }
}
