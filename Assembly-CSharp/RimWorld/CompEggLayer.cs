using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001871 RID: 6257
	public class CompEggLayer : ThingComp
	{
		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x06009C85 RID: 40069 RVA: 0x00385AF0 File Offset: 0x00383CF0
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable) && !pawn.Sterile() && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x06009C86 RID: 40070 RVA: 0x00385B57 File Offset: 0x00383D57
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x06009C87 RID: 40071 RVA: 0x00385B73 File Offset: 0x00383D73
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x17001AC2 RID: 6850
		// (get) Token: 0x06009C88 RID: 40072 RVA: 0x00385B8B File Offset: 0x00383D8B
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.Props.eggProgressUnfertilizedMax < 1f && this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x06009C89 RID: 40073 RVA: 0x00385BBF File Offset: 0x00383DBF
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x06009C8A RID: 40074 RVA: 0x00385BCC File Offset: 0x00383DCC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x06009C8B RID: 40075 RVA: 0x00385C18 File Offset: 0x00383E18
		public override void CompTick()
		{
			if (!this.parent.IsHashIntervalTick(2500))
			{
				return;
			}
			if (!this.Active)
			{
				return;
			}
			float num = 2500f / (this.Props.eggLayIntervalDays * 60000f);
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
			}
			this.eggProgress += num;
			if (this.eggProgress > 1f)
			{
				this.eggProgress = 1f;
			}
			if (this.ProgressStoppedBecauseUnfertilized)
			{
				this.eggProgress = this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x06009C8C RID: 40076 RVA: 0x00385CB0 File Offset: 0x00383EB0
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x06009C8D RID: 40077 RVA: 0x00385CCA File Offset: 0x00383ECA
		public ThingDef NextEggType()
		{
			if (this.fertilizationCount > 0)
			{
				return this.Props.eggFertilizedDef;
			}
			return this.Props.eggUnfertilizedDef;
		}

		// Token: 0x06009C8E RID: 40078 RVA: 0x00385CEC File Offset: 0x00383EEC
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				string str = "LayEgg while not Active: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
			}
			this.eggProgress = 0f;
			int randomInRange = this.Props.eggCountRange.RandomInRange;
			if (randomInRange == 0)
			{
				return null;
			}
			Thing thing;
			if (this.fertilizationCount > 0)
			{
				thing = ThingMaker.MakeThing(this.Props.eggFertilizedDef, null);
				this.fertilizationCount = Mathf.Max(0, this.fertilizationCount - randomInRange);
			}
			else
			{
				thing = ThingMaker.MakeThing(this.Props.eggUnfertilizedDef, null);
			}
			thing.stackCount = randomInRange;
			CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
			if (compHatcher != null)
			{
				compHatcher.hatcheeFaction = this.parent.Faction;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					compHatcher.hatcheeParent = pawn;
				}
				if (this.fertilizedBy != null)
				{
					compHatcher.otherParent = this.fertilizedBy;
				}
			}
			return thing;
		}

		// Token: 0x06009C8F RID: 40079 RVA: 0x00385DD8 File Offset: 0x00383FD8
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			string text = "EggProgress".Translate() + ": " + this.eggProgress.ToStringPercent();
			if (this.fertilizationCount > 0)
			{
				text += "\n" + "Fertilized".Translate();
			}
			else if (this.ProgressStoppedBecauseUnfertilized)
			{
				text += "\n" + "ProgressStoppedUntilFertilized".Translate();
			}
			return text;
		}

		// Token: 0x06009C90 RID: 40080 RVA: 0x00385E6D File Offset: 0x0038406D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompEggLayer.<CompGetGizmosExtra>d__20 <CompGetGizmosExtra>d__ = new CompEggLayer.<CompGetGizmosExtra>d__20(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005CF1 RID: 23793
		private const int EggTickInterval = 2500;

		// Token: 0x04005CF2 RID: 23794
		private float eggProgress;

		// Token: 0x04005CF3 RID: 23795
		private int fertilizationCount;

		// Token: 0x04005CF4 RID: 23796
		private Pawn fertilizedBy;
	}
}
