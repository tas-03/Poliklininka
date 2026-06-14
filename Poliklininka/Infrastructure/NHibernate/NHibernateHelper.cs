using System.Reflection;

namespace Poliklininka.Infrastructure.NHibernate;

public static class NHibernateHelper
{
    private static global::NHibernate.ISessionFactory? _sessionFactory;

    public static void Initialize(string connectionString)
    {
        if (_sessionFactory != null)
            return;

        var configuration = new global::NHibernate.Cfg.Configuration();

        configuration.Configure("hibernate.cfg.xml");

        configuration.SetProperty(
            global::NHibernate.Cfg.Environment.ConnectionString,
            connectionString);

        var assembly = Assembly.GetExecutingAssembly();

        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateUser.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernatePatient.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateDoctor.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateAppointment.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateMedService.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateVisitHistory.hbm.xml", assembly);

        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateMedCard.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateBloodGroup.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateAllergy.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateChronicDisease.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateAnalysis.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateRecipe.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateAnalysisHistory.hbm.xml", assembly);
        configuration.AddResource("Poliklininka.Infrastructure.NHibernate.Mappings.HibernateRecipeHistory.hbm.xml", assembly);

        _sessionFactory = configuration.BuildSessionFactory();
    }

    public static global::NHibernate.ISession OpenSession()
    {
        if (_sessionFactory == null)
        {
            throw new InvalidOperationException(
                "NHibernate не инициализирован. Вызови NHibernateHelper.Initialize(connectionString).");
        }

        return _sessionFactory.OpenSession();
    }
}