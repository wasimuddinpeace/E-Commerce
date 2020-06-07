using System;
using System.Collections.Generic;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchRepository _repository;
       // private readonly DutchContext context;

        public AppController(IMailService mailService,IDutchRepository repository)
        {
            this._mailService = mailService;
            this._repository = repository;
      
        }
        // GET: /<controller>/
        [HttpGet("App")]
        public IActionResult Index()
        {
            //throw new InvalidOperationException();
            var results = _repository.GetAllProducts();
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact() {
            @ViewBag.title = "Contact";
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel Model)
        {
            if (ModelState.IsValid)
            {
                //send the email
                _mailService.sendMessage("V-wauddi@microsoft.com", Model.Subject, $"From:{Model.Name}-{Model.Email},Message:{Model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            return View();
        }

      //  [Authorize]
        public IActionResult AboutUs() {
            @ViewBag.title = "About Us";
            return View();
        }

        
  
        public IActionResult Shop() {
            //var results = _repository.GetAllProducts();
            //return View(results);
            return View();

        }
    }
}
