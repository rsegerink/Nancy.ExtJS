using System;
using System.Collections.Generic;
using System.IO;

using Nancy.Conventions;

using Nancy.ExtJS.Configuration;

namespace Nancy.ExtJS.Conventions
{
    public class ExtJSContentsConventions : IConvention
    {
        public void Initialise(NancyConventions conventions)
        {
            var settings = ExtJSConfiguration.Settings;

            conventions.StaticContentsConventions = new List<Func<NancyContext, string, Response>>
            {
                StaticContentConventionBuilder.AddFile("/microloader.js", settings.GetMicroLoaderFile()),
                StaticContentConventionBuilder.AddFile("/bootstrap.json", settings.GetManifestFile()),
                StaticContentConventionBuilder.AddFile("/app.js", settings.GetAppFile()),
#if DEBUG
                StaticContentConventionBuilder.AddDirectory("app", Path.Combine(settings.ApplicationPath, "app"), "js"),
                StaticContentConventionBuilder.AddDirectory("build", Path.Combine(settings.ApplicationPath, "build")),
                StaticContentConventionBuilder.AddDirectory("ext", Path.Combine(settings.ApplicationPath, "ext"), "js")
#else
                StaticContentConventionBuilder.AddDirectory("resources", Path.Combine(settings.GetBuildPath(), "resources"))
#endif
            };
        }

        public Tuple<bool, string> Validate(NancyConventions conventions)
        {
            if (conventions.StaticContentsConventions == null)
            {
                return Tuple.Create(false, "The static contents conventions cannot be null.");
            }

            return (conventions.StaticContentsConventions.Count > 0) ?
                Tuple.Create(true, string.Empty) :
                Tuple.Create(false, "The static contents conventions cannot be empty.");
        }
    }
}
