﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.TestCommon;
using Microsoft.Azure.WebJobs.ServiceBus.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xunit;

namespace Microsoft.Azure.WebJobs.ServiceBus.UnitTests.Config
{
    public class ServiceBusJobHostConfigurationExtensionsTests
    {
        [Fact]
        public void UseServiceBus_ThrowsArgumentNull_WhenServiceBusConfigIsNull()
        {
            IHost host = new HostBuilder()
                .ConfigureDefaultTestHost(b =>
                {
                    b.AddServiceBus();
                })
                .ConfigureServices(s => s.AddSingleton<IOptions<ServiceBusOptions>>(p => null))
                .Build();

            var exception = Assert.Throws<ArgumentNullException>(() => host.Services.GetServices<IExtensionConfigProvider>());

            Assert.Equal("serviceBusOptions", exception.ParamName);
        }

        [Fact]
        public void UseServiceBus_NoServiceBusConfiguration_PerformsExpectedRegistration()
        {
            IHost host = new HostBuilder()
                .ConfigureDefaultTestHost(b =>
                {
                    b.AddServiceBus();
                })
                .Build();

            var extensions = host.Services.GetService<IExtensionRegistry>();
            IExtensionConfigProvider[] configProviders = extensions.GetExtensions<IExtensionConfigProvider>().ToArray();

            // verify that the service bus config provider was registered
            var serviceBusExtensionConfig = configProviders.OfType<ServiceBusExtensionConfigProvider>().Single();
        }

        [Fact]
        public void UseServiceBus_ServiceBusConfigurationProvided_PerformsExpectedRegistration()
        {
            string fakeConnStr = "test service bus connection";

            IHost host = new HostBuilder()
                .ConfigureDefaultTestHost(b =>
                {
                    b.AddServiceBus();
                })
                .ConfigureServices(s =>
                {
                    s.Configure<ServiceBusOptions>(o =>
                    {
                        o.ConnectionString = fakeConnStr;
                    });
                })
                .Build();

            // verify that the service bus config provider was registered
            var extensions = host.Services.GetService<IExtensionRegistry>();
            IExtensionConfigProvider[] configProviders = extensions.GetExtensions<IExtensionConfigProvider>().ToArray();

            // verify that the service bus config provider was registered
            var serviceBusExtensionConfig = configProviders.OfType<ServiceBusExtensionConfigProvider>().Single();

            Assert.Equal(fakeConnStr, serviceBusExtensionConfig.Options.ConnectionString);
        }
    }
}
