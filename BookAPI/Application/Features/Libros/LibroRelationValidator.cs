using Application.Abstractions.Persistence;

namespace Application.Features.Libros;

internal static class LibroRelationValidator
{
    public static async Task ValidateAsync(
        Guid? editoraId,
        IReadOnlyList<Guid>? autorIds,
        IEditoraRepository editoraRepo,
        IAutorRepository autorRepo,
        CancellationToken ct)
    {
        if (editoraId.HasValue)
        {
            var editora = await editoraRepo.GetByIdAsync(editoraId.Value, ct);
            if (editora is null)
                throw new KeyNotFoundException("Editora no encontrada.");
        }

        if (autorIds is { Count: > 0 })
        {
            var allExist = await autorRepo.AllExistAsync(autorIds, ct);
            if (!allExist)
                throw new KeyNotFoundException("Uno o más autores no existen.");
        }
    }
}
