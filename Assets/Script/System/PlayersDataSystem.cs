using System.Collections.Generic; 
using UnityEngine;

public class PlayersDataSystem : MonoBehaviour
{
    public static PlayersDataSystem Instance = null;  
    public string _playerID;//��ǰ���ID
    public PlayerData PlayerData { private set; get; }//��ǰ�������
    private ResSvc _resSvc; 
    private PlayerDataDic _playerDataDic = new();//�����������

    private bool _isNewInviorment;
    public bool IsNewInviorment {
        get { return  _isNewInviorment; }
    }
      
    public void InitSys() {
        _resSvc = ResSvc.Instance;
        Instance = this;  
        _resSvc.LoadPlayerData(out _playerDataDic);
        _isNewInviorment = _playerDataDic.Count == 0;
        if(!IsNewInviorment) {
            var firstPlayerData = _playerDataDic.GetEnumerator();
            firstPlayerData.MoveNext();
            _playerID = firstPlayerData.Current.Key;
            PlayerData = firstPlayerData.Current.Value;
        }
    }

    public PlayerData GetPlayerData(string Playerid) {
        if(_playerDataDic.TryGetValue(Playerid,out PlayerData data)) {
            return data;
        } else {
            Debug.Log("δ��ȡ���������");
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
        _playerDataDic[_playerID] = PlayerData;
        return _resSvc.SavePlayerData(_playerDataDic);
    }

}
