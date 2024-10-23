using UnityEngine;

namespace FridayNightFunkin.Settings
{
    public class Links : MonoBehaviour
    {
        public void OpenLink(string link)
        {
            Application.OpenURL(link);
        }
    }
}