using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001509 RID: 5385
	public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x060083E7 RID: 33767 RVA: 0x002F6A1D File Offset: 0x002F4C1D
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_CallAid.<GetRoyalAidOptions>d__2 <GetRoyalAidOptions>d__ = new RoyalTitlePermitWorker_CallAid.<GetRoyalAidOptions>d__2(-2);
			<GetRoyalAidOptions>d__.<>4__this = this;
			<GetRoyalAidOptions>d__.<>3__map = map;
			<GetRoyalAidOptions>d__.<>3__pawn = pawn;
			<GetRoyalAidOptions>d__.<>3__faction = faction;
			return <GetRoyalAidOptions>d__;
		}

		// Token: 0x060083E8 RID: 33768 RVA: 0x002F6A44 File Offset: 0x002F4C44
		private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			RoyalTitlePermitWorker_CallAid.<>c__DisplayClass3_0 CS$<>8__locals1 = new RoyalTitlePermitWorker_CallAid.<>c__DisplayClass3_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.caller = caller;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.free = free;
			CS$<>8__locals1.biocodeChance = biocodeChance;
			IEnumerable<Faction> source = from f in (from p in CS$<>8__locals1.map.mapPawns.AllPawnsSpawned
			where p.Faction != null && !p.Faction.IsPlayer && p.Faction != CS$<>8__locals1.faction && !p.IsPrisonerOfColony
			select p.Faction).Distinct<Faction>()
			where f.HostileTo(Faction.OfPlayer) && !CS$<>8__locals1.faction.HostileTo(f)
			select f;
			if (source.Any<Faction>())
			{
				Find.WindowStack.Add(new Dialog_MessageBox("CommandCallRoyalAidWarningNonHostileFactions".Translate(CS$<>8__locals1.faction, (from f in source
				select f.NameColored.Resolve()).ToCommaList(false, false)), "Confirm".Translate(), new Action(CS$<>8__locals1.<BeginCallAid>g__Call|0), "GoBack".Translate(), null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			CS$<>8__locals1.<BeginCallAid>g__Call|0();
		}

		// Token: 0x060083E9 RID: 33769 RVA: 0x002F6B76 File Offset: 0x002F4D76
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallAid(this.caller, this.map, target.Cell, this.calledFaction, this.free, this.biocodeChance);
		}

		// Token: 0x060083EA RID: 33770 RVA: 0x002F6BA4 File Offset: 0x002F4DA4
		private void CallAid(Pawn caller, Map map, IntVec3 spawnPos, Faction faction, bool free, float biocodeChance = 1f)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.biocodeApparelChance = biocodeChance;
			incidentParms.biocodeWeaponsChance = biocodeChance;
			incidentParms.spawnCenter = spawnPos;
			if (this.def.royalAid.pawnKindDef != null)
			{
				incidentParms.pawnKind = this.def.royalAid.pawnKindDef;
				incidentParms.pawnCount = this.def.royalAid.pawnCount;
			}
			else
			{
				incidentParms.points = (float)this.def.royalAid.points;
			}
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			if (IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				if (!free)
				{
					caller.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
				}
				caller.royalty.GetPermit(this.def, faction).Notify_Used();
				return;
			}
			Log.Error("Could not send aid to map " + ((map != null) ? map.ToString() : null) + " from faction " + ((faction != null) ? faction.ToString() : null));
		}

		// Token: 0x060083EB RID: 33771 RVA: 0x002F6CC8 File Offset: 0x002F4EC8
		protected override bool TemperatureIsAcceptable(Map map, Faction faction)
		{
			if (this.def.royalAid.pawnKindDef == null)
			{
				return base.TemperatureIsAcceptable(map, faction);
			}
			if (this.def.royalAid.overrideAcceptableTemperatureRange == null)
			{
				return base.TemperatureIsAcceptable(map, faction);
			}
			return this.def.royalAid.overrideAcceptableTemperatureRange.Value.Includes(map.mapTemperature.SeasonalTemp);
		}

		// Token: 0x0400517F RID: 20863
		private Faction calledFaction;

		// Token: 0x04005180 RID: 20864
		private float biocodeChance;
	}
}
