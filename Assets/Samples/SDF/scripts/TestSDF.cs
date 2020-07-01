using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestSDF : MonoBehaviour {
    public GameObject goChar1;
    public GameObject goChar2;
    [Range(0.4f, 2f)] public float fadeDuration = 0.7f;
    [Range(0.4f, 2f)] public float transformDuration = 0.7f;

    public const float VISIBLE = 1f;
    public const float HIDDEN  = 0f;

    protected List<Material> mat1;
    protected List<Material> mat2;

    protected void Start () {
        mat1 = new List<Material>();
        CollectMatFor( goChar1, mat1 );

        mat2 = new List<Material>();
        CollectMatFor( goChar2, mat2 );

        goChar1.SetActive( true );
        goChar2.SetActive( false );

        DOTween.Sequence()
            .Append(
                DOVirtual.Float(VISIBLE, HIDDEN, fadeDuration, (t) => {
                    foreach (Material material in mat1) {
                        material.SetFloat( "_ClipYAmount", t );
                    }
                })
            )
            .AppendInterval( transformDuration )
            .AppendCallback( () => {
                goChar1.SetActive( false );
                goChar2.SetActive( true );
            })
            .Append(
                DOVirtual.Float(HIDDEN, VISIBLE, fadeDuration, (t) => {
                    foreach (Material material in mat2) {
                        material.SetFloat( "_ClipYAmount", t );
                    }
                })
            )
        ;
    }

    protected void CollectMatFor (GameObject goChar, List<Material> matList) {
        foreach (SkinnedMeshRenderer renderer in goChar.GetComponentsInChildren<SkinnedMeshRenderer>()) {
            foreach (Material material in renderer.materials) {
                matList.Add( material );
            }
        }
    }

    // public GameObject VFX;
    // private Texture2D pointCache;
    // private float size;
    // void UpdateCachePointFor (GameObject goChar) {
    //     Mesh baked;
    //     Vector3[] vertices;
    //     Transform parent;
    //     SkinnedMeshRenderer[] renderers = goChar.GetComponentsInChildren<SkinnedMeshRenderer>();
    //     List<Color> normalizedVertices = new List<Color>();
    //     float inversedSize = 1f / size;
    //     Transform tChar = goChar.transform;
    //     Vector3 normalSize = new Vector3( size * 0.5f, 0f, size * 0.5f );
    //     foreach (SkinnedMeshRenderer renderer in renderers) {
    //         Transform tRenderer = renderer.transform;
    //         parent = tRenderer.parent;
    //         baked  = new Mesh();
    //         renderer.BakeMesh( baked );
    //         vertices = baked.vertices;
    //         for (int i = 0; i < vertices.Length; i++) {
    //             vertices[i] = (tChar.InverseTransformPoint( tRenderer.TransformPoint(vertices[i])) + normalSize ) * inversedSize;
    //             normalizedVertices.Add( new Color(vertices[i].x, vertices[i].y, vertices[i].z) );
    //         }
    //     }
    //     if (pointCache == null || pointCache.width != normalizedVertices.Count) {
    //         pointCache = new Texture2D(1, normalizedVertices.Count, TextureFormat.RGBA32, false, true);
    //         pointCache.filterMode = FilterMode.Point;
    //     }
    //     pointCache.SetPixels(normalizedVertices.ToArray());
    //     pointCache.Apply();
    // }
}
