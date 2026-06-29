using HisaabPlus.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HisaabPlus.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync (RegisterDTO registerDTO);
        Task<AuthResponseDTO> LoginAsync (LoginDTO loginDTO);
        Task<AuthResponseDTO> AdminLoginAsync(AdminLoginDTO adminLoginDTO);
    }
}
