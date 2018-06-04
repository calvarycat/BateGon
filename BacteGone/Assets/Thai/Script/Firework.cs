using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum FireworkType
{
    Firework0,
    Firework1,
    Firework2
}

public class Firework : MonoBehaviour
{
    public Image MainImage;

    private Animator _animator;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(FireworkType fireworkType, Vector2 position, Vector3 scale)
    {
        _rectTransform.anchoredPosition = position;
        _rectTransform.localScale = scale;
        _animator.SetTrigger(fireworkType.ToString());
    }

    private void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    public void Show()
    {
        MainImage.color = new Color(MainImage.color.r, MainImage.color.g, MainImage.color.b, 1);
    }

    public void Hide()
    {
        MainImage.color = new Color(MainImage.color.r, MainImage.color.g, MainImage.color.b, 0);
    }
}