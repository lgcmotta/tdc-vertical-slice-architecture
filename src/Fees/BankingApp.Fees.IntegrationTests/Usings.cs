global using BankingApp.Application.Core.Behaviors;
global using BankingApp.Application.Core.Extensions;
global using BankingApp.Fees.API.Features.CreateAccount;
global using BankingApp.Fees.API.Infrastructure.Handlers;
global using BankingApp.Fees.API.Infrastructure;
global using BankingApp.Infrastructure.Core.Extensions;
global using BankingApp.Infrastructure.Core.Handlers;
global using BankingApp.Messaging.Contracts;
global using FluentAssertions;
global using MassTransit.Testing;
global using MassTransit;
global using MediatR.NotificationPublishers;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using System.Reflection;
global using Xunit;
global using BankingApp.Application.Core.Exceptions;
global using MediatR;
global using Moq;
global using System.Collections;
global using Bogus;
global using Bogus.Extensions.Brazil;
global using BankingApp.Fees.API.Features.OverdraftFee;
global using BankingApp.Fees.API.Features.ProfitFee;
global using BankingApp.Fees.API.Features.UpdateBalance;
global using BankingApp.Fees.Domain.ValueObjects;
global using BankingApp.Fees.Domain;
global using BankingApp.Fees.API.Features.UpdateAccount;
global using BankingApp.Fees.Domain.Exceptions;