using AutoMapper;
using CRUD.Domain.Employee;
using CRUD.EF;
using CRUDWeb;
using CRUDWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app);