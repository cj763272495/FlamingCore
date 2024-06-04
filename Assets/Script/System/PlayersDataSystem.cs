using System.Collections.Generic;
using UnityEngine;

public class PlayersDataSystem : MonoBehaviour
{
    public static PlayersDataSystem Instance = null;  
    public string playerID;//当前玩家ID
    public PlayerData PlayerData;//当前玩家数据
    private ResSvc _resSvc; 
    private PlayerDataDic _playerDataDic = new();//所有玩家数据

    private bool _isNewInviorment;
    public bool IsNewInviorment {
        get { return  _isNewInviorment; }
    }
      
    public void InitSys() {
        _resSvc = ResSvc.Instance;
        Instance = this;
        _playerDataDic = _resSvc.LoadPlayerData();
        if(_playerDataDic==null || _playerDataDic.Count==0) {
            _isNewInviorment = true;
        } else {
            _isNewInviorment = false;
        }
        if(!IsNewInviorment) {
            var firstPlayerData = _playerDataDic.GetEnumerator();
            firstPlayerData.MoveNext();
            playerID = firstPlayerData.Current.Key;
            PlayerData = firstPlayerData.Current.Value;
        }
        GameRoot.Instance.CoinCached = PlayerData.coin;
        GameRoot.Instance.EnergyCached = PlayerData.energy;
    }

    public PlayerData GetPlayerData(string Playerid) {
        if(_playerDataDic.TryGetValue(Playerid,out PlayerData data)) {
            return data;
        } else {
            //Debug.Log("未获取到玩家数据");
            return null;
        }
    }

    public bool PlayerLogin(string ID) {
        playerID = ID;
        PlayerData = GetPlayerData(ID);
        if(PlayerData == null) {//新玩家初始数据
            PlayerData pd = new() {
                coin = 0,
                skin = new List<int> { 0 },
                trail = new List<int> { 0 },
                energy = 5,
                max_unLock_wave = 1,
                cur_skin = 0,
                cur_trail = 0
            };
            PlayerData = pd;
            return SavePlayerData();
        } 
        return true;
    }
     
    public int GetMaxUnLockWave() {
        return PlayerData.max_unLock_wave;
    }

    public bool SavePlayerData() {
        _playerDataDic[playerID] = PlayerData;
        return _resSvc.SavePlayerData(_playerDataDic);
    }

    public void SetCoin(int coin) {
        PlayerData.coin = coin;
        SavePlayerData();
    }
    public void SetEnergy(int energy) {
        PlayerData.energy = energy;
        SavePlayerData();
    }

    public void SetMaxUnLockWave(int wave) {
        PlayerData.max_unLock_wave = wave;
        SavePlayerData();
    }
}
