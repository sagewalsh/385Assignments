using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    [Range(5, 60)]
    private int frameRate = 30;
    [SerializeField]
    private Sprite[] sprites;

    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(WaitForNextFrame());
    }

    private IEnumerator WaitForNextFrame()
    {
        while(currentSpriteIndex < sprites.Length)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex++];
            yield return new WaitForSeconds(1f / frameRate);
        }
    }
}
