namespace Poliklininka.Infrastructure.NHibernate.Models;

public class HibernateAnalysis
{
    public virtual int Id { get; set; }

    public virtual string AnalysisName { get; set; } = string.Empty;

    public virtual string? Description { get; set; }

    public virtual IList<HibernateAnalysisHistory> AnalysisHistories { get; set; } = new List<HibernateAnalysisHistory>();
}