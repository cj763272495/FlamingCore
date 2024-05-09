using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersDataSystem : MonoBehaviour
{
    public static PlayersDataSystem Instance = null;  
    public string _playerID;//当前玩家ID
    public PlayerData PlayerData { private set; get; }//当前玩家数据
    private ResSvc _resSvc; 
    private PlayerDataDic _playerDataDic = new();//所有玩家数据

    private bool _isNewInviorment;
    public bool IsNewInviorment {
        get { return  _isNewInviorment; }
    }
      
    public void InitSys() {
        _resSvc = ResSvc.Instance;
        Instance = this; 
        //PlayerData = resSvc.GetPlayerData(playerID);
        _resSvc.LoadPlayerData(out _playerDataDic);
        _isNewInviorment = _playerDataDic.Count == 0;
        if(!IsNewInviorment) {
            _playerID = _playerDataDic.First().Key;
            PlayerData = _playerDataDic.First().Value;
        }
    }

    public PlayerData GetPlayerData(string Playerid) {
        if(_playerDataDic.TryGetValue(Playerid,out PlayerData data)) {
            return data;
        } else {
            Debug.Log("未获取到玩家数据");
            return null;
        }
    }

    public bool PlayerLogin(string ID) {
        _playerID = ID;
        PlayerData = GetPlayerData(ID);
        if(PlayerData == null) {
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

    public bool SavePlayerData() {
        if(_playerDataDic.ContainsKey(_playerID)) {
            _playerDataDic[_playerID] = PlayerData;
        } else {
            _playerDataDic.Add(_playerID,PlayerData);
        }
         return _resSvc.SavePlayerData(_playerDataDic);
    }

}
