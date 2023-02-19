using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;

namespace FootballManagement.Models
{
    public class TeamMetaData : IValidatableObject
    {
        [Display(Name = "Team Name")]
        [Required(ErrorMessage = "You cannot leave the team name blank.")]
        [StringLength(70, ErrorMessage = "Team name cannot be more than 70 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You cannot leave the Budget blank.")]
        [Range(500.0, 10000.0, ErrorMessage = "Budget must be between $500 and $10,000.")]
        [DataType(DataType.Currency)]
        public double Budget { get; set; }

        [Display(Name = "League")]
        [Required(ErrorMessage = "You cannot leave the League blank.")]
        public string LeagueCode { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Name[0] == 'X' || Name[0] == 'F' || Name[0] == 'S')
            {
                yield return new ValidationResult("Team names are not allowed to start with the letters X, F, or S.", new[] { "Name" });
            }
        }
    }
}
