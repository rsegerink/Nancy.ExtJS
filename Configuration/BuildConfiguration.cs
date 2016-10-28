using System.Configuration;

namespace Nancy.ExtJS.Configuration
{
    public class BuildConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("buildPath", IsRequired = true)]
        public string BuildPath
        {
            get
            {
                return (string)this["buildPath"];
            }
        }
    }
}
