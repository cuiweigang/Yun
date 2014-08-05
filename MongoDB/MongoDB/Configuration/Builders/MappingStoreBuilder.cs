﻿using System;
using System.Collections.Generic;
using MongoDB.Configuration.Mapping.Auto;
using MongoDB.Configuration.Mapping;

namespace MongoDB.Configuration.Builders
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingStoreBuilder
    {
        private IAutoMappingProfile _defaultProfile;
        private readonly List<Type> _eagerMapTypes;
        private readonly ClassOverridesMap _overrides;
        private readonly List<FilteredProfile> _profiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStoreBuilder"/> class.
        /// </summary>
        public MappingStoreBuilder()
        {
            _eagerMapTypes = new List<Type>();
            _overrides = new ClassOverridesMap();
            _profiles = new List<FilteredProfile>();
        }

        /// <summary>
        /// Gets the mapping store.
        /// </summary>
        /// <returns></returns>
        public IMappingStore BuildMappingStore()
        {
            IAutoMapper autoMapper;
            if (_profiles.Count > 0)
            {
                var agg = new AggregateAutoMapper();
                foreach (var p in _profiles)
                    agg.AddAutoMapper(new AutoMapper(CreateOverrideableProfile(p.Profile), p.Filter));

                    agg.AddAutoMapper(new AutoMapper(CreateOverrideableProfile(_defaultProfile ?? new AutoMappingProfile())));
                    autoMapper = agg;
                }
                else
                    autoMapper = new AutoMapper(CreateOverrideableProfile(_defaultProfile ?? new AutoMappingProfile()));

                var store = new AutoMappingStore(autoMapper);

                foreach (var type in _eagerMapTypes)
                    store.GetClassMap(type);

            return store;
        }

        /// <summary>
        /// Configures the default profile.
        /// </summary>
        /// <param name="config">The config.</param>
        public void DefaultProfile(Action<AutoMappingProfileBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            var dp = _defaultProfile as AutoMappingProfile ?? new AutoMappingProfile();

            config(new AutoMappingProfileBuilder(dp));
            _defaultProfile = dp;
        }

        /// <summary>
        /// Configures the default profile.
        /// </summary>
        /// <param name="defaultProfile">The default profile.</param>
        public void DefaultProfile(IAutoMappingProfile defaultProfile)
        {
            if (defaultProfile == null)
                throw new ArgumentNullException("defaultProfile");

            _defaultProfile = defaultProfile;
        }

        /// <summary>
        /// Configures a custom profile.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="config">The config.</param>
        public void CustomProfile(Func<Type, bool> filter, Action<AutoMappingProfileBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            var p = new AutoMappingProfile();
            config(new AutoMappingProfileBuilder(p));
            CustomProfile(filter, p);
        }

        /// <summary>
        /// Adds a custom profile.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="profile">The profile.</param>
        public void CustomProfile(Func<Type, bool> filter, IAutoMappingProfile profile)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            if (profile == null)
                throw new ArgumentNullException("profile");

            _profiles.Add(new FilteredProfile { Filter = filter, Profile = profile });
        }

        /// <summary>
        /// Maps this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Map<T>()
        {
            _eagerMapTypes.Add(typeof(T));
        }

        /// <summary>
        /// Maps the specified config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The config.</param>
        public void Map<T>(Action<ClassOverridesBuilder<T>> config)
        {
            var c = new ClassOverridesBuilder<T>(_overrides.GetOverridesForType(typeof(T)));
            config(c);
            Map<T>();
        }

        /// <summary>
        /// Creates the overrideable profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        private IAutoMappingProfile CreateOverrideableProfile(IAutoMappingProfile profile)
        {
            return new OverridableAutoMappingProfile(profile, _overrides);
        }

        /// <summary>
        /// 
        /// </summary>
        private class FilteredProfile
        {
            public Func<Type, bool> Filter;
            public IAutoMappingProfile Profile;
        }
    }
}
