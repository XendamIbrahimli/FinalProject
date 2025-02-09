﻿using HMS.Core.Dtos.PatienceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsPatienceAsync(PatienceCreateDto dto);
    }
}
