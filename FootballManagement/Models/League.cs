using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace FootballManagement.Models
{
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class League :Auditable
    {
        
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}
