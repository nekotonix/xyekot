using System.IO;

public class Saver
{
    private string SaveName = "Save.cat";
    private int WaitTime = 1000 * 60 * 3;
    public int ClickMouse { get; set; } = 0;
    public int ClickKeyboard { get; set; } = 0;
    public int Deform { get; set; } = 24;

    public Saver()
    {
        GetConfig(); //init cfg
    }

    public void CreateSaveThread()
    {
        Task.Run(() => SaveThread());
    }

    public (int, int) GetConfig()
    {
        if (File.Exists(SaveName))
        {
            string dataRaw = File.ReadAllText(SaveName);
            string[] datas = dataRaw.Split("|");
            if (int.TryParse(datas[0], out var cm))
            {
                ClickMouse = cm;
            }
            if (datas.Length > 1 && int.TryParse(datas[1], out var ck))
            {
                ClickKeyboard = ck;
            }
            if (datas.Length > 2 && int.TryParse(datas[2], out var df))
            {
                Deform = df;
            }
        }
        return (ClickMouse, ClickKeyboard);
    }
    public void ForceSave()
    {
        File.WriteAllText(SaveName, $"{ClickMouse}|{ClickKeyboard}|{Deform}");
    }

    //тред для обновления. Ждёмс форса или 3 минуты бездействия
    private void SaveThread()
    {
        while (true)
        {
            Thread.Sleep(WaitTime);
            File.WriteAllText(SaveName, $"{ClickMouse}|{ClickKeyboard}");
        }
    }


}