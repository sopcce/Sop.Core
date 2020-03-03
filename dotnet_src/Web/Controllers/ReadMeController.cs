using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sop.Domain.Service;
using Sop.Domain.VModel;

namespace Web.Controllers
{
    /// <summary>
    /// </summary>
    public class ReadMeController : Controller
    {
        private readonly IUserConsultService _userConsultService;

        public ReadMeController(IUserConsultService userConsultService)
        {
            _userConsultService = userConsultService;
        }


        [HttpPost]
        public RedirectResult Contact(UserConsultVm consultVm)
        {
            _userConsultService.Edit(consultVm);
           return Redirect("/");
        }
        
        
        
        
    }
     
}