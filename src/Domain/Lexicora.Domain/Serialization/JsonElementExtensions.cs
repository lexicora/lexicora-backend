using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Lexicora.Domain.Serialization;

public static class JsonElementExtensions
{
    /// <param name="element"></param>
    extension(JsonElement element)
    {
        /// <summary>
        /// Gets the value of a <see cref="JsonElement"/> in an easy to write way.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Value of provided type, given it's correct.</returns>
        public T? As<T>() => element.Deserialize<T>();

        /// <summary>
        /// Gets the value of a <see cref="JsonElement"/> in an easy to write way.
        /// </summary>
        /// <remarks>
        /// This method uses the provided <see cref="JsonTypeInfo{T}"/> for deserialization, which is useful for source-generated serialization scenarios.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="jti">The <see cref="JsonTypeInfo{T}"/> to use for deserialization.</param>
        /// <returns>Value of provided type, given it's correct.</returns>
        public T? As<T>(JsonTypeInfo<T> jti) => element.Deserialize(jti);
    }


    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(T value)
    {
        /// <summary>
        /// Sets the value of a <see cref="JsonElement"/> in an easy to write way.
        /// </summary>
        /// <returns>Value of provided type, given it's correct.</returns>
        public JsonElement ToJsonElement() => JsonSerializer.SerializeToElement(value);

        /// <summary>
        /// Sets the value of a <see cref="JsonElement"/> in an easy to write way.
        /// </summary>
        /// <remarks>
        /// This method uses the provided <see cref="JsonTypeInfo{T}"/> for serialization, which is useful for source-generated serialization scenarios.
        /// </remarks>
        /// <param name="jti">The <see cref="JsonTypeInfo{T}"/> to use for serialization.</param>
        /// <returns>Value of provided type, given it's correct.</returns>
        public JsonElement ToJsonElement(JsonTypeInfo<T> jti) => JsonSerializer.SerializeToElement(value, jti);
    }
}
