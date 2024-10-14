using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLoopScroll_Ctrl : MonoBehaviour
{
    public enum DirectionType
    {
        Up,
        Left,
        Down,
        Right,
    }
    [SerializeField]
    private ScrollRect _scrollRect;

    public ScrollRect scrollRect
    {
        get
        {
            if (_scrollRect == null)
            {
                _scrollRect = this.GetComponent<ScrollRect>();
            }
            return _scrollRect;
        }
    }

    public DirectionType directionType;

    public RectTransform item_Prefab;

    public int itemCount;
    public List<RectTransform> items;
    private List<RectTransform> itemPool=new List<RectTransform>();


    public int count = 0;

    public int column=1;//每行几个

    public int4 offset;//偏移
    //public float bottomPadding;//触底间距
    public Vector2 spacing;//间隔


    public int viewRowCount;//行上限
    public int currentRow = -1;//当前行
    public int maxIndex = 0;

    public System.Action scrollOverBottomAction;

    public System.Action<int, int, int, RectTransform> scrollEnterEvent;
    public System.Action<int, int, int, RectTransform> scrollExitEvent;
    public List<Int2> activeIndexs = new List<Int2>();
    public List<int> realIndex = new List<int>();
    public bool isOvered = false;
    private void Awake()
    {
        scrollRect.onValueChanged.AddListener((pos)=> {
            OnScroll();
        });
    }
    [ContextMenu("TestInit")]
    public void Test()
    {
        Init(15, 0);
    }

    public void Init(int count, int currentRow,System.Action<Transform> itemAction=null)
    {
        item_Prefab.gameObject.SetActive(false);
        this.count = count;
        switch (directionType)
        {
            case DirectionType.Up:
            case DirectionType.Down:
                viewRowCount = Mathf.CeilToInt(((scrollRect.viewport.rect.size.y) / (spacing.y + item_Prefab.sizeDelta.y))) + 1;
                break;
            case DirectionType.Left:
            case DirectionType.Right:
                viewRowCount = Mathf.CeilToInt(((scrollRect.viewport.rect.size.x) / (spacing.y + item_Prefab.sizeDelta.x))) + 1;
                break;
        }
        int rowCount=  Mathf.CeilToInt(count / (float)column) ;
        itemCount = viewRowCount * column;
        foreach (RectTransform rectTransform in items)
        {
            if (!itemPool.Contains(rectTransform))
            {
                itemPool.Add(rectTransform);
            }
            rectTransform.gameObject.SetActive(false);
        }
        items.Clear();
        for (int i = Mathf.Max(itemCount, itemPool.Count)-1; i >=0; i--)
        {
            int index = i;
            RectTransform item = null;
            if (i < itemCount)
            {
                if (i < itemPool.Count)
                {
                    item = itemPool[i];
                    itemPool.RemoveAt(i);
                }
                else if (i >= itemPool.Count)
                {
                    item = Instantiate(item_Prefab, scrollRect.content);
                }
                items.Add(item);
                item.gameObject.SetActive(true);
                itemAction?.Invoke(item);
            }
            else
            {
                itemPool[i].gameObject.SetActive(false);
            }
        }
       
        SetContentSizeByItemCount(rowCount);
        SetPosByIndex(0);
    }
    public void Refresh(int count)
    {
        item_Prefab.gameObject.SetActive(false);
        this.count = count;
        int rowCount = Mathf.CeilToInt(count / (float)column);
        SetContentSizeByItemCount(rowCount);
        OnScroll(true);
    }
    public void RefreshContentSize()
    {
        float size = 0;
        float itemSize = 0;
        switch (directionType)
        {
            case DirectionType.Up:
            case DirectionType.Down:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                size = offset.top + offset.bottom + itemSize * viewRowCount- spacing.y;
                break;
            case DirectionType.Left:
            case DirectionType.Right:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                size = offset.left + offset.right + itemSize * viewRowCount- spacing.x;
                break;
        }
        SetContentSize(size);
    }
    public void SetContentSizeByItemCount(int itemCount)
    {
        float itemSize = 0;
        switch (directionType)
        {
            case DirectionType.Up:
            case DirectionType.Down:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                break;
            case DirectionType.Left:
            case DirectionType.Right:
                itemSize = (spacing.x + item_Prefab.sizeDelta.x);
                break;
        }
        //print(itemSize+""+ itemCount);
        switch (directionType)
        {
            case DirectionType.Up:
            case DirectionType.Down:
                scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, offset.top + offset.bottom + itemSize* itemCount);
                break;
            case DirectionType.Left:
            case DirectionType.Right:
                scrollRect.content.sizeDelta = new Vector2(offset.left + offset.right + itemSize * itemCount , scrollRect.content.sizeDelta.y);
                break;
        }
    }
    public void SetContentSize(float value)
    {
        switch (directionType)
        {
            case DirectionType.Up:
            case DirectionType.Down:
                scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, value);
                break;
            case DirectionType.Left:
            case DirectionType.Right:
                scrollRect.content.sizeDelta = new Vector2(value, scrollRect.content.sizeDelta.y);
                break;
        }
    }

    public void OnScroll(bool isReal=false)
    {
        if (item_Prefab == null)
        {
            return;
        }

        float distance = 0;
        switch (directionType)
        {
            case DirectionType.Up:
                distance = Mathf.Max(scrollRect.content.anchoredPosition3D.y, 0);
                break;
            case DirectionType.Down:
                distance = Mathf.Max(-scrollRect.content.anchoredPosition3D.y, 0);
                break;
            case DirectionType.Left:
                distance = Mathf.Max(-scrollRect.content.anchoredPosition3D.x, 0);
                break;
            case DirectionType.Right:
                distance = Mathf.Max(scrollRect.content.anchoredPosition3D.x, 0);
                break;
        }

        Vector2 itemSize = new Vector2(spacing.x + item_Prefab.sizeDelta.x, spacing.y + item_Prefab.sizeDelta.y);
        int _currentRow = 0;
        maxIndex = 0;
        switch (directionType)
        {
            case DirectionType.Up:
                _currentRow = (int)((distance- offset.top + spacing.y) / itemSize.y);
                break;
            case DirectionType.Down:
                _currentRow = (int)((distance- offset.bottom +spacing.y) / itemSize.y );
                break;
            case DirectionType.Left:
                _currentRow = (int)((distance- offset.left +spacing.x) / itemSize.x);
                break;
            case DirectionType.Right:
                _currentRow = (int)((distance- offset.right + spacing.x) / itemSize.x );
                break;
        }
        if (_currentRow != currentRow|| isReal)
        {
            maxIndex = currentRow * column + itemCount;
            currentRow = Mathf.Max(_currentRow,0);
            activeIndexs.Clear();
            for (int i = 0; i < itemCount; i++)
            {
                int realIndex = (currentRow * column + i);//item的真索引,在实际效果中的索引
                int itemIndex = realIndex % itemCount;//item在items中的索引
                int rowIndex = realIndex / column;//item在第几排
                int columnIndex = realIndex % column;//item在每排所处第几个
                Int2 data = new Int2(itemIndex, realIndex);
                activeIndexs.Add(data);

                //float posX = items[data.itemIndex].anchoredPosition3D.x;
                //float posY = items[data.itemIndex].anchoredPosition3D.y;
                if (items.Count > itemIndex)
                {
                    switch (directionType)
                    {
                        case DirectionType.Up:
                            items[itemIndex].anchoredPosition3D = new Vector3(columnIndex * itemSize.x + offset.left + offset.right, -(rowIndex * itemSize.y + offset.top));
                            break;
                        case DirectionType.Down:
                            items[itemIndex].anchoredPosition3D = new Vector3(columnIndex * itemSize.x + offset.left + offset.right, rowIndex * itemSize.y + offset.bottom);
                            break;
                        case DirectionType.Left:
                            items[itemIndex].anchoredPosition3D = new Vector3((rowIndex * itemSize.x + offset.left), -columnIndex * itemSize.y + offset.top + offset.bottom);
                            break;
                        case DirectionType.Right:
                            items[itemIndex].anchoredPosition3D = new Vector3(-(rowIndex * itemSize.x + offset.right), -columnIndex * itemSize.y + offset.top + offset.bottom);
                            break;
                    }
                    
                }
                if (realIndex >= 0 && realIndex < count)
                {
                    items[itemIndex].gameObject.SetActive(true);
                    scrollEnterEvent?.Invoke(realIndex, rowIndex, columnIndex, items[itemIndex]);
                }
                else
                {
                    items[itemIndex].gameObject.SetActive(false);
                    scrollExitEvent?.Invoke(realIndex, rowIndex, columnIndex, items[itemIndex]);
                }
            }
        }

        switch (directionType) 
        {
            case DirectionType.Up:
                if ((scrollRect.content.anchoredPosition3D.y - scrollRect.content.rect.size.y) > -scrollRect.viewport.rect.size.y)
                {
                    if (!isOvered)
                    {
                        isOvered = true;
                        scrollOverBottomAction?.Invoke();
                    }
                }
                else
                {
                    isOvered = false;
                }
                break;
            case DirectionType.Down:
                if ((-scrollRect.content.anchoredPosition3D.y - scrollRect.content.rect.size.y) > scrollRect.viewport.rect.size.y)
                {
                    if (!isOvered)
                    {
                        isOvered = true;
                        scrollOverBottomAction?.Invoke();
                    }
                }
                else
                {
                    isOvered = false;
                }
                break;
            case DirectionType.Left:
                if ((-scrollRect.content.anchoredPosition3D.x - scrollRect.content.rect.size.x) > scrollRect.viewport.rect.size.x)
                {
                    if (!isOvered)
                    {
                        isOvered = true;
                        scrollOverBottomAction?.Invoke();
                    }
                }
                else
                {
                  
                    isOvered = false;
                }
                break;
            case DirectionType.Right:
                if ((scrollRect.content.anchoredPosition3D.x - scrollRect.content.rect.size.x) > -scrollRect.viewport.rect.size.x)
                {
                    if (!isOvered)
                    {
                        isOvered = true;
                        scrollOverBottomAction?.Invoke();
                    }
                }
                else
                {
                    isOvered = false;
                }
                break;
        }

    }
    public void SetPosByIndex(int index)
    {
        float itemSize = 0;
        currentRow = Mathf.FloorToInt(index / (float)column);
        switch (directionType)
        {
            case DirectionType.Up:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                scrollRect.content.anchoredPosition3D = new Vector2(scrollRect.content.anchoredPosition3D.x, Mathf.Clamp(itemSize * currentRow, 0,Mathf.Max(0, scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.size.y)));
                break;
            case DirectionType.Down:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                scrollRect.content.anchoredPosition3D = new Vector2(scrollRect.content.anchoredPosition3D.x, Mathf.Clamp(itemSize * currentRow, Mathf.Min(0, scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.size.y), 0));
                break;
            case DirectionType.Left:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                scrollRect.content.anchoredPosition3D = new Vector2(Mathf.Clamp(itemSize * currentRow, scrollRect.viewport.rect.size.x - scrollRect.content.sizeDelta.x , 0), scrollRect.content.anchoredPosition3D.y);
                break;
            case DirectionType.Right:
                itemSize = (spacing.y + item_Prefab.sizeDelta.y);
                scrollRect.content.anchoredPosition3D = new Vector2(Mathf.Clamp(itemSize * currentRow, 0, scrollRect.content.sizeDelta.x - scrollRect.viewport.rect.size.x), scrollRect.content.anchoredPosition3D.y);
                break;
        }

        OnScroll(true);
    }
    public Transform GetItemByRealIndex(int realIndex)
    {
        Int2 int2= activeIndexs.Find(item => { return item.realIndex == realIndex; });
        if (int2 != null)
        {
            return items[int2.itemIndex];
        }
        else
        {
            return null;
        }
    }
    [System.Serializable]
    public class Int2
    {
        public int itemIndex;
        public int realIndex;
        public Int2(int itemIndex, int realIndex)
        {
            this.itemIndex = itemIndex;
            this.realIndex = realIndex;
        }
    }

    [System.Serializable]
    public struct int4
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }
}
