using DijitaruVatigoGengar.Enums;

namespace DijitaruVatigoGengar.Extensions;

public static class ContractModalityDictionary
{
    private static Dictionary<string, ContractModality> map = new Dictionary<string, ContractModality>
        {
            {"Clt", ContractModality.Clt},
            {"Pj", ContractModality.Pj},
            {"Intern", ContractModality.Intern}
        };
    public static string ModalityToString(this ContractModality contract)
    {
        return map.First(c => c.Value == contract).Key;
    }

    public static ContractModality ModalityToValue(this string text)
    {
        return map.First(c => c.Key == text).Value;
    }
}
