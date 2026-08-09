using System.Reflection;
using NetArchTest.Rules;

namespace ArchitectureTests;

[TestFixture]
public sealed class LayerDependencyTests
{
  private static readonly Assembly DomainAssembly = typeof(Domain.Entities.Libro).Assembly;
  private static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
  private static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;
  private static readonly Assembly ApiAssembly = typeof(Program).Assembly;
  private static readonly Assembly SharedAssembly = typeof(Shared.Results.PagedResult<>).Assembly;

  [Test]
  public void Domain_NoDebeDependerDeOtrasCapas()
  {
    var result = Types.InAssembly(DomainAssembly)
      .ShouldNot()
      .HaveDependencyOnAny(
        ApplicationAssembly.GetName().Name!,
        InfrastructureAssembly.GetName().Name!,
        ApiAssembly.GetName().Name!,
        SharedAssembly.GetName().Name!)
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void Application_NoDebeDependerDeInfrastructureNiApi()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .ShouldNot()
      .HaveDependencyOnAny(
        InfrastructureAssembly.GetName().Name!,
        ApiAssembly.GetName().Name!)
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void Application_NoDebeReferenciarEntityFramework()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .ShouldNot()
      .HaveDependencyOn("Microsoft.EntityFrameworkCore")
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void Infrastructure_NoDebeDependerDeApi()
  {
    var result = Types.InAssembly(InfrastructureAssembly)
      .ShouldNot()
      .HaveDependencyOn(ApiAssembly.GetName().Name!)
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void Handlers_DebenResidirEnApplication()
  {
    var result = Types.InAssembly(ApplicationAssembly)
      .That()
      .ImplementInterface(typeof(MediatR.IRequestHandler<,>))
      .Or()
      .ImplementInterface(typeof(MediatR.IRequestHandler<>))
      .Should()
      .ResideInNamespace("Application.Features")
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void Controllers_DebenResidirEnApi()
  {
    var result = Types.InAssembly(ApiAssembly)
      .That()
      .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
      .Should()
      .ResideInNamespace("Api.Controllers")
      .GetResult();

    Assert.That(result.IsSuccessful, Is.True, FormatFailures(result));
  }

  [Test]
  public void ModulosVerticales_NoDebenImportarHandlersDeOtrosModulos()
  {
    var featureNamespaces = new[]
    {
      "Application.Features.Libros",
      "Application.Features.Autores",
      "Application.Features.Editoras",
      "Application.Features.Auth",
      "Application.Features.Admin"
    };

    foreach (var feature in featureNamespaces)
    {
      var others = featureNamespaces.Where(f => f != feature).ToArray();

      var result = Types.InAssembly(ApplicationAssembly)
        .That()
        .ResideInNamespace(feature)
        .ShouldNot()
        .HaveDependencyOnAny(others)
        .GetResult();

      Assert.That(result.IsSuccessful, Is.True, $"Violación en {feature}: {FormatFailures(result)}");
    }
  }

  private static string FormatFailures(TestResult result)
    => result.FailingTypes is { Count: > 0 }
      ? string.Join(", ", result.FailingTypes.Select(t => t.FullName))
      : "Violación de regla de arquitectura.";
}
