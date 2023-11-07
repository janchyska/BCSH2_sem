namespace BCSH2_Sem_prace_Chyska.Models
{
    public class TaskReminder
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime ReminderDate { get; set; }
        public ProjectTask Task { get; set; }
    }

}
