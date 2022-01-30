using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Infrastructure.Database
{
    public class MySqlOptions
    {
        public const string SectionName = "ConnectionStrings:MySql";
        [MinLength(1)]
        public string Master { get; set; } = String.Empty;
        public List<string> Replicas { get; set; } = new List<string>();
    }
}