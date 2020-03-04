using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボード関連の処理を行う
/// ボード/光の柱生成、システム管理
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize;

    private BoardFactory boardFactory;
    private LightPillarFactory lightPillarFactory;
    private GameObject[,] lightPillars;

    private void Start()
    {
        boardFactory = GetComponentInChildren<BoardFactory>();
        lightPillarFactory = GetComponentInChildren<LightPillarFactory>();

        // ボード/光の柱作成
        var board = boardFactory.Create(boardSize.x, boardSize.y);
        lightPillars = lightPillarFactory.Create(boardSize.x, boardSize.y);

        // 
    }
}
