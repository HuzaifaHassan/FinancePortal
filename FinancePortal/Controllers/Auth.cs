using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbHandler.Data;
using DbHandler.Model;
using DbHandler.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using static FinancePortal.DTO.DTO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using FinancePortal.Responses;
using FinancePortal.Models;
using FinancePortal.DTO;
using FinancePortal.Helper;
using System.Transactions;
namespace FinancePortal.Controllers
{

    public class Auth : ControllerBase
    {
      
    }
}
