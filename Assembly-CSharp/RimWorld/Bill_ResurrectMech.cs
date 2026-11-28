using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001ADB RID: 6875
	public class Bill_ResurrectMech : Bill_Mech
	{
		// Token: 0x17001F9F RID: 8095
		// (get) Token: 0x0600AF6D RID: 44909 RVA: 0x003DC218 File Offset: 0x003DA418
		public override float BandwidthCost
		{
			get
			{
				if (base.Gestator.ActiveBill == this)
				{
					Pawn gestatingMech = base.Gestator.GestatingMech;
					if (gestatingMech != null)
					{
						return gestatingMech.GetStatValue(StatDefOf.BandwidthCost, true, -1);
					}
					Corpse resurrectingMechCorpse = base.Gestator.ResurrectingMechCorpse;
					if (resurrectingMechCorpse != null)
					{
						return resurrectingMechCorpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost, true, -1);
					}
				}
				return 0f;
			}
		}

		// Token: 0x0600AF6E RID: 44910 RVA: 0x003DC277 File Offset: 0x003DA477
		public Bill_ResurrectMech()
		{
		}

		// Token: 0x0600AF6F RID: 44911 RVA: 0x003DC27F File Offset: 0x003DA47F
		public Bill_ResurrectMech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF70 RID: 44912 RVA: 0x003DC28C File Offset: 0x003DA48C
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			if (!base.PawnAllowedToStartAnew(p))
			{
				return false;
			}
			if (base.State == FormingState.Gathering && base.Gestator.ResurrectingMechCorpse == null)
			{
				IEnumerable<Corpse> source = this.AllAvailableResurrectionCorpses(p);
				if (source.Any<Corpse>() && !source.Any((Corpse c) => p.mechanitor.HasBandwidthToResurrect(c)))
				{
					JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AF71 RID: 44913 RVA: 0x003DC30C File Offset: 0x003DA50C
		public override Thing CreateProducts()
		{
			Pawn innerPawn = base.Gestator.ResurrectingMechCorpse.InnerPawn;
			ResurrectionUtility.TryResurrect(innerPawn, new ResurrectionParams
			{
				dontSpawn = true
			});
			innerPawn.needs.energy.CurLevel = innerPawn.needs.energy.MaxLevel * 0.5f;
			innerPawn.health.RemoveAllHediffs();
			base.BoundPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, innerPawn);
			if (innerPawn.IsWorldPawn())
			{
				Find.WorldPawns.RemovePawn(innerPawn);
			}
			return innerPawn;
		}

		// Token: 0x0600AF72 RID: 44914 RVA: 0x003DC398 File Offset: 0x003DA598
		public override bool IsFixedOrAllowedIngredient(Thing thing)
		{
			if (!base.IsFixedOrAllowedIngredient(thing))
			{
				return false;
			}
			Corpse corpse = thing as Corpse;
			return corpse == null || corpse.InnerPawn.Faction == Faction.OfPlayer;
		}

		// Token: 0x0600AF73 RID: 44915 RVA: 0x003DC3CF File Offset: 0x003DA5CF
		private IEnumerable<Corpse> AllAvailableResurrectionCorpses(Pawn pawn)
		{
			Bill_ResurrectMech.<AllAvailableResurrectionCorpses>d__7 <AllAvailableResurrectionCorpses>d__ = new Bill_ResurrectMech.<AllAvailableResurrectionCorpses>d__7(-2);
			<AllAvailableResurrectionCorpses>d__.<>4__this = this;
			<AllAvailableResurrectionCorpses>d__.<>3__pawn = pawn;
			return <AllAvailableResurrectionCorpses>d__;
		}

		// Token: 0x0600AF74 RID: 44916 RVA: 0x003DC3E8 File Offset: 0x003DA5E8
		public override void AppendInspectionData(StringBuilder sb)
		{
			switch (base.State)
			{
			case FormingState.Gathering:
				base.AppendCurrentIngredientCount(sb);
				break;
			case FormingState.Forming:
			{
				Building_MechGestator gestator = base.Gestator;
				if (((gestator != null) ? gestator.ResurrectingMechCorpse : null) != null)
				{
					sb.AppendLine("ResurrectingMech".Translate(base.Gestator.ResurrectingMechCorpse.InnerPawn.LabelCap));
				}
				break;
			}
			case FormingState.Formed:
			{
				Building_MechGestator gestator2 = base.Gestator;
				if (((gestator2 != null) ? gestator2.GestatingMech : null) != null && base.BoundPawn != null)
				{
					sb.AppendLine("ResurrectedMech".Translate(base.Gestator.GestatingMech.LabelCap, base.BoundPawn.Named("PAWN")) + " (" + "GestatedMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")) + ")");
				}
				break;
			}
			}
			base.AppendInspectionData(sb);
		}
	}
}
