using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject restartScreenObject;
    [SerializeField] private SlilngShotHandler slingShotHandler;
    [SerializeField] private Image nextLevelImage;
    public static GameManager instance;
    public int MaxNumberOfShot =3;
    private int UsedNumberOfShot;
     private IconHandler iconHandler;
     private List<Piggie> _piggies = new List<Piggie>();
    void Awake()
    {
        if (instance == null){
            instance = this;
        }
         iconHandler = FindObjectOfType<IconHandler>();

         Piggie[] piggies = FindObjectsOfType<Piggie>();
         for(int i = 0; i < piggies.Length; i++) {
            _piggies.Add(piggies[i]);
         }
        nextLevelImage.enabled = false ;
    }
    public void UseShot(){
        UsedNumberOfShot++;
        iconHandler.UseShot(UsedNumberOfShot);
        
        CheckForLastShot();
   
    }
    public bool HasEnoughShot(){
        if(UsedNumberOfShot < MaxNumberOfShot){
            return true;
        }
        else{
            return false;
        }
    }
    public void CheckForLastShot(){
        if(UsedNumberOfShot == MaxNumberOfShot){
            StartCoroutine(checkAfterWaitTime());
        }
    }
    private IEnumerator checkAfterWaitTime(){
        yield return new WaitForSeconds(secondsToWaitBeforeDeathCheck);
        if(_piggies.Count ==0){
            WinGame();
        }
        else{
            LoseGame();
        }
    }

    public void RemovePiggie( Piggie piggie){
        _piggies.Remove(piggie);
        CheckForAllDeadPiggies();
    }
    private void CheckForAllDeadPiggies(){
        if(_piggies.Count ==0){
            WinGame();
        }
        
    }
    #region Win/Lose

    private void WinGame(){
        restartScreenObject.SetActive(true);
        slingShotHandler.enabled =false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        if(currentSceneIndex + 1 < maxLevels){
            nextLevelImage.enabled = true;
        }
    }
    public void LoseGame(){
        DOTween.Clear(true);
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
    public void NextLevel () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
    }

}

    
