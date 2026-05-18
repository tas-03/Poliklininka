namespace Poliklininka.Entities;

public class Doctor : User
{

    public string Specialization { get; set; } = string.Empty;
    public string Office { get; set; } = string.Empty;


    public ICollection<Schedule> Schedules { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}
