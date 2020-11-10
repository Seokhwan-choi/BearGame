using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
public class GoogleManager : MonoBehaviour
{
    /* 구글로그인 */
    public Text LogText;

    private void Start()
    {
        //자기구글계정에 세이브기능
        //var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        //PlayGamesPlatform.InitializeInstance(config);

        //구글계정 로그인
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();

    }

    public void LogIn()
    {
        //Social.localUser.Authenticate(LoginMethod);
        Social.localUser.Authenticate(LoginResult);

        void LoginResult(bool success)
        {
            if (success) { LogText.text = Social.localUser.id + " \n " + Social.localUser.userName; }
            else { LogText.text = success+" /구글 로그인 실패"; }
        }
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        LogText.text = "구글 로그아웃";
    }


    #region 클라우드 저장

    ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }


    public void LoadCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
    }

    void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
            SavedGame().ReadBinaryData(game, LoadData);
    }

    void LoadData(SavedGameRequestStatus status, byte[] LoadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(LoadedData);
            LogText.text = data;
        }
        else LogText.text = "로드 실패";
    }



    public void SaveCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, SaveGame);
    }

    public void SaveGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("흐하하");
            SavedGame().CommitUpdate(game, update, bytes, SaveData);
        }
    }

    void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            LogText.text = "저장 성공";
        }
        else LogText.text = "저장 실패";
    }



    public void DeleteCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, DeleteGame);
    }

    void DeleteGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame().Delete(game);
            LogText.text = "삭제 성공";
        }
        else LogText.text = "삭제 실패";
    }

    #endregion




    /* 업적 */
    public InputField ScoreInput;

    public void ShowAchievementUI() => Social.ShowAchievementsUI();

    //100으로 넣으면 프로그래스(업적달성 퍼센트)가 바로 열림
    public void UnlockAchievement_1() => Social.ReportProgress(GPGSIds.achievement_one, 100, (bool success) => { });
    public void UnlockAchievement_2() => Social.ReportProgress(GPGSIds.achievement_two, 100, (bool success) => { });

    //스텝 단계를 주는 함수?를 사용함. 1씩 총 10번 사용하면 해금됨
    public void UnlockAchievement_3() => PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_three, 1, (bool success) => { });


    public void ShowLeaderboardUI() => Social.ShowLeaderboardUI();
    
    //순위표 한개만 보기
    public void ShowLeaderboardUI_4() => ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_four);

    //필드에 입력받은 값을 받아서 순위표에 등록
    public void AddLeaderboard_5() => Social.ReportScore(int.Parse(ScoreInput.text), GPGSIds.leaderboard_five, (bool success) => { });
}
