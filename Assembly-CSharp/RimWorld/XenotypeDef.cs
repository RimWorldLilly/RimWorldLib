using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001016 RID: 4118
	public class XenotypeDef : Def
	{
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x060067BE RID: 26558 RVA: 0x0024A850 File Offset: 0x00248A50
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060067BF RID: 26559 RVA: 0x0024A89D File Offset: 0x00248A9D
		public List<GeneDef> AllGenes
		{
			get
			{
				return this.genes;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x060067C0 RID: 26560 RVA: 0x0024A8A5 File Offset: 0x00248AA5
		public bool Archite
		{
			get
			{
				return this.AllGenes.Any((GeneDef g) => g.displayCategory == GeneCategoryDefOf.Archite);
			}
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x0024A8D4 File Offset: 0x00248AD4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.genes.NullOrEmpty<GeneDef>())
			{
				if (this.descriptionHyperlinks == null)
				{
					this.descriptionHyperlinks = new List<DefHyperlink>();
				}
				foreach (GeneDef def in this.genes)
				{
					this.descriptionHyperlinks.Add(new DefHyperlink(def));
				}
			}
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0024A958 File Offset: 0x00248B58
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			return this.nameMaker;
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x0024A973 File Offset: 0x00248B73
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			XenotypeDef.<SpecialDisplayStats>d__26 <SpecialDisplayStats>d__ = new XenotypeDef.<SpecialDisplayStats>d__26(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x0024A98A File Offset: 0x00248B8A
		public override IEnumerable<string> ConfigErrors()
		{
			XenotypeDef.<ConfigErrors>d__27 <ConfigErrors>d__ = new XenotypeDef.<ConfigErrors>d__27(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004223 RID: 16931
		public List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x04004224 RID: 16932
		public bool inheritable;

		// Token: 0x04004225 RID: 16933
		[NoTranslate]
		public string iconPath;

		// Token: 0x04004226 RID: 16934
		[MustTranslate]
		public string descriptionShort;

		// Token: 0x04004227 RID: 16935
		public float combatPowerFactor = 1f;

		// Token: 0x04004228 RID: 16936
		public bool canGenerateAsCombatant = true;

		// Token: 0x04004229 RID: 16937
		public float factionlessGenerationWeight = 1f;

		// Token: 0x0400422A RID: 16938
		public RulePackDef nameMaker;

		// Token: 0x0400422B RID: 16939
		public RulePackDef nameMakerFemale;

		// Token: 0x0400422C RID: 16940
		public float chanceToUseNameMaker;

		// Token: 0x0400422D RID: 16941
		public List<WeaponClassDef> forbiddenWeaponClasses;

		// Token: 0x0400422E RID: 16942
		public float generateWithXenogermReplicatingHediffChance;

		// Token: 0x0400422F RID: 16943
		public FloatRange xenogermReplicatingDurationLeftDaysRange = FloatRange.Zero;

		// Token: 0x04004230 RID: 16944
		public SoundDef soundDefOnImplant;

		// Token: 0x04004231 RID: 16945
		public List<XenotypeChance> doubleXenotypeChances;

		// Token: 0x04004232 RID: 16946
		public float displayPriority;

		// Token: 0x04004233 RID: 16947
		[Unsaved(false)]
		private Texture2D cachedIcon;

		// Token: 0x04004234 RID: 16948
		public static readonly Color IconColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
