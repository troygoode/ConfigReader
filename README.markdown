# ConfigReader

Type-safe, convention-over-configuration access to the .Net application configuration, web.config, or other configuration source.

# How do I use it?

## Step One: Install the Library

Install ["ConfigReader"](http://nuget.org/List/Packages/ConfigReader) via [NuGet](http://nuget.org) via the GUI or "Install-Package ConfigReader".

## Step Two: Create the Interface & Default-Supplying Class

Create an interface representing your configuration object.

    public interface IMailNotificationSettings{
        Uri SmtpServerAddress { get; }
        string Username { get; }
        string Password { get; }
    }

Create a class that implements the above interface that supplies the defaults for the configuration.

    public class DefaultMailNotificationSettings : IMailNotificationSettings{
        public Uri SmtpServerAddress { get { return new Uri("http://localhost"); } }
        public string Username { get { return string.Empty; } }
        public string Password { get { return string.Empty; } }     
    }

## Step Four: Retrieve Instance of IMailNotificationSettings With Defaults From DefaultMailNotificationSettings

Instantiate an instance of `ConfigReader` and `SetupConfigFor<IYourInterface>`.

    ConfigurationReader configReader = new ConfigurationReader().SetupConfigOf<IMailNotificationSettings>();

Supply defaults from `DefaultMailNotificationSettings` to ConfigReader.

    IMailNotificationSettings mySettings = configReader.ConfigBrowser.Get<IMailNotificationSettings>(new DefaultMailNotificationSettings());

The `mailNotificationSettings` now contains an instance implementing `IMailNotificationSettings` with defaults from `DefaultMailNotificationSettings`:

    Assert.Equal("http://localhost", mySettings.SmtpServerAddress);
    Assert.Equal("", mySettings.Username);
    Assert.Equal("", mySettings.Password);

## Step Five (Optional): Override Default Settings via App.config/Web.config

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <appSettings>
        <add key="MailNotificationSettings.Username" value="user123" />
        <add key="MailNotificationSettings.Password" value="passwd123" />
      </appSettings>
    </configuration>

Using the above config file, `Username` and `Password` are overriden, but the default value for `SmtpServerAddress` remains:

    Assert.Equal("http://localhost", mySettings.SmtpServerAddress); // default
    Assert.Equal("user123", mySettings.Username); // overridden from config!
    Assert.Equal("passwd123", mySettings.Password); // overridden from config!

# License

Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).