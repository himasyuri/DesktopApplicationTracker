namespace DesktopTracker.Utils
{
    internal interface IJsonUtil
    {
        T JsonToItem<T>(string jsonString);

        (bool result, Exception? exception) JsonToFile<T>(T sender, string fileName, bool format);
    }
}
