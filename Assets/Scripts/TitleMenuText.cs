using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TitleMenuText : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public float animationTime = 2f;
    public float waveHeight = 10f;
    public float wavesPerSecond = 1f;
    private float timer;
    private Vector3[] originalVertices;
    private TMP_TextInfo textInfo;


    public AnimationCurve waveCurve;

    void Start()
    {
        textInfo = titleText.textInfo;
        titleText.ForceMeshUpdate();
        CacheOriginalVertices();
    }

    void CacheOriginalVertices()
    {
        int totalVertices = textInfo.characterCount * 4;
        originalVertices = new Vector3[totalVertices];
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            for (int j = 0; j < 4; j++)
            {
                originalVertices[vertexIndex + j] = textInfo.meshInfo[0].vertices[vertexIndex + j];
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        timer %= animationTime;
        
        titleText.ForceMeshUpdate();
        var meshInfo = textInfo.meshInfo[0];

        float timerAsIndex = timer / animationTime;
        timerAsIndex *= textInfo.characterCount;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            float distanceToCurrentCharacter = Mathf.Min(
                Mathf.Abs(i - timerAsIndex),
                Mathf.Abs(i - timerAsIndex + textInfo.characterCount),
                Mathf.Abs(i - timerAsIndex - textInfo.characterCount)
            );
            float  inverseDistanceClamped = 3- distanceToCurrentCharacter;
            inverseDistanceClamped = Mathf.Clamp(inverseDistanceClamped, 0, 3);

            float wave =  waveCurve.Evaluate(inverseDistanceClamped/3f)   * waveHeight;

            for (int j = 0; j < 4; j++)
            {
                Vector3 offset = Vector3.up * wave;
                meshInfo.vertices[vertexIndex + j] = originalVertices[vertexIndex + j] + offset;
            }
        }

        titleText.UpdateVertexData();
    }
}