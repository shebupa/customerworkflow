﻿global using ETX.Metrics.Logging;
global using ETX.Workflow.Customer.Application.Configurations;
global using ETX.Workflow.Customer.Application.Contracts.Infrastructure;
global using ETX.Workflow.Customer.Application.Contracts.Infrastructure.Policies;
global using ETX.Workflow.Customer.Application.Features.Responses;
global using ETX.Workflow.Customer.Infrastructure.Constants;
global using ETX.Workflow.Customer.Infrastructure.Helpers;
global using ETX.Workflow.Customer.Infrastructure.Services;
global using ETX.Workflow.Customer.Infrastructure.Services.Policies;
global using Hazelcast;
global using Hazelcast.DistributedObjects;
global using Microsoft.Extensions.Configuration;
global using Moq;
global using Polly.CircuitBreaker;
global using Polly.Retry;
global using Shouldly;
global using System.Diagnostics.CodeAnalysis;
global using Xunit;