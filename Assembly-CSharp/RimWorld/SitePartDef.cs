using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE3 RID: 4067
	public class SitePartDef : Def
	{
		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x060066DB RID: 26331 RVA: 0x002479F2 File Offset: 0x00245BF2
		public SitePartWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SitePartWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x060066DC RID: 26332 RVA: 0x00247A24 File Offset: 0x00245C24
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x060066DD RID: 26333 RVA: 0x00247A74 File Offset: 0x00245C74
		public List<GenStepDef> ExtraGenSteps
		{
			get
			{
				if (this.extraGenSteps == null)
				{
					this.extraGenSteps = new List<GenStepDef>();
					List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].linkWithSite == this)
						{
							this.extraGenSteps.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.extraGenSteps;
			}
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x00247AD4 File Offset: 0x00245CD4
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x00247B4C File Offset: 0x00245D4C
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.Hidden)) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x00247BB4 File Offset: 0x00245DB4
		public bool CompatibleWith(SitePartDef part)
		{
			for (int i = 0; i < part.excludesTags.Count; i++)
			{
				if (this.tags.Contains(part.excludesTags[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < this.excludesTags.Count; j++)
			{
				if (part.tags.Contains(this.excludesTags[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004026 RID: 16422
		public ThingDef conditionCauserDef;

		// Token: 0x04004027 RID: 16423
		public float activeThreatDisturbanceFactor = 1f;

		// Token: 0x04004028 RID: 16424
		public bool defaultHidden;

		// Token: 0x04004029 RID: 16425
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x0400402A RID: 16426
		[NoTranslate]
		public string siteTexture;

		// Token: 0x0400402B RID: 16427
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x0400402C RID: 16428
		public List<TileMutatorDef> forceMutators = new List<TileMutatorDef>();

		// Token: 0x0400402D RID: 16429
		public bool applyFactionColorToSiteTexture;

		// Token: 0x0400402E RID: 16430
		public bool showFactionInInspectString;

		// Token: 0x0400402F RID: 16431
		public bool requiresFaction;

		// Token: 0x04004030 RID: 16432
		public bool disallowsAutomaticDetectionTimerStart;

		// Token: 0x04004031 RID: 16433
		public TechLevel minFactionTechLevel;

		// Token: 0x04004032 RID: 16434
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x04004033 RID: 16435
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x04004034 RID: 16436
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04004035 RID: 16437
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		// Token: 0x04004036 RID: 16438
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x04004037 RID: 16439
		[MustTranslate]
		public string arrivedLetterLabelPart;

		// Token: 0x04004038 RID: 16440
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		// Token: 0x04004039 RID: 16441
		public LetterDef arrivedLetterDef;

		// Token: 0x0400403A RID: 16442
		public bool wantsThreatPoints;

		// Token: 0x0400403B RID: 16443
		public float minThreatPoints;

		// Token: 0x0400403C RID: 16444
		public bool increasesPopulation;

		// Token: 0x0400403D RID: 16445
		public bool badEvenIfNoMap;

		// Token: 0x0400403E RID: 16446
		public float forceExitAndRemoveMapCountdownDurationDays = 4f;

		// Token: 0x0400403F RID: 16447
		public bool handlesWorldObjectTimeoutInspectString;

		// Token: 0x04004040 RID: 16448
		public string mainPartAllThreatsLabel;

		// Token: 0x04004041 RID: 16449
		public IntVec3? minMapSize;

		// Token: 0x04004042 RID: 16450
		public float selectionWeight;

		// Token: 0x04004043 RID: 16451
		public bool considerEnteringAsAttack = true;

		// Token: 0x04004044 RID: 16452
		public bool gravShipsCanLandOn;

		// Token: 0x04004045 RID: 16453
		public bool copyQuestName;

		// Token: 0x04004046 RID: 16454
		public bool leaveAbandonedSettlement;

		// Token: 0x04004047 RID: 16455
		public bool displayOnInspectPane = true;

		// Token: 0x04004048 RID: 16456
		public List<SitePartDef.WorkSiteLootThing> lootTable;

		// Token: 0x04004049 RID: 16457
		[Unsaved(false)]
		private SitePartWorker workerInt;

		// Token: 0x0400404A RID: 16458
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x0400404B RID: 16459
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;

		// Token: 0x02002F8A RID: 12170
		public class WorkSiteLootThing
		{
			// Token: 0x0400C149 RID: 49481
			public ThingDef thing;

			// Token: 0x0400C14A RID: 49482
			public float weight;
		}
	}
}
