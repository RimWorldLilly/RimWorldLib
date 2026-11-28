using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x020004FC RID: 1276
	public static class FadedMaterialPool
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x000E6B86 File Offset: 0x000E4D86
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x000E6B94 File Offset: 0x000E4D94
		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMaterialPool.FadedMatRequest, Material> keyValuePair in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(keyValuePair.Value);
				}
				return num;
			}
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000E6BF4 File Offset: 0x000E4DF4
		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			if (num == 0)
			{
				return BaseContent.ClearMat;
			}
			if (num == 29)
			{
				return sourceMat;
			}
			FadedMaterialPool.FadedMatRequest key = new FadedMaterialPool.FadedMatRequest(sourceMat, num);
			Material material;
			if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
			{
				material = MaterialAllocator.Create(sourceMat);
				material.color = new Color(1f, 1f, 1f, (float)FadedMaterialPool.IndexFromAlpha(alpha) / 30f);
				FadedMaterialPool.cachedMats.Add(key, material);
			}
			return material;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000E6C6C File Offset: 0x000E4E6C
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04001B4D RID: 6989
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04001B4E RID: 6990
		private const int NumFadeSteps = 30;

		// Token: 0x02002749 RID: 10057
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0600F7A0 RID: 63392 RVA: 0x0059EC05 File Offset: 0x0059CE05
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x0600F7A1 RID: 63393 RVA: 0x0059EC15 File Offset: 0x0059CE15
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x0600F7A2 RID: 63394 RVA: 0x0059EC30 File Offset: 0x0059CE30
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x0600F7A3 RID: 63395 RVA: 0x0059EC55 File Offset: 0x0059CE55
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x0600F7A4 RID: 63396 RVA: 0x0059EC6D File Offset: 0x0059CE6D
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600F7A5 RID: 63397 RVA: 0x0059EC77 File Offset: 0x0059CE77
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400A0D8 RID: 41176
			private Material mat;

			// Token: 0x0400A0D9 RID: 41177
			private int alphaIndex;
		}

		// Token: 0x0200274A RID: 10058
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0600F7A6 RID: 63398 RVA: 0x0059EC83 File Offset: 0x0059CE83
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x0600F7A7 RID: 63399 RVA: 0x0059EC8D File Offset: 0x0059CE8D
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x0400A0DA RID: 41178
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
