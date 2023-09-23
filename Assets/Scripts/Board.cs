using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardsOfChiikawa; // 리스트를 만들어 여러 카드 이미지를 저장

    private List<int> cardIDList = new List<int>();

    private List<Card> cardList = new List<Card>();   
    
     // Start is called before the first frame update
    void Start()
    {
        GenerateCardID();
        ShuffleCardID();
        InitBoard(); 
    }

    void GenerateCardID () {
        for (int i = 0; i < cardsOfChiikawa.Length; i++) {
            cardIDList.Add(i);
            cardIDList.Add(i); //카드가 20장 생성돼야 하니까
        }
    }

    void ShuffleCardID() { //20번 순회하며 랜덤된 값으로 카드 Id를 바꿔 카드를 섞음
        int cardCount = cardIDList.Count;
        for (int i = 0; i < cardCount; i++){
            int randomIndex = Random.Range(i, cardCount);
            int temp = cardIDList[randomIndex];
            cardIDList[randomIndex] = cardIDList[i];
            cardIDList[i] = temp;
        }
    }

    void InitBoard() { //세로 5 * 가로 4의 카드 생성
        float spaceY = 1.8f;
        float spaceX = 1.3f;
        // row
        // 0 - 2 = -2 * spaceY = -3.6
        // 1 - 2 = -1 * spaceY = -1.8
        // 2 - 2 = 0 * spaceY = -
        // 3 - 2 = 1 * spaceY = 1.8
        // 4 - 2 = 2 * spaceY = 3.6
        
        // (row - (int)(rowCount / 2)) * spaceY

        // (col - (colCount / 2)) * spaceX + (spaceX / 2);

        int rowCount = 5;
        int colCount = 4;
        int cardIndex = 0;

        for (int row = 0; row < rowCount; row++) {
            for (int col = 0; col < colCount; col++) {
                float posX = (col - (colCount / 2)) * spaceX + (spaceX / 2);
                float posY = (row - (int)(rowCount / 2)) * spaceY;
                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity);
                Card card = cardObject.GetComponent<Card>();
                int cardID = cardIDList[cardIndex++];
                card.setCardID(cardID);
                card.SetChiikawa(cardsOfChiikawa[cardID]);
                cardList.Add(card);
            }
        }
    }

    public List<Card> GetCards() {
        return cardList;
    }
}
