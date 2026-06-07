using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snack : MonoBehaviour
{
    [SerializeField] private GameObject BuySnack;
    BuySnackUI snackUIScript;
    Image image;
    public int snackInfo;
    float alpha;
    public AudioClip pick;
    AudioSource sfx;

    private void Start()
    {
        alpha = 1.0f;
        snackUIScript = BuySnack.GetComponent<BuySnackUI>();
        image = gameObject.GetComponent<Image>();
        sfx = GameObject.Find("SFx").GetComponent<AudioSource>();
    }

    public void SelectSnack()
    {
        if(snackUIScript.isDisappearing == false)
        {
            StartCoroutine("Fadeout");
            sfx.clip = pick;
            sfx.Play();
        }
    }

    IEnumerator Fadeout()
    {
        snackUIScript.isDisappearing = true;
        snackUIScript.isPlayed = false;
        snackUIScript.cur = snackInfo;
        while (alpha > 0f)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        snackUIScript.isDisappearing = false;
    }
}
