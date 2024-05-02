using VContainer;
using VContainer.Unity;
using XO;

public sealed class RootScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.Register<GameField>(Lifetime.Singleton);
    }
}
