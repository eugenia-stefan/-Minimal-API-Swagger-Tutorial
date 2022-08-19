﻿using System.Security.Claims;

namespace MinimalJwt.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public ClaimsIdentity EmailAddress { get; internal set; }
    }
}
