using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireworkCreator : MonoBehaviour
{
    public Transform FireworkParent;
    public GameObject FireworkPrefab;
    public Vector2 FireworkMinPosition;
    public Vector2 FireworkMaxPosition;
    public float FireworkMinScale;
    public float FireworkMaxScale;
    public float Interval;

    public bool IsShowing { get; private set; }

    private float _timeToCreateFirework;
    private bool _canCreateFirework;
    private readonly List<Firework> _fireworkList = new List<Firework>();

    public void Init()
    {
        //Reset();
    }

    private void Update()
    {
        if (_canCreateFirework)
        {
            _timeToCreateFirework -= Time.deltaTime;
            if (_timeToCreateFirework <= 0)
            {
                _timeToCreateFirework += Interval;

                FireworkType type = (FireworkType)Utility.RandomEnum<FireworkType>();
                Vector2 position = Utility.RandomVector2(FireworkMinPosition, FireworkMaxPosition);
                float scale = Random.Range(FireworkMinScale, FireworkMaxScale);
                GameObject fireworkObject = Utils.Spawn(FireworkPrefab, FireworkParent);
                Firework firework = fireworkObject.GetComponent<Firework>();
                firework.Init(type, position, new Vector3(scale, scale, scale));
                if (IsShowing)
                    firework.Show();
                else
                    firework.Hide();
                _fireworkList.Add(firework);
            }
        }
    }

    public void Play()
    {
        _canCreateFirework = true;
    }

    public void Stop()
    {
        _canCreateFirework = false;
    }

    public void Show()
    {
        IsShowing = true;
        CleanUp();
        for (int i = 0; i < _fireworkList.Count; i++)
            _fireworkList[i].Show();
    }

    public void Hide()
    {
        IsShowing = false;
        CleanUp();
        for (int i = 0; i < _fireworkList.Count; i++)
            _fireworkList[i].Hide();
    }

    public void CleanUp()
    {
        _fireworkList.RemoveAll(x => x == null);
    }

    public void Reset()
    {
        Utils.RemoveAllChildren(FireworkParent);
        _timeToCreateFirework = Interval;
        Hide();
        Stop();
    }
}