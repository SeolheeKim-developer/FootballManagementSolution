using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace FootballManagement.Models
{
    public class LeagueMetaData
    {
        
        [Required(ErrorMessage = "Code cannot be left blank.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The Code must contain exactly 2 letters.")]
        [RegularExpression("[A-Za-z]{2}", ErrorMessage = "The Code must contain exactly 2 letters.")]
        public string Code { get; set; }

        [Display(Name = "League Name")]
        [Required(ErrorMessage = "League name cannot be left blank.")]
        [StringLength(70, ErrorMessage = "League name cannot be more than 70 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Team")]
        public ICollection<Team> Teams { get; set; }
    }
}
