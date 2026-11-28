using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001508 RID: 5384
	public class RoyalTitlePermitWorker
	{
		// Token: 0x060083DD RID: 33757 RVA: 0x002F66A8 File Offset: 0x002F48A8
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x060083DE RID: 33758 RVA: 0x002F66AB File Offset: 0x002F48AB
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x060083DF RID: 33759 RVA: 0x002F66AE File Offset: 0x002F48AE
		public virtual IEnumerable<DiaOption> GetFactionCommDialogOptions(Map map, Pawn pawn, Faction factionInFavor)
		{
			return null;
		}

		// Token: 0x060083E0 RID: 33760 RVA: 0x002F66B1 File Offset: 0x002F48B1
		public virtual IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x060083E1 RID: 33761 RVA: 0x002F66B4 File Offset: 0x002F48B4
		[Obsolete]
		protected virtual bool AidDisabled(Map map, Pawn pawn, Faction faction, out string reason)
		{
			return this.AidDisabled_NewTemp(map, pawn, faction, out reason, true);
		}

		// Token: 0x060083E2 RID: 33762 RVA: 0x002F66C4 File Offset: 0x002F48C4
		protected virtual bool AidDisabled_NewTemp(Map map, Pawn pawn, Faction faction, out string reason, bool temperatureMatters = true)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				reason = "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION"));
				return true;
			}
			if (map.generatorDef.isUnderground)
			{
				reason = "CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION"));
				return true;
			}
			if (this.def.layerBlacklist.Contains(pawn.MapHeld.Tile.LayerDef))
			{
				reason = "CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION"));
				return true;
			}
			if (temperatureMatters && !this.TemperatureIsAcceptable(map, faction))
			{
				reason = "BadTemperature".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x060083E3 RID: 33763 RVA: 0x002F6794 File Offset: 0x002F4994
		protected bool FillAidOption(Pawn pawn, Faction faction, ref string description, out bool free)
		{
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += "CommandCallRoyalAidFreeOption".Translate();
				free = true;
				return true;
			}
			int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
			description += "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION"));
			if (pawn.royalty.GetFavor(faction) >= this.def.royalAid.favorCost)
			{
				free = false;
				return true;
			}
			free = false;
			return false;
		}

		// Token: 0x060083E4 RID: 33764 RVA: 0x002F6894 File Offset: 0x002F4A94
		protected bool FillCaravanAidOption(Pawn pawn, Faction faction, out string description, out bool free, out bool disableNotEnoughFavor)
		{
			description = this.def.description;
			disableNotEnoughFavor = false;
			free = false;
			if (!this.def.usableOnWorldMap)
			{
				free = false;
				return false;
			}
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += " (" + "CommandCallRoyalAidFreeOption".Translate() + ")";
				free = true;
			}
			else
			{
				int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
				description += " (" + "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION")) + ")";
				if (pawn.royalty.GetFavor(faction) < this.def.royalAid.favorCost)
				{
					disableNotEnoughFavor = true;
				}
			}
			return true;
		}

		// Token: 0x060083E5 RID: 33765 RVA: 0x002F69E0 File Offset: 0x002F4BE0
		protected virtual bool TemperatureIsAcceptable(Map map, Faction faction)
		{
			return faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp);
		}

		// Token: 0x0400517E RID: 20862
		public RoyalTitlePermitDef def;
	}
}
