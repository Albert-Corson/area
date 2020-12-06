using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Repositories;
using Dashboard.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dashboard.API.Services
{
    public class ServiceManagerService
    {
        private readonly DatabaseRepository _database;
        private readonly IDictionary<string, IServiceService> _service;
        private readonly ILogger _logger;

        public ServiceManagerService(DatabaseRepository database, ILoggerFactory loggerFactory, ImgurServiceService imgur)
        {
            _logger = loggerFactory.CreateLogger("Service manager");
            _database = database;
            _service = new Dictionary<string, IServiceService> {
                {imgur.Name, imgur}
            };
        }

        public string? LogInServiceById(HttpContext context, int serviceId)
        {
            var serviceName = _database.Services.FirstOrDefault(model => model.Id == serviceId)?.Name;
            
            if (serviceName == null || !_service.TryGetValue(serviceName, out var service))
                throw new NotFoundHttpException();
            return service.LogIn(context);
        }

        public void HandleServiceLoginCallbackById(HttpContext context, int serviceId)
        {
            var serviceName = _database.Services.FirstOrDefault(model => model.Id == serviceId)?.Name;

            if (serviceName == null || !_service.TryGetValue(serviceName, out var service)) {
                _logger.LogError($"Received login callback with an invalid {{serviceId}} ({serviceId})");
                return;
            }
            service.HandleLogInCallback(context);
        }
    }
}