[System.Serializable]
public class Colors{
    public ColorPallete[] colors;

    public void GetRgb(int index, out float r, out float g, out float b){
        r = colors[index].rgb[0]/255f;
        g = colors[index].rgb[1]/255f;
        b = colors[index].rgb[2]/255f;
    }
    
}