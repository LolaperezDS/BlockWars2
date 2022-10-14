using UnityEngine;
using UnityEngine.UI;
using SaveData;

public class GenerateNoise : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    [SerializeField] private int seed = 0;

    [SerializeField] private float power = 2f;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private float[,] heights;
    private Image rend;

    private static int[] RedMainTowerId = new int[2] { 11, 11 };
    private static int[] BlueMainTowerId = new int[2] { 0, 0 };

    [SerializeField] private GameObject buttonGet;

    void Start()
    {
        rend = GetComponent<Image>();


        Button btn = buttonGet.GetComponent<Button>();
        btn.onClick.AddListener(CalcNoise);

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        noiseTex.filterMode = FilterMode.Point;
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        CalcNoise();
    }

    public void CalcNoise()
    {
        heights = new float[pixWidth, pixHeight]; // —оздаЄм массив вершин

        // ...
        // ƒелаем с heights всЄ, что хотим
        // ...

        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float sample1 = GetNoise(x, y, scale) * 1.0f;
                float sample2 = GetNoise(x, y, scale * 2) * 0.3f;
                float sample3 = GetNoise(x, y, scale * 4) * 0.2f;

                // float sample = Mathf.Pow((sample1 + sample2 + sample3 + sample4) / 1.55f, power);
                float sample = QuinticCurve((sample1 + sample2 + sample3) / 1.50f) + power - 0.5f;
                if (sample > 1)
                {
                    sample = 1;
                }
                if (sample < 0)
                {
                    sample = 0;
                }

                heights[(int)y, (int)x] = sample;
                pix[(int)y * noiseTex.width + (int)x] = Paint(sample);
                x++;
            }
            y++;
        }

        SaveNoise();
        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private float GetNoise(float width, float height, float scale)
    {
        float sample = Mathf.PerlinNoise(seed + width / noiseTex.width * scale, seed + height / noiseTex.height * scale);
        return sample;
    }

    private void SaveNoise()
    {
        var data = new BoardData();
        int board_length = pixHeight * pixWidth;

        if (BlueMainTowerId[0] > pixHeight || BlueMainTowerId[0] < 0 || BlueMainTowerId[1] > pixWidth || BlueMainTowerId[1] < 0 || RedMainTowerId[0] > pixHeight || RedMainTowerId[0] < 0 || RedMainTowerId[1] > pixWidth || RedMainTowerId[1] < 0)
        {
            Debug.LogError("IndexOutOfRangeException");
            return;
        }

        // tiles save
        data.tiles_to_save = new tile_to_save[board_length];


        for (int i = 0; i < pixWidth; i++)
        {
            for (int j = 0; j < pixHeight; j++)
            {
                data.tiles_to_save[j + i * pixWidth].id = new int[2] { j, i };
                if (heights[i, j] > 0.75)
                {
                    data.tiles_to_save[j + i * pixWidth].type = TileType.Forest;
                }
                else if (heights[i, j] > 0.28)
                {
                    data.tiles_to_save[j + i * pixWidth].type = TileType.Default;
                }
                else
                {
                    data.tiles_to_save[j + i * pixWidth].type = TileType.River;
                }

                if (j == BlueMainTowerId[0] && i == BlueMainTowerId[1]) { data.tiles_to_save[j + i * pixWidth].command = Commands.Blue; }
                else if (j == RedMainTowerId[0] && i == RedMainTowerId[1]) { data.tiles_to_save[j + i * pixWidth].command = Commands.Red; }
                else { data.tiles_to_save[j + i * pixWidth].command = Commands.Empty; }
            }
        }

        // buildings save
        data.buildings_to_save = new building_to_save[2];

        data.buildings_to_save[0] = new building_to_save();
        data.buildings_to_save[1] = new building_to_save();

        data.buildings_to_save[0].id = BlueMainTowerId;
        data.buildings_to_save[1].id = RedMainTowerId;

        data.buildings_to_save[0].command = Commands.Blue;
        data.buildings_to_save[1].command = Commands.Red;

        data.buildings_to_save[0].type = BuildingsType.MainBuilding;
        data.buildings_to_save[1].type = BuildingsType.MainBuilding;

        data.buildings_to_save[0].healths = 0;
        data.buildings_to_save[0].healths = 0;

        // other save
        data.BoardTopology = new int[2] { pixWidth, pixHeight };

        data.gold_red = 100;
        data.gold_blue = 100;
        data.att_red = 100;
        data.att_blue = 100;

        data.current_turn = Commands.Blue;

        SaveManager.Save(data, "procedualTempData.json");
    }

    public void SetPower(float new_power)
    {
        power = new_power;
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }

    public static void SetCoordsOfTowers(int[] red, int[] blue)
    {
        RedMainTowerId = red;
        BlueMainTowerId = blue;
    } 

    private Color Paint(float value)
    {
        if (value >= 0.75)
        {
            return new Color(0, 0.5f, 0);
        }
        else if (value >= 0.28)
        {
            return new Color(0.5f, 1f, 0.5f);
        }
        return new Color(value + 0.1f, value + 0.1f, value * 2f + 0.25f);
    }

    private float QuinticCurve(float x)
    {
        return x * x * x * (x * (x * 6 - 15) + 10);
    }
}
