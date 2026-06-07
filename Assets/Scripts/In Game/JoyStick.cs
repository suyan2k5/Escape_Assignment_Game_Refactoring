using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원
using Photon.Pun;
using Photon.Realtime;

public class JoyStick : MonoBehaviourPunCallbacks, IPointerDownHandler, IPointerUpHandler
{
    // 조이스틱 구현 스크립트
    #region declare variables
    [SerializeField] private Transform stick;        // 조이스틱 막대기
    float stickRange;  // 막대기 이동반경

    [SerializeField, Range(0f, 15f)] private float moveSpeed;   // 캐릭터 이동속도 5

    public GameObject character;    // 캐릭터

    public Vector2 stickDir;        // 막대기가 드래그된 방향
    Vector2 center;
    private bool isInput;           // 막대기가 드래그되고 있나?
    PointerEventData ed;
    public PhotonView PV;
    public PlayerMulti playerMulti;

    // Image
    public Sprite stickgroundImg;
    public Sprite stickImg;
    public Sprite none;

    // Joystick Fix Optioin
    public bool fixOption;

    #endregion
    private void Awake()
    {
        isInput = false;
        
        RectTransform rectTr = gameObject.GetComponent<RectTransform>();
        stickRange = rectTr.rect.width * rectTr.localScale.x * 0.3f;

        imgSet(0);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isInput = true;
        ed = eventData;

        if(!fixOption)
        {
            gameObject.transform.parent.position = (Vector2)Camera.main.ScreenToWorldPoint(ed.position);

            imgSet(1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isInput = false;

        if(!fixOption)
        {
            gameObject.transform.parent.localPosition = new Vector2(746f, -355f);

            imgSet(0);
        }
    }

    public void imgSet(int mode)
    {
        // mode == 0 => delete
        if(mode == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = none;
            stick.GetComponent<SpriteRenderer>().sprite = none;
        }
        // mode == 1 => fill
        else if(mode == 1)
        {
            // gameObject.GetComponent<SpriteRenderer>().sprite = stickgroundImg;
            stick.GetComponent<SpriteRenderer>().sprite = stickImg;
        }
    }

    // 드래그 종료 시 막대를 원점으로 초기화 하는 함수
    void setzero()
    {
        stick.transform.localPosition = Vector2.zero;
        stickDir = Vector2.zero;
        if(playerMulti != null) playerMulti.stickDir = Vector2.zero;
    }

    // 조이스틱 막대기 드래그 방향에 따라 플레이어 이동 함수
    public void JoyStickControl(Vector2 touch)
    {
        // 드래그하는 커서 방향으로 막대기가 움직이도록 함
        center = gameObject.transform.position;
        Vector2 inputDir = (touch - center) * stickRange;  // 조이스틱 중심으로부터 드래그 중의 마우스 위치까지의 방향 + 크기
        
        // 막대기의 방향 추출
        if(inputDir.magnitude < stickRange * 0.2f) stickDir = Vector2.zero; // 작은 조이스틱 컨트롤은 움직이지 않도록 함...
        else stickDir = inputDir.normalized;

        // 조이스틱 영역을 벗어난 경우는 최대치까지만 스틱이 이동하도록 함
        Vector2 clampedDir = inputDir.magnitude < stickRange ? inputDir : stickDir * stickRange;
        stick.transform.localPosition = clampedDir;

        //isInput = true;
    }

    // 드래그 중인 경우 캐릭터 이동
    void Update()
    {
        if(character == null) return;
        
        if(isInput) JoyStickControl(Camera.main.ScreenToWorldPoint(ed.position));

        if (isInput && character.GetComponent<PhotonView>().IsMine)
        {
            playerMulti = character.GetComponent<PlayerMulti>();
            playerMulti.stickDir = stickDir;
        }
        else
        {
            setzero();
        }
    }
}
