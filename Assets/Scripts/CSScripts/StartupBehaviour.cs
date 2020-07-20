using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.Utilities;
using UnityEngine;

public class StartupBehaviour : MonoBehaviour
{
    public TextAsset logConfig = null;
    private IFacade facade;
    void Start()
    {
        //string logConfigText = logConfig.text;
        //logConfigText = logConfigText.Replace("#OUTPUT_DIR#", Application.dataPath);

        DotEngine.Log.ILogger logger = new UnityLogger();
        LogUtil.SetLogger(logger);

        facade = GameFacade.GetInstance();

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

    private void OnDestroy()
    {
        LogUtil.DisposeLogger();
        facade = null;
    }
}
