using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar
{
    public class PremiarClubeAuthorizationSettings : ConfigurationSection
    {
        public PremiarClubeAuthorizationSettings()
        {
        }

        [ConfigurationProperty(name: "clientId", IsRequired = true)]
        public Guid ClientId {
            get { return (Guid)this["clientId"]; }
            set { this["clientId"] = value; }
        }
        [ConfigurationProperty(name: "clientSecret", IsRequired = true)]
        public string ClientSecret
        {
            get { return Uri.UnescapeDataString((string)this["clientSecret"]); }
            set { this["clientSecret"] = value; }
        }
        [ConfigurationProperty(name: "subscriptionKey", IsRequired = true)]
        public string SubscriptionKey
        {
            get { return Uri.UnescapeDataString((string)this["subscriptionKey"]); }
            set { this["subscriptionKey"] = value; }
        }
    }

    public class PremiarClubeCredentialsSettings : ConfigurationSection
    {
        public PremiarClubeCredentialsSettings()
        {
        }

        [ConfigurationProperty(name: "campaignId", IsRequired = true)]
        public int CampaignId
        {
            get { return (int)this["campaignId"]; }
            set { this["campaignId"] = value; }
        }
        [ConfigurationProperty(name: "campaignName", IsRequired = true)]
        public string CampaignName
        {
            get { return (string)this["campaignName"]; }
            set { this["campaignName"] = value; }
        }
        [ConfigurationProperty(name: "clientId", IsRequired = true)]
        public int ClientId
        {
            get { return (int)this["clientId"]; }
            set { this["clientId"] = value; }
        }
        [ConfigurationProperty(name: "catalogId", IsRequired = true)]
        public int CatalogId
        {
            get { return (int)this["catalogId"]; }
            set { this["catalogId"] = value; }
        }
        [ConfigurationProperty(name: "profileId", IsRequired = true)]
        public int ProfileId
        {
            get { return (int)this["profileId"]; }
            set { this["profileId"] = value; }
        }
    }

    public class PremiarClubeApiSettings : ConfigurationSection
    {
        public PremiarClubeApiSettings()
        {
        }

        [ConfigurationProperty(name: "baseUri", IsRequired = true)]
        public string BaseUri
        {
            get { return (string)this["baseUri"]; }
            set { this["baseUri"] = value; }
        }
    }

    public class PremiarClubeSettings : ConfigurationSectionGroup
    {
        public PremiarClubeSettings()
        {
        }

        [ConfigurationProperty("api", IsRequired = true)]
        public PremiarClubeApiSettings Api
        {
            get { return (PremiarClubeApiSettings)Sections["api"]; }
        }

        [ConfigurationProperty("authorization", IsRequired = true)]
        public PremiarClubeAuthorizationSettings Authorization
        {
            get { return (PremiarClubeAuthorizationSettings)Sections["authorization"]; }
        }

        [ConfigurationProperty("credentials", IsRequired = true)]
        public PremiarClubeCredentialsSettings Credentials
        {
            get { return (PremiarClubeCredentialsSettings)Sections["credentials"]; }
        }
    }
}
