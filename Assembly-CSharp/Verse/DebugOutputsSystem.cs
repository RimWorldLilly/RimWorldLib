using System;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x020005D0 RID: 1488
	public static class DebugOutputsSystem
	{
		// Token: 0x06002F40 RID: 12096 RVA: 0x0012D7AC File Offset: 0x0012B9AC
		[DebugOutput("System", false)]
		public static void LoadedAssets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Mesh[] array = Resources.FindObjectsOfTypeAll<Mesh>();
			StringBuilder stringBuilder2 = stringBuilder;
			string[] array2 = new string[5];
			array2[0] = "Meshes: ";
			array2[1] = array.Length.ToString();
			array2[2] = " (";
			int num = 3;
			Object[] arr = array;
			array2[num] = DebugOutputsSystem.TotalBytes(arr).ToStringBytes("F2");
			array2[4] = ")";
			stringBuilder2.AppendLine(string.Concat(array2));
			Material[] array3 = Resources.FindObjectsOfTypeAll<Material>();
			StringBuilder stringBuilder3 = stringBuilder;
			string[] array4 = new string[5];
			array4[0] = "Materials: ";
			array4[1] = array3.Length.ToString();
			array4[2] = " (";
			int num2 = 3;
			arr = array3;
			array4[num2] = DebugOutputsSystem.TotalBytes(arr).ToStringBytes("F2");
			array4[4] = ")";
			stringBuilder3.AppendLine(string.Concat(array4));
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount.ToString());
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"   Faded: ",
				FadedMaterialPool.TotalMaterialCount.ToString(),
				" (",
				FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount.ToString());
			Texture[] array5 = Resources.FindObjectsOfTypeAll<Texture>();
			StringBuilder stringBuilder4 = stringBuilder;
			string[] array6 = new string[5];
			array6[0] = "Textures: ";
			array6[1] = array5.Length.ToString();
			array6[2] = " (";
			int num3 = 3;
			arr = array5;
			array6[num3] = DebugOutputsSystem.TotalBytes(arr).ToStringBytes("F2");
			array6[4] = ")";
			stringBuilder4.AppendLine(string.Concat(array6));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Texture list:");
			Texture[] array7 = array5;
			for (int i = 0; i < array7.Length; i++)
			{
				string text = array7[i].name;
				if (text.NullOrEmpty())
				{
					text = "-";
				}
				stringBuilder.AppendLine(text);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x0012D99C File Offset: 0x0012BB9C
		private static long TotalBytes(Object[] arr)
		{
			long num = 0L;
			foreach (Object o in arr)
			{
				num += Profiler.GetRuntimeMemorySizeLong(o);
			}
			return num;
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x0012D9CA File Offset: 0x0012BBCA
		[DebugOutput("System", true)]
		public static void DynamicDrawThingsList()
		{
			Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x0012D9DC File Offset: 0x0012BBDC
		[DebugOutput("System", false)]
		public static void RandByCurveTests()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(0, 30)
			select (float)x).ToArray<float>());
			SimpleCurve curve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(10f, 1f),
					true
				},
				{
					new CurvePoint(15f, 2f),
					true
				},
				{
					new CurvePoint(20f, 2f),
					true
				},
				{
					new CurvePoint(21f, 0.5f),
					true
				},
				{
					new CurvePoint(30f, 0f),
					true
				}
			};
			float num = 0f;
			for (int i = 0; i < 1000000; i++)
			{
				float num2 = Rand.ByCurve(curve);
				num += num2;
				debugHistogram.Add(num2);
			}
			debugHistogram.Display();
			Log.Message(string.Format("Average {0}, calculated as {1}", num / 1000000f, Rand.ByCurveAverage(curve)));
		}
	}
}
