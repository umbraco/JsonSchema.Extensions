# Umbraco.JsonSchema.Extensions

Allows adding one or more references to a JSON schema using an MSBuild task.

```xml
<Target Name="AddJsonSchemaReferences" BeforeTargets="Build">
  <ItemGroup>
    <_References Include="https://json.schemastore.org/appsettings.json" />
    <_References Include="./appsettings-schema.Umbraco.Cms.json" />
  </ItemGroup>
  <JsonSchemaAddReferences JsonSchemaFile="$(MSBuildProjectDirectory)\appsettings-schema.json" References="@(_References)" />
</Target>
```
