using System;
using System.Linq;
using System.Threading;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008B RID: 139
	public static class UnityData
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x000254C1 File Offset: 0x000236C1
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x000254D4 File Offset: 0x000236D4
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000254DE File Offset: 0x000236DE
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x000254E8 File Offset: 0x000236E8
		public static int MaxJobWorkerThreadCount
		{
			get
			{
				return UnityData.maximumJobWorkerThreads;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000254EF File Offset: 0x000236EF
		public static int MaxJobWorkerCount
		{
			get
			{
				return UnityData.maximumJobWorkerCount;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000254F6 File Offset: 0x000236F6
		public static bool ComputeShadersSupported
		{
			get
			{
				return UnityData.computeShadersSupported;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000254FD File Offset: 0x000236FD
		public static int GetIdealBatchCount(int items)
		{
			return Mathf.Max(items / UnityData.maximumJobWorkerThreads, 4);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000656 RID: 1622 RVA: 0x0002550C File Offset: 0x0002370C
		// (remove) Token: 0x06000657 RID: 1623 RVA: 0x00025540 File Offset: 0x00023740
		public static event Action DisposeStatic;

		// Token: 0x06000658 RID: 1624 RVA: 0x00025573 File Offset: 0x00023773
		public static void DisposeStaticResources()
		{
			Action disposeStatic = UnityData.DisposeStatic;
			if (disposeStatic != null)
			{
				disposeStatic();
			}
			UnityData.DisposeStatic = null;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002558B File Offset: 0x0002378B
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.");
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000255A8 File Offset: 0x000237A8
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.maximumJobWorkerThreads = JobsUtility.ThreadIndexCount + 1;
			UnityData.maximumJobWorkerCount = JobsUtility.JobWorkerCount;
			UnityData.computeShadersSupported = (SystemInfo.supportsComputeShaders && SystemInfo.graphicsMemorySize > 5000 && !UnityData.IsIntegratedGraphicsCard() && !Environment.GetCommandLineArgs().Contains("-disable-compute-shaders"));
			UnityData.initialized = true;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002563C File Offset: 0x0002383C
		private static bool IsIntegratedGraphicsCard()
		{
			return SystemInfo.graphicsDeviceVendorID == 32902 || SystemInfo.graphicsDeviceVendorID == 4203 || UnityData.IsAmdIntegratedGraphics();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00025660 File Offset: 0x00023860
		private static bool IsAmdIntegratedGraphics()
		{
			if (SystemInfo.graphicsDeviceVendorID != 4098)
			{
				return false;
			}
			string text = SystemInfo.graphicsDeviceName.ToLowerInvariant();
			int num = text.IndexOf("m graphics", StringComparison.InvariantCulture);
			if (num > 1 && char.IsDigit(text[num - 1]))
			{
				return true;
			}
			if (text.Contains("vega"))
			{
				return true;
			}
			if (text.Contains("integrated") || text.Contains("apu"))
			{
				return true;
			}
			if (text.Contains("ryzen"))
			{
				return true;
			}
			string a = text.ToLowerInvariant().Trim();
			return a == "amd radeon(tm) graphics" || a == "amd radeon graphics";
		}

		// Token: 0x0400020E RID: 526
		private static bool initialized;

		// Token: 0x0400020F RID: 527
		public static bool isEditor;

		// Token: 0x04000210 RID: 528
		public static string dataPath;

		// Token: 0x04000211 RID: 529
		public static RuntimePlatform platform;

		// Token: 0x04000212 RID: 530
		public static string persistentDataPath;

		// Token: 0x04000213 RID: 531
		private static int mainThreadId;

		// Token: 0x04000214 RID: 532
		private static int maximumJobWorkerThreads;

		// Token: 0x04000215 RID: 533
		private static int maximumJobWorkerCount;

		// Token: 0x04000216 RID: 534
		private static bool computeShadersSupported;
	}
}
