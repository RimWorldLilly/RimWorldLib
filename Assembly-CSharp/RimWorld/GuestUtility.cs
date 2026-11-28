using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001551 RID: 5457
	[StaticConstructorOnStartup]
	public static class GuestUtility
	{
		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06008560 RID: 34144 RVA: 0x00301636 File Offset: 0x002FF836
		public static Texture2D SlaveIcon
		{
			get
			{
				if (GuestUtility.slaveIcon == null)
				{
					GuestUtility.slaveIcon = ContentFinder<Texture2D>.Get("UI/Icons/Slavery", true);
				}
				return GuestUtility.slaveIcon;
			}
		}

		// Token: 0x06008561 RID: 34145 RVA: 0x0030165A File Offset: 0x002FF85A
		public static Texture2D GetGuestIcon(GuestStatus guestStatus)
		{
			if (guestStatus == GuestStatus.Slave)
			{
				return GuestUtility.SlaveIcon;
			}
			return null;
		}

		// Token: 0x06008562 RID: 34146 RVA: 0x00301668 File Offset: 0x002FF868
		public static List<WorkTypeDef> GetDisabledWorkTypes(this Pawn_GuestTracker guest)
		{
			GuestUtility.tmpDisabledWorkTypes.Clear();
			if (guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (workTypeDef.disabledForSlaves)
					{
						GuestUtility.tmpDisabledWorkTypes.Add(workTypeDef);
					}
				}
			}
			return GuestUtility.tmpDisabledWorkTypes;
		}

		// Token: 0x06008563 RID: 34147 RVA: 0x003016D8 File Offset: 0x002FF8D8
		public static bool IsSellingToSlavery(Pawn slave, Faction slaveOwner)
		{
			return (slave.IsSlave || slave.IsPrisoner) && slave.HomeFaction != slaveOwner;
		}

		// Token: 0x06008564 RID: 34148 RVA: 0x003016F8 File Offset: 0x002FF8F8
		public static void GetExtraFactionsFromGuestStatus(Pawn pawn, List<ExtraFaction> outExtraFactions)
		{
			if (pawn.SlaveFaction != null)
			{
				outExtraFactions.Add(new ExtraFaction(pawn.SlaveFaction, ExtraFactionType.HomeFaction));
			}
		}

		// Token: 0x06008565 RID: 34149 RVA: 0x00301714 File Offset: 0x002FF914
		public static void Notify_PrisonerEscaped(Pawn prisoner)
		{
			ThoughtToAddToAll thoughtToAddToAll = new ThoughtToAddToAll(ThoughtDefOf.ColonyPrisonerEscaped, prisoner);
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (pawn != prisoner)
				{
					thoughtToAddToAll.Add(pawn);
				}
			}
		}

		// Token: 0x06008566 RID: 34150 RVA: 0x00301778 File Offset: 0x002FF978
		public static bool PrisonerCanReturnToCell(Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.guest.ShouldWaitInsteadOfEscaping;
		}

		// Token: 0x0400520D RID: 21005
		public static Texture2D SlaveSuppressionFillTex = SolidColorMaterials.NewSolidColorTexture(new Color32(245, 209, 66, byte.MaxValue));

		// Token: 0x0400520E RID: 21006
		public static readonly Texture2D RansomIcon = ContentFinder<Texture2D>.Get("UI/Icons/Ransom", true);

		// Token: 0x0400520F RID: 21007
		private static Texture2D slaveIcon;

		// Token: 0x04005210 RID: 21008
		private static List<WorkTypeDef> tmpDisabledWorkTypes = new List<WorkTypeDef>();
	}
}
