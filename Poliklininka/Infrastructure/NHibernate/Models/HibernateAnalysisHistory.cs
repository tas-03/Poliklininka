namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateAnalysisHistory
{
    public virtual int Id { get; set; }

    public virtual int VisitHistoryId { get; set; }

    public virtual int AnalysisId { get; set; }

    public virtual string? Result { get; set; }

    public virtual HibernateVisitHistory? VisitHistory { get; set; }

    public virtual HibernateAnalysis? Analysis { get; set; }
}