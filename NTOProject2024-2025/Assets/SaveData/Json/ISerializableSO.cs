public interface ISerializableSO
{
    string SerializeToJson(); // Преобразование в JSON
    void DeserializeFromJson(string json); // Заполнение из JSON
}

