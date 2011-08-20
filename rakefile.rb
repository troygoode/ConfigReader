require 'albacore' # >= 0.2.7
require 'fileutils'
load './version.rb'

task :default => [:build]

assemblyinfo :generate_assemblyInfo do |asm|
  asm.version = VERSION
  asm.company_name = "Reshef Mann"
  asm.product_name = "ConfigReader"
  asm.title = "ConfigReader"
  asm.description = "Type-safe, convention-over-configuration access to the .Net application configuration, web.config, or other configuration source."
  asm.copyright = "Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0.html)"
  asm.custom_attributes \
    :CLSCompliant => true,
    :ComVisible => false,
    :Guid => "4622b561-de80-4f3f-9869-50647c33d779",
    :AllowPartiallyTrustedCallers => nil,
    :AssemblyFileVersion => VERSION,
    :AssemblyConfiguration => '',
    :AssemblyTrademark => '',
    :AssemblyCulture => '',
    :InternalsVisibleTo => 'ConfigReader.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a530732931867ea26331e8756ee235dc44d74dba33d4bc0c79d4a2363a3970de876605ff3e536065993d6fa7f69ae3c08d96dea44ff53098ef1db45e7a22c7469a30bab6e9566c6edaccaec2d5e871c54f7d5b8ea0ace6a85876ec19c84e9416419726ac44f9e268fbabd4f265d563c0aa251936a334e6431ed0dcc600577385'
  asm.namespaces "System", "System.Security", "System.Runtime.CompilerServices"
  asm.output_file = "src/ConfigReader/Properties/AssemblyInfo.cs"
end

msbuild :build => :generate_assemblyInfo do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/ConfigReader.sln"
end

msbuild :release_build => :generate_assemblyInfo do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/ConfigReader.sln"
end

exec :release => :release_build do |cmd|
  cmd.command = 'tools\ILMerge-2.11\ILMerge.exe'
  cmd.parameters [
    'ConfigReader.dll',
    'Castle.Components.DictionaryAdapter.dll',
    '/lib:src\ConfigReader\bin\Release',
    '/out:output\ConfigReader.dll',
    '/targetplatform:v4',
    '/internalize',
    '/log'
  ]
end

nunit :test => :build do |nunit|
  nunit.command = "src/packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
  nunit.assemblies "src/ConfigReader.Tests/bin/Debug/ConfigReader.Tests.dll"
end

nuspec :generate_nuspec => [:test, :release] do |nuspec|
  nuspec.title = "ConfigReader"
  nuspec.id = "ConfigReader"
  nuspec.version = VERSION
  nuspec.authors = "Reshef Mann"
  nuspec.owners = "TroyGoode"
  nuspec.description = "Type-safe, convention-over-configuration access to the .Net application configuration, web.config, or other configuration source."
  nuspec.language = "en-US"
  nuspec.licenseUrl = "http://www.apache.org/licenses/LICENSE-2.0.html"
  nuspec.projectUrl = "http://github.com/TroyGoode/ConfigReader"
  nuspec.tags = "configuration config conventions defaults"
  nuspec.file "ConfigReader.dll", "lib/net40"
  nuspec.file "ConfigReader.pdb", "lib/net40"
  nuspec.output_file = "./output/ConfigReader.nuspec"
end

nugetpack :package => :generate_nuspec do |nuget|
  nuget.nuspec = './output/ConfigReader.nuspec'
  nuget.output = './output/'
end

nugetpush :push => :package do |nuget|
  nuget.package = "./output/ConfigReader.#{VERSION}.nupkg"
end