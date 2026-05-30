namespace Poliklininka.Entities;

public class Patient : User
{

    public string Phone_number { get; set; } = string.Empty;
    public string Insurance_Policy {  get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public MedCard? MedCard { get; set; }


    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<VisitHistory> VisitHistories { get; set; } = [];
    
}
