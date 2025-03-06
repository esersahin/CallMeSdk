global using CallMeSdk.Abstractions;
global using CallMeSdk.Configurations;
global using CallMeSdk.DataProviders;
global using CallMeSdk.DomainServices;
global using CallMeSdk.Extensions;
global using CallMeSdk.Models;
global using CallMeSdk.Services;

global using CallMeSdk.Client.DataAdapters;
global using CallMeSdk.Client.DataAdapters.AzonBank;
global using CallMeSdk.Client.Terminal.Configurations;
global using CallMeSdk.Client.Terminal.Extensions;
global using CallMeSdk.Client.Terminal.Factories;
global using CallMeSdk.Client.Terminal.Processors;
global using CallMeSdk.Client.Terminal.Services;
global using CallMeSdk.Client.Terminal.Strategies;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using System.Text.Json;

global using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;