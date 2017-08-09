﻿using System.Collections.Generic;

namespace DotNetPivotalTrackerApi.Portable.Models.User
{
    public class PivotalUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Initials { get; set; }

        public List<PivotalUserProject> Projects { get; set; }
    }
}