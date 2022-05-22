using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TextHighlighting : MonoBehaviour
   {
       [FormerlySerializedAs("HighlightTexture")] 
       public Texture highlightTexture;
       
       [FormerlySerializedAs("HighlightSize")] 
       public float highlightSize = 5;
      
       [FormerlySerializedAs("HighlightHeight")] 
       public float highlightHeight = 5;
       
       private TextMeshPro _textComponent;
       
       private GameObject _wordHighlighter;
       private Mesh _mesh;
       private TMP_MeshInfo _meshInfo;
       private MeshFilter _meshFilter;
       private MeshRenderer _meshRenderer;
       private Material _wavyLineMaterial;

       public void HighlightTextCharacters(int start, int end)
       {
           if (_textComponent == null || _wordHighlighter == null)
               return;

           if (_meshInfo.vertices == null)
               _meshInfo = new TMP_MeshInfo(_mesh, 4);

           if (start > end)
           {
               (start, end) = (end, start);
           }
           
           if(start < 0 || end >= _textComponent.textInfo.characterCount) 
               return;

           var textInfo = _textComponent.textInfo;
           var firstChar = textInfo.characterInfo[start];
           var lastChar = textInfo.characterInfo[end];
           var wordScale = firstChar.scale * _textComponent.fontSize;

           var bottomLeft = new Vector3(firstChar.bottomLeft.x, firstChar.baseLine + highlightHeight * wordScale, 0);
           var topLeft = new Vector3(bottomLeft.x, firstChar.baseLine, 0);
           var topRight = new Vector3(lastChar.topRight.x, topLeft.y, 0);
           var bottomRight = new Vector3(topRight.x, bottomLeft.y, 0);

           var vertices = _meshInfo.vertices;
           vertices[0] = bottomLeft;
           vertices[1] = topLeft;
           vertices[2] = topRight;
           vertices[3] = bottomRight;
           
           var uvs0 = _meshInfo.uvs0;
           var length = Mathf.Abs(topRight.x - bottomLeft.x) / wordScale * highlightSize;
           var tiling = length / (highlightTexture == null ? 1 : highlightTexture.width);
 
           uvs0[0] = new Vector2(0, 0);
           uvs0[1] = new Vector2(0, 1);
           uvs0[2] = new Vector2(tiling, 1);
           uvs0[3] = new Vector2(tiling, 0);

           _mesh.vertices = _meshInfo.vertices;
           _mesh.uv = _meshInfo.uvs0;
           _mesh.RecalculateBounds();
       }
       
       private void Awake()
       {
           _textComponent = GetComponent<TextMeshPro>();

           if (_mesh == null)
           {
               _mesh = new Mesh();
               _meshInfo = new TMP_MeshInfo(_mesh, 4);
           }

           if (_wordHighlighter != null) 
               return;
           
           _wordHighlighter = new GameObject();
           _wavyLineMaterial = new Material(Shader.Find("TextMeshPro/Sprite"));
           _meshRenderer = _wordHighlighter.AddComponent<MeshRenderer>();
           _meshFilter = _wordHighlighter.AddComponent<MeshFilter>();
           
           _wordHighlighter.transform.SetParent(this.transform, false);
           _wavyLineMaterial.SetTexture(ShaderUtilities.ID_MainTex, highlightTexture);
           _meshRenderer.sharedMaterial = _wavyLineMaterial;
           _meshFilter.mesh = _mesh;
       }
       
       private void Start()
       {
           // _textComponent.ForceMeshUpdate();
           // HighlightTextCharacters(0, 4);
       }

       private void OnDestroy()
       {
           if (_mesh != null)
               DestroyImmediate(_mesh);
       }
   }
