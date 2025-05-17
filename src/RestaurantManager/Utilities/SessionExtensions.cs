using System.Text.Json;

namespace RestaurantManager.Utilities;

public static class SessionExtensions
{
    private readonly static JsonSerializerOptions options = new()
    {
        // Enable handling of circular references in the object graph
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
        MaxDepth = 64,  // You can adjust this if your objects are very deep
    };

    /// <summary>
    /// Set an object into the session as JSON, allowing for circular references.
    /// </summary>
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        // Check for null values to avoid serializing null objects
        if (value == null)
        {
            session.Remove(key); // Optionally, remove the key if value is null
            return;
        }

        var json = JsonSerializer.Serialize(value, options);
        session.SetString(key, json); // Store as JSON
    }

    /// <summary>
    /// Get an object from the session by deserializing JSON.
    /// </summary>
    public static T? GetObject<T>(this ISession session, string key)
    {
        var json = session.GetString(key);

        // If the session doesn't contain the key, return default
        if (json == null)
        {
            return default;
        }

        // Specify JsonSerializerOptions if needed (e.g., for handling circular references or custom converters)
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,  // Enable circular reference handling
            MaxDepth = 64  // Adjust if necessary
        };

        return JsonSerializer.Deserialize<T>(json, options);
    }
}