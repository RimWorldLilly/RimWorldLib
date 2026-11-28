using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9C RID: 3996
	public class LifeStageDef : Def
	{
		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x060065D8 RID: 26072 RVA: 0x00243B18 File Offset: 0x00241D18
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x060065D9 RID: 26073 RVA: 0x00243B2A File Offset: 0x00241D2A
		public LifeStageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LifeStageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060065DA RID: 26074 RVA: 0x00243B5C File Offset: 0x00241D5C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.icon.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				});
			}
		}

		// Token: 0x04003D1C RID: 15644
		[MustTranslate]
		private string adjective;

		// Token: 0x04003D1D RID: 15645
		public bool visible = true;

		// Token: 0x04003D1E RID: 15646
		public bool reproductive;

		// Token: 0x04003D1F RID: 15647
		public bool milkable;

		// Token: 0x04003D20 RID: 15648
		public bool shearable;

		// Token: 0x04003D21 RID: 15649
		public bool caravanRideable;

		// Token: 0x04003D22 RID: 15650
		public bool alwaysDowned;

		// Token: 0x04003D23 RID: 15651
		public bool claimable;

		// Token: 0x04003D24 RID: 15652
		public DevelopmentalStage developmentalStage = DevelopmentalStage.Adult;

		// Token: 0x04003D25 RID: 15653
		public Type workerClass = typeof(LifeStageWorker);

		// Token: 0x04003D26 RID: 15654
		public bool involuntarySleepIsNegativeEvent = true;

		// Token: 0x04003D27 RID: 15655
		public ThinkTreeDef thinkTreeMainOverride;

		// Token: 0x04003D28 RID: 15656
		public ThinkTreeDef thinkTreeConstantOverride;

		// Token: 0x04003D29 RID: 15657
		public bool canDoRandomMentalBreaks = true;

		// Token: 0x04003D2A RID: 15658
		[MustTranslate]
		public string customMoodTipString;

		// Token: 0x04003D2B RID: 15659
		public bool canSleepWhileHeld;

		// Token: 0x04003D2C RID: 15660
		public bool canVoluntarilySleep = true;

		// Token: 0x04003D2D RID: 15661
		public bool canSleepWhenStarving = true;

		// Token: 0x04003D2E RID: 15662
		public bool canInitiateSocialInteraction = true;

		// Token: 0x04003D2F RID: 15663
		public float equipmentDrawDistanceFactor = 1f;

		// Token: 0x04003D30 RID: 15664
		public float soundAttackChance = 0.25f;

		// Token: 0x04003D31 RID: 15665
		public float voxPitch = 1f;

		// Token: 0x04003D32 RID: 15666
		public float voxVolume = 1f;

		// Token: 0x04003D33 RID: 15667
		[NoTranslate]
		public string icon;

		// Token: 0x04003D34 RID: 15668
		[Unsaved(false)]
		public Texture2D iconTex;

		// Token: 0x04003D35 RID: 15669
		public GraphicData silhouetteGraphicData;

		// Token: 0x04003D36 RID: 15670
		public List<StatModifier> statOffsets = new List<StatModifier>();

		// Token: 0x04003D37 RID: 15671
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x04003D38 RID: 15672
		public float bodySizeFactor = 1f;

		// Token: 0x04003D39 RID: 15673
		public float healthScaleFactor = 1f;

		// Token: 0x04003D3A RID: 15674
		public float hungerRateFactor = 1f;

		// Token: 0x04003D3B RID: 15675
		public float marketValueFactor = 1f;

		// Token: 0x04003D3C RID: 15676
		public float foodMaxFactor = 1f;

		// Token: 0x04003D3D RID: 15677
		public float attachPointScaleFactor = 1f;

		// Token: 0x04003D3E RID: 15678
		public float meleeDamageFactor = 1f;

		// Token: 0x04003D3F RID: 15679
		public SimpleCurve involuntarySleepMTBDaysFromRest;

		// Token: 0x04003D40 RID: 15680
		public float? fallAsleepMaxThresholdOverride;

		// Token: 0x04003D41 RID: 15681
		public float? naturalWakeThresholdOverride;

		// Token: 0x04003D42 RID: 15682
		public float? bodyWidth;

		// Token: 0x04003D43 RID: 15683
		public Vector3 bodyDrawOffset = Vector3.zero;

		// Token: 0x04003D44 RID: 15684
		public float? headSizeFactor;

		// Token: 0x04003D45 RID: 15685
		public float? eyeSizeFactor;

		// Token: 0x04003D46 RID: 15686
		public float? sittingOffset;

		// Token: 0x04003D47 RID: 15687
		public TagFilter hairStyleFilter;

		// Token: 0x04003D48 RID: 15688
		[Unsaved(false)]
		private LifeStageWorker workerInt;
	}
}
