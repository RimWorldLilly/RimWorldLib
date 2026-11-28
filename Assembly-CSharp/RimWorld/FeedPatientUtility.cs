using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0B RID: 3083
	public static class FeedPatientUtility
	{
		// Token: 0x06005811 RID: 22545 RVA: 0x00209700 File Offset: 0x00207900
		public static bool ShouldBeFed(Pawn p)
		{
			if (p.GetPosture() == PawnPosture.Standing)
			{
				return false;
			}
			if (p.NonHumanlikeOrWildMan())
			{
				Building_Bed building_Bed = p.CurrentBed();
				if (building_Bed == null || building_Bed.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (!p.InBed())
				{
					return false;
				}
			}
			if (p.RaceProps.EatsFood)
			{
				Pawn_NeedsTracker needs = p.needs;
				if (((needs != null) ? needs.food : null) != null)
				{
					if (p.Spawned && p.ShouldBeSlaughtered())
					{
						return false;
					}
					if (!HealthAIUtility.ShouldSeekMedicalRest(p))
					{
						return false;
					}
					if (p.HostFaction != null)
					{
						if (p.HostFaction != Faction.OfPlayer)
						{
							return false;
						}
						if (p.guest != null && !p.guest.CanBeBroughtFood)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x002097CC File Offset: 0x002079CC
		public static bool IsHungry(Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.CurLevelPercentage <= p.needs.food.PercentageThreshHungry + 0.02f;
		}
	}
}
