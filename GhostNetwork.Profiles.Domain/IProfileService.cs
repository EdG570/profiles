﻿using System;
using System.Threading.Tasks;
using Domain;

namespace GhostNetwork.Profiles.Domain
{
    public interface IProfileService
    {
        Task<Profile> GetByIdAsync(long id);

        Task<(DomainResult, long)> CreateAsync(string firstName, string lastName, bool gender, DateTime dateOfBirth, string city);

        Task<DomainResult> UpdateAsync(long id, string firstName, string lastName, bool gender, DateTime dateOfBirth, string city);

        Task DeleteAsync(long id);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileStorage profileStorage;
        private readonly IValidator<ProfileContext> profileValidator;

        public ProfileService(IProfileStorage profileStorage, IValidator<ProfileContext> profileValidator)
        {
            this.profileStorage = profileStorage;
            this.profileValidator = profileValidator;
        }

        public async Task<(DomainResult, long)> CreateAsync(string firstName, string lastName, bool gender,
            DateTime dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, dateOfBirth, city));

            if (!result.Successed)
            {
                return (result, -1);
            }

            var profile = new Profile(0, firstName, lastName, gender, dateOfBirth, city);

            var profileId = await profileStorage.InsertAsync(profile);

            return (DomainResult.Success(), profileId);
        }

        public async Task DeleteAsync(long id)
        {
            await profileStorage.DeleteAsync(id);
        }

        public async Task<Profile> GetByIdAsync(long id)
        {
            return await profileStorage.FindByIdAsync(id);
        }

        public async Task<DomainResult> UpdateAsync(long id, string firstName, string lastName, bool gender, DateTime dateOfBirth, string city)
        {
            var result = profileValidator.Validate(new ProfileContext(firstName, lastName, dateOfBirth, city));

            if (result.Successed)
            {
                var profile = await profileStorage.FindByIdAsync(id);

                if (profile == null)
                {
                    return DomainResult.Error("Profile not found.");
                }

                var updatedProfile = new Profile(id, firstName, lastName, gender, dateOfBirth, city);
                await profileStorage.UpdateAsync(id, updatedProfile);
                return DomainResult.Success();
            }

            return result;
        }
    }
}