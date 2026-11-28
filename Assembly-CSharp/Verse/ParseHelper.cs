using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Steamworks;
using UnityEngine;
using UnityEngine.Rendering;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x02000088 RID: 136
	public static class ParseHelper
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x000241A7 File Offset: 0x000223A7
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000241B9 File Offset: 0x000223B9
		public static uint ParseUInt(string str)
		{
			return uint.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000241CC File Offset: 0x000223CC
		public static int ParseIntPermissive(string str)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = (int)float.Parse(str, CultureInfo.InvariantCulture);
				Log.Warning("Parsed " + str + " as int.");
			}
			return result;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00024210 File Offset: 0x00022410
		public static Vector3 FromStringVector3(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = float.Parse(array[0], invariantCulture);
			float y = float.Parse(array[1], invariantCulture);
			float z = float.Parse(array[2], invariantCulture);
			return new Vector3(x, y, z);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00024268 File Offset: 0x00022468
		public static Vector2 FromStringVector2(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x;
			float y;
			if (array.Length == 1)
			{
				y = (x = Convert.ToSingle(array[0], invariantCulture));
			}
			else
			{
				if (array.Length != 2)
				{
					throw new InvalidOperationException();
				}
				x = Convert.ToSingle(array[0], invariantCulture);
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			return new Vector2(x, y);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000242D8 File Offset: 0x000224D8
		public static Vector4 FromStringVector4Adaptive(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in vector {0}", Str), 16139142);
			}
			return new Vector4(x, y, z, w);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0002438C File Offset: 0x0002258C
		public static Quaternion FromStringQuaternion(string Str)
		{
			Str = Str.TrimStart('(');
			Str = Str.TrimEnd(')');
			string[] array = Str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in quaternion {0}", Str), 16139142);
			}
			return new Quaternion(x, y, z, w);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00024440 File Offset: 0x00022640
		public static Rect FromStringRect(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float width = Convert.ToSingle(array[2], invariantCulture);
			float height = Convert.ToSingle(array[3], invariantCulture);
			return new Rect(x, y, width, height);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000244A2 File Offset: 0x000226A2
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000244AF File Offset: 0x000226AF
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000244B7 File Offset: 0x000226B7
		public static ulong ParseULong(string str)
		{
			return ulong.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000244C9 File Offset: 0x000226C9
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000244D6 File Offset: 0x000226D6
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000244E3 File Offset: 0x000226E3
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000244F0 File Offset: 0x000226F0
		public static byte ParseByte(string str)
		{
			return byte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000244FD File Offset: 0x000226FD
		public static ushort ParseUShort(string str)
		{
			return ushort.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0002450A File Offset: 0x0002270A
		public static short ParseShort(string str)
		{
			return short.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00024518 File Offset: 0x00022718
		public static Type ParseType(string str)
		{
			if (str == "null" || str == "Null")
			{
				return null;
			}
			Type type = GenTypes.GetTypeInAnyAssembly(str, null);
			if (type == null)
			{
				type = BackCompatibility.GetBackCompatibleTypeDirect(typeof(Type), str);
				if (type == null)
				{
					Log.Error("Could not find a type named " + str);
				}
			}
			return type;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00024580 File Offset: 0x00022780
		public static Action ParseAction(string str)
		{
			string[] array = str.Split('.', StringSplitOptions.None);
			string methodName = array[array.Length - 1];
			string typeName;
			if (array.Length == 3)
			{
				typeName = array[0] + "." + array[1];
			}
			else
			{
				typeName = array[0];
			}
			MethodInfo method = GenTypes.GetTypeInAnyAssembly(typeName, null).GetMethods().First((MethodInfo m) => m.Name == methodName);
			return (Action)Delegate.CreateDelegate(typeof(Action), method);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000245FC File Offset: 0x000227FC
		public static Color ParseColor(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(',', StringSplitOptions.None);
			float num = ParseHelper.ParseFloat(array[0]);
			float num2 = ParseHelper.ParseFloat(array[1]);
			float num3 = ParseHelper.ParseFloat(array[2]);
			bool flag = num > 1f || num3 > 1f || num2 > 1f;
			float num4 = (float)(flag ? 255 : 1);
			if (array.Length == 4)
			{
				num4 = ParseHelper.FromString<float>(array[3]);
			}
			Color result;
			if (!flag)
			{
				result.r = num;
				result.g = num2;
				result.b = num3;
				result.a = num4;
			}
			else
			{
				result = GenColor.FromBytes(Mathf.RoundToInt(num), Mathf.RoundToInt(num2), Mathf.RoundToInt(num3), Mathf.RoundToInt(num4));
			}
			return result;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000246C8 File Offset: 0x000228C8
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000246D5 File Offset: 0x000228D5
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000246DD File Offset: 0x000228DD
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000246E5 File Offset: 0x000228E5
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000246ED File Offset: 0x000228ED
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000246F5 File Offset: 0x000228F5
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000246FD File Offset: 0x000228FD
		public static NameTriple ParseNameTriple(string str)
		{
			return NameTriple.FromString(str, false);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00024706 File Offset: 0x00022906
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002470E File Offset: 0x0002290E
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00024716 File Offset: 0x00022916
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00024720 File Offset: 0x00022920
		public static ColorInt ParseColorInt(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(',', StringSplitOptions.None);
			ColorInt result = new ColorInt(255, 255, 255, 255);
			result.r = ParseHelper.ParseIntPermissive(array[0]);
			result.g = ParseHelper.ParseIntPermissive(array[1]);
			result.b = ParseHelper.ParseIntPermissive(array[2]);
			if (array.Length == 4)
			{
				result.a = ParseHelper.ParseIntPermissive(array[3]);
			}
			else
			{
				result.a = 255;
			}
			return result;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000247BD File Offset: 0x000229BD
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000247C5 File Offset: 0x000229C5
		public static PlanetTile ParsePlanetTile(string str)
		{
			return PlanetTile.FromString(str);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000247D0 File Offset: 0x000229D0
		public static RenderQueueValue ParseRenderQueue(string str)
		{
			str = str.Trim();
			int value;
			if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				return new RenderQueueValue(value);
			}
			int num = str.IndexOfAny(ParseHelper.renderQueueOffsetSeparators);
			RenderQueue renderQueue;
			if (!Enum.TryParse<RenderQueue>(((num < 0) ? str : str.Substring(0, num)).Trim(), out renderQueue))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(91, 3);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				defaultInterpolatedStringHandler.AppendFormatted(str);
				defaultInterpolatedStringHandler.AppendLiteral("' is not a valid render queue. Expected an integer, a ");
				defaultInterpolatedStringHandler.AppendFormatted("RenderQueue");
				defaultInterpolatedStringHandler.AppendLiteral(" name, or Name±offset. Valid names: ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", Enum.GetNames(typeof(RenderQueue))));
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num2 = 0;
			if (num >= 0)
			{
				num2 = ((str[num] == '-') ? -1 : 1) * int.Parse(str.AsSpan(num + 1), NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			return new RenderQueueValue((int)(renderQueue + num2));
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000248C8 File Offset: 0x00022AC8
		static ParseHelper()
		{
			ParseHelper.Parsers<string>.Register(new Func<string, string>(ParseHelper.ParseString));
			ParseHelper.Parsers<bool>.Register(new Func<string, bool>(ParseHelper.ParseBool));
			ParseHelper.Parsers<sbyte>.Register(new Func<string, sbyte>(ParseHelper.ParseSByte));
			ParseHelper.Parsers<byte>.Register(new Func<string, byte>(ParseHelper.ParseByte));
			ParseHelper.Parsers<uint>.Register(new Func<string, uint>(ParseHelper.ParseUInt));
			ParseHelper.Parsers<int>.Register(new Func<string, int>(ParseHelper.ParseIntPermissive));
			ParseHelper.Parsers<ushort>.Register(new Func<string, ushort>(ParseHelper.ParseUShort));
			ParseHelper.Parsers<short>.Register(new Func<string, short>(ParseHelper.ParseShort));
			ParseHelper.Parsers<ulong>.Register(new Func<string, ulong>(ParseHelper.ParseULong));
			ParseHelper.Parsers<long>.Register(new Func<string, long>(ParseHelper.ParseLong));
			ParseHelper.Parsers<float>.Register(new Func<string, float>(ParseHelper.ParseFloat));
			ParseHelper.Parsers<double>.Register(new Func<string, double>(ParseHelper.ParseDouble));
			ParseHelper.Parsers<Vector3>.Register(new Func<string, Vector3>(ParseHelper.FromStringVector3));
			ParseHelper.Parsers<Vector2>.Register(new Func<string, Vector2>(ParseHelper.FromStringVector2));
			ParseHelper.Parsers<Vector4>.Register(new Func<string, Vector4>(ParseHelper.FromStringVector4Adaptive));
			ParseHelper.Parsers<Quaternion>.Register(new Func<string, Quaternion>(ParseHelper.FromStringQuaternion));
			ParseHelper.Parsers<Rect>.Register(new Func<string, Rect>(ParseHelper.FromStringRect));
			ParseHelper.Parsers<Type>.Register(new Func<string, Type>(ParseHelper.ParseType));
			ParseHelper.Parsers<Action>.Register(new Func<string, Action>(ParseHelper.ParseAction));
			ParseHelper.Parsers<Color>.Register(new Func<string, Color>(ParseHelper.ParseColor));
			ParseHelper.Parsers<PublishedFileId_t>.Register(new Func<string, PublishedFileId_t>(ParseHelper.ParsePublishedFileId));
			ParseHelper.Parsers<IntVec2>.Register(new Func<string, IntVec2>(ParseHelper.ParseIntVec2));
			ParseHelper.Parsers<IntVec3>.Register(new Func<string, IntVec3>(ParseHelper.ParseIntVec3));
			ParseHelper.Parsers<Rot4>.Register(new Func<string, Rot4>(ParseHelper.ParseRot4));
			ParseHelper.Parsers<CellRect>.Register(new Func<string, CellRect>(ParseHelper.ParseCellRect));
			ParseHelper.Parsers<CurvePoint>.Register(new Func<string, CurvePoint>(ParseHelper.ParseCurvePoint));
			ParseHelper.Parsers<NameTriple>.Register(new Func<string, NameTriple>(ParseHelper.ParseNameTriple));
			ParseHelper.Parsers<FloatRange>.Register(new Func<string, FloatRange>(ParseHelper.ParseFloatRange));
			ParseHelper.Parsers<IntRange>.Register(new Func<string, IntRange>(ParseHelper.ParseIntRange));
			ParseHelper.Parsers<QualityRange>.Register(new Func<string, QualityRange>(ParseHelper.ParseQualityRange));
			ParseHelper.Parsers<ColorInt>.Register(new Func<string, ColorInt>(ParseHelper.ParseColorInt));
			ParseHelper.Parsers<TaggedString>.Register(new Func<string, TaggedString>(ParseHelper.ParseTaggedString));
			ParseHelper.Parsers<PlanetTile>.Register(new Func<string, PlanetTile>(ParseHelper.ParsePlanetTile));
			ParseHelper.Parsers<RenderQueueValue>.Register(new Func<string, RenderQueueValue>(ParseHelper.ParseRenderQueue));
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00024B5C File Offset: 0x00022D5C
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00024B90 File Offset: 0x00022D90
		public static object FromString(string str, Type itemType)
		{
			object result;
			try
			{
				itemType = (Nullable.GetUnderlyingType(itemType) ?? itemType);
				if (itemType.IsEnum)
				{
					try
					{
						object obj = BackCompatibility.BackCompatibleEnum(itemType, str);
						if (obj != null)
						{
							return obj;
						}
						return Enum.Parse(itemType, str);
					}
					catch (ArgumentException innerException)
					{
						string[] array = new string[5];
						array[0] = "'";
						array[1] = str;
						array[2] = "' is not a valid value for ";
						int num = 3;
						Type type = itemType;
						array[num] = ((type != null) ? type.ToString() : null);
						array[4] = ". Valid values are: \n";
						throw new ArgumentException(string.Concat(array) + GenText.StringFromEnumerable(Enum.GetValues(itemType)), innerException);
					}
				}
				Func<string, object> func;
				if (ParseHelper.parsers.TryGetValue(itemType, out func))
				{
					result = func(str);
				}
				else if (GenTypes.IsSlateRef(itemType))
				{
					ISlateRef slateRef = (ISlateRef)Activator.CreateInstance(itemType);
					slateRef.SlateRef = str;
					result = slateRef;
				}
				else
				{
					if (!itemType.IsSubclassOf(typeof(Delegate)))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Trying to parse to unknown data type ",
							itemType.Name,
							". Content is '",
							str,
							"'."
						}));
					}
					int num2 = str.LastIndexOf('.');
					string typeName = (num2 >= 0) ? str.Substring(0, num2) : "";
					string method = str.Substring(num2 + 1, str.Length - (num2 + 1));
					Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
					result = Delegate.CreateDelegate(itemType, typeInAnyAssembly, method);
				}
			}
			catch (Exception innerException2)
			{
				string[] array2 = new string[5];
				array2[0] = "Exception parsing ";
				int num3 = 1;
				Type type2 = itemType;
				array2[num3] = ((type2 != null) ? type2.ToString() : null);
				array2[2] = " from \"";
				array2[3] = str;
				array2[4] = "\"";
				throw new ArgumentException(string.Concat(array2), innerException2);
			}
			return result;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00024D68 File Offset: 0x00022F68
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || type.IsSubclassOf(typeof(Delegate)) || GenTypes.IsSlateRef(type);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00024DBC File Offset: 0x00022FBC
		public static bool CanParse(Type type, string str)
		{
			if (!ParseHelper.HandlesType(type))
			{
				return false;
			}
			try
			{
				ParseHelper.FromString(str, type);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x04000206 RID: 518
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		// Token: 0x04000207 RID: 519
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		// Token: 0x04000208 RID: 520
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		// Token: 0x04000209 RID: 521
		private static readonly char[] renderQueueOffsetSeparators = new char[]
		{
			'+',
			'-'
		};

		// Token: 0x020024CD RID: 9421
		public static class Parsers<T>
		{
			// Token: 0x0600EB94 RID: 60308 RVA: 0x005706A0 File Offset: 0x0056E8A0
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			// Token: 0x040096AF RID: 38575
			public static Func<string, T> parser;

			// Token: 0x040096B0 RID: 38576
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
