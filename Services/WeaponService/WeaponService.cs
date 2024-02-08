using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using net7.Dtos.Weapon;

namespace net7.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public WeaponService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response  = new ServiceResponse<GetCharacterDto>();

            try
            {
                var userId = GetUserId();
                var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User!.Id == userId);

                if(character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }
             
              var weapon = _mapper.Map<Weapon>(newWeapon);
              _dataContext.Weapon.Add(weapon);
              await  _dataContext.SaveChangesAsync();
              response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {  
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}