using System;
using System.Collections.Generic;
using UnityEngine;
using Waylith.Rendering;

namespace Verse
{
	// Token: 0x020001BB RID: 443
	public static class ShaderWarmup
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00040A55 File Offset: 0x0003EC55
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00040A5C File Offset: 0x0003EC5C
		public static bool PrewarmEnabled { get; set; }

		// Token: 0x06000C41 RID: 3137 RVA: 0x00040A64 File Offset: 0x0003EC64
		public static void KickPrewarmOnce()
		{
			if (!ShaderWarmup.PrewarmEnabled || ShaderWarmup.prewarmKicked)
			{
				return;
			}
			ShaderWarmup.prewarmKicked = true;
			ShaderPrewarmer.WarmInBackground(ShaderWarmup.BuildPlan());
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00040A85 File Offset: 0x0003EC85
		public static ShaderWarmupPlan BuildPlan()
		{
			return ShaderVariantEnumerator.Enumerate(ShaderWarmup.CollectKeywordContributions(), ShaderWarmup.CollectGlobalKeywordKeys());
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00040A98 File Offset: 0x0003EC98
		private static List<ShaderKeywordContribution> CollectKeywordContributions()
		{
			List<ShaderKeywordContribution> list = new List<ShaderKeywordContribution>();
			foreach (MaterialDef materialDef in DefDatabase<MaterialDef>.AllDefs)
			{
				if (materialDef.shader != null && !materialDef.keywords.NullOrEmpty<string>())
				{
					list.Add(new ShaderKeywordContribution(materialDef.shader.defName, string.Join(";", materialDef.keywords)));
				}
			}
			return list;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00040B20 File Offset: 0x0003ED20
		private static List<string> CollectGlobalKeywordKeys()
		{
			return new List<string>
			{
				UnityEngine.Shader.GlobalKeywordKey
			};
		}

		// Token: 0x04000B92 RID: 2962
		private static bool prewarmKicked;
	}
}
