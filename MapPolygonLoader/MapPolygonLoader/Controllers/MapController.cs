﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPolygonLoader.Controllers
{
    public class GetPolygonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
