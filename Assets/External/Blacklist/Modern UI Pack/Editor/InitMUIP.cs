using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    public class InitMUIP
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("MUIPv4.Installed"))
                {
                    EditorPrefs.SetInt("MUIPv4.Installed", 1);
                    EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing Modern UI Pack.\r\rFirst of all, import TextMesh Pro from Package Manager if you haven't already." +
                        "\r\rTo change UI element values, go to Window > Tools > Modern UI Pack > Show UI Manager.\r\rYou can contact me at support@michsky.com for support.", "Got it!");
                }
            }
        }
    }
}