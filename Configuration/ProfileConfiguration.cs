using System.Configuration;

namespace Nancy.ExtJS.Configuration
{
    public class ProfileConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("development")]
        public BuildConfiguration Development
        {
            get
            {
                return (BuildConfiguration)this["development"];
            }
        }

        [ConfigurationProperty("production")]
        public BuildConfiguration Production
        {
            get
            {
                return (BuildConfiguration)this["production"];
            }
        }
    }
}
