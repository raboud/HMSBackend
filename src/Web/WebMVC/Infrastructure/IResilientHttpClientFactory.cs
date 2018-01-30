using Microsoft.BuildingBlocks.Resilience.Http;
using System;

namespace Microsoft.WebMVC.Infrastructure
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}