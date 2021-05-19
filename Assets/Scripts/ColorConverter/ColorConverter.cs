using UnityEngine;

public static class ColorConverter{
    
    private static Map<int, Color> colorMap;
    
    static ColorConverter(){
        colorMap = new Map<int, Color>();
        Initialization();

    }

    private static void Initialization(){
        TextAsset json = Resources.Load<TextAsset>("ColorPalette"); // read all the file
        Colors colors = JsonUtility.FromJson<Colors>(json.text);  // read from json string
        for(int i = 0; i < colors.colors.Length; i++){
            float r, g, b;
            colors.GetRgb(i, out r, out g, out b);
            colorMap.Add(i, new Color(r, g, b, 1));
        } 
    }

    public static int ToInt(Color color){
        return colorMap.Reverse(color);
    }

    public static Color ToColor(int c){
        return colorMap.Forward(c);
    }


    

}