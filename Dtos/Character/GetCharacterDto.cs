using net7.Dtos.Skill;
using net7.Dtos.Weapon;

namespace net7.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id {get; set;}
        public string Name {get; set;} = "Ano";
        public int HitPoint {get; set;} = 100;
        public int Strength {get; set;} = 10;
        public int Defense {get; set;} = 10;
        public int Intelligence {get; set;} = 10;
        public RpgClass Class {get; set;} = RpgClass.Knight;
        public GetWeaponDto? Weapon {get; set;} = null;
        public List<GetSkillDto>? Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}