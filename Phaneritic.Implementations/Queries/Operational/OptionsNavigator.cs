using Phaneritic.Interfaces;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Queries.Operational;
public class OptionsNavigator(
    ILudDictionary<ProcessNodeTypeKey, ProcessNodeTypeDto> procNodeTypes,
    ILudDictionary<ProcessNodeKey, ProcessNodeDto> procNodes,
    ILudDictionary<OptionTypeKey, OptionTypeDto> optionTypes,
    ICanonicalDictionary<(ProcessNodeKey ProcessNodeKey, OptionTypeKey OptionTypeKey), OptionValue?> optionCache
    ) : IOptionsNavigator
{
    public Dictionary<OptionTypeKey, OptionValue> GetDirectOptions(ProcessNodeKey processNodeKey)
        => procNodes.Get(processNodeKey)?.Options?
        .ToDictionary() ?? [];

    protected void FillAllOptions(
        ProcessNodeDto? processNode,
        ProcessNodeTypeDto targetProcessNodeType,
        HashSet<ProcessNodeKey> processed,
        Dictionary<OptionTypeKey, OptionDto> collector
        )
    {
        if (processNode == null)
        {
            return;
        }

        if (processed.Contains(processNode.ProcessNodeKey))
        {
            // oops!
            // TODO: log
            return;
        }

        // look at each option
        foreach (var _opt in processNode.Options!)
        {
            // do not add if one was found closer to original process node
            if (!collector.ContainsKey(_opt.Key))
            {
                // get it's type
                if (optionTypes.Get(_opt.Key) is OptionTypeDto _optType)
                {
                    // ensure type is valid for target
                    var _ogks = _optType.ValidOptionGroups;
                    if (targetProcessNodeType.ValidOptionGroups.Overlaps(_ogks))
                    {
                        collector.Add(_opt.Key,
                            new OptionDto
                            {
                                OptionTypeKey = _opt.Key,
                                OptionValue = _opt.Value,
                                ProcessNodeKey = processNode.ProcessNodeKey
                            });
                    }
                }
            }
        }

        // sanity assurance
        processed.Add(processNode.ProcessNodeKey);

        // try to go to parent and repeat
        FillAllOptions(procNodes.Get(processNode.ParentNodeKey), targetProcessNodeType, processed, collector);
    }

    public Dictionary<OptionTypeKey, OptionDto> GetEffectiveOptions(ProcessNodeKey processNodeKey)
    {
        var _collector = new Dictionary<OptionTypeKey, OptionDto>();
        var _procNode = procNodes.Get(processNodeKey);
        if (procNodeTypes.Get(_procNode?.ProcessNodeTypeKey) is ProcessNodeTypeDto _type)
        {
            FillAllOptions(_procNode, _type, [], _collector);
        }
        return _collector;
    }

    protected OptionValue? SearchForOption(
        ProcessNodeDto? processNode,
        OptionTypeKey optionTypeKey,
        HashSet<ProcessNodeKey> processed)
    {
        if (processNode == null)
        {
            return null;
        }

        if (processed.Contains(processNode.ProcessNodeKey))
        {
            // oops!
            // TODO: log
            return null;
        }

        if (processNode.Options.TryGetValue(optionTypeKey, out var _option))
        {
            // found one
            return _option;
        }

        // sanity assurance
        processed.Add(processNode.ProcessNodeKey);
        return SearchForOption(procNodes.Get(processNode.ParentNodeKey ), optionTypeKey, processed);
    }

    public OptionValue? GetOption(ProcessNodeKey processNodeKey, OptionTypeKey optionTypeKey)
    {
        if (optionCache.TryGetValue((processNodeKey, optionTypeKey)) is OptionValue optionValue)
        {
            return optionValue;
        }
        if (procNodes.Get(processNodeKey) is ProcessNodeDto _procNode)
        {
            var _found = SearchForOption(_procNode, optionTypeKey, []);
            optionCache.AddOrUpdate((processNodeKey, optionTypeKey), _found, (pn, otk) => _found);
            return _found;
        }
        return null;
    }
}
