using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class MetadataIndex
    {
        private readonly Dictionary<string, Dictionary<PathLevel, Node>> roots = new Dictionary<string, Dictionary<PathLevel, Node>>();

        public MetadataIndex(int batchIndex)
        {
            this.BatchIndex = batchIndex;
        }

        public int BatchIndex { get; private set; }

        public Node GetExactNode(SemanticPath path, bool orParent = false)
        {
            if (path.Parts.Count == 0 || !this.roots.ContainsKey(path.Language))
            {
                return null;
            }

            var language = this.roots[path.Language];
            if (language.ContainsKey(path.Parts[0]))
            {
                var node = language[path.Parts[0]];
                var i = 0;
                for (var p = 0; p < path.Parts.Count; p++)
                {
                    if (node.PathLevels[i].Equals(path.Parts[p]))
                    {
                        if (p == path.Parts.Count - 1)
                        {
                            if (i == node.PathLevels.Count - 1)
                            {
                                return node;
                            }
                            else
                            {
                                if (orParent)
                                {
                                    return node.Parent;
                                }

                                return null;
                            }
                        }
                        else
                        {
                            if (i == node.PathLevels.Count - 1)
                            {
                                if (node.Children.ContainsKey(path.Parts[p + 1]))
                                {
                                    node = node.Children[path.Parts[p + 1]];
                                    i = -1;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (orParent)
                        {
                            return node.Parent;
                        }
                        else
                        {
                            return null;
                        }
                    }

                    i++;
                }
            }

            return null;
        }

        public Node AllocateNode(SemanticPath path)
        {
            if (!this.roots.ContainsKey(path.Language))
            {
                this.roots.Add(path.Language, new Dictionary<PathLevel, Node>());
            }

            if (path.Parts == null || path.Parts.Count == 0)
            {
                throw new InvalidOperationException("Cannot allocate node for empty path.");
            }

            var language = this.roots[path.Language];
            Node result = null;

            if (!language.ContainsKey(path.Parts[0]))
            {
                result = new Node(null, path.Language, this, path.Parts);
                language.Add(path.Parts[0], result);
                return result;
            }
            else
            {
                var node = language[path.Parts[0]];
                var l = 0;
                var nl = 0;
                while (l < path.Parts.Count)
                {
                    if (!path.Parts[l].Equals(node.PathLevels[nl]))
                    {
                        var fork = node.Split(nl - 1);
                        result = new Node(fork, path.Language, this, path.Parts.Skip(l));
                        break;
                    }
                    else
                    {
                        if (l == path.Parts.Count - 1)
                        {
                            if (nl == node.PathLevels.Count - 1)
                            {
                                result = node;
                                break;
                            }
                            else
                            {
                                result = node.Split(nl);
                                break;
                            }
                        }
                        else
                        {
                            if (nl == node.PathLevels.Count - 1)
                            {
                                if (node.Children.ContainsKey(path.Parts[l + 1]))
                                {
                                    node = node.Children[path.Parts[l + 1]];
                                    nl = -1;
                                }
                                else
                                {
                                    result = new Node(node, node.Language, node.RootIndex, path.Parts.Skip(l + 1));
                                    break;
                                }
                            }
                        }
                    }

                    l++;
                    nl++;
                }

                return result;
            }
        }
    }
}
