using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;

namespace Nancy.ExtJS.Configuration
{
    public class ExtJSConfiguration : ConfigurationSection
    {
        protected BuildConfiguration GetBuild(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            if (Builds[name] == null)
            {
                throw new ArgumentException(string.Format("Build '{0}' does not exist."));
            }

            #if DEBUG
                return Builds[name].Development;
            #else
                return Builds[name].Production;
            #endif
        }

        protected string ResolvePath(string filename)
        {
            #if DEBUG
                return Path.Combine(ApplicationPath, Path.GetFileName(filename));
            #else
                return Path.Combine(GetBuildPath(), Path.GetFileName(filename));
            #endif
        }

        private static ExtJSConfiguration _appConfig = (ExtJSConfiguration)ConfigurationManager.GetSection("ExtJS");
        public static ExtJSConfiguration Settings { get { return _appConfig; } }

        [ConfigurationProperty("appPath", IsRequired = true)]
        public string ApplicationPath
        {
            get
            {
                return (string)base["appPath"];
            }
        }

        [ConfigurationProperty("builds", IsRequired = true)]
        public ProfilesConfiguration Builds
        {
            get
            {
                return (ProfilesConfiguration)base["builds"];
            }
        }

        public string GetBuildPath(string name = "default")
        {
            return Path.Combine(ApplicationPath, this.GetBuild(name).BuildPath);
        }

        public string GetMicroLoaderFile()
        {
            #if DEBUG
            return ResolvePath("bootstrap.js");
            #else
            return ResolvePath("microloader.js");
            #endif
        }

        public string GetManifestFile()
        {
            return ResolvePath("bootstrap.json");
        }

        public string GetAppFile()
        {
            return ResolvePath("app.js");
        }
    }
}
