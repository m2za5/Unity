using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite Chiikawa;

    [SerializeField]
    private Sprite backChiikawa;

    private bool isFlipped = false;
    private bool isFlipping = false;

    private bool isMatched = false;

    public int cardID;

    public void setCardID(int id) {
        cardID = id;
    }

    public void SetMatched() {
        isMatched = true;
    }

    public void SetChiikawa(Sprite sprite) {
       Chiikawa = sprite;
    }

    public void FlipCard() {

        isFlipping = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, 0.2f).OnComplete(() => // 이미지를 반쯤 뒤집음
        {
            isFlipped = !isFlipped;

            if (isFlipped) { //이미지를 바꿈
                cardRenderer.sprite = Chiikawa;
            } else {
                cardRenderer.sprite = backChiikawa;
            } 

            transform.DOScale(originalScale, 0.2f).OnComplete(() => {
                isFlipping = false;
            }); //다시 카드의 크기를 키움
        });
    }

    void OnMouseDown() {
        if (!isFlipping && !isMatched && !isFlipped) {
            GameManager.instance.CardClicked(this);
        }
    }
}
