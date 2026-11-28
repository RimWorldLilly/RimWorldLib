using System;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195D RID: 6493
	public class CompRottable : ThingComp
	{
		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x0600A2F5 RID: 41717 RVA: 0x003A35C2 File Offset: 0x003A17C2
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x17001C38 RID: 7224
		// (get) Token: 0x0600A2F6 RID: 41718 RVA: 0x003A35CF File Offset: 0x003A17CF
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x0600A2F7 RID: 41719 RVA: 0x003A35E4 File Offset: 0x003A17E4
		// (set) Token: 0x0600A2F8 RID: 41720 RVA: 0x003A35EC File Offset: 0x003A17EC
		public float RotProgress
		{
			get
			{
				return this.rotProgressInt;
			}
			set
			{
				RotStage stage = this.Stage;
				this.rotProgressInt = value;
				if (stage != this.Stage)
				{
					this.StageChanged();
				}
			}
		}

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x0600A2F9 RID: 41721 RVA: 0x003A3609 File Offset: 0x003A1809
		public RotStage Stage
		{
			get
			{
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					return RotStage.Fresh;
				}
				if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					return RotStage.Rotting;
				}
				return RotStage.Dessicated;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x0600A2FA RID: 41722 RVA: 0x003A3638 File Offset: 0x003A1838
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x0600A2FB RID: 41723 RVA: 0x003A3660 File Offset: 0x003A1860
		public bool Active
		{
			get
			{
				if (this.PropsRot.disableIfHatcher)
				{
					CompHatcher compHatcher = this.parent.TryGetComp<CompHatcher>();
					if (compHatcher != null && !compHatcher.TemperatureDamaged)
					{
						return false;
					}
				}
				return !this.disabled;
			}
		}

		// Token: 0x0600A2FC RID: 41724 RVA: 0x003A369C File Offset: 0x003A189C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
			Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
		}

		// Token: 0x0600A2FD RID: 41725 RVA: 0x003A36CC File Offset: 0x003A18CC
		public override void CompTickInterval(int delta)
		{
			this.TickInterval(delta);
		}

		// Token: 0x0600A2FE RID: 41726 RVA: 0x003A36D5 File Offset: 0x003A18D5
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x0600A2FF RID: 41727 RVA: 0x003A36E4 File Offset: 0x003A18E4
		private void TickInterval(int delta)
		{
			if (!this.Active)
			{
				return;
			}
			float rotProgress = this.RotProgress;
			float num = GenTemperature.RotRateAtTemperature(this.parent.AmbientTemperature);
			this.RotProgress += num * (float)delta;
			if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
			{
				if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
				{
					Messages.Message("MessageRottedAwayInStorage".Translate(this.parent.Label, this.parent).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
				}
				if (this.parent.Spawned && this.parent.def.thingCategories.NotNullAndContains(ThingCategoryDefOf.MeatRaw))
				{
					int amount = 2 * this.parent.stackCount;
					GasUtility.AddGas(this.parent.Position, this.parent.Map, GasType.RotStink, amount);
				}
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f) && this.ShouldTakeRotDamage())
			{
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return;
				}
				if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}
			}
		}

		// Token: 0x0600A300 RID: 41728 RVA: 0x003A3904 File Offset: 0x003A1B04
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x0600A301 RID: 41729 RVA: 0x003A3948 File Offset: 0x003A1B48
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x0600A302 RID: 41730 RVA: 0x003A398B File Offset: 0x003A1B8B
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x0600A303 RID: 41731 RVA: 0x003A39A3 File Offset: 0x003A1BA3
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh && FoodUtility.GetFoodPoisonChanceFactor(ingester) > 1E-45f)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
			}
		}

		// Token: 0x0600A304 RID: 41732 RVA: 0x003A39C8 File Offset: 0x003A1BC8
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.Stage)
			{
			case RotStage.Fresh:
				stringBuilder.Append("RotStateFresh".Translate());
				break;
			case RotStage.Rotting:
				stringBuilder.Append("RotStateRotting".Translate());
				break;
			case RotStage.Dessicated:
				stringBuilder.Append("RotStateDessicated".Translate());
				break;
			}
			if ((float)this.PropsRot.TicksToRotStart - this.RotProgress > 0f)
			{
				float num = GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(this.parent.AmbientTemperature));
				int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
				if (num < 0.001f)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CurrentlyFrozen".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else if (num < 0.999f)
				{
					StringBuilder sb = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true, false)));
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					sb.AppendTagged(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				else
				{
					StringBuilder sb2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler3.AppendLiteral(" (");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true, false)));
					defaultInterpolatedStringHandler3.AppendLiteral(")");
					sb2.AppendTagged(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
			}
			else
			{
				stringBuilder.Append(".");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A305 RID: 41733 RVA: 0x003A3B98 File Offset: 0x003A1D98
		public int ApproxTicksUntilRotWhenAtTempOfTile(PlanetTile tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x0600A306 RID: 41734 RVA: 0x003A3BB4 File Offset: 0x003A1DB4
		public int TicksUntilRotAtTemp(float temp)
		{
			if (!this.Active)
			{
				return 72000000;
			}
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0f)
			{
				return 72000000;
			}
			float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num2 <= 0f)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

		// Token: 0x0600A307 RID: 41735 RVA: 0x003A3C0C File Offset: 0x003A1E0C
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x0600A308 RID: 41736 RVA: 0x003A3C30 File Offset: 0x003A1E30
		public void RotImmediately(RotStage stage = RotStage.Rotting)
		{
			if (stage == RotStage.Rotting && this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
				return;
			}
			if (stage == RotStage.Dessicated && this.RotProgress < (float)this.PropsRot.TicksToDessicated)
			{
				this.RotProgress = (float)this.PropsRot.TicksToDessicated;
			}
		}

		// Token: 0x04006052 RID: 24658
		private float rotProgressInt;

		// Token: 0x04006053 RID: 24659
		public bool disabled;
	}
}
