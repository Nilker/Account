using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}