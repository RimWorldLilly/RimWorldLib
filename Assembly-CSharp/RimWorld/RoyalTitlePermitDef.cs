using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002010 RID: 8208
	public class RoyalTitlePermitDef : Def
	{
		// Token: 0x17002566 RID: 9574
		// (get) Token: 0x0600CFE2 RID: 53218 RVA: 0x004C7569 File Offset: 0x004C5769
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		// Token: 0x17002567 RID: 9575
		// (get) Token: 0x0600CFE3 RID: 53219 RVA: 0x004C7578 File Offset: 0x004C5778
		public RoyalTitlePermitWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (RoyalTitlePermitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x0600CFE4 RID: 53220 RVA: 0x004C75AC File Offset: 0x004C57AC
		public bool AvailableForPawn(Pawn pawn, Faction faction)
		{
			if (pawn.royalty == null)
			{
				return false;
			}
			if (pawn.royalty.HasPermit(this, faction))
			{
				return false;
			}
			if (this.prerequisite != null && !pawn.royalty.HasPermit(this.prerequisite, faction))
			{
				return false;
			}
			if (pawn.royalty.GetPermitPoints(faction) < this.permitPointCost)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			return (currentTitle == null && this.minTitle == null) || ((currentTitle != null || this.minTitle == null) && currentTitle.seniority >= this.minTitle.seniority);
		}

		// Token: 0x0600CFE5 RID: 53221 RVA: 0x004C7648 File Offset: 0x004C5848
		public bool IsPrerequisiteOfHeldPermit(Pawn pawn, Faction faction)
		{
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == this && allFactionPermits[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CFE6 RID: 53222 RVA: 0x004C7698 File Offset: 0x004C5898
		public override IEnumerable<string> ConfigErrors()
		{
			RoyalTitlePermitDef.<ConfigErrors>d__17 <ConfigErrors>d__ = new RoyalTitlePermitDef.<ConfigErrors>d__17(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04008434 RID: 33844
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		// Token: 0x04008435 RID: 33845
		public RoyalAid royalAid;

		// Token: 0x04008436 RID: 33846
		public float cooldownDays;

		// Token: 0x04008437 RID: 33847
		public RoyalTitleDef minTitle;

		// Token: 0x04008438 RID: 33848
		public int permitPointCost;

		// Token: 0x04008439 RID: 33849
		public FactionDef faction;

		// Token: 0x0400843A RID: 33850
		public bool usableOnWorldMap;

		// Token: 0x0400843B RID: 33851
		public RoyalTitlePermitDef prerequisite;

		// Token: 0x0400843C RID: 33852
		public Vector2 uiPosition;

		// Token: 0x0400843D RID: 33853
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x0400843E RID: 33854
		private RoyalTitlePermitWorker worker;
	}
}
