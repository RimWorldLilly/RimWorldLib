using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020006C2 RID: 1730
	public static class DevelopmentalStageExtensions
	{
		// Token: 0x060036D8 RID: 14040 RVA: 0x0015FFFC File Offset: 0x0015E1FC
		public static CachedTexture Icon(this DevelopmentalStage developmentalStage)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(developmentalStage))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Exactly one developmental stage may be set to get an icon, but was ");
				defaultInterpolatedStringHandler.AppendFormatted<DevelopmentalStage>(developmentalStage);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (developmentalStage == DevelopmentalStage.Baby)
			{
				return DevelopmentalStageExtensions.BabyTex;
			}
			if (developmentalStage == DevelopmentalStage.Child)
			{
				return DevelopmentalStageExtensions.ChildTex;
			}
			if (developmentalStage != DevelopmentalStage.Adult)
			{
				throw new NotImplementedException();
			}
			return DevelopmentalStageExtensions.AdultTex;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x00160070 File Offset: 0x0015E270
		public static bool ExactlyOneDevelopmentalStageSet(DevelopmentalStage developmentalStage)
		{
			return developmentalStage != DevelopmentalStage.None && (developmentalStage & developmentalStage - 1U) == DevelopmentalStage.None;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x0016008C File Offset: 0x0015E28C
		public static bool Newborn(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Newborn) > DevelopmentalStage.None;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x00160094 File Offset: 0x0015E294
		public static bool Baby(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Baby) > DevelopmentalStage.None;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x0016009C File Offset: 0x0015E29C
		public static bool Child(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Child) > DevelopmentalStage.None;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x001600A4 File Offset: 0x0015E2A4
		public static bool Adult(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Adult) > DevelopmentalStage.None;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x001600AC File Offset: 0x0015E2AC
		public static bool Juvenile(this DevelopmentalStage developmentalStage)
		{
			return developmentalStage.Baby() || developmentalStage.Child();
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x001600BE File Offset: 0x0015E2BE
		public static bool Has(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(query))
			{
				Log.ErrorOnce("A single DevelopmentalStage was expected but multiple were set.", 845116642);
			}
			return developmentalStage.HasAny(query);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x001600DE File Offset: 0x0015E2DE
		public static bool HasAny(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) > DevelopmentalStage.None;
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x001600E6 File Offset: 0x0015E2E6
		public static bool HasAll(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) == query;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x001600F0 File Offset: 0x0015E2F0
		public static string ToCommaListOr(this DevelopmentalStage developmentalStages)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				DevelopmentalStageExtensions.developmentalStageStrings.Add(developmentalStage.ToString().Translate());
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaListOr(false);
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x00160158 File Offset: 0x0015E358
		public static string ToCommaList(this DevelopmentalStage developmentalStages, bool capitalize = false)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				TaggedString taggedString = developmentalStage.ToString().Translate();
				DevelopmentalStageExtensions.developmentalStageStrings.Add(capitalize ? taggedString.CapitalizeFirst() : taggedString);
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaList(false, false);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x001601CC File Offset: 0x0015E3CC
		public static DevelopmentalStageExtensions.EnumerationCodebehind Enumerate(this DevelopmentalStage developmentalStages)
		{
			return new DevelopmentalStageExtensions.EnumerationCodebehind(developmentalStages);
		}

		// Token: 0x04002415 RID: 9237
		private static List<string> developmentalStageStrings = new List<string>();

		// Token: 0x04002416 RID: 9238
		private const DevelopmentalStage lastdevelopmentalStage = DevelopmentalStage.Adult;

		// Token: 0x04002417 RID: 9239
		public static CachedTexture BabyTex = new CachedTexture("UI/Icons/DevelopmentalStages/Baby");

		// Token: 0x04002418 RID: 9240
		public static CachedTexture ChildTex = new CachedTexture("UI/Icons/DevelopmentalStages/Child");

		// Token: 0x04002419 RID: 9241
		public static CachedTexture AdultTex = new CachedTexture("UI/Icons/DevelopmentalStages/Adult");

		// Token: 0x02002A2B RID: 10795
		public struct EnumerationCodebehind
		{
			// Token: 0x06010589 RID: 66953 RVA: 0x005C3F91 File Offset: 0x005C2191
			public EnumerationCodebehind(DevelopmentalStage stages)
			{
				this.bit = 0U;
				this.stages = (uint)stages;
			}

			// Token: 0x17002BC2 RID: 11202
			// (get) Token: 0x0601058A RID: 66954 RVA: 0x005C3FA1 File Offset: 0x005C21A1
			public DevelopmentalStage Current
			{
				get
				{
					return (DevelopmentalStage)this.bit;
				}
			}

			// Token: 0x0601058B RID: 66955 RVA: 0x005C3FAC File Offset: 0x005C21AC
			public bool MoveNext()
			{
				if (this.bit == 0U)
				{
					this.bit = 1U;
				}
				else
				{
					this.bit <<= 1;
				}
				while (this.bit <= 8U)
				{
					if ((this.bit & this.stages) != 0U)
					{
						return true;
					}
					this.bit <<= 1;
				}
				return false;
			}

			// Token: 0x0601058C RID: 66956 RVA: 0x005C4003 File Offset: 0x005C2203
			public DevelopmentalStageExtensions.EnumerationCodebehind GetEnumerator()
			{
				return this;
			}

			// Token: 0x0601058D RID: 66957 RVA: 0x005C400B File Offset: 0x005C220B
			public void Dispose()
			{
			}

			// Token: 0x0400ACB0 RID: 44208
			private uint bit;

			// Token: 0x0400ACB1 RID: 44209
			private uint stages;
		}
	}
}
