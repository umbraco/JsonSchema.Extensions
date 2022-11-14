using System;
using Microsoft.Build.Framework;
using NUnit.Framework;

namespace Umbraco.JsonSchema.Extensions.UnitTests;

/// <summary>
/// Tests for the <see cref="JsonSchemaAddReferences" /> MSBuild task.
/// </summary>
[TestFixture]
public class JsonSchemaAddReferencesTests
{
    /// <summary>
    /// Tests whether passing empty references to the task does not create the file.
    /// </summary>
    [Test]
    public void EmptyReferences_DoesNotCreateFile()
    {
        var sut = new JsonSchemaAddReferences()
        {
            JsonSchemaFile = "should-not-exist.json",
            References = Array.Empty<ITaskItem>()
        };

        var result = sut.Execute();

        Assert.True(result);
        FileAssert.DoesNotExist("should-not-exist.json");
    }
}
