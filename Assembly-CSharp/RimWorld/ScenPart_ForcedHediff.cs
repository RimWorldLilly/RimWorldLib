using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178B RID: 6027
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x06009624 RID: 38436 RVA: 0x00368A3F File Offset: 0x00366C3F
		private float MaxSeverity
		{
			get
			{
				if (this.hediff.lethalSeverity <= 0f)
				{
					return 1f;
				}
				return this.hediff.lethalSeverity * 0.99f;
			}
		}

		// Token: 0x06009625 RID: 38437 RVA: 0x00368A6C File Offset: 0x00366C6C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<HediffDef>(this.PossibleHediffs(), (HediffDef hd) => hd.LabelCap, (HediffDef hd) => delegate()
				{
					this.hediff = hd;
					if (this.severityRange.max > this.MaxSeverity)
					{
						this.severityRange.max = this.MaxSeverity;
					}
					if (this.severityRange.min > this.MaxSeverity)
					{
						this.severityRange.min = this.MaxSeverity;
					}
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo, 0f, GameFont.Small, null, 0f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x06009626 RID: 38438 RVA: 0x00368B70 File Offset: 0x00366D70
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x06009627 RID: 38439 RVA: 0x00368B9C File Offset: 0x00366D9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x06009628 RID: 38440 RVA: 0x00368BDC File Offset: 0x00366DDC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		// Token: 0x06009629 RID: 38441 RVA: 0x00368C30 File Offset: 0x00366E30
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600962A RID: 38442 RVA: 0x00368CA4 File Offset: 0x00366EA4
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		// Token: 0x0600962B RID: 38443 RVA: 0x00368CE4 File Offset: 0x00366EE4
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
			if (this.hideOffMap)
			{
				if (!req.AllowDead && !req.ForceDead && pawn.health.WouldDieAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
				if (!req.AllowDowned && !req.ForceDead && pawn.health.WouldBeDownedAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600962C RID: 38444 RVA: 0x00368D6D File Offset: 0x00366F6D
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600962D RID: 38445 RVA: 0x00368D76 File Offset: 0x00366F76
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600962E RID: 38446 RVA: 0x00368D80 File Offset: 0x00366F80
		private void AddHediff(Pawn p)
		{
			if (Find.GameInitData != null && Find.GameInitData.QuickStarted && Prefs.DisableQuickStartCryptoSickness && this.hediff == HediffDefOf.CryptosleepSickness)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0600962F RID: 38447 RVA: 0x00368DEB File Offset: 0x00366FEB
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.hediff == null;
		}

		// Token: 0x06009630 RID: 38448 RVA: 0x00368E00 File Offset: 0x00367000
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.hediff != null) ? this.hediff.GetHashCode() : 0) ^ this.severityRange.GetHashCode();
		}

		// Token: 0x04005A3A RID: 23098
		private HediffDef hediff;

		// Token: 0x04005A3B RID: 23099
		private FloatRange severityRange;
	}
}
