using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDeckSelector : MonoBehaviour
{
    public GameObject deckPreviewHolderPrefab;

    public GameObject centerDeck;

    Vector3 centerPosition;
    Vector3 centerScale;
    Vector3 cornerScale;

    public GameObject leftDeck;
    public GameObject leftDeckContainer;

    Vector3 leftPosition;

    public GameObject rightDeck;
    public GameObject rightDeckContainer;

    Vector3 rightPosition;

    public PlayerSelectionManager selector;

    List<DeckHolder> deckList;
    List<DeckPreviewHolder> decksPreview = new List<DeckPreviewHolder>();

    DeckPreviewHolder getDeckAtPos(DeckPreviewPosition pos)
    {
        for (int i = 0; i < decksPreview.Count; i++)
        {
            if (decksPreview[i].deckPreviewPosition == pos)
                return decksPreview[i];
        }

        return null;
    }

    int nextDeck(int index, int modify)
    {
        int calc = index + modify;

        if (calc >= deckList.Count)
        {
            return calc - deckList.Count;
        }
        if (calc < 0)
        {
            return deckList.Count + calc;
        }

        return calc;
    }

    [SerializeField]
    float transition_time = 0.28f;

    int current_deck_index;

    bool isMoving = false;
    bool canMove = true;

    public void StopSelection()
    {
        canMove = false;
    }

    public void ContinueSelection()
    {
        canMove = true;
    }

    //[SerializeField]
    LeanTweenType moveAnimation = LeanTweenType.easeInOutQuad;
    //[SerializeField]
    LeanTweenType scaleAnimation = LeanTweenType.linear;

    [SerializeField]
    List<AudioClip> selectDeckSfx = new List<AudioClip>();

    internal void Init(List<DeckHolder> available_decks, int saved_deck)
    {
        isMoving = false;
        canMove = true;
        
        deckList = available_decks;

        centerPosition = centerDeck.transform.position;
        centerScale = centerDeck.transform.localScale;
        cornerScale = rightDeck.transform.localScale;

        leftPosition = leftDeck.transform.position;
        rightPosition = rightDeck.transform.position;

        Destroy(centerDeck);
        Destroy(leftDeck);
        Destroy(rightDeck);

        current_deck_index = saved_deck;

        PopulateScroll();
    }

    void PopulateScroll()
    {
        PopulateCard(centerPosition, centerScale, gameObject.transform, current_deck_index, DeckPreviewPosition.CENTER);

        PopulateCard(leftPosition, cornerScale, leftDeckContainer.transform, nextDeck(current_deck_index, -1), DeckPreviewPosition.LEFT);
        PopulateCard(leftPosition, cornerScale, gameObject.transform, nextDeck(current_deck_index, -2), DeckPreviewPosition.LEFT_HIDDEN, false);

        PopulateCard(rightPosition, cornerScale, rightDeckContainer.transform, nextDeck(current_deck_index, 1), DeckPreviewPosition.RIGHT);
        PopulateCard(rightPosition, cornerScale, gameObject.transform, nextDeck(current_deck_index, 2), DeckPreviewPosition.RIGHT_HIDDEN, false);
    }

    void PopulateCard(Vector3 pos, Vector3 scale, Transform parent, int deckIndex, DeckPreviewPosition pos_desc, bool isActive = true)
    {
        GameObject go = Instantiate(deckPreviewHolderPrefab, pos, Quaternion.identity, parent);
        go.transform.localScale = scale;

        go.GetComponent<DeckPreviewHolder>().Populate(deckList[deckIndex].deckName, deckList[deckIndex].deckArt, deckList[deckIndex].deckIcon);

        DeckPreviewHolder previewHolder = go.GetComponent<DeckPreviewHolder>();

        previewHolder.deckPreviewPosition = pos_desc;

        decksPreview.Add(previewHolder);

        go.SetActive(isActive);
    }

    void PopulateCard(DeckPreviewHolder target, int deckIndex, DeckPreviewPosition pos)
    {
        DeckPreviewHolder previewHolder = target;

        target.Populate(deckList[deckIndex].deckName, deckList[deckIndex].deckArt, deckList[deckIndex].deckIcon);
        previewHolder.deckPreviewPosition = pos;
    }

    public void MoveLeft()
    {
        if (isMoving || !canMove)
            return;

        isMoving = true;

        LeanAudio.play(selectDeckSfx[Random.Range(0, selectDeckSfx.Count)]);

        LeanTween.value(0, 1, transition_time + 0.01f).setOnComplete(() => {
            isMoving = false;
        });

        current_deck_index = nextDeck(current_deck_index, 1);

        DeckPreviewHolder center = getDeckAtPos(DeckPreviewPosition.CENTER);
        DeckPreviewHolder left = getDeckAtPos(DeckPreviewPosition.LEFT);
        DeckPreviewHolder right = getDeckAtPos(DeckPreviewPosition.RIGHT);

        MoveDeckHolder(getDeckAtPos(DeckPreviewPosition.CENTER), left.transform.position, left.transform.localScale, DeckPreviewPosition.LEFT);
        MoveDeckHolder(getDeckAtPos(DeckPreviewPosition.RIGHT), center.transform.position, center.transform.localScale, DeckPreviewPosition.CENTER);

        ScaleDeckHolder(getDeckAtPos(DeckPreviewPosition.LEFT), Vector3.one, Vector3.zero, DeckPreviewPosition.LEFT_HIDDEN, left.transform.position);
        ScaleDeckHolder(getDeckAtPos(DeckPreviewPosition.RIGHT_HIDDEN), Vector3.zero, Vector3.one, DeckPreviewPosition.RIGHT, right.transform.position);

        PopulateCard(getDeckAtPos(DeckPreviewPosition.LEFT_HIDDEN), nextDeck(current_deck_index, 2), DeckPreviewPosition.RIGHT_HIDDEN);
    }

    public void MoveRight()
    {
        if (isMoving || !canMove)
            return;

        isMoving = true;

        LeanAudio.play(selectDeckSfx[Random.Range(0, selectDeckSfx.Count)]);

        LeanTween.value(0, 1, transition_time + 0.01f).setOnComplete(() => {
            isMoving = false;
        });

        current_deck_index = nextDeck(current_deck_index, -1);

        DeckPreviewHolder center = getDeckAtPos(DeckPreviewPosition.CENTER);
        DeckPreviewHolder left = getDeckAtPos(DeckPreviewPosition.LEFT);
        DeckPreviewHolder right = getDeckAtPos(DeckPreviewPosition.RIGHT);

        MoveDeckHolder(getDeckAtPos(DeckPreviewPosition.CENTER), right.transform.position, right.transform.localScale, DeckPreviewPosition.RIGHT);
        MoveDeckHolder(getDeckAtPos(DeckPreviewPosition.LEFT), center.transform.position, center.transform.localScale, DeckPreviewPosition.CENTER);

        ScaleDeckHolder(getDeckAtPos(DeckPreviewPosition.RIGHT), Vector3.one, Vector3.zero, DeckPreviewPosition.RIGHT_HIDDEN, right.transform.position);
        ScaleDeckHolder(getDeckAtPos(DeckPreviewPosition.LEFT_HIDDEN), Vector3.zero, Vector3.one, DeckPreviewPosition.LEFT, left.transform.position);

        PopulateCard(getDeckAtPos(DeckPreviewPosition.RIGHT_HIDDEN), nextDeck(current_deck_index, -2), DeckPreviewPosition.LEFT_HIDDEN);
    }

    void MoveDeckHolder(DeckPreviewHolder holder, Vector3 moveTo, Vector3 scaleTo, DeckPreviewPosition newPosition)
    {
        holder.gameObject.transform.SetParent(gameObject.transform, true);

        LeanTween.move(holder.gameObject, moveTo, transition_time).setEase(moveAnimation);

        LeanTween.scale(holder.gameObject, scaleTo, transition_time).setEase(scaleAnimation)
            .setOnComplete(() => {
                holder.deckPreviewPosition = newPosition;
                holder.transform.SetParent(gameObject.transform, true);
            });
    }

    void ScaleDeckHolder(DeckPreviewHolder holder, Vector3 scaleFrom, Vector3 scaleTo, DeckPreviewPosition newPosition, Vector3 startPosition)
    {
        GameObject target;

        if(holder.deckPreviewPosition == DeckPreviewPosition.LEFT || holder.deckPreviewPosition == DeckPreviewPosition.LEFT_HIDDEN)
        {
            target = leftDeckContainer;
        }
        else if (holder.deckPreviewPosition == DeckPreviewPosition.RIGHT || holder.deckPreviewPosition == DeckPreviewPosition.RIGHT_HIDDEN)
        {
            target = rightDeckContainer;
        }
        else
        {
            Debug.LogError("Error inesperado en ScrollDeckSelector");
            return;
        }

        holder.gameObject.SetActive(true);

        holder.transform.localScale = cornerScale;
        holder.transform.position = startPosition;
        holder.transform.SetParent(target.transform, true);

        target.transform.localScale = scaleFrom;

        LeanTween.scale(target, scaleTo, transition_time).setEase(scaleAnimation)
        .setOnComplete(()=> {

            holder.deckPreviewPosition = newPosition;
            holder.transform.localScale = cornerScale;
            holder.transform.SetParent(gameObject.transform, true);

            target.transform.localScale = Vector3.one;

            if (newPosition == DeckPreviewPosition.RIGHT_HIDDEN || newPosition == DeckPreviewPosition.LEFT_HIDDEN)
            {
                holder.gameObject.SetActive(false);
            }
        });
    }
}
