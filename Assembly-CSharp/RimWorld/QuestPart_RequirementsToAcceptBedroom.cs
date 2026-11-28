using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E3 RID: 4323
	public class QuestPart_RequirementsToAcceptBedroom : QuestPart_RequirementsToAccept
	{
		// Token: 0x06006C0C RID: 27660 RVA: 0x0025F08C File Offset: 0x0025D28C
		public override AcceptanceReport CanAccept()
		{
			int num = this.CulpritsAre().Count<Pawn>();
			if (num > 0)
			{
				return ((num > 1) ? "QuestBedroomRequirementsUnsatisfied" : "QuestBedroomRequirementsUnsatisfiedSingle").Translate() + " " + (from p in this.CulpritsAre()
				select p.royalty.MainTitle().GetLabelFor(p).CapitalizeFirst() + " " + p.LabelShort).ToCommaList(true, false);
			}
			return true;
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x0025F10C File Offset: 0x0025D30C
		private List<Pawn> CulpritsAre()
		{
			this.culpritsResult.Clear();
			if (this.targetPawns.Any<Pawn>())
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_RoyaltyTracker royalty = pawn.royalty;
					if (((royalty != null) ? royalty.HighestTitleWithBedroomRequirements() : null) != null && !pawn.Suspended && (!pawn.royalty.HasPersonalBedroom() || pawn.royalty.AnyUnmetBedroomRequirements()))
					{
						this.culpritsResult.Add(pawn);
					}
				}
			}
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return this.culpritsResult;
			}
			this.tmpOccupiedBeds.Clear();
			List<Thing> list = this.mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Bed);
			foreach (Pawn pawn2 in this.targetPawns)
			{
				RoyalTitle royalTitle = pawn2.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null)
				{
					Thing thing = null;
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2.Faction == Faction.OfPlayer && thing2.GetRoom(RegionType.Set_All) != null && !this.tmpOccupiedBeds.Contains(thing2))
						{
							CompAssignableToPawn compAssignableToPawn = thing2.TryGetComp<CompAssignableToPawn>();
							if (compAssignableToPawn != null && compAssignableToPawn.AssignedPawnsForReading.Count <= 0 && RoyalTitleUtility.BedroomSatisfiesRequirements(thing2.GetRoom(RegionType.Set_All), royalTitle))
							{
								thing = thing2;
								break;
							}
						}
					}
					if (thing != null)
					{
						this.tmpOccupiedBeds.Add(thing);
					}
					else
					{
						this.culpritsResult.Add(pawn2);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			return this.culpritsResult;
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x0025F2F4 File Offset: 0x0025D4F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.targetPawns, "targetPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.targetPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06006C0F RID: 27663 RVA: 0x0025F361 File Offset: 0x0025D561
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.CulpritsAre().Select(delegate(Pawn p)
				{
					RoyalTitle royalTitle = p.royalty.HighestTitleWithBedroomRequirements();
					return new Dialog_InfoCard.Hyperlink(royalTitle.def, royalTitle.faction, -1);
				});
			}
		}

		// Token: 0x06006C10 RID: 27664 RVA: 0x0025F38D File Offset: 0x0025D58D
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.targetPawns.Replace(replace, with);
		}

		// Token: 0x040044D1 RID: 17617
		public List<Pawn> targetPawns = new List<Pawn>();

		// Token: 0x040044D2 RID: 17618
		public MapParent mapParent;

		// Token: 0x040044D3 RID: 17619
		private List<Thing> tmpOccupiedBeds = new List<Thing>();

		// Token: 0x040044D4 RID: 17620
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
