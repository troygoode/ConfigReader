require 'albacore' # >= 0.2.7
require 'fileutils'
load './version.rb'

task :default => [:build]

assemblyinfo :generate_assemblyInfo do |asm|
  asm.version = VERSION
  asm.company_name = "Reshef Mann"
  asm.product_name = "ConfigReader"
  asm.title = "ConfigReader"
  asm.description = ""
  asm.copyright = "Apache License, Version 2.0"
  asm.custom_attributes \
    :CLSCompliant => true,
    :ComVisible => false,
    :Guid => "4622b561-de80-4f3f-9869-50647c33d779",
    #:AllowPartiallyTrustedCallers => nil,
    :AssemblyFileVersion => VERSION,
    :AssemblyConfiguration => '',
    :AssemblyTrademark => '',
    :AssemblyCulture => '',
    :InternalsVisibleTo => 'ConfigReader.Tests'
  asm.namespaces "System", "System.Security", "System.Runtime.CompilerServices"
  asm.output_file = "src/ConfigReader/Properties/AssemblyInfo.cs"
end

msbuild :build => [:generate_assemblyInfo] do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/ConfigReader.sln"
end

msbuild :release_build do |msb|
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
    '/out:Output\ConfigReader.dll',
    '/targetplatform:v4',
    '/internalize',
    '/log'
  ]
end

nunit :test => [:build] do |nunit|
  nunit.command = "src/packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
  nunit.assemblies "src/ConfigReader.Tests/bin/Debug/ConfigReader.Tests.dll"
end

#TODO: ILMerge dependency on Castle.DictionaryAdapter
#TODO: generate NuSpec
#TODO: package to .nupkg
#TODO: push nupkg to nuget.org