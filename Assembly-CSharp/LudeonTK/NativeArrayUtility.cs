using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace LudeonTK
{
	// Token: 0x0200005D RID: 93
	public static class NativeArrayUtility
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00013620 File Offset: 0x00011820
		public static void EnsureDisposed<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00013630 File Offset: 0x00011830
		public static void EnsureDisposed<[IsUnmanaged] T>(this NativeList<T> array) where T : struct, ValueType
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00013640 File Offset: 0x00011840
		public static void EnsureDisposed(this NativeBitArray array)
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00013650 File Offset: 0x00011850
		public static void EnsureDisposed<[IsUnmanaged] TElement, [IsUnmanaged] TPriority, [IsUnmanaged] TComparer>(this NativePriorityQueue<TElement, TPriority, TComparer> array) where TElement : struct, ValueType where TPriority : struct, ValueType where TComparer : struct, ValueType, IComparer<TPriority>
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00013660 File Offset: 0x00011860
		public static void EnsureDisposed<[IsUnmanaged] T>(this UnsafeList<T> list) where T : struct, ValueType
		{
			if (list.IsCreated)
			{
				list.Dispose();
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00013670 File Offset: 0x00011870
		public static void EnsureDisposed<[IsUnmanaged] T, [IsUnmanaged] U>(this UnsafeHeap<T, U> list) where T : struct, ValueType where U : struct, ValueType, IComparer<T>
		{
			if (list.IsCreated)
			{
				list.Dispose();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00013680 File Offset: 0x00011880
		public static void EnsureDisposed(this UnsafeBitArray bitArray)
		{
			if (bitArray.IsCreated)
			{
				bitArray.Dispose();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00013690 File Offset: 0x00011890
		public static void MemClear<[IsUnmanaged] T>(NativeArray<T> array) where T : struct, ValueType
		{
			UnsafeUtility.MemClear(array.GetUnsafePtr<T>(), (long)array.Length * (long)sizeof(T));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000136AD File Offset: 0x000118AD
		public unsafe static void MemClear<[IsUnmanaged] T>(UnsafeList<T> list) where T : struct, ValueType
		{
			UnsafeUtility.MemClear((void*)list.Ptr, (long)list.Length * (long)sizeof(T));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000136CA File Offset: 0x000118CA
		public unsafe static void MemClear<[IsUnmanaged] T>(NativeList<T> list) where T : struct, ValueType
		{
			UnsafeUtility.MemClear((void*)list.GetUnsafePtr<T>(), (long)list.Length * (long)sizeof(T));
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000136E7 File Offset: 0x000118E7
		public static void Clear<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000136EF File Offset: 0x000118EF
		public static void Clear<[IsUnmanaged] T>(this NativeList<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000136F7 File Offset: 0x000118F7
		public static void Clear<[IsUnmanaged] T>(this UnsafeList<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000136FF File Offset: 0x000118FF
		public static T[] GetInternalArray<T>(List<T> list)
		{
			return NativeArrayUtility.ArrayAccessor<T>.Getter(list);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0001370C File Offset: 0x0001190C
		public unsafe static NativeArray<T> GetNativeArrayCopy<[IsUnmanaged] T>(List<T> source, Allocator allocator, bool clearMemory = false) where T : struct, ValueType
		{
			T[] internalArray = NativeArrayUtility.GetInternalArray<T>(source);
			NativeArray<T> nativeArray = new NativeArray<T>(source.Count, allocator, clearMemory ? NativeArrayOptions.ClearMemory : NativeArrayOptions.UninitializedMemory);
			fixed (T[] array = internalArray)
			{
				void* source2;
				if (internalArray == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(nativeArray), source2, (long)source.Count * (long)UnsafeUtility.SizeOf<T>());
			}
			return nativeArray;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00013768 File Offset: 0x00011968
		public unsafe static NativeArray<T> GetNativeArrayCopy<[IsUnmanaged] T>(T[] source, Allocator allocator, bool clearMemory = false) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = new NativeArray<T>(source.Length, allocator, clearMemory ? NativeArrayOptions.ClearMemory : NativeArrayOptions.UninitializedMemory);
			fixed (T[] array = source)
			{
				void* source2;
				if (source == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(nativeArray), source2, (long)source.Length * (long)UnsafeUtility.SizeOf<T>());
			}
			return nativeArray;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000137B8 File Offset: 0x000119B8
		public unsafe static void CopyNativeToArray<[IsUnmanaged] T>(T[] destination, NativeArray<T> source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to copy a native array of length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(source.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" into an array with length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(destination.Length);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			fixed (T[] array = destination)
			{
				void* destination2;
				if (destination == null || array.Length == 0)
				{
					destination2 = null;
				}
				else
				{
					destination2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(destination2, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(source), (long)destination.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0001384C File Offset: 0x00011A4C
		public unsafe static void CopyUnsafeListToArray<[IsUnmanaged] T>(T[] destination, UnsafeList<T> source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to copy a unsafe array of length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(source.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" into an array with length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(destination.Length);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			fixed (T[] array = destination)
			{
				void* destination2;
				if (destination == null || array.Length == 0)
				{
					destination2 = null;
				}
				else
				{
					destination2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(destination2, (void*)source.Ptr, (long)destination.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000138E0 File Offset: 0x00011AE0
		public static void CopyNativeListToList<[IsUnmanaged] T>(List<T> destination, NativeList<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(source.Length, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001392C File Offset: 0x00011B2C
		public static void CopyUnsafeListToList<[IsUnmanaged] T>(List<T> destination, UnsafeList<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(destination.Count, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00013978 File Offset: 0x00011B78
		public static void CopyArrayToList<[IsUnmanaged] T>(List<T> destination, NativeArray<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(destination.Count, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000139C4 File Offset: 0x00011BC4
		public unsafe static void CopyArrayToNative<[IsUnmanaged] T>(NativeArray<T> destination, T[] source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to copy an array of length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(source.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" into a native array with length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(destination.Length);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			fixed (T[] array = source)
			{
				void* source2;
				if (source == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(destination), source2, (long)source.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00013A55 File Offset: 0x00011C55
		public static void CopyFrom(this NativeBitArray destination, NativeBitArray source)
		{
			destination.Copy(0, ref source, 0, Mathf.Min(destination.Length, source.Length));
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00013A78 File Offset: 0x00011C78
		public static void Reverse<[IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			int length = list.Length;
			int i = 0;
			int num = length - 1;
			while (i < num)
			{
				ref NativeList<T> ptr = ref list;
				int index = i;
				int index2 = num;
				T value = list[num];
				T value2 = list[i];
				ptr[index] = value;
				list[index2] = value2;
				i++;
				num--;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00013ACF File Offset: 0x00011CCF
		public static NativeArray<T> EmptyArray<[IsUnmanaged] T>() where T : struct, ValueType
		{
			return new NativeArray<T>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00013AD9 File Offset: 0x00011CD9
		public static NativeList<T> EmptyList<[IsUnmanaged] T>() where T : struct, ValueType
		{
			return new NativeList<T>(0, Allocator.Persistent);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00013AE7 File Offset: 0x00011CE7
		public static NativeBitArray EmptyBitArray()
		{
			return new NativeBitArray(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x02002480 RID: 9344
		private static class ArrayAccessor<T>
		{
			// Token: 0x0600E9D8 RID: 59864 RVA: 0x0056B364 File Offset: 0x00569564
			static ArrayAccessor()
			{
				DynamicMethod dynamicMethod = new DynamicMethod("get", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(T[]), new Type[]
				{
					typeof(List<T>)
				}, typeof(NativeArrayUtility.ArrayAccessor<T>), true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic));
				ilgenerator.Emit(OpCodes.Ret);
				NativeArrayUtility.ArrayAccessor<T>.Getter = (Func<List<T>, T[]>)dynamicMethod.CreateDelegate(typeof(Func<List<T>, T[]>));
			}

			// Token: 0x04009516 RID: 38166
			public static readonly Func<List<T>, T[]> Getter;
		}
	}
}
