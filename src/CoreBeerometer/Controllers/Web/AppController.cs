using System;
using System.Linq;
using CoreBeerometer.Models;
using CoreBeerometer.Sevices;
using CoreBeerometer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreBeerometer.Controllers.Web
{
    public class AppController: Controller
    {
        private readonly IMailService _mailService;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IBeerRepository _repository;
        private readonly ILogger<AppController> _logger;

        public AppController(IMailService mailService, IConfigurationRoot configurationRoot, IBeerRepository repository, ILogger<AppController> logger)
        {
            _mailService = mailService;
            _configurationRoot = configurationRoot;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var data = _repository.GetAllTrips();
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index Page: {ex.Message}");
                return Redirect("/error");
            }
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com")) ModelState.AddModelError("", "We don't support AOL addresses");

            if (ModelState.IsValid)
            {
                _mailService.SendMail(_configurationRoot["MailSettings:ToAddress"], model.Email, "From Beerman",
                    model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}