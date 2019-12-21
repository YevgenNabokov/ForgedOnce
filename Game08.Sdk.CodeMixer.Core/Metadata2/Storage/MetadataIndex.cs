using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Storage
{
    public class MetadataIndex
    {
        private readonly Dictionary<string, Dictionary<NodePathLevel, Node>> roots = new Dictionary<string, Dictionary<NodePathLevel, Node>>();

        public MetadataIndex()
        {
        }

        public void ReplaceNode(Node subject, Node replacement)
        {
            if (subject.RootIndex != this)
            {
                throw new InvalidOperationException("Cannot replace node not belonging to metadata index.");
            }

            if (!subject.PathLevels.First().Equals(replacement.PathLevels.First()))
            {
                throw new InvalidOperationException("Replacement node should have the same starting path level.");
            }

            var lang = roots[subject.Language];
            if (!lang.ContainsKey(subject.PathLevels.First()))
            {
                throw new InvalidOperationException("Node was not found in metadata index.");
            }

            lang[subject.PathLevels.First()] = replacement;
        }

        public Node GetExactNode(NodePath path, bool orParent = false)
        {
            if (path.Levels.Count == 0 || !this.roots.ContainsKey(path.Language))
            {
                return null;
            }

            var language = this.roots[path.Language];
            if (language.ContainsKey(path.Levels[0]))
            {
                var node = language[path.Levels[0]];
                var i = 0;
                for (var p = 0; p < path.Levels.Count; p++)
                {
                    if (node.PathLevels[i].Equals(path.Levels[p]))
                    {
                        if (p == path.Levels.Count - 1)
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
                                if (node.Children.ContainsKey(path.Levels[p + 1]))
                                {
                                    node = node.Children[path.Levels[p + 1]];
                                    i = -1;
                                }
                                else
                                {
                                    if (orParent)
                                    {
                                        return node;
                                    }

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

        public Node AllocateNode(NodePath path)
        {
            if (!this.roots.ContainsKey(path.Language))
            {
                this.roots.Add(path.Language, new Dictionary<NodePathLevel, Node>());
            }

            if (path.Levels == null || path.Levels.Count == 0)
            {
                throw new InvalidOperationException("Cannot allocate node for empty path.");
            }

            var language = this.roots[path.Language];
            Node result = null;

            if (!language.ContainsKey(path.Levels[0]))
            {
                result = new Node(null, path.Language, this, path.Levels);
                language.Add(path.Levels[0], result);
                return result;
            }
            else
            {
                var node = language[path.Levels[0]];
                var l = 0;
                var nl = 0;
                while (l < path.Levels.Count)
                {
                    if (!path.Levels[l].Equals(node.PathLevels[nl]))
                    {
                        var fork = node.Split(nl - 1);
                        result = new Node(fork, path.Language, this, path.Levels.Skip(l));
                        break;
                    }
                    else
                    {
                        if (l == path.Levels.Count - 1)
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
                                if (node.Children.ContainsKey(path.Levels[l + 1]))
                                {
                                    node = node.Children[path.Levels[l + 1]];
                                    nl = -1;
                                }
                                else
                                {
                                    result = new Node(node, node.Language, node.RootIndex, path.Levels.Skip(l + 1));
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
