using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BulletHoleDecal : MonoBehaviour
{
    [Tooltip("X: columns, Y: rows")]
    public Vector2Int frames = new Vector2Int(2, 2); // 2x2 프레임
    public float lifetime = 5f;
    public float fadeOutTime = 1f;

    private float timer;
    private Material mat;
    private Color originalColor;

    void OnEnable()
    {
        // 랜덤 프레임 선택
        int totalFrames = frames.x * frames.y;
        int rand = Random.Range(0, totalFrames);
        int fx = rand % frames.x;
        int fy = rand / frames.x;

        Vector2[] baseUVs = new Vector2[]
        {
            new Vector2(0,0), new Vector2(0,1),
            new Vector2(1,0), new Vector2(1,1)
        };

        Vector2[] newUVs = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            newUVs[i].x = (baseUVs[i].x + fx) / frames.x;
            newUVs[i].y = (baseUVs[i].y + fy) / frames.y;
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh = Instantiate(mesh); // 원본 파괴 방지
        mesh.uv = newUVs;
        GetComponent<MeshFilter>().mesh = mesh;

        // 페이드 초기화
        mat = GetComponent<Renderer>().material;
        originalColor = mat.GetColor("_TintColor");
        originalColor.a = 1f;
        mat.SetColor("_TintColor", originalColor);
        timer = lifetime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= fadeOutTime)
        {
            float t = timer / fadeOutTime;
            Color c = originalColor;
            c.a = Mathf.Clamp01(t);
            mat.SetColor("_TintColor", c);
        }

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}