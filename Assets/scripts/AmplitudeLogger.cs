using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeLogger : MonoBehaviour
{
    public static AmplitudeLogger instance;

    [SerializeField] private string ApiKey;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        DontDestroyOnLoad(instance);
    }

    public void LogEvent(string EventName, string par1, string par2)
    {
        Amplitude amplitude = Amplitude.Instance;
        amplitude.trackSessionEvents(true);
        amplitude.useAdvertisingIdForDeviceId();
        amplitude.init(ApiKey);
        amplitude.logEvent(EventName);
        amplitude.setUserProperty(par1, par2);
        amplitude.uploadEvents();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
