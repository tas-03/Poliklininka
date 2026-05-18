namespace Poliklininka.Entities;

public class Reception
{
    public int ReceptionId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime DateRes { get; set; }
    public int ScheduleId { get; set; }
    public bool Status { get; set; }

}
