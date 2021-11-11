using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using PnP.PowerShell.Commands.Attributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Graph;
using PnP.PowerShell.Commands.Model.Planner;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Utilities.REST;

namespace PnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Set, "PnPPlannerTaskDetail")]
    [RequiredMinimalApiPermissions("Group.ReadWrite.All")]
    public class SetPlannerTaskDetail : PnPGraphCmdlet
    {
        [Parameter(Mandatory = true)]
        public string TaskId;

        [Parameter(Mandatory = false)]
        public string Description { get; set; }
        [Parameter(Mandatory = false)]
        public string PreviewType { get; set; }

        [Parameter(Mandatory = false)]
        public Dictionary<string, PlannerTaskExternalReference> References { get; set; }

        [Parameter(Mandatory = false)]
        public Dictionary<string, PlannerTaskCheckListItem> Checklist { get; set; }

        protected override void ExecuteCmdlet()
        {
            var existingTask = PlannerUtility.GetTaskAsync(HttpClient, AccessToken, TaskId, false, true).GetAwaiter().GetResult();
            if (existingTask != null)
            {
                var plannerTask = new PlannerTask();
                plannerTask.Details = new PlannerTaskDetails();

                if (ParameterSpecified(nameof(Description)))
                {
                    plannerTask.Details.Description = Description;
                }
                if (ParameterSpecified(nameof(PreviewType)))
                {
                    plannerTask.Details.PreviewType = PreviewType;
                }
                if (ParameterSpecified(nameof(References)))
                {
                    Dictionary<string, PlannerTaskExternalReference> newReferenceItems = new Dictionary<string, PlannerTaskExternalReference>();
                    foreach (var referencesItem in References)
                    {
                        var newExternalReference = new PlannerTaskExternalReference();
                        newExternalReference.Alias = referencesItem.Value.Alias;
                        newExternalReference.Type = referencesItem.Value.Type;
                        newReferenceItems.Add(referencesItem.Key, newExternalReference);
                    }
                    plannerTask.Details.References = newReferenceItems;
                }
                if (ParameterSpecified(nameof(Checklist)))
                {
                    Dictionary<string, PlannerTaskCheckListItem> newItems = new Dictionary<string, PlannerTaskCheckListItem>();
                    foreach (var checklistItem in Checklist)
                    {
                        var newCheckListItem = new PlannerTaskCheckListItem();
                        newCheckListItem.IsChecked = checklistItem.Value.IsChecked;
                        newCheckListItem.Title = checklistItem.Value.Title;
                        newItems.Add(checklistItem.Key, newCheckListItem);
                    }
                    plannerTask.Details.Checklist = newItems;
                }
                PlannerUtility.UpdateTaskDetailAsync(HttpClient, AccessToken, existingTask, plannerTask).GetAwaiter().GetResult();
            }
            else
            {
                throw new PSArgumentException("Task not found", nameof(TaskId));
            }
        }
    }
}