using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class SlilngShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer LeftLineRenderer;
    [SerializeField] private LineRenderer RightLineRenderer;

    [Header("Transform Refrences")]
    [SerializeField] private Transform LeftStartPosition;
    [SerializeField] private Transform RightStartPosition;
    [SerializeField] private Transform CenterPosition;
    [SerializeField] private Transform IdlePosition;
    [SerializeField] private Transform ElasticTransform;

    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance =3.5f;
    [SerializeField] private float shotForce =9f;
    [SerializeField] private float timeBetweenBirdRespown = 2f;
    [SerializeField] private float elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;
    [SerializeField] private float maxAnimationTime = 1f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingShotArea;
    [SerializeField] private CameraManager cameraManager;

    [Header("Bird")]
    [SerializeField] private RedBird redBirdPreFab;
    [SerializeField] private float redBirdPositionOffset = 2f;

    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPulledClip;
    [SerializeField] private AudioClip[] elasticReleasedClip;
    private Vector2 SlingShotLinesPosition;
    private Vector2 direction;
    private Vector2 directionNormalized;
    private bool ClickedWithInArea;
    private bool bridOnSlingShot;
    private RedBird spawnedRedBird;
    private AudioSource _audioSource;
    void Start()
    {
        
    }
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        LeftLineRenderer.enabled = false ;
        RightLineRenderer.enabled = false;
        
        SpawnRedBird();
        }
    

    // Update is called once per frame
    void Update()
    {
        if ( InputManager.WasLeftMouseButtonPressed && slingShotArea.IsWithInSlingShotArea())
        {
            ClickedWithInArea= true;
            

            if(bridOnSlingShot){
                SoundManager.instance.PlayClip(elasticPulledClip, _audioSource);
                cameraManager.SwitchToFollowCam(spawnedRedBird.transform);
            }
        }

        if ( InputManager.IsLeftMouseButtonPressed && ClickedWithInArea && bridOnSlingShot)
        {
            DrawSlingShot();
            PositionandRotateBird();
        }
        if (InputManager.WasLeftMouseButtonReleased && bridOnSlingShot && ClickedWithInArea)
        {
            if (GameManager.instance.HasEnoughShot()){
                ClickedWithInArea = false;
                spawnedRedBird.LaunchBird(direction, shotForce);

                SoundManager.instance.PlayRandomClip(elasticReleasedClip, _audioSource);
                GameManager.instance.UseShot();
                bridOnSlingShot = false;
                AnimateSlingshot();
                if(GameManager.instance.HasEnoughShot()){
                    StartCoroutine(SpawnRedbirdAfterTime());
                }
                
            }
            
        }
    }
    #region SlingShot Methods
    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        SlingShotLinesPosition = CenterPosition.position + Vector3.ClampMagnitude(touchPosition - CenterPosition.position, _maxDistance);
        SetLines(SlingShotLinesPosition);

        direction = (Vector2)CenterPosition.position - SlingShotLinesPosition;
        directionNormalized = direction.normalized;
    }
    private void SetLines(Vector2 position)
    {
        if(!LeftLineRenderer.enabled && !RightLineRenderer.enabled)
        {
            LeftLineRenderer.enabled = true;
            RightLineRenderer.enabled=true;
        }
        LeftLineRenderer.SetPosition(0,position);
        LeftLineRenderer.SetPosition(1,LeftStartPosition.position);
        RightLineRenderer.SetPosition(0,position);
        RightLineRenderer.SetPosition(1,RightStartPosition.position);

    }
    #endregion

    #region Red Bird
     private void SpawnRedBird(){
        ElasticTransform.DOComplete();
        SetLines(IdlePosition.position);
        Vector2 dir = (CenterPosition.position -IdlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)IdlePosition.position + dir * redBirdPositionOffset;
        spawnedRedBird = Instantiate(redBirdPreFab, spawnPosition, Quaternion.identity);
        spawnedRedBird.transform.right = dir;
        bridOnSlingShot = true;
     }
    private void PositionandRotateBird(){
        spawnedRedBird.transform.position = SlingShotLinesPosition + directionNormalized * redBirdPositionOffset;
        spawnedRedBird.transform.right = directionNormalized;
    }
    private IEnumerator SpawnRedbirdAfterTime(){
        yield return new WaitForSeconds(timeBetweenBirdRespown);
        //some code run after some amount of time
        SpawnRedBird();
        cameraManager.SwitchToIdleCam();
    }

    #endregion
   
    #region Animate SlingShot
    private void AnimateSlingshot () {
        ElasticTransform.position =LeftLineRenderer.GetPosition(0);
        float dist = Vector2.Distance(ElasticTransform.position, CenterPosition.position);

        float time = dist / elasticDivider;

        ElasticTransform.DOMove(CenterPosition.position,time).SetEase(elasticCurve);
        StartCoroutine(AnimateSlingShotLine(ElasticTransform,time));
    }
    private IEnumerator AnimateSlingShotLine(Transform trans, float time){
        float elasepedTime = 0f;
        while(elasepedTime < time && elasepedTime < maxAnimationTime){
            elasepedTime += Time.deltaTime;

            SetLines(trans.position);
            yield return null;
        }
    }
    #endregion
}
