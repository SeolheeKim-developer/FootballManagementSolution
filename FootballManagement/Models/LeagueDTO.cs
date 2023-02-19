using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class LeagueDTO :Auditable
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int? NumberOfTeams { get; set; } =null;

        public ICollection<TeamDTO> Teams { get; set; }
    }
}
