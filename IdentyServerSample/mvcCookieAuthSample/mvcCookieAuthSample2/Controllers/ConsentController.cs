using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using mvcCookieAuthSample.Services;
using mvcCookieAuthSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.Controllers
{
    public class ConsentController : Controller
    {


        private ConsentService _consentService;



        public ConsentController(ConsentService consentService)
        {
            _consentService = consentService;
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consentService.BuildConsentViewModelAsync(returnUrl);
            if (model == null)
            {
                return View();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            var result = await _consentService.PorcessConsentAsync(viewModel);
            if (result.IsRedirect) {
                return Redirect(result.RedirectUrl);
            }
            if (string.IsNullOrEmpty(result.ValidationError)) {
                ModelState.AddModelError("", result.ValidationError);
            }
            return View(result.viewModel);
        }
    }
}
