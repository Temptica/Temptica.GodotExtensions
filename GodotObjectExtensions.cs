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
				return value != null;

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
						return value != null;
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
					return value != null;
				}

				break;
			case Variant.Type.Vector2:
				break;
			case Variant.Type.Vector2I:
				break;
			case Variant.Type.Rect2:
				break;
			case Variant.Type.Rect2I:
				break;
			case Variant.Type.Vector3:
				break;
			case Variant.Type.Vector3I:
				break;
			case Variant.Type.Transform2D:
				break;
			case Variant.Type.Vector4:
				break;
			case Variant.Type.Vector4I:
				break;
			case Variant.Type.Plane:
				break;
			case Variant.Type.Quaternion:
				break;
			case Variant.Type.Aabb:
				break;
			case Variant.Type.Basis:
				break;
			case Variant.Type.Transform3D:
				break;
			case Variant.Type.Projection:
				break;
			case Variant.Type.Color:
				break;
			case Variant.Type.StringName:
				break;
			case Variant.Type.NodePath:
				break;
			case Variant.Type.Rid:
				break;
			case Variant.Type.Object:
				break;
			case Variant.Type.Callable:
				break;
			case Variant.Type.Signal:
				break;
			case Variant.Type.Dictionary:
				break;
			case Variant.Type.Array:
				break;
			case Variant.Type.PackedByteArray:
				break;
			case Variant.Type.PackedInt32Array:
				break;
			case Variant.Type.PackedInt64Array:
				break;
			case Variant.Type.PackedFloat32Array:
				break;
			case Variant.Type.PackedFloat64Array:
				break;
			case Variant.Type.PackedStringArray:
				break;
			case Variant.Type.PackedVector2Array:
				break;
			case Variant.Type.PackedVector3Array:
				break;
			case Variant.Type.PackedColorArray:
				break;
			case Variant.Type.Max:
				break;
			default:
				throw new NotImplementedException();
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

	public static async Task<TArg0?> ToSignalAsync<TArg0>(this GodotObject godotObject, StringName signal)
	{
		var results = await ToSignalAsync(godotObject, signal);

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
		var results = await ToSignalAsync(godotObject, signal);

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
		var results = await ToSignalAsync(godotObject, signal);

		if (results.Length < 2)
		{
			throw new InvalidOperationException($"The signal has {results.Length} parameters but expected at least 3.");
		}

		var arg0 = CastOrThrow<TArg0>(results, 0);
		var arg1 = CastOrThrow<TArg1>(results, 1);
		var arg2 = CastOrThrow<TArg2>(results, 2);
		return (arg0, arg1, arg2);
	}
}