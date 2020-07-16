using DotEngine.Utilities;
using UnityEngine;

public class StartupBehaviour : MonoBehaviour
{
    private GameFacade facade;
    void Start()
    {
        facade = new GameFacade();
        DontDestroyHandler.AddTransform(transform);
    }

    private void Update()
    {
        facade.DoUpdate(Time.deltaTime);
        facade.DoUnscaleUpdate(Time.unscaledDeltaTime);
    }

    private void LateUpdate()
    {
        facade.DoLateUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        facade.DoFixedUpdate(Time.fixedDeltaTime);
    }
}
