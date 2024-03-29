﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace BankProject.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));

        [Authorize(Roles = "Użytkownicy")]
        public IActionResult OtherAction() => View("Index",
            GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>
            new Dictionary<string, object>
            {
                ["Akcja"] = actionName,
                ["Użytkownik"] = HttpContext.User.Identity.Name,
                ["Uwierzytelniony?"] = HttpContext.User.Identity.IsAuthenticated,
                ["Typ uwierzytelnienia"] = HttpContext.User.Identity.AuthenticationType,
                ["Przypisany do roli Użytkownicy?"] = HttpContext.User.IsInRole("Użytkownicy")
            };
    }
}