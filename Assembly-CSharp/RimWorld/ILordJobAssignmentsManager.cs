using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001703 RID: 5891
	public interface ILordJobAssignmentsManager<RoleType> where RoleType : ILordJobRole
	{
		// Token: 0x06009317 RID: 37655
		IEnumerable<IGrouping<string, RoleType>> RoleGroups();

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x06009318 RID: 37656
		bool SpectatorsAllowed { get; }

		// Token: 0x06009319 RID: 37657
		IEnumerable<Pawn> AssignedPawns(RoleType role);

		// Token: 0x0600931A RID: 37658
		RoleType ForcedRole(Pawn pawn);

		// Token: 0x0600931B RID: 37659
		RoleType RoleForPawn(Pawn pawn, bool includeForced = true);

		// Token: 0x0600931C RID: 37660
		Pawn FirstAssignedPawn(RoleType role);

		// Token: 0x0600931D RID: 37661
		bool Required(Pawn pawn);

		// Token: 0x0600931E RID: 37662
		bool PawnParticipating(Pawn pawn);

		// Token: 0x0600931F RID: 37663
		bool PawnSpectating(Pawn pawn);

		// Token: 0x06009320 RID: 37664
		bool CanParticipate(Pawn pawn, out TaggedString reason);

		// Token: 0x06009321 RID: 37665
		bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null);

		// Token: 0x06009322 RID: 37666
		bool TryAssign(Pawn pawn, RoleType role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null);

		// Token: 0x06009323 RID: 37667
		bool TryUnassignAnyRole(Pawn pawn);

		// Token: 0x06009324 RID: 37668
		void RemoveParticipant(Pawn pawn);

		// Token: 0x06009325 RID: 37669
		string PawnNotAssignableReason(Pawn p, RoleType role);

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x06009326 RID: 37670
		List<Pawn> SpectatorsForReading { get; }
	}
}
