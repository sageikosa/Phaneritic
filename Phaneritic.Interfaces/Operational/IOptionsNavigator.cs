namespace Phaneritic.Interfaces.Operational;
public interface IOptionsNavigator
{
    Dictionary<OptionTypeKey, OptionValue> GetDirectOptions(ProcessNodeKey processNodeKey);
    Dictionary<OptionTypeKey, OptionDto> GetEffectiveOptions(ProcessNodeKey processNodeKey);
    OptionValue? GetOption(ProcessNodeKey processNodeKey, OptionTypeKey optionTypeKey);
}
