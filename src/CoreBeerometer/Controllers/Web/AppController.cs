using System;
using CoreBeerometer.Sevices;
using CoreBeerometer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreBeerometer.Controllers.Web
{
    public class AppController: Controller
    {
        private readonly IMailService _mailService;
        private readonly IConfigurationRoot _configurationRoot;

        public AppController(IMailService mailService, IConfigurationRoot configurationRoot)
        {
            _mailService = mailService;
            _configurationRoot = configurationRoot;
        }

        public IActionResult Index()
        {
            return View();
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