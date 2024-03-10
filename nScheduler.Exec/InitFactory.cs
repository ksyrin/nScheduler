using Docker.DotNet;
using k8s;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nScheduler.Common.Extensions;
using nScheduler.Domain.Events;
using nScheduler.Exec.Dockers;
using nScheduler.Exec.K8s;

namespace nScheduler.Exec;

public enum InitType
{
    K8s,
    Docker
}

public static class InitFactory
{
    public static void RegisterClient(this IServiceCollection services, IConfiguration configuration)
    {
        InitType type = InitType.K8s;
        // Kubernetes 相关的配置
        if (KubernetesClientConfiguration.IsInCluster())
        {
            services.AddScoped(x => new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig()));
        }
        else if (configuration.GetSection("client:kubeconfig") is var kubecfg && kubecfg.Exists() && !kubecfg.Value.IsEmpty())
        {
            services.AddScoped(x => new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile(kubecfg.Value)));
        }
        // Docker 相关的配置
        else if (configuration.GetSection("client:dockersock") is var sock && sock.Exists() && !sock.Value.IsEmpty())
        {
            services.AddScoped<IDockerClient>(x => new DockerClientConfiguration(new Uri(sock.Value)).CreateClient());
            type = InitType.Docker;
        }
        else
        {
            services.AddScoped<IDockerClient>(x => new DockerClientConfiguration().CreateClient());
            type = InitType.Docker;
        }

        if (type == InitType.K8s)
        {
            services.AddScoped<ISchedulerEvent, KubernetesEvent>();
        }
        else
        {
            services.AddScoped<ISchedulerEvent, DockerEvent>();
        }
    }
}