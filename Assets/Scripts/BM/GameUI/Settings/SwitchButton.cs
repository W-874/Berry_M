using UnityEngine;
using UnityEngine.UI;

namespace BM.GameUI.Settings
{
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image bg;
        [SerializeField] private Text buttonText;
        [SerializeField] private string buttonName;

        private bool nowState;
        public bool CurrentState
        {
            get
            {
                return !nowState;
            }
            set
            {
                nowState = value;
                ChangeState();
            }
        }

        public void Awake()
        {
            button.onClick.AddListener(ChangeState);
        }

        private void ChangeState()
        {
            if (nowState)
            {
                OpenState();
            }
            else
            {
                CloseState();
            }
            nowState = !nowState;
        }

        private void OpenState()
        {
            button.image.color = new Color(0f, 1f, 1f, 0.47059f);
            bg.color = new Color(1f, 0f, 0f, 0.47059f);
            buttonText.text = $"{buttonName}True";
        }

        private void CloseState()
        {
            button.image.color = new Color(1f, 0f, 0f, 0.47059f);
            bg.color = new Color(0f, 1f, 1f, 0.47059f);
            buttonText.text = $"{buttonName}False";
        }
    }
}
