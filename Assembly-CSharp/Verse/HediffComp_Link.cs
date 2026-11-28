using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000456 RID: 1110
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x000C002A File Offset: 0x000BE22A
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000C0037 File Offset: 0x000BE237
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.other;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000C0044 File Offset: 0x000BE244
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.SpawnedOrAnyParentSpawned || !this.other.SpawnedOrAnyParentSpawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.PositionHeld.InHorDistOf(this.other.PositionHeld, this.Props.maxDistance))
				{
					return true;
				}
				if (this.Props.requireLinkOnOtherPawn)
				{
					Pawn pawn = this.other as Pawn;
					if (pawn != null)
					{
						foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
						{
							HediffWithComps hediffWithComps = hediff as HediffWithComps;
							if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
							{
								HediffComp_Link hediffComp_Link = c as HediffComp_Link;
								return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
							}) != null)
							{
								return false;
							}
						}
						return true;
					}
					Log.Error("HediffComp_Link requires link on other pawn, but other thing is not a pawn!");
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000C0168 File Offset: 0x000BE368
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.drawConnection || this.other.MapHeld != this.parent.pawn.MapHeld)
			{
				return;
			}
			ThingDef moteDef = this.Props.customMote ?? ThingDefOf.Mote_PsychicLinkLine;
			if (this.mote == null)
			{
				this.mote = MoteMaker.MakeInteractionOverlay(moteDef, this.parent.pawn, this.other);
			}
			this.mote.Maintain();
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000C01EA File Offset: 0x000BE3EA
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Thing>(ref this.other, "other", false);
			Scribe_Values.Look<bool>(ref this.drawConnection, "drawConnection", false, false);
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000C0215 File Offset: 0x000BE415
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		// Token: 0x040018A6 RID: 6310
		public Thing other;

		// Token: 0x040018A7 RID: 6311
		private MoteDualAttached mote;

		// Token: 0x040018A8 RID: 6312
		public bool drawConnection;
	}
}
