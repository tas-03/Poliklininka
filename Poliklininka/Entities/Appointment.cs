namespace Poliklininka.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string BookingStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public Schedule Schedule { get; set; } = null!;
    public VisitHistory? VisitHistory { get; set; }
}
