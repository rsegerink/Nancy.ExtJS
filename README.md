Nancy.ExtJS 
===================

Even though the whole Sencha Ext JS build process is highly configurable (down to custom Ant-based tasks), integrating an application built on top of Sencha Cmd into a server-side Nancy web application is cumbersome and requires a lot of configuration and boiler-plate code on the server. This is especially true because applications based on Sencha Cmd differentiate between production and development mode. While production mode requires a fully built application package, development mode can be run just from the uncompiled and unpacked source files – a must during development. These two modes have different characteristics with different requirements, esp. when not running the Sencha Cmd built-in development server but running on a full server-side application stack (e.g. backed by IIS and Nancy). To mitigate this difference when developing Ext JS 6 based application on basis of a Nancy web application, I've greated this Nancy.ExtJS package.

How to use
-

Download this project and reference it in your Nancy application.

Generate Ext JS application using Sencha Cmd (in a subfolder of your Nancy root path):
```
sencha generate app -ext -classic MyApp my-app
```
Test the generated Ext JS application:
```
cd my-app
sencha app watch // CTRL+C to stop
```
You now have a Ext JS application running in development mode on localhost port 1841. Test if it loads correctly in your browser.

Add Ext JS configuration to the web.config of Nancy:
```xml
<configSections>
  <section name="ExtJS" type="Nancy.ExtJS.Configuration.ExtJSConfiguration, Nancy.ExtJS"/>
</configSections>
<ExtJS appPath="my-app">
  <builds>
    <profile name="default">
      <development buildPath="build/development/MyApp" />
      <production buildPath="build/production/MyApp" />
    </profile>
  </builds>
</ExtJS>
```
Above items deserve some additional comments. The **appPath** variable points to the path of the Ext JS application we created above relative to the Nancy root path. The builds configuration section contains information about the build profiles configured in the Ext JS application. In our example we just use one build profile, which should be named default, it’s just used to reference a build in multi-build setups. Each build configuration is split into development and production settings respectively. The **development** section defines the application environment used in *development* mode (running Nancy in Debug mode). The **production** section on the other hand configures the application environment for *production* stage (running Nancy in Release mode). The **buildPath** variable is the path to the directory containing build artifacts produced by either sencha app build (*production*) or sencha app watch (*development*), this path is relative to **appPath**.

Add Index.html to Nancy View folder:
```html
<!DOCTYPE HTML>
<html manifest="">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=10, user-scalable=yes">

    <title>MyApp</title>

    <script id="microloader" data-app="e64f50b5-1f03-4476-8af3-4282620bd175" type="text/javascript" src="microloader.js"></script>
</head>
<body>
</body>
</html>
```
You can copy the index.html file from the Ext JS application folder, the only thing you need to change is the src filename property in the script tag from **bootstrapper.js** to **microloader.js**

Define a route to the index.html:
```c#
public class IndexModule : NancyModule
{
    public IndexModule()
    {
        Get["/"] = _ => View["index"];
    }
}
```
Restarting Nancy and navigate to the nancy index html will show the Ext JS demo application but served via our Nancy based backend.

There are two small problems however when trying to run the application in *production* mode. First of all we have to build the *production* package for our application first.
```
cd my-app
sencha app build
```
But secondly our application is configured to use a so-called embedded micro loader in *production* mode by default. This means that the micro loader code is literally copied into the index page of our application (by default that’s **index.html**). To see how that looks like you can take a look into **my-app/build/production/MyApp/index.html**. There are two ways to solve this issue. First is to copy the micro loader code from the **index.html** file into our Nancy index view. Second option (that’s the one we’re going to use) is to tweak the application build configuration so that Sencha Cmd produces an external micro loader build artifact. Open **my-app/app.json** and change the output microloader property to match:
```json
"output": {
    "base": "${workspace.build.dir}/${build.environment}/${app.name}",
    "microloader": {
      "embed": false
    },
    ...
}
```
This instructs Sencha Cmd to disable the embedded micro loader. 
There's only one small problem left, the name of the manifest file is, by default, different between *development* and *production* mode. It would be nice if it stays the same (**bootstrap.json**), so we will again change the output settings in our **my-app/app.json**, add the manifest property with a path property like:
```json
"output": {
    "base": "${workspace.build.dir}/${build.environment}/${app.name}",
    "microloader": {
      "embed": false
    },
    "manifest": {
      "path": "bootstrap.json"
    },
    ...
}
```
This instructs Sencha Cmd to output the manifest details in a file called **bootstrap.json** in *production* mode. Now run again:
```
sencha app build
```
This produces **my-app/build/production/MyApp/microloader.js** and **my-app/build/production/MyApp/bootstrap.json** which is being picked up by our StaticContentConventionBuilder and served as application microloader and manifest respectively. Now you can run your Nancy application in Release mode. 

That’s how to integrate an Ext JS application into a Nancy based backend!
