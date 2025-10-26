using UnityEngine;
using UnityEngine.UIElements;

public class CertificatePage : MonoBehaviour
{
    private VisualElement V_Main;
    private VisualElement popUpPage;
    private VisualElement V_CertificatePage;
    private Button B_ViewCertificateBackBtn;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        popUpPage = V_Main.Q<VisualElement>("popUpPage");

        V_CertificatePage = popUpPage.Q<VisualElement>("certificatePage");
        B_ViewCertificateBackBtn = V_CertificatePage.Q<Button>("viewCertificateBackBtn");

        B_ViewCertificateBackBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            HideCertificatePage();
        });
    }

    public void ShowCertificatePage()
    {
        popUpPage.style.display = DisplayStyle.Flex;
        V_CertificatePage.style.display = DisplayStyle.Flex;
    }

    public void HideCertificatePage()
    {
        popUpPage.style.display = DisplayStyle.None;
        V_CertificatePage.style.display = DisplayStyle.None;
    }
}
