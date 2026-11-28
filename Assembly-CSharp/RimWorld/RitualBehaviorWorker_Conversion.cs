using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168A RID: 5770
	public class RitualBehaviorWorker_Conversion : RitualBehaviorWorker
	{
		// Token: 0x060090A7 RID: 37031 RVA: 0x00351323 File Offset: 0x0034F523
		public RitualBehaviorWorker_Conversion()
		{
		}

		// Token: 0x060090A8 RID: 37032 RVA: 0x0035132B File Offset: 0x0034F52B
		public RitualBehaviorWorker_Conversion(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x060090A9 RID: 37033 RVA: 0x00351334 File Offset: 0x0034F534
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Moralist);
			if (precept_Role == null)
			{
				return null;
			}
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			bool flag = false;
			using (List<Pawn>.Enumerator enumerator = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (RitualBehaviorWorker_Conversion.ValidateConvertee(enumerator.Current, precept_Role.ChosenPawnSingle(), false))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "CantStartRitualNoConvertee".Translate(precept_Role.ChosenPawnSingle().Ideo.name);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}

		// Token: 0x060090AA RID: 37034 RVA: 0x0035142C File Offset: 0x0034F62C
		public static bool ValidateConvertee(Pawn convertee, Pawn leader, bool throwMessages)
		{
			return convertee != null && AbilityUtility.ValidateNoMentalState(convertee, throwMessages, null) && AbilityUtility.ValidateCanWalk(convertee, throwMessages, null) && AbilityUtility.ValidateNotSameIdeo(leader, convertee, throwMessages, null);
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x0035145C File Offset: 0x0034F65C
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("moralist");
			Pawn pawn = ritual.PawnWithRole("convertee");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
