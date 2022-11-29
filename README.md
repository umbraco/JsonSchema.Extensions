# Umbraco.JsonSchema.Extensions

Extensions for Umbraco to add JSON schema references and update JSON properties using MSBuild tasks.

## JsonSchemaAddReferences

Adds references to a JSON schema file.

```xml
<Target Name="AddJsonSchemaReferences" BeforeTargets="Build">
  <ItemGroup>
    <_References Include="https://json.schemastore.org/appsettings.json" />
    <_References Include="appsettings-schema.Umbraco.Cms.json#" />
  </ItemGroup>
  <JsonSchemaAddReferences JsonSchemaFile="$(MSBuildProjectDirectory)\appsettings-schema.json" References="@(_References)" />
</Target>
```

## JsonPathUpdateValue

Updates the value of a property in a JSON file using a JSON path expression.

```xml
<Target Name="UpdatePackageManifestVersion" DependsOnTargets="Build" AfterTargets="GetBuildVersion;GetUmbracoBuildVersion">
  <ItemGroup>
    <_PackageManifestFiles Include="**\package.manifest" />
  </ItemGroup>
  <JsonPathUpdateValue JsonFile="%(_PackageManifestFiles.FullPath)" Path="$.version" Value="&quot;$(PackageVersion)&quot;" />
</Target>
```
