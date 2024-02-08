using System.Security.Claims;

namespace net7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper ,DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var newCharacter = _mapper.Map<Character>(character);
                var userId = GetUserId();
                newCharacter.User = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
                _dataContext.Characters.Add(newCharacter);
                await _dataContext.SaveChangesAsync(); 
               var dbCharacters = await _dataContext.Characters.Where(character => character.User!.Id == userId).ToListAsync();
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error adding character to the database. Error - {ex}";
            }

            return serviceResponse;
        }

        public async  Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var userId = GetUserId();
            var dbCharacters = await _dataContext.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(character => character.User!.Id == userId)
                .ToListAsync();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
            {
                Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var userId = GetUserId();
            var character = await _dataContext.Characters.FirstOrDefaultAsync(character => character.Id == id && character.User!.Id == userId);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;  
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto newCharacter) 
        {   
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try 
            {
                var userId = GetUserId();
                var character = await _dataContext.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == newCharacter.Id);
                if(character is null || character!.User!.Id != userId)
                    throw new Exception($"Characters Id is incorrect");
     
                _mapper.Map(newCharacter, character);
                await _dataContext.SaveChangesAsync(); 
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                return serviceResponse; 
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try 
            {
                var userId = GetUserId();
                var character = await _dataContext.Characters.FirstOrDefaultAsync(character => character.Id == id && character.User!.Id == userId) 
                ?? throw new Exception($"Characters is not available for you");
                _dataContext.Characters.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = await _dataContext.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                return serviceResponse; 
            } 
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto skill)
        {
             var response = new ServiceResponse<GetCharacterDto>();
            try 
            {
              var userId = GetUserId();
              var character = await _dataContext.Characters.Include(c => c.Weapon).Include(c => c.Skills).FirstOrDefaultAsync(character => character.Id == skill.CharacterId && character.User!.Id == userId);
             if(character is null){
                response.Success = false;
                response.Message = "Character not found"; 
                return response;
             }

             var skillFromDb = await _dataContext.Skill.FirstOrDefaultAsync(s => s.Id == skill.SkillId);

             if(skillFromDb is null){
                response.Success = false;
                response.Message = "Skill not found"; 
                return response;
             }

             character.Skills!.Add(skillFromDb);
             await _dataContext.SaveChangesAsync();
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