using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UI_RankingPopUp : MonoBehaviour
{
    [SerializeField] private UI_RankingItem[] _topItems;
    [SerializeField] private UI_RankingItem _myRankingItem;

    private IReadOnlyList<RankingData> _topRankings;
    private RankingData _myRanking;
    private int _myRankPosition;

    public async void Open()
    {
        gameObject.SetActive(true);
        await RefreshAsync();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private async UniTask RefreshAsync()
    {
        await RankingManager.Instance.RefreshAsync(10);

        _topRankings = RankingManager.Instance.TopRankings;        
        _myRanking = RankingManager.Instance.MyRanking;
        _myRankPosition = RankingManager.Instance.MyRankingPosition;

        RefreshTopRankings();
        RefreshMyRanking();
    }

    private void RefreshTopRankings()
    {
        Debug.Log(_topRankings.Count);
        
        for(int i = 0; i < _topItems.Length; i++)
        {
            if(i <  _topRankings.Count)
            {
                _topItems[i].Bind(i + 1, _topRankings[i]);
                _topItems[i].gameObject.SetActive(true);
            }
            else
            {
                _topItems[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshMyRanking()
    {
        if(_myRanking == null)
        {
            _myRankingItem.gameObject.SetActive(false);
            return;
        }
        
        _myRankingItem.Bind(_myRankPosition, _myRanking);
    }
}
