﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Survey.Shared.Common.Interfaces;

namespace API.Controllers
{
   // [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        private ICurrentUserService _userService = null!;
        private IMapper _mapper = null;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected ICurrentUserService UserService => _userService ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
