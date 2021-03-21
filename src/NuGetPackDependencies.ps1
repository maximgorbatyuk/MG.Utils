function Format-XML {Param ([string]$xmlfile) 
  $Doc=New-Object system.xml.xmlDataDocument 
  $doc.Load((Resolve-Path $xmlfile)) 
  $sw=New-Object system.io.stringwriter 
  $writer=New-Object system.xml.xmltextwriter($sw) 
  $writer.Formatting = [System.xml.formatting]::Indented 
  $doc.WriteContentTo($writer) 
  $sw.ToString() 
}

'*****'
'***** PowerShell script NugetPackDependencies 1.0.'
'***** Insert project package references as dependencies into package manifest (nuspec file)'
'*****'
'***** Start script'
'*****'


# Get VB.NET or C# project file.
$projFile = (ls -Path "*.vbproj", "*.csproj" | Select-Object -First 1).Name

# If project file cannot be found exit script.
if ($projFile -eq $null) {
    "***** Project file (" + $projFile + ") not found. Exit script"
    exit
}
else {
    "***** Get package references from project file: '" + $projFile + "'"
} 


                  
# Get content from project file.
$projFileContent = ls -Filter $projFile | Get-Content

# Convert content from project file to XML.
$projFileXml  = [xml]$projFileContent


# Namespace 
$nm = New-Object -TypeName System.Xml.XmlNamespaceManager -ArgumentList $projFileXml.NameTable
$nm.AddNamespace('x', 'http://schemas.microsoft.com/developer/msbuild/2003')


# Get package references from project file xml and put them in an list of new objects containg id and version.
$packRefs=$projFileXml.SelectNodes('/x:Project/x:ItemGroup/x:PackageReference', $nm) | 
ForEach-Object {New-Object -TypeName PSObject -Property @{
                    id = New-Object -TypeName Reflection.AssemblyName -ArgumentList $_.Include
                    version = New-Object -TypeName Reflection.AssemblyName -ArgumentList $_.Version}               
                } 

Write-Output $packRefs

# Create new XML tags for the nuspec file containing the id and version.
$packRefsXml= $packRefs | Select-Object @{L='deps'; E ={ "<dependency id=""" + $_.id + """ version=""" + $_.version + """ />"}}


# concatenate the tags.
$packRefsXmlConcat = ""
$packRefsXml | ForEach-Object { 
$packRefsXmlConcat = $packRefsXmlConcat +  $_.deps
}

# Get the nuspec file.
$nuspec = (ls -Path "*.nuspec" | Select-Object -First 1)
$nuspecFile = $nuspec.FullName

# If nuspec file cannot be found exit script.
"*****"
if (!$nuspecFile) {Write-Output '***** Nuspec file not found. Exit script'
                    exit}
else{"***** Insert dependencies into nuspec file: '" + $nuspec.NAme + "'"} 

# Put the nuspec XML in a var using .NET XmlDocument
$xmlNuspec = New-Object System.Xml.XmlDocument
$xmlNuspec.PreserveWhitespace = $true
$xmlNuspec.Load($nuspecFile)

# Remove all dependencies elements if present.
$tags =$xmlNuspec.package.metadata.SelectNodes("dependencies")

ForEach($tag in $tags) {
$xmlNuspec.package.metadata.RemoveChild($tag) | Out-Null # Suppress unwanted Output
}

# Namespace.
$nm = New-Object -TypeName System.Xml.XmlNamespaceManager -ArgumentList $xmlNuspec.NameTable
$nm.AddNamespace('x', '')

# Get the metadata tag from the xml
$metaDataElement = $xmlNuspec.SelectNodes('/x:package/x:metadata', $nm)  

# Create new dependecies element
$newDependenciesChild = $xmlNuspec.CreateElement("dependencies") 

# Add dependency elements to dependencies element
$newDependenciesChild.set_innerXML($packRefsXmlConcat) | Out-Null # Suppress unwanted Output

# Append dependencies child to metdata child
$metaDataElement.AppendChild($newDependenciesChild) | Out-Null # Suppress unwanted Output

# Write output to temporary nuspec file
$xmlNuspec.OuterXml | Out-File -filepath temp.nuspec 

# Pretty the nuspec file and overwrite original nupec file using the format-XML function.
Format-XML -xmlfile temp.nuspec | Out-File -filepath $nuspecFile

# delete temp nuspec.
del temp.nuspec

"*****"
"***** Finished script"
"*****"