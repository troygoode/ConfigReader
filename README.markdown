# ConfigReader

Type-safe, convention-over-configuration access to the .Net application configuration, web.config, or other configuration source.

# How do I use it?

### Step One: Install the Library

Install ["ConfigReader"](http://nuget.org/List/Packages/ConfigReader) via [NuGet](http://nuget.org) via the GUI or "Install-Package ConfigReader".

### Step Two: Create the Interface & Default-Supplying Class

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

### Step Three: Retrieve Instance of IMailNotificationSettings With Defaults From DefaultMailNotificationSettings

Instantiate an instance of `ConfigReader` and `SetupConfigFor<IYourInterface>`.

    ConfigurationReader configReader = new ConfigurationReader().SetupConfigOf<IMailNotificationSettings>();

Supply defaults from `DefaultMailNotificationSettings` to ConfigReader.

    IMailNotificationSettings mySettings = configReader.ConfigBrowser.Get<IMailNotificationSettings>(new DefaultMailNotificationSettings());

The `mySettings` now contains an instance implementing `IMailNotificationSettings` with defaults from `DefaultMailNotificationSettings`:

    Assert.Equal("http://localhost", mySettings.SmtpServerAddress);
    Assert.Equal("", mySettings.Username);
    Assert.Equal("", mySettings.Password);

### Step Four (Optional): Override Default Settings via App.config/Web.config

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

# Defaults Via Anonymous Object

    IMailNotificationSettings mySettings = configReader.ConfigBrowser.Get<IMailNotificationSettings>(new {
        SmtpServerAddress = new Uri("http://localhost"),
        Username = String.Empty,
        Password = String.Empty
    });

# No Defaults, Config File Only

The above example works even with creating/using `DefaultMailNotificationSettings`:

    ConfigurationReader configReader = new ConfigurationReader().SetupConfigOf<IMailNotificationSettings>();
    IMailNotificationSettings mySettings = configReader.ConfigBrowser.Get<IMailNotificationSettings>();

Note that doing this requires you to satisfy the `IMailNotificationSettings` with appropriate entries in your configuration file:

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <appSettings>
        <add key="MailNotificationSettings.SmtpServerAddress" value="http://localhost" />
        <add key="MailNotificationSettings.Username" value="user123" />
        <add key="MailNotificationSettings.Password" value="passwd123" />
      </appSettings>
    </configuration>

# Custom Type Converters

Let's say you have the following configuration interface:

    public interface ICustomConversionExample
    {
        IPAddress Address { get; }
        string[] AdminUsernames { get; }
    }

And the following configuration file:

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <appSettings>
        <add key="CustomConversionExample.Address" value="127.0.0.1" />
        <add key="CustomConversionExample.AdminUsernames" value="Reshef.Mann;Troy.Goode" />
      </appSettings>
    </configuration>

You can process these by registering your own custom type converters:

    var configReader = new ConfigurationReader()
        .SetupCustomConverter(source => IPAddress.Parse(source))
        .SetupCustomConverter(source => source.Split(';'))
        .SetupConfigOf<ICustomConversionExample>();
    
    var mySettings = configReader.ConfigBrowser.Get<ICustomConversionExample>;
    Assert.False(mySettings.Address.IsIPv6LinkLocal);
    Assert.Equal(2, mySettings.AdminUsernames.Count);
    Assert.Equal("Reshef.Mann", mySettings.AdminUsernames[0]);
    Assert.Equal("Troy.Goode", mySettings.AdminUsernames[1]);

# License

Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).