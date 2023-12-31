using System;
using System.Collections.Generic;
using BCSH2_Sem_prace_Chyska.Models;

public class EditTaskViewModel
{
    public ProjectTask Task { get; set; }
    public List<ProjectTaskStatus> Statuses { get; set; }
    public List<ProjectTaskPriority> Priorities { get; set; }
    public int? SelectedStatusId { get; set; }
    public int? SelectedPriorityId { get; set; }
}
