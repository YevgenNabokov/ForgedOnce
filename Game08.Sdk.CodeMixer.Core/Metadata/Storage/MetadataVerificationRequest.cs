using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata.Storage
{
    public class MetadataVerificationRequest
    {
        public MetadataVerificationRequest(
            SearchMode searchMode,
            AncestrySearchDirection ancestrySearchDirection,
            Func<Node, bool?> resultEvaluator,
            Func<NodeRelation, bool> relationSelector = null,
            Func<Node, bool> ancestryNeighborSelector = null,
            RelationKind[] relationsToCheck = null)
        {
            this.SearchMode = searchMode;
            this.AncestrySearchDirection = ancestrySearchDirection;
            this.ResultEvaluator = resultEvaluator;
            this.RelationSelector = relationSelector;
            this.AncestryNeighborSelector = ancestryNeighborSelector;
            this.RelationsToCheck = relationsToCheck;
        }

        public SearchMode SearchMode { get; private set; }

        public AncestrySearchDirection AncestrySearchDirection { get; private set; }

        public RelationKind[] RelationsToCheck { get; private set; }

        public Func<NodeRelation, bool> RelationSelector { get; private set; }

        public Func<Node, bool> AncestryNeighborSelector { get; private set; }

        public Func<Node, bool?> ResultEvaluator { get; private set; }
    }
}
