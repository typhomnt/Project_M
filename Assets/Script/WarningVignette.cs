using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using DG.Tweening;

public class WarningVignette : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette_;
    private Tween current_animation = null;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayVignette();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StopVignette();
        }
    }

    public void PlayVignette()
    {
        if (current_animation != null)
        {
            return;
        }

        current_animation = DOTween.To(() => vignette_.intensity.value, x => vignette_.intensity.value = x, 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopVignette()
    {
        if(current_animation == null)
        {
            return;
        }

        current_animation.Kill();
        current_animation = DOTween.To(() => vignette_.intensity.value, x => vignette_.intensity.value = x, 0f, 0.25f).OnComplete(() => { current_animation = null; });
    }
}
