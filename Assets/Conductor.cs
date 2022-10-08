using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
public class Conductor : MonoBehaviour
{
    public GameObject circle;
    float lastBeat = -1;
    bool left = true;
    double window = 0.2; // second after beat allowed to follow
    // Start is called before the first frame update
    [SerializeField] private AudioSource beat;
    void Start()
    {
        Play(100, (left) => Move(left));
    }

    void Move(bool left)
    {
        Vector2 direction = left ? Vector2.left : Vector2.right;
        circle.transform.Translate(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            float error = Time.time - lastBeat;
            print($"space pressed: {error}");
            if (error < window) {
                Validate();
            }
        }
    }

    void Validate()
    {
        SpriteRenderer s = circle.GetComponent<SpriteRenderer>();
        Color color = s.color;
        s.color = new Color(0, 1, 0, 1);
        SetTimeout(() => s.color = color, 100);
    }

    async void SetTimeout(Action callback, int ms)
    {
        await Task.Delay(ms, destroyCancellationToken);
        callback();
    }


    async void Play(int beats, Action<bool> callback)
    {
        while ((beats--) > 0)
        {
            await Task.Delay(500, destroyCancellationToken);
            beat.Play();
            lastBeat = Time.time;
            left = !left;
            callback(left);
        }
    }
}
