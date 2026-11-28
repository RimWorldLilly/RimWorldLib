using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F4 RID: 6388
	public class CompMechRelay : ThingComp
	{
		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600A025 RID: 40997 RVA: 0x00395D8C File Offset: 0x00393F8C
		public CompProperties_MechRelay Props
		{
			get
			{
				return (CompProperties_MechRelay)this.props;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600A026 RID: 40998 RVA: 0x00395D99 File Offset: 0x00393F99
		public int DestabilizationTick
		{
			get
			{
				return this.destabilizationTick;
			}
		}

		// Token: 0x0600A027 RID: 40999 RVA: 0x00395DA4 File Offset: 0x00393FA4
		public void Deactivate()
		{
			if (!this.active || this.inert)
			{
				return;
			}
			this.active = false;
			if (!this.Props.destabilizationMessage.NullOrEmpty())
			{
				Messages.Message(this.Props.destabilizationMessage, this.parent, MessageTypeDefOf.PositiveEvent, true);
			}
			this.destabilizationTick = Find.TickManager.TicksGame + 7500;
		}

		// Token: 0x0600A028 RID: 41000 RVA: 0x00395E14 File Offset: 0x00394014
		public override void CompTick()
		{
			if (this.inert)
			{
				return;
			}
			if (this.destabilizationTick > 0 && Find.TickManager.TicksGame >= this.destabilizationTick)
			{
				this.destabilizationTick = -1;
				Map map = this.parent.Map;
				IntVec3 position = this.parent.Position;
				this.parent.Destroy(DestroyMode.Vanish);
				if (this.Props.crashedThingDef != null)
				{
					Thing thing = ThingMaker.MakeThing(this.Props.crashedThingDef, null);
					GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Direct, null, null, null, 1);
					CellRect cellRect = this.parent.OccupiedRect();
					for (int i = 0; i < cellRect.Area; i++)
					{
						FleckMaker.ThrowDustPuff(cellRect.RandomVector3.WithY(AltitudeLayer.MoteLow.AltitudeFor()), map, 2f);
					}
					if (thing.Faction != this.parent.Faction)
					{
						thing.SetFaction(this.parent.Faction, null);
					}
					if (!this.Props.crashedLetterLabel.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(this.Props.crashedLetterLabel, this.Props.crashedLetterText, LetterDefOf.NeutralEvent, thing, null, null, null, null, 0, true);
					}
				}
			}
		}

		// Token: 0x0600A029 RID: 41001 RVA: 0x00395F64 File Offset: 0x00394164
		public override string CompInspectStringExtra()
		{
			if (this.inert)
			{
				return null;
			}
			if (this.active && !this.Props.activeInspectString.NullOrEmpty())
			{
				return this.Props.activeInspectString;
			}
			if (this.destabilizationTick > 0 && !this.Props.destabilizationInspectString.NullOrEmpty())
			{
				return this.Props.destabilizationInspectString + ": " + (this.destabilizationTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x0600A02A RID: 41002 RVA: 0x00395FFD File Offset: 0x003941FD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompMechRelay.<CompGetGizmosExtra>d__12 <CompGetGizmosExtra>d__ = new CompMechRelay.<CompGetGizmosExtra>d__12(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A02B RID: 41003 RVA: 0x0039600D File Offset: 0x0039420D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", true, false);
			Scribe_Values.Look<bool>(ref this.inert, "inert", false, false);
			Scribe_Values.Look<int>(ref this.destabilizationTick, "destabilizationTick", -1, false);
		}

		// Token: 0x04005EBE RID: 24254
		public const int DestabilizationTicks = 7500;

		// Token: 0x04005EBF RID: 24255
		private const int CrashFlecksPerCell = 1;

		// Token: 0x04005EC0 RID: 24256
		private bool active = true;

		// Token: 0x04005EC1 RID: 24257
		public bool inert;

		// Token: 0x04005EC2 RID: 24258
		private int destabilizationTick = -1;
	}
}
