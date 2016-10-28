using System.Configuration;

namespace Nancy.ExtJS.Configuration
{
    [ConfigurationCollection(typeof(ProfileConfiguration), AddItemName = "profile", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ProfilesConfiguration : ConfigurationElementCollection
    {
        public ConfigurationElementCollectionType CollectionType
        { get { return ConfigurationElementCollectionType.BasicMap; } }

        protected override string ElementName
        {
            get
            {
                return "profile";
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProfileConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ProfileConfiguration).Name;
        }

        public ProfileConfiguration this[int index]
        {
            get { return (ProfileConfiguration)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public ProfileConfiguration this[string name]
        {
            get { return (ProfileConfiguration)BaseGet(name); }
        }
    }
}
