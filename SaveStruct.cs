using System.IO;
using System.Windows;

public class CatSave
{
    private string _filename;
    private int MouseModif = 12;//умножаем чтобы уменьшить читерство
    private int KeyBoardModif = 18;

    public CatSave(string filename)
    {
        _filename = filename;
        ReadSave();
    }

    public void WriteSave()
    {
        string savedata = $"{ClickMouse* MouseModif}|{ClickKeyboard * KeyBoardModif}|{Deform}|{DailyClicks}|{LastPosX}|{LastPosY}";
        try
        {
            using (StreamWriter writer = new StreamWriter(_filename))
            {
                writer.WriteLine(savedata);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file. Write it by yourself: \n{savedata}","CAT::ERROR");
        }
    }
    
    public void ReadSave()
    {
        if (!File.Exists(_filename))
            return;

        try
        {
            string content = File.ReadAllText(_filename).Trim();
            string[] values = content.Split('|');

            if (values.Length >= 6)
            {
                ClickMouse = int.Parse(values[0])/ MouseModif;
                ClickKeyboard = int.Parse(values[1])/KeyBoardModif;
                Deform = int.Parse(values[2]);
                DailyClicks = int.Parse(values[3]);
                LastPosX = int.Parse(values[4]);
                LastPosY = int.Parse(values[5]);
            }
            else
            {
                WriteSave();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Save corrupted", "CAT::ERROR");
        }
    }

    public int ClickMouse { get; set; } = 0;
    public int ClickKeyboard { get; set; } = 0;
    public int Deform { get; set; } = 24;
    public int DailyClicks { get; set; } = 24;
    public double LastPosX { get; set; } = 500.0;
    public double LastPosY { get; set; } = 500.0;
}