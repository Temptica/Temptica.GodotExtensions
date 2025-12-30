using System.Diagnostics.CodeAnalysis;
using Godot;

namespace Temptica.GodotExtensions;

public static class GodotObjectExtensions
{
	public static Task<Variant> CallAsync(this GodotObject godotObject, StringName methodName, params Variant[] args)
	{
		TaskCompletionSource<Variant> taskCompletionSource = new();
		Callable.From(() => taskCompletionSource.SetResult(godotObject.Call(methodName, args))).CallDeferred();
		return taskCompletionSource.Task;
	}

	public static Task<Error> EmitSignalAsync(this GodotObject godotObject, StringName signal, params Variant[] args)
	{
		TaskCompletionSource<Error> taskCompletionSource = new();
		Callable.From(() => taskCompletionSource.SetResult(godotObject.EmitSignal(signal, args))).CallDeferred();
		return taskCompletionSource.Task;
	}

	public static Task<Variant[]> ToSignalAsync(this GodotObject godotObject, StringName signal)
	{
		TaskCompletionSource<Variant[]> taskCompletionSource = new();

		Callable.From(
			() =>
			{
				var signalAwaiter = godotObject.ToSignal(godotObject, signal);
				Task.Run(
					async () =>
					{
						try
						{
							var signalArgs = await signalAwaiter;
							taskCompletionSource.SetResult(signalArgs);
						}
						catch (Exception exception)
						{
							GD.PushError($"Error occurred while await {nameof(GodotObject.ToSignal)}: {exception}");
							taskCompletionSource.SetException(exception);
						}
					}
				);
			}
		).CallDeferred();

		return taskCompletionSource.Task;
	}
	
	public static async Task<TArg0?> ToSignalAsync<TArg0>(this GodotObject godotObject, StringName signal)
	{
		var results = await godotObject.ToSignalAsync(signal);

		if (results.Length < 1)
		{
			throw new InvalidOperationException($"The signal has {results.Length} parameters but expected at least 1.");
		}

		var arg0 = CastOrThrow<TArg0>(results, 0);
		return arg0;
	}

	public static async Task<(TArg0?, TArg1?)> ToSignalAsync<TArg0, TArg1>(
		this GodotObject godotObject,
		StringName signal
	)
	{
		var results = await godotObject.ToSignalAsync(signal);

		if (results.Length < 2)
		{
			throw new InvalidOperationException($"The signal has {results.Length} parameters but expected at least 2.");
		}

		var arg0 = CastOrThrow<TArg0>(results, 0);
		var arg1 = CastOrThrow<TArg1>(results, 1);
		return (arg0, arg1);
	}

	public static async Task<(TArg0?, TArg1?, TArg2?)> ToSignalAsync<TArg0, TArg1, TArg2>(
		this GodotObject godotObject,
		StringName signal
	)
	{
		var results = await godotObject.ToSignalAsync(signal);

		if (results.Length < 2)
		{
			throw new InvalidOperationException($"The signal has {results.Length} parameters but expected at least 3.");
		}

		var arg0 = CastOrThrow<TArg0>(results, 0);
		var arg1 = CastOrThrow<TArg1>(results, 1);
		var arg2 = CastOrThrow<TArg2>(results, 2);
		return (arg0, arg1, arg2);
	}

	private static bool TryCast<TValue>(Variant variant, [NotNullWhen(true)] out TValue? value)
	{
		if (variant is TValue typedVariant)
		{
			value = typedVariant;
			return true;
		}

		switch (variant.VariantType)
		{
			case Variant.Type.Nil:
				value = default;
				return !Equals(value, default(TValue));

			case Variant.Type.Bool:
				if (typeof(TValue) != typeof(bool))
				{
					throw new InvalidCastException();
				}

				value = (TValue)(object)variant.AsBool();
				return true;

			case Variant.Type.Int:
				if (typeof(TValue).IsEnum)
				{
					try
					{
						var castedEnum = Enum.ToObject(typeof(TValue), (int)variant);
						value = (TValue)castedEnum;
						return !Equals(value, default(TValue));
					}
					catch (InvalidCastException)
					{
						value = default;
						return false;
					}
				}

				if (typeof(TValue) == typeof(sbyte))
				{
					value = (TValue)(object)variant.AsSByte();
					return true;
				}

				if (typeof(TValue) == typeof(byte))
				{
					value = (TValue)(object)variant.AsByte();
					return true;
				}

				if (typeof(TValue) == typeof(short))
				{
					value = (TValue)(object)variant.AsInt16();
					return true;
				}

				if (typeof(TValue) == typeof(ushort))
				{
					value = (TValue)(object)variant.AsUInt16();
					return true;
				}

				if (typeof(TValue) == typeof(int))
				{
					value = (TValue)(object)variant.AsInt32();
					return true;
				}

				if (typeof(TValue) == typeof(uint))
				{
					value = (TValue)(object)variant.AsUInt32();
					return true;
				}

				if (typeof(TValue) == typeof(long))
				{
					value = (TValue)(object)variant.AsInt64();
					return true;
				}

				if (typeof(TValue) == typeof(ulong))
				{
					value = (TValue)(object)variant.AsUInt64();
					return true;
				}

				throw new InvalidCastException();

			case Variant.Type.Float:
				if (typeof(TValue) == typeof(float))
				{
					value = (TValue)(object)variant.AsSingle();
					return true;
				}

				if (typeof(TValue) == typeof(double))
				{
					value = (TValue)(object)variant.AsDouble();
					return true;
				}

				break;
			case Variant.Type.String:
				if (typeof(TValue) == typeof(string))
				{
					value = (TValue)(object)variant.AsString();
					return !Equals(value, default(TValue));
				}

				break;
			case Variant.Type.Vector2:
			case Variant.Type.Vector2I:
			case Variant.Type.Rect2:
			case Variant.Type.Rect2I:
			case Variant.Type.Vector3:
			case Variant.Type.Vector3I:
			case Variant.Type.Transform2D:
			case Variant.Type.Vector4:
			case Variant.Type.Vector4I:
			case Variant.Type.Plane:
			case Variant.Type.Quaternion:
			case Variant.Type.Aabb:
			case Variant.Type.Basis:
			case Variant.Type.Transform3D:
			case Variant.Type.Projection:
			case Variant.Type.Color:
			case Variant.Type.StringName:
			case Variant.Type.NodePath:
			case Variant.Type.Rid:
			case Variant.Type.Object:
			case Variant.Type.Callable:
			case Variant.Type.Signal:
			case Variant.Type.Dictionary:
			case Variant.Type.Array:
			case Variant.Type.PackedByteArray:
			case Variant.Type.PackedInt32Array:
			case Variant.Type.PackedInt64Array:
			case Variant.Type.PackedFloat32Array:
			case Variant.Type.PackedFloat64Array:
			case Variant.Type.PackedStringArray:
			case Variant.Type.PackedVector2Array:
			case Variant.Type.PackedVector3Array:
			case Variant.Type.PackedColorArray:
			case Variant.Type.Max:
			case Variant.Type.PackedVector4Array:
			default:
				break;
		}

		throw new NotImplementedException();
	}

	private static TArg? CastOrThrow<TArg>(Variant[] results, int index)
	{
		var resultAtIndex = results[index];
		if (TryCast(resultAtIndex, out TArg? value))
		{
			return value;
		}

		var expectedType = typeof(TArg);
		var expectedTypeName = expectedType.FullName ?? expectedType.Name;
		var actualType = resultAtIndex.GetType();
		var actualTypeName = actualType.FullName ?? actualType.Name;

		throw new InvalidOperationException(
			$"Expected index {index} to be {expectedTypeName} but it was {actualTypeName}"
		);
	}
}