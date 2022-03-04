using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;
    [SerializeField] private float _speed;

    private bool _direct;

    private void Update()
    {
        if (_direct)
        {
            _loadingImage.fillAmount += _speed;
            if (_loadingImage.fillAmount >= 1)
                _direct = false;
        }
        else
        {
            _loadingImage.fillAmount -= _speed;
            if (_loadingImage.fillAmount <= 0)
                _direct = true;
        }
    }

    public void Show()
    {
        _loadingImage.gameObject.SetActive(true);
        _direct = true;
        _loadingImage.fillAmount = 0;
    }

    public void Hide()
    {
        _loadingImage.gameObject.SetActive(false);
    }
}

