using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001946 RID: 6470
	public class CompPsychicRitualSpot : ThingComp
	{
		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x0600A24C RID: 41548 RVA: 0x003A0202 File Offset: 0x0039E402
		private bool Obstructed
		{
			get
			{
				return this.obstructingThings.Count > 0;
			}
		}

		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x0600A24D RID: 41549 RVA: 0x003A0212 File Offset: 0x0039E412
		public HashSet<IntVec3> OccupiedCells
		{
			get
			{
				return this.occupiedCells;
			}
		}

		// Token: 0x0600A24E RID: 41550 RVA: 0x003A021C File Offset: 0x0039E41C
		public Lord GetLord()
		{
			Building building = this.parent as Building;
			if (building != null)
			{
				return building.GetLord();
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				return pawn.GetLord();
			}
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				return corpse.GetLord();
			}
			return null;
		}

		// Token: 0x0600A24F RID: 41551 RVA: 0x003A026C File Offset: 0x0039E46C
		public PsychicRitual GetPsychicRitual()
		{
			Lord lord = this.GetLord();
			LordToil_PsychicRitual lordToil_PsychicRitual = ((lord != null) ? lord.CurLordToil : null) as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return null;
			}
			return lordToil_PsychicRitual.RitualData.psychicRitual;
		}

		// Token: 0x0600A250 RID: 41552 RVA: 0x003A0298 File Offset: 0x0039E498
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.occupiedCells = new HashSet<IntVec3>(GenRadial.RadialCellsAround(this.parent.Position, (float)this.parent.def.Size.x + 0.9f, true));
		}

		// Token: 0x0600A251 RID: 41553 RVA: 0x003A02E4 File Offset: 0x0039E4E4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra());
			if (this.Obstructed)
			{
				stringBuilder.AppendInNewLine("PsychicRitualSpotObstructed".Translate());
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				LordToil_PsychicRitual lordToil_PsychicRitual = lord.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					PsychicRitual psychicRitual = lordToil_PsychicRitual.RitualData.psychicRitual;
					PsychicRitualToil psychicRitualToil = lordToil_PsychicRitual.RitualData.psychicRitualToil;
					StringBuilder sb = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PsychicRitual".Translate().CapitalizeFirst());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(psychicRitual.def.LabelCap);
					sb.AppendInNewLine(defaultInterpolatedStringHandler.ToStringAndClear());
					stringBuilder.AppendInNewLine(psychicRitualToil.GetReport(psychicRitual, null).CapitalizeFirst().EndWithPeriod());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A252 RID: 41554 RVA: 0x003A03C4 File Offset: 0x0039E5C4
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (this.parent.Faction == Faction.OfPlayer)
			{
				foreach (Thing t in this.obstructingThings)
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), t.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x0600A253 RID: 41555 RVA: 0x003A0444 File Offset: 0x0039E644
		public override void CompTick()
		{
			base.CompTick();
			this.obstructingThings.Clear();
			foreach (IntVec3 c in this.occupiedCells)
			{
				List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.passability != Traversability.Standable)
					{
						this.obstructingThings.Add(list[i]);
					}
				}
			}
			PsychicRitual psychicRitual = this.GetPsychicRitual();
			if (psychicRitual != null && this.parent.Faction == Faction.OfPlayer && this.Obstructed)
			{
				psychicRitual.CancelPsychicRitual("PsychicRitualAreaObstructed".Translate());
			}
		}

		// Token: 0x0600A254 RID: 41556 RVA: 0x003A0528 File Offset: 0x0039E728
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPsychicRitualSpot.<CompGetGizmosExtra>d__12 <CompGetGizmosExtra>d__ = new CompPsychicRitualSpot.<CompGetGizmosExtra>d__12(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005FE6 RID: 24550
		private List<Thing> obstructingThings = new List<Thing>();

		// Token: 0x04005FE7 RID: 24551
		private HashSet<IntVec3> occupiedCells;
	}
}
