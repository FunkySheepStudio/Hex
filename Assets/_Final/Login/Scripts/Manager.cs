using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.login
{
    public class Manager : MonoBehaviour
    {
        public void Login()
        {
            SceneManager.LoadScene("_Final/Modes/Modes", LoadSceneMode.Single);
        }
    }
}
