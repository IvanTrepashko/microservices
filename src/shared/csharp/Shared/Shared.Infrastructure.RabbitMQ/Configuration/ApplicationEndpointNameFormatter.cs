using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.RabbitMQ.Options;

namespace Shared.Infrastructure.RabbitMQ.Configuration;

public class ApplicationEndpointNameFormatter : IEndpointNameFormatter
{
    private readonly DefaultEndpointNameFormatter _formatter;

    public ApplicationEndpointNameFormatter(
        IOptions<RabbitMqOptions> options)
    {
        _formatter = new DefaultEndpointNameFormatter(options.Value.EndpointPrefix.TrimEnd('.') + ".", includeNamespace: false);
    }

    public string TemporaryEndpoint(string tag)
    {
        return _formatter.TemporaryEndpoint(tag);
    }

    public string Consumer<T>() where T : class, IConsumer
    {
        return _formatter.Consumer<T>();
    }

    public string Message<T>() where T : class
    {
        return DefaultEndpointNameFormatter.Instance.Message<T>();
    }

    public string Saga<T>() where T : class, ISaga
    {
        return _formatter.Saga<T>();
    }

    public string ExecuteActivity<T, TArguments>()
        where T : class, IExecuteActivity<TArguments> where TArguments : class
    {
        return _formatter.ExecuteActivity<T, TArguments>();
    }

    public string CompensateActivity<T, TLog>() where T : class, ICompensateActivity<TLog> where TLog : class
    {
        return _formatter.CompensateActivity<T, TLog>();
    }

    public string SanitizeName(string name)
    {
        return _formatter.SanitizeName(name);
    }

    public string Separator { get; } = "";
}