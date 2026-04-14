using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public FoodData foodData;
    [HideInInspector] public OmprengGrid targetGrid;

    public TextMeshProUGUI namaText; // Referensi teks di UI

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalPos;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // --- FUNGSI BARU UNTUK INTEGRASI ---
    public void Setup(FoodData data, OmprengGrid grid, float cellSize)
    {
        foodData = data;
        targetGrid = grid;

        // Otomatis ubah ukuran UI berdasarkan data CSV! (Misal: 2x2 jadi 200x200 pixel)
        rectTransform.sizeDelta = new Vector2(data.gridWidth * cellSize, data.gridHeight * cellSize);
        
        if (namaText != null) namaText.text = data.foodName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform); 
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // KITA KIRIMKAN eventData.position KE DALAM FUNGSI GRID!
        if (targetGrid.TryPlaceFoodUI(this, eventData.position))
        {
            // Berhasil ditaruh di piring
        }
        else
        {
            // Gagal, kembali ke inventaris
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPos;
        }
    }
}