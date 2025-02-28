global using CallMeSdk.Models;
global using CallMeSdk.Configurations;
global using CallMeSdk.DataProviders;
global using CallMeSdk.DomainServices;
global using CallMeSdk.ValueObjects;
global using CallMeSdk.Converters;
global using CallMeSdk.Services;
global using CallMeSdk.Abstractions;
global using CallMeSdk.FtpClients;

global using IFtpClient = CallMeSdk.FtpClients.IFtpClient;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using System.Diagnostics;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Xml.Serialization;

global using FluentFTP;