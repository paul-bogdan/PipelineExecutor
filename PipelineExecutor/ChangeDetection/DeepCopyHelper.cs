using System.Collections;
using System.Reflection;

namespace PipelineExecutor.ChangeDetection;

/// <summary>
/// Provides deep copy functionality for objects, supporting arrays, lists, dictionaries, and circular references.
/// </summary>
public static class DeepCopyHelper
{
    /// <summary>
    /// Creates a deep copy of the given object.
    /// </summary>
    /// <typeparam name="T">Type of the object to copy.</typeparam>
    /// <param name="original">The object to deep copy.</param>
    /// <returns>A deep copy of the original object.</returns>
    public static T DeepCopy<T>(T original)
    {
        var clonedObject= (T?)DeepCopyObject(original!, new Dictionary<object, object>());
        ArgumentNullException.ThrowIfNull(clonedObject);
        return clonedObject;
    }

    /// <summary>
    /// Recursively deep copies an object, handling collections and circular references.
    /// </summary>
    /// <param name="original">The object to copy.</param>
    /// <param name="visited">Dictionary to track already copied objects (for circular reference handling).</param>
    /// <returns>A deep copy of the object.</returns>
    private static object? DeepCopyObject(object? original, Dictionary<object, object> visited)
    {
        if (original == null) return null;

        var type = original.GetType();

        // Return directly for primitive types, enums, strings, decimals, and DateTime
        if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime))
            return original;

        // Prevent circular references
        if (visited.ContainsKey(original))
            return visited[original];

        // Handle arrays
        if (type.IsArray)
        {
            var array = (Array)original;
            var elementType = type.GetElementType()!;
            var copied = Array.CreateInstance(elementType, array.Length);
            visited[original] = copied;
            for (int i = 0; i < array.Length; i++)
                copied.SetValue(DeepCopyObject(array.GetValue(i), visited), i);
            return copied;
        }

        // Handle lists/collections
        if (typeof(IList).IsAssignableFrom(type))
        {
            var list = (IList?)Activator.CreateInstance(type);
            if (list == null) return null;
            visited[original] = list;
            foreach (var item in (IList)original)
                list.Add(DeepCopyObject(item, visited));
            return list;
        }

        // Handle dictionaries
        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            var dict = (IDictionary?)Activator.CreateInstance(type);
            if (dict == null) return null;
            visited[original] = dict;
            var originalDict = (IDictionary)original;
            foreach (var key in originalDict.Keys)
                dict[DeepCopyObject(key, visited)!] = DeepCopyObject(originalDict[key], visited);
            return dict;
        }

        // Create a new instance for other reference types
        var copy = Activator.CreateInstance(type);
        if (copy == null) return null;
        visited[original] = copy;

        // Copy properties
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead || !prop.CanWrite) continue;
            var value = prop.GetValue(original);
            prop.SetValue(copy, DeepCopyObject(value, visited));
        }

        // Copy fields (including private)
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var value = field.GetValue(original);
            field.SetValue(copy, DeepCopyObject(value, visited));
        }

        return copy;
    }
}
