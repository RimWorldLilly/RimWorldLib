using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using Verse;

namespace RimWorld
{
	// Token: 0x02001814 RID: 6164
	public class CompApparelVerbOwner_Charged : CompApparelVerbOwner, ICompWithCharges
	{
		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x06009A01 RID: 39425 RVA: 0x0037A4DA File Offset: 0x003786DA
		public new CompProperties_ApparelVerbOwnerCharged Props
		{
			get
			{
				return this.props as CompProperties_ApparelVerbOwnerCharged;
			}
		}

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x06009A02 RID: 39426 RVA: 0x0037A4E7 File Offset: 0x003786E7
		public int RemainingCharges
		{
			get
			{
				return this.remainingCharges;
			}
		}

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x06009A03 RID: 39427 RVA: 0x0037A4EF File Offset: 0x003786EF
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x06009A04 RID: 39428 RVA: 0x0037A4FC File Offset: 0x003786FC
		public string LabelRemaining
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.RemainingCharges);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.MaxCharges);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x06009A05 RID: 39429 RVA: 0x0037A53F File Offset: 0x0037873F
		public override string GizmoExtraLabel
		{
			get
			{
				return this.LabelRemaining;
			}
		}

		// Token: 0x06009A06 RID: 39430 RVA: 0x0037A547 File Offset: 0x00378747
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.remainingCharges = this.MaxCharges;
		}

		// Token: 0x06009A07 RID: 39431 RVA: 0x0037A55B File Offset: 0x0037875B
		public override string CompInspectStringExtra()
		{
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x06009A08 RID: 39432 RVA: 0x0037A58C File Offset: 0x0037878C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompApparelVerbOwner_Charged.<SpecialDisplayStats>d__13 <SpecialDisplayStats>d__ = new CompApparelVerbOwner_Charged.<SpecialDisplayStats>d__13(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06009A09 RID: 39433 RVA: 0x0037A59C File Offset: 0x0037879C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.remainingCharges, "remainingCharges", -999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.remainingCharges == -999)
			{
				this.remainingCharges = this.MaxCharges;
			}
		}

		// Token: 0x06009A0A RID: 39434 RVA: 0x0037A5DB File Offset: 0x003787DB
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			CompApparelVerbOwner_Charged.<CompGetWornGizmosExtra>d__15 <CompGetWornGizmosExtra>d__ = new CompApparelVerbOwner_Charged.<CompGetWornGizmosExtra>d__15(-2);
			<CompGetWornGizmosExtra>d__.<>4__this = this;
			return <CompGetWornGizmosExtra>d__;
		}

		// Token: 0x06009A0B RID: 39435 RVA: 0x0037A5EC File Offset: 0x003787EC
		public override string CompTipStringExtra()
		{
			TaggedString value = "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument).CapitalizeFirst();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.RemainingCharges);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.MaxCharges);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06009A0C RID: 39436 RVA: 0x0037A670 File Offset: 0x00378870
		public override void UsedOnce()
		{
			base.UsedOnce();
			if (this.remainingCharges > 0)
			{
				this.remainingCharges--;
			}
			if (this.Props.destroyOnEmpty && this.remainingCharges == 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04005BC0 RID: 23488
		protected int remainingCharges;
	}
}
