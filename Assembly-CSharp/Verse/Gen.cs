using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000074 RID: 116
	public static class Gen
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x00019D08 File Offset: 0x00017F08
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00019D78 File Offset: 0x00017F78
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int minInclusive = (disallowFirstValue > false) ? 1 : 0;
			T[] array = (T[])Enum.GetValues(typeof(T));
			int num = Rand.Range(minInclusive, array.Length);
			return array[num];
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00019DAE File Offset: 0x00017FAE
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00019DCA File Offset: 0x00017FCA
		public static Vector3 Random2DVector(Vector3 max)
		{
			return new Vector3(Rand.Range(-max.x, max.x), 0f, Rand.Range(-max.z, max.z));
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00019DFC File Offset: 0x00017FFC
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00019E1D File Offset: 0x0001801D
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			Gen.<GetAllSelectedItems>d__5<T> <GetAllSelectedItems>d__ = new Gen.<GetAllSelectedItems>d__5<T>(-2);
			<GetAllSelectedItems>d__.<>3__value = value;
			return <GetAllSelectedItems>d__;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00019E2D File Offset: 0x0001802D
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			Gen.<YieldSingle>d__6<T> <YieldSingle>d__ = new Gen.<YieldSingle>d__6<T>(-2);
			<YieldSingle>d__.<>3__val = val;
			return <YieldSingle>d__;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00019E3D File Offset: 0x0001803D
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			Gen.<YieldSingleNonGeneric>d__7<T> <YieldSingleNonGeneric>d__ = new Gen.<YieldSingleNonGeneric>d__7<T>(-2);
			<YieldSingleNonGeneric>d__.<>3__val = val;
			return <YieldSingleNonGeneric>d__;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00019E50 File Offset: 0x00018050
		public static string ToStringSafe<T>(this T obj)
		{
			if (obj == null)
			{
				return "null";
			}
			string result;
			try
			{
				result = obj.ToString();
			}
			catch (Exception ex)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = obj.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					string str = "Exception in ToString(): ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + ((ex2 != null) ? ex2.ToString() : null), num ^ 1857461521);
				}
				else
				{
					string str2 = "Exception in ToString(): ";
					Exception ex3 = ex;
					Log.Error(str2 + ((ex3 != null) ? ex3.ToString() : null));
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00019F00 File Offset: 0x00018100
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return "null";
			}
			string result;
			try
			{
				string text = "";
				foreach (object obj in enumerable)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToStringSafe<object>();
				}
				result = text;
			}
			catch (Exception ex)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = enumerable.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					string str = "Exception while enumerating: ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + ((ex2 != null) ? ex2.ToString() : null), num ^ 581736153);
				}
				else
				{
					string str2 = "Exception while enumerating: ";
					Exception ex3 = ex;
					Log.Error(str2 + ((ex3 != null) ? ex3.ToString() : null));
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001A004 File Offset: 0x00018204
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001A02B File Offset: 0x0001822B
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001A06C File Offset: 0x0001826C
		public static void MemberwiseShallowCopy(object from, object to)
		{
			foreach (FieldInfo fieldInfo in from.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				fieldInfo.SetValue(to, fieldInfo.GetValue(from));
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001A0A8 File Offset: 0x000182A8
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001A0E8 File Offset: 0x000182E8
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001A125 File Offset: 0x00018325
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001A14A File Offset: 0x0001834A
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001A164 File Offset: 0x00018364
		public static int HashCombineInt(int v1, int v2, int v3, int v4)
		{
			int num = 352654597;
			int num2 = num;
			num = ((num << 5) + num + (num >> 27) ^ v1);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v2);
			num = ((num << 5) + num + (num >> 27) ^ v3);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v4);
			return num + num2 * 1566083941;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001A1B6 File Offset: 0x000183B6
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001A1C3 File Offset: 0x000183C3
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001A1D0 File Offset: 0x000183D0
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001A1DD File Offset: 0x000183DD
		public static bool IsHashIntervalTick(this Map m, int interval)
		{
			return m.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001A1EA File Offset: 0x000183EA
		public static bool IsHashIntervalTick(this Map m, int interval, int offset)
		{
			return (m.HashOffsetTicks() + offset) % interval == 0;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001A1F9 File Offset: 0x000183F9
		public static int HashOffsetTicks(this Map m)
		{
			return Find.TickManager.TicksGame + m.uniqueID.HashOffset();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001A211 File Offset: 0x00018411
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001A21E File Offset: 0x0001841E
		public static bool IsHashIntervalTick(this Thing t, int interval, int delta)
		{
			return Math.Abs(t.HashOffsetTicks() % interval) < delta;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001A230 File Offset: 0x00018430
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001A248 File Offset: 0x00018448
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			if (interval <= 0)
			{
				return true;
			}
			int num = o.HashOffsetTicks() % interval;
			if (num < 0)
			{
				num += interval;
			}
			return num == 0;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001A270 File Offset: 0x00018470
		public static bool IsHashIntervalTick(this WorldObject o, int interval, int delta)
		{
			if (interval <= 0)
			{
				return true;
			}
			if (delta <= 0)
			{
				return false;
			}
			int num = o.HashOffsetTicks() % interval;
			if (num < 0)
			{
				num += interval;
			}
			return num < delta;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001A29E File Offset: 0x0001849E
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001A2B6 File Offset: 0x000184B6
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001A2C3 File Offset: 0x000184C3
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001A2DB File Offset: 0x000184DB
		public static int HashOrderless(int v1, int v2)
		{
			return Gen.HashCombineInt(Math.Min(v1, v2), Math.Max(v1, v2));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001A2F0 File Offset: 0x000184F0
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001A31C File Offset: 0x0001851C
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn == null || replaceWith == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(replaceIn) == null)
				{
					object value = fieldInfo.GetValue(replaceWith);
					if (value != null)
					{
						object obj = replaceIn;
						fieldInfo.SetValue(obj, value);
						replaceIn = (T)((object)obj);
					}
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001A3AC File Offset: 0x000185AC
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType && !(Nullable.GetUnderlyingType(fieldType) != null))
				{
					Log.Warning(string.Concat(new string[]
					{
						"Field ",
						type.Name,
						".",
						fieldInfo.Name,
						" is not nullable."
					}));
				}
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001A42C File Offset: 0x0001862C
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001A4AE File Offset: 0x000186AE
		public static bool InBounds<T>(this T[,] array, int i, int j)
		{
			return i >= 0 && j >= 0 && i < array.GetLength(0) && j < array.GetLength(1);
		}

		// Token: 0x040001AA RID: 426
		private static MethodInfo s_memberwiseClone;
	}
}
