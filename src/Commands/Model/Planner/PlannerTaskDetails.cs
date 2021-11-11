using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using PnP.PowerShell.Commands.Model.Graph;

namespace PnP.PowerShell.Commands.Model.Planner
{
    public class PlannerTaskDetails
    {
        [JsonPropertyName("@odata.etag")]
        public string ETag { get; set; }
        public string Description { get; set; }
        public string PreviewType { get; set; }
        public Dictionary<string, PlannerTaskExternalReference> References { get; set; }
        public Dictionary<string, PlannerTaskCheckListItem> Checklist { get; set; }
    }

    public class PlannerTaskExternalReference
    {
        [JsonPropertyName("@odata.type")]
        public string oType { get; set; } = "microsoft.graph.plannerExternalReference";
        public string Alias { get; set; }
        public string PreviewPriority { get; set; }
        public string Type { get; set; }
        public LastModifiedBy LastModifiedBy { get; set; }
    }

    public class PlannerTaskCheckListItem
    {
        [JsonPropertyName("@odata.type")]
        public string Type { get; set; } = "microsoft.graph.plannerChecklistItem";
        public bool? IsChecked { get; set; }
        public string Title { get; set; }
        public string OrderHint { get; set; }
        public LastModifiedBy LastModifiedBy { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }

    public class LastModifiedBy
    {
        public Identity User { get; set; }
    }
    
}
