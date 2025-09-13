using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    public PlayerMovement ai;
    public CameraFollow gameCamera;

    [Header("UI Elements")]
    public Button rollButton;
    public Button endTurnButton;

    private bool isPlayerTurn = true;

    void Start()
    {
        rollButton.onClick.AddListener(OnRollButtonClicked);
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
        SetupPlayerTurn();
    }

    void OnRollButtonClicked()
    {
        if (isPlayerTurn)
        {
            StartCoroutine(PlayerTurnSequence());
        }
    }

    void OnEndTurnButtonClicked()
    {
        if (!isPlayerTurn) // ตรวจสอบอีกชั้นว่าใช่รอบ AI จริงๆ
        {
            StartCoroutine(AITurnSequence());
        }
    }

    private IEnumerator PlayerTurnSequence()
    {
        isPlayerTurn = false;
        rollButton.gameObject.SetActive(false);

        int stepsToMove = Random.Range(1, 7) + Random.Range(1, 7);
        Debug.Log($"ผู้เล่นทอยได้: {stepsToMove} แต้ม");

        gameCamera.StartFollowing(player.transform);
        yield return new WaitForSeconds(0.5f);
        player.StartMoveSequence(stepsToMove);
        yield return new WaitUntil(() => player.isMoving == false);

        // --- ส่วนที่แก้ไข ---
        // สั่งให้กล้องกลับที่เดิม "ทันที" ที่ผู้เล่นเดินเสร็จ
        gameCamera.StopFollowing();
        yield return new WaitForSeconds(1.0f); // รอให้กล้องซูมออกเสร็จ

        endTurnButton.gameObject.SetActive(true);
    }

    private IEnumerator AITurnSequence()
    {
        endTurnButton.gameObject.SetActive(false);

        int stepsToMove = Random.Range(1, 7) + Random.Range(1, 7);
        Debug.Log($"AI ทอยได้: {stepsToMove} แต้ม");

        gameCamera.StartFollowing(ai.transform);
        yield return new WaitForSeconds(0.5f);
        ai.StartMoveSequence(stepsToMove);
        yield return new WaitUntil(() => ai.isMoving == false);

        // --- ส่วนที่แก้ไข ---
        // สั่งให้กล้องกลับที่เดิม "ทันที" ที่ AI เดินเสร็จ
        gameCamera.StopFollowing();
        yield return new WaitForSeconds(1.0f); // รอให้กล้องซูมออกเสร็จ

        SetupPlayerTurn();
    }

    void SetupPlayerTurn()
    {
        isPlayerTurn = true;
        rollButton.gameObject.SetActive(true);
        endTurnButton.gameObject.SetActive(false);
    }
}
