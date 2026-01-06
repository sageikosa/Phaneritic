using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Queries.Operational;
public class ProcessNodesNavigator(
    ILudDictionary<ProcessNodeKey, ProcessNodeDto> procNodes
    ) : IProcessNodesNavigator
{
    public bool IsInitialized
        => procNodes.RefreshCycle > 0;

    public ProcessNodeDto? GetProcessNode(ProcessNodeKey? processNode)
        => procNodes.Get(processNode);

    public List<ProcessNodeDto> AllProcessNodes()
        => procNodes.All();

    public List<ProcessNodeDto> RootProcessNodes()
        => [.. procNodes.All().Where(_pn => _pn.ParentNodeKey == null)];

    public List<ProcessNodeDto> GetProcessNodes(HashSet<ProcessNodeTypeKey> processNodeTypes)
        => [.. procNodes.All().Where(_pn => processNodeTypes.Contains(_pn.ProcessNodeTypeKey))];

    protected ProcessNodeDto? SearchToRoot(
        ProcessNodeDto? processNode,
        Func<ProcessNodeDto, bool> searchFor,
        HashSet<ProcessNodeKey> processed)
    {
        if (processNode == null)
        {
            // ran out of process nodes!
            return null;
        }

        if (processed.Contains(processNode.ProcessNodeKey))
        {
            // oops!
            // TODO: log
            return null;
        }

        if (searchFor(processNode))
        {
            // victory
            return processNode;
        }

        // catch and continue
        processed.Add(processNode.ProcessNodeKey);
        return SearchToRoot(procNodes.Get(processNode.ParentNodeKey), searchFor, processed);
    }

    public List<ProcessNodeKey> GetPathToRoot(ProcessNodeKey? processNode)
    {
        // hashset for sanity check also builds path
        var _path = new HashSet<ProcessNodeKey>();
        var _found = SearchToRoot(procNodes.Get(processNode),
            (pn) => pn.ParentNodeKey == null,
            _path);
        if (_found != null)
        {
            // matching record and path followed
            return [_found?.ProcessNodeKey ?? default, .. _path];
        }

        // probably bad parent
        return [.. _path];
    }

    public ProcessNodeDto? SearchToRoot(ProcessNodeKey? processNode, HashSet<ProcessNodeTypeKey> processNodeTypes)
        => SearchToRoot(procNodes.Get(processNode),
            (pn) => processNodeTypes.Contains(pn.ProcessNodeTypeKey), []);

    public ProcessNodeDto? SearchToRoot(ProcessNodeKey? processNode, HashSet<ProcessNodeKey> processNodes)
        => SearchToRoot(procNodes.Get(processNode),
            (pn) => processNodes.Contains(pn.ProcessNodeKey), []);

    protected IEnumerable<ProcessNodeDto> FindChildren(ProcessNodeKey? parent)
        => procNodes.All().Where(_pn => parent == _pn.ParentNodeKey);

    public List<ProcessNodeDto> GetBranchProcessNodes(ProcessNodeKey? parent)
        => [.. FindChildren(parent ?? default)];

    protected IEnumerable<ProcessNodeDto> BloomProcessNode(
        ProcessNodeDto? processNode,
        Func<ProcessNodeDto, bool> includeFilter,
        HashSet<ProcessNodeKey> processed)
    {
        // not found or already processed
        if ((processNode == null)
            || !processed.Contains(processNode.ProcessNodeKey))
        {
            yield break;
        }

        // sanity assurance
        processed.Add(processNode.ProcessNodeKey);

        if (includeFilter(processNode))
        {
            yield return processNode;
        }

        // continue bloom
        foreach (var _sub in FindChildren(processNode.ProcessNodeKey))
        {
            foreach (var _child in BloomProcessNode(_sub, includeFilter, processed))
            {
                yield return _child;
            }
        }
    }

    public List<ProcessNodeDto> ExpandBranchProcessNodes(ProcessNodeKey? parent)
        => [.. BloomProcessNode(procNodes.Get(parent), (pn) => true, [])];

    public List<ProcessNodeDto> ExpandBranchProcessNodes(ProcessNodeKey? parent, HashSet<ProcessNodeTypeKey> processNodeTypes)
        => [.. BloomProcessNode(
            procNodes.Get(parent),
            (pn) => processNodeTypes.Contains(pn.ProcessNodeTypeKey),
            [])];

    public bool IsAncestor(ProcessNodeKey? ancestor, ProcessNodeKey? descendant)
        => SearchToRoot(procNodes.Get(descendant),
            (pn) => pn.ProcessNodeKey == ancestor, []) != null;
}
