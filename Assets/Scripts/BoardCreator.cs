using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

public class BoardCreator : MonoBehaviour
{
    private readonly float boardBaseMargin = 0.5f;
    private readonly float lineHeight = 0.12f;

    [SerializeField] private GameObject boardBasePrefab;
    [SerializeField] private GameObject linePrefab;

    private void Start()
    {
        Create(3, 3);
    }

    public void Create(int width, int height)
    {
        Assert.IsTrue(width > 0);
        Assert.IsTrue(height > 0);

        CreateBoardBase(width, height);
        CreateLines(width, height);
    }

    private void CreateBoardBase(int width, int height)
    {
        var boardBase = Instantiate(boardBasePrefab, transform);
        var scale = boardBase.transform.localScale;
        scale.x = width + boardBaseMargin;
        scale.z = height + boardBaseMargin;
        boardBase.transform.localScale = scale;
    }

    private void CreateLines(int width, int height)
    {
        var XEdgePos = width / 2.0f;
        var ZEdgePos = height / 2.0f;
        for(int i = 0; i <= width; ++i)
        {
            var obj = Instantiate(linePrefab,
                new Vector3(i - XEdgePos, lineHeight, 0),
                Quaternion.identity, transform);
            var scale = obj.transform.localScale;
            scale.z = height * 0.1f + 0.01f; // *0.1f for adjust size scale for plane, 0.01f for reach to edge
            obj.transform.localScale = scale;
        }
        for (int i = 0; i <= height; ++i)
        {
            var obj = Instantiate(linePrefab,
                new Vector3(0, lineHeight, i - ZEdgePos),
                Quaternion.identity, transform);
            var scale = obj.transform.localScale;
            scale.x = width * 0.1f + 0.01f; // *0.1f for adjust size scale for plane, 0.01f for reach to edge
            obj.transform.localScale = scale;
        }
    }
}
