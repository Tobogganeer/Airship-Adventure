using UnityEngine;

public class MapColours : MonoBehaviour
{
    private static MapColours _instance;
    public static MapColours instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MapColours>();

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private Color tundra = Get(188, 13, 87);
    [SerializeField] private Color grasslands = Get(67, 38, 82);
    [SerializeField] private Color desert = Get(46, 60, 100);

    [SerializeField] private Color bareForest = Get(18, 44, 100);
    [SerializeField] private Color drylands = Get(19, 50, 54);
    [SerializeField] private Color savanna = Get(66, 50, 44);

    [SerializeField] private Color moderateForest = Get(103, 61, 68);
    [SerializeField] private Color lushForest = Get(123, 71, 87);
    [SerializeField] private Color jungle = Get(101, 85, 61);



    public static Color Tundra => instance.tundra;
    public static Color Grasslands => instance.grasslands;
    public static Color Desert => instance.desert;

    public static Color BareForest => instance.bareForest;
    public static Color Drylands => instance.drylands;
    public static Color Savanna => instance.savanna;

    public static Color ModerateForest => instance.moderateForest;
    public static Color LushForest => instance.lushForest;
    public static Color Jungle => instance.jungle;

    private static Color Get(float h, float s, float v)
    {
        //360, 100, 100
        return Color.HSVToRGB(h / 360f, s / 100f, v / 100f);
    }
}
