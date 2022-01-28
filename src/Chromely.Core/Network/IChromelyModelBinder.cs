namespace Chromely.Core.Network;

public interface IChromelyModelBinder
{
    /// <summary>
    /// Binds a request property to controller action argument based on request property/contoller action argument name.
    /// </summary>
    /// <param name="parameterName">The argument name.</param>
    /// <param name="type">The action argument <see cref="Type"/>.</param>
    /// <param name="content">The request json property <see cref="JsonElement"/>.</param>
    /// <returns>The resultant object.</returns>
    object Bind(string parameterName, Type type, JsonElement content);
}