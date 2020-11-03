﻿using System;

namespace GhostNetwork.Profiles.WorkExperiences
{
    public class WorkExperience
    {
        public WorkExperience(string id, string profileId, DateTime? finishWork, DateTime startWork, string companyName)
        {
            Id = id;
            ProfileId = profileId;
            FinishWork = finishWork;
            StartWork = startWork;
            CompanyName = companyName;
        }

        public string Id { get; }

        public string CompanyName { get; private set; }

        public DateTime StartWork { get; private set; }

        public DateTime? FinishWork { get; private set; }

        public string ProfileId { get; private set; }
    }
}
