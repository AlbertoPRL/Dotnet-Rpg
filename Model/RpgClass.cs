using System.Text.Json.Serialization;

namespace dotnet_rpg.Model

{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight =1,
        Mage =2,
        Clreric=3
    }
}