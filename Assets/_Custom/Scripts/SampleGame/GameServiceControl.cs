using UnityEngine;
using UnityEngine.UnityConsent;
using Unity.Services.Core;

public class GameServiceControl : MonoBehaviour
{
    [SerializeField] private GameObject consentPanel;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if(EndUserConsent.GetConsentState().AnalyticsIntent == ConsentStatus.Unspecified)
        {
            AskForConsent();
        }
    }

    private void AskForConsent()
    {
        consentPanel.SetActive(true);
    }

    private void SetConsent(ConsentStatus status)
    {
        ConsentState consent = EndUserConsent.GetConsentState();
        consent.AnalyticsIntent = status;
        EndUserConsent.SetConsentState(consent);

        consentPanel.SetActive(false);
    }

    public void ConsentGiven()
    {
        SetConsent(ConsentStatus.Granted);
    }

    public void ConsentDenied()
    {
        SetConsent(ConsentStatus.Denied);
    }
}