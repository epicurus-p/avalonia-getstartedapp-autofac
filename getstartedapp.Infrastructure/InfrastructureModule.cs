using Autofac;
using getstartedapp.Services;
using getstartedapp.Infrastructure.Data;

namespace getstartedapp.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TodoService>().As<ITodoService>().SingleInstance();
        builder.RegisterType<ClockService>().As<IClockService>().SingleInstance();
  
        builder.Register(c => new AppDbContext(DbConfig.GetConnectionString()))
            .AsSelf()
            .InstancePerDependency();
    }
}