namespace Phaneritic.Interfaces.Operational;
public interface IProcessNodesNavigator
{
    bool IsInitialized { get; }

    ProcessNodeDto? GetProcessNode(ProcessNodeKey? processNode);

    List<ProcessNodeDto> RootProcessNodes();

    List<ProcessNodeDto> AllProcessNodes();

    List<ProcessNodeDto> GetProcessNodes(HashSet<ProcessNodeTypeKey> processNodeTypes);

    List<ProcessNodeKey> GetPathToRoot(ProcessNodeKey? processNode);

    ProcessNodeDto? SearchToRoot(ProcessNodeKey? processNode, HashSet<ProcessNodeTypeKey> processNodeTypes);

    ProcessNodeDto? SearchToRoot(ProcessNodeKey? processNode, HashSet<ProcessNodeKey> processNodes);

    List<ProcessNodeDto> GetBranchProcessNodes(ProcessNodeKey? parent);

    List<ProcessNodeDto> ExpandBranchProcessNodes(ProcessNodeKey? parent);

    List<ProcessNodeDto> ExpandBranchProcessNodes(ProcessNodeKey? parent, HashSet<ProcessNodeTypeKey> processNodeTypes);

    bool IsAncestor(ProcessNodeKey? ancestor, ProcessNodeKey? descendant);
}
