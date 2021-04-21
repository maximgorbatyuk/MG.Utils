# MaximGorbatyuk.Utils.Validation

This library is purposed for class validation based on Data Annotation attributes. Example:

```csharp

class Person
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [StringLength(200)]
    public string Email { get; set; }
}

[Xunit.Fact]
public void Cases()
{
    var person = new Person
    {
        FirstName = "John",
        LastName = "Smith",
        Email = "j.smith@example.com"
    };

    // No exception is thrown because the class is valid
    person.ThrowIfInvalid();

    var personWithInvalidData = new Person();

    Assert.Throws<MG.Utils.Validation.Exception.EntityInvalidException>(() => personWithInvalidData.ThrowIfInvalid());
}

```

## Tech spec

- Target Framework is `netstandard2.1`
- Dependencies:
    - `System.ComponentModel.Annotations`
    - `MaximGorbatyuk.Utils.Abstract`
