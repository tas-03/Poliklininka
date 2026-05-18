namespace Poliklininka.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Office { get; set; } = string.Empty;
    public string SlotStatus { get; set; } = string.Empty;

    public Doctor Doctor { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
}
