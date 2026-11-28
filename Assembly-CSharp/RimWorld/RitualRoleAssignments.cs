using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001704 RID: 5892
	public class RitualRoleAssignments : ILordJobAssignmentsManager<RitualRole>, ILordJobCandidatePool, IExposable
	{
		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x06009327 RID: 37671 RVA: 0x0035B999 File Offset: 0x00359B99
		public List<Pawn> SpectatorsForReading
		{
			get
			{
				return this.spectators;
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x06009328 RID: 37672 RVA: 0x0035B9A1 File Offset: 0x00359BA1
		public Precept_Ritual Ritual
		{
			get
			{
				return this.ritual;
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x06009329 RID: 37673 RVA: 0x0035B9A9 File Offset: 0x00359BA9
		public bool SpectatorsAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x0600932A RID: 37674 RVA: 0x0035B9AC File Offset: 0x00359BAC
		public List<Pawn> Participants
		{
			get
			{
				this.tmpParticipants.Clear();
				foreach (Pawn pawn in this.allPawns)
				{
					if (this.PawnParticipating(pawn))
					{
						this.tmpParticipants.Add(pawn);
					}
				}
				return this.tmpParticipants;
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600932B RID: 37675 RVA: 0x0035BA20 File Offset: 0x00359C20
		public List<Pawn> AllCandidatePawns
		{
			get
			{
				return this.allPawns;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600932C RID: 37676 RVA: 0x0035BA28 File Offset: 0x00359C28
		public List<Pawn> NonAssignablePawns
		{
			get
			{
				return this.nonAssignablePawns;
			}
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x0600932D RID: 37677 RVA: 0x0035BA30 File Offset: 0x00359C30
		public List<RitualRole> AllRolesForReading
		{
			get
			{
				return this.allRoles;
			}
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x0600932E RID: 37678 RVA: 0x0035BA38 File Offset: 0x00359C38
		public List<Pawn> ExtraRequiredPawnsForReading
		{
			get
			{
				return this.requiredPawns;
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600932F RID: 37679 RVA: 0x0035BA40 File Offset: 0x00359C40
		public Dictionary<string, Pawn> ForcedRolesForReading
		{
			get
			{
				return this.forcedRoles;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x06009330 RID: 37680 RVA: 0x0035BA48 File Offset: 0x00359C48
		public Precept_Role RoleChangeSelection
		{
			get
			{
				return this.roleChangeSelection;
			}
		}

		// Token: 0x06009331 RID: 37681 RVA: 0x0035BA50 File Offset: 0x00359C50
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.allPawns, "allPawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.nonAssignablePawns, "nonAssignablePawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.spectators, "spectators", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<string, Pawn>(ref this.forcedRoles, "forcedRoles", LookMode.Value, LookMode.Reference, ref this.tmpForcedRoleIds, ref this.tmpForcedRolePawns, true, false, true);
			Scribe_Collections.Look<string, List<Pawn>>(ref this.assignedRoles, "assignedRoles", LookMode.Value, LookMode.Reference, ref this.tmpAssignedRoleIds, ref this.tmpAssignedRolePawns, true, false, true);
			Scribe_References.Look<Precept_Ritual>(ref this.ritual, "ritual", false);
			Scribe_TargetInfo.Look(ref this.ritualTarget, true, "ritualTarget");
			Scribe_References.Look<Pawn>(ref this.selectedPawn, "selectedPawn", true);
			Scribe_References.Look<Precept_Role>(ref this.roleChangeSelection, "roleChangeSelection", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			}
		}

		// Token: 0x06009332 RID: 37682 RVA: 0x0035BB76 File Offset: 0x00359D76
		protected RitualRoleAssignments()
		{
		}

		// Token: 0x06009333 RID: 37683 RVA: 0x0035BB9F File Offset: 0x00359D9F
		public RitualRoleAssignments(Precept_Ritual ritual, TargetInfo ritualTarget)
		{
			this.ritual = ritual;
			this.ritualTarget = ritualTarget;
		}

		// Token: 0x06009334 RID: 37684 RVA: 0x0035BBD8 File Offset: 0x00359DD8
		public void Setup(List<Pawn> allPawns, List<Pawn> nonAssignablePawns, Dictionary<string, Pawn> forcedRoles = null, List<Pawn> requiredPawns = null, Pawn selectedPawn = null)
		{
			this.allPawns = allPawns;
			this.nonAssignablePawns = nonAssignablePawns;
			this.forcedRoles = forcedRoles;
			this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			this.selectedPawn = selectedPawn;
			this.requiredPawns = new List<Pawn>();
			if (forcedRoles != null)
			{
				this.requiredPawns.AddRange(forcedRoles.Values);
			}
			if (requiredPawns != null)
			{
				this.requiredPawns.AddRange(requiredPawns);
			}
			allPawns.SortBy((Pawn p) => p.Faction == null || !p.Faction.IsPlayer, (Pawn p) => !Faction.OfPlayer.ideos.Has(p.Ideo), (Pawn p) => !p.IsFreeNonSlaveColonist);
		}

		// Token: 0x06009335 RID: 37685 RVA: 0x0035BCC4 File Offset: 0x00359EC4
		public bool Forced(Pawn pawn)
		{
			return this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn);
		}

		// Token: 0x06009336 RID: 37686 RVA: 0x0035BCDC File Offset: 0x00359EDC
		public RitualRole ForcedRole(Pawn pawn)
		{
			if (this.forcedRoles == null)
			{
				return null;
			}
			string text = null;
			foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
			{
				if (keyValuePair.Value == pawn)
				{
					text = keyValuePair.Key;
				}
			}
			if (text == null)
			{
				return null;
			}
			foreach (RitualRole ritualRole in this.allRoles)
			{
				if (ritualRole.id == text)
				{
					return ritualRole;
				}
			}
			return null;
		}

		// Token: 0x06009337 RID: 37687 RVA: 0x0035BDA0 File Offset: 0x00359FA0
		public void RemoveParticipant(Pawn pawn)
		{
			this.TryUnassignAnyRole(pawn);
			this.spectators.Remove(pawn);
			this.allPawns.Remove(pawn);
			this.allPawns.Add(pawn);
		}

		// Token: 0x06009338 RID: 37688 RVA: 0x0035BDD0 File Offset: 0x00359FD0
		public bool TryUnassignAnyRole(Pawn pawn)
		{
			foreach (KeyValuePair<string, List<Pawn>> keyValuePair in this.assignedRoles)
			{
				if (keyValuePair.Value.Remove(pawn))
				{
					if (this.CanEverSpectate(pawn))
					{
						this.spectators.Add(pawn);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009339 RID: 37689 RVA: 0x0035BE48 File Offset: 0x0035A048
		public bool TryAssign(Pawn pawn, RitualRole role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null)
		{
			reason = PsychicRitualRoleDef.Reason.None;
			if (this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn))
			{
				return false;
			}
			bool flag;
			if (this.PawnNotAssignableReason(pawn, role, out flag) != null)
			{
				return false;
			}
			if (role == null)
			{
				return this.TryAssignSpectate(pawn, null);
			}
			string text;
			if (role.AppliesToPawn(pawn, out text, this.ritualTarget, null, this, null, true) && (role.maxCount <= 0 || this.AssignedPawns(role).Count<Pawn>() < role.maxCount))
			{
				this.TryUnassignAnyRole(pawn);
				this.spectators.Remove(pawn);
				List<Pawn> list;
				if (!this.assignedRoles.TryGetValue(role.id, out list))
				{
					list = new List<Pawn>();
					this.assignedRoles.Add(role.id, list);
				}
				if (insertBefore != null && list.Contains(insertBefore))
				{
					list.Insert(list.IndexOf(insertBefore), pawn);
				}
				else
				{
					list.Add(pawn);
				}
				if (role is RitualRoleIdeoRoleChanger)
				{
					this.UpdateRoleChangeTargetRole(pawn);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600933A RID: 37690 RVA: 0x0035BF44 File Offset: 0x0035A144
		public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
		{
			bool flag;
			if (this.spectators.Contains(pawn) || !this.CanEverSpectate(pawn) || this.PawnNotAssignableReason(pawn, null, out flag) != null)
			{
				return false;
			}
			this.TryUnassignAnyRole(pawn);
			if (!this.spectators.Contains(pawn))
			{
				if (insertBefore != null && this.spectators.Contains(insertBefore))
				{
					this.spectators.Insert(this.spectators.IndexOf(insertBefore), pawn);
				}
				else
				{
					this.spectators.Add(pawn);
				}
			}
			return this.RoleForPawn(pawn, true) == null;
		}

		// Token: 0x0600933B RID: 37691 RVA: 0x0035BFD0 File Offset: 0x0035A1D0
		public RitualRole GetRole(string roleId)
		{
			if (!this.AllRolesForReading.NullOrEmpty<RitualRole>())
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					if (ritualRole.id == roleId)
					{
						return ritualRole;
					}
				}
			}
			return null;
		}

		// Token: 0x0600933C RID: 37692 RVA: 0x0035C040 File Offset: 0x0035A240
		public bool CanParticipate(Pawn pawn, out TaggedString reason)
		{
			if (this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			if (this.Required(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			if (pawn == this.selectedPawn)
			{
				reason = TaggedString.Empty;
				return true;
			}
			foreach (RitualRole role in this.AllRolesForReading)
			{
				bool flag;
				if (this.PawnNotAssignableReason(pawn, role, out flag) == null)
				{
					reason = TaggedString.Empty;
					return true;
				}
			}
			if (this.CanEverSpectate(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			reason = this.PawnNotAssignableReason(pawn, null);
			return false;
		}

		// Token: 0x0600933D RID: 37693 RVA: 0x0035C124 File Offset: 0x0035A324
		public Pawn FirstAssignedPawn(RitualRole role)
		{
			return this.FirstAssignedPawn(role.id);
		}

		// Token: 0x0600933E RID: 37694 RVA: 0x0035C134 File Offset: 0x0035A334
		public Pawn FirstAssignedPawn(string roleId)
		{
			Pawn result;
			if (this.forcedRoles != null && this.forcedRoles.TryGetValue(roleId, out result))
			{
				return result;
			}
			List<Pawn> list;
			if (this.assignedRoles.TryGetValue(roleId, out list) && list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x0600933F RID: 37695 RVA: 0x0035C180 File Offset: 0x0035A380
		public bool CanEverSpectate(Pawn pawn)
		{
			if (this.ritual != null && this.ritual.ritualOnlyForIdeoMembers && pawn.Ideo != this.ritual.ideo && !this.ritual.def.allowSpectatorsFromOtherIdeos)
			{
				return false;
			}
			if (this.ritual != null && this.ritual.behavior.def.spectatorFilter != null && !this.ritual.behavior.def.spectatorFilter.Allowed(pawn))
			{
				return false;
			}
			string text;
			if (this.ritual != null && !this.ritual.behavior.PawnCanFillRole(pawn, null, out text, this.ritualTarget))
			{
				return false;
			}
			if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
			{
				Precept_Ritual precept_Ritual = this.ritual;
				Precept_Ritual precept_Ritual2 = this.ritual;
				return GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, precept_Ritual, precept_Ritual2 != null && precept_Ritual2.behavior.def.spectatorsIgnoreBleeding);
			}
			return false;
		}

		// Token: 0x06009340 RID: 37696 RVA: 0x0035C26C File Offset: 0x0035A46C
		public IEnumerable<Pawn> SpectatorCandidates()
		{
			RitualRoleAssignments.<SpectatorCandidates>d__51 <SpectatorCandidates>d__ = new RitualRoleAssignments.<SpectatorCandidates>d__51(-2);
			<SpectatorCandidates>d__.<>4__this = this;
			return <SpectatorCandidates>d__;
		}

		// Token: 0x06009341 RID: 37697 RVA: 0x0035C27C File Offset: 0x0035A47C
		public IEnumerable<Pawn> CandidatesForRole(string roleId, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			return this.CandidatesForRole(this.GetRole(roleId), ritualTarget, includeAssigned, includeAssignedForSameRole, includeForced);
		}

		// Token: 0x06009342 RID: 37698 RVA: 0x0035C291 File Offset: 0x0035A491
		public IEnumerable<Pawn> CandidatesForRole(RitualRole role, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			RitualRoleAssignments.<CandidatesForRole>d__53 <CandidatesForRole>d__ = new RitualRoleAssignments.<CandidatesForRole>d__53(-2);
			<CandidatesForRole>d__.<>4__this = this;
			<CandidatesForRole>d__.<>3__role = role;
			<CandidatesForRole>d__.<>3__ritualTarget = ritualTarget;
			<CandidatesForRole>d__.<>3__includeAssigned = includeAssigned;
			<CandidatesForRole>d__.<>3__includeAssignedForSameRole = includeAssignedForSameRole;
			return <CandidatesForRole>d__;
		}

		// Token: 0x06009343 RID: 37699 RVA: 0x0035C2BE File Offset: 0x0035A4BE
		public IEnumerable<Pawn> AssignedPawns(RitualRole role)
		{
			RitualRoleAssignments.<AssignedPawns>d__54 <AssignedPawns>d__ = new RitualRoleAssignments.<AssignedPawns>d__54(-2);
			<AssignedPawns>d__.<>4__this = this;
			<AssignedPawns>d__.<>3__role = role;
			return <AssignedPawns>d__;
		}

		// Token: 0x06009344 RID: 37700 RVA: 0x0035C2D5 File Offset: 0x0035A4D5
		public bool AnyPawnAssigned(string roleId)
		{
			return (this.forcedRoles != null && this.forcedRoles.ContainsKey(roleId)) || (this.assignedRoles.ContainsKey(roleId) && this.assignedRoles[roleId].Any<Pawn>());
		}

		// Token: 0x06009345 RID: 37701 RVA: 0x0035C310 File Offset: 0x0035A510
		public bool AnyPawnAssigned(RitualRole role)
		{
			return this.AnyPawnAssigned(role.id);
		}

		// Token: 0x06009346 RID: 37702 RVA: 0x0035C31E File Offset: 0x0035A51E
		public IEnumerable<Pawn> AssignedPawns(string roleId)
		{
			return this.AssignedPawns(this.GetRole(roleId));
		}

		// Token: 0x06009347 RID: 37703 RVA: 0x0035C330 File Offset: 0x0035A530
		public RitualRole RoleForPawn(Pawn pawn, bool includeForced = true)
		{
			if (this.spectators.Contains(pawn))
			{
				return null;
			}
			if (includeForced && this.forcedRoles != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
				{
					if (keyValuePair.Value == pawn)
					{
						return this.GetRole(keyValuePair.Key);
					}
				}
			}
			foreach (KeyValuePair<string, List<Pawn>> keyValuePair2 in this.assignedRoles)
			{
				if (!keyValuePair2.Value.NullOrEmpty<Pawn>() && keyValuePair2.Value.Contains(pawn))
				{
					return this.GetRole(keyValuePair2.Key);
				}
			}
			return null;
		}

		// Token: 0x06009348 RID: 37704 RVA: 0x0035C420 File Offset: 0x0035A620
		public bool PawnParticipating(Pawn pawn)
		{
			return this.RoleForPawn(pawn, true) != null || this.PawnSpectating(pawn);
		}

		// Token: 0x06009349 RID: 37705 RVA: 0x0035C435 File Offset: 0x0035A635
		public bool PawnSpectating(Pawn pawn)
		{
			return this.spectators.Contains(pawn);
		}

		// Token: 0x0600934A RID: 37706 RVA: 0x0035C444 File Offset: 0x0035A644
		public void FillPawns(Dialog_BeginRitual.PawnFilter filter, TargetInfo ritualTarget)
		{
			if (!this.requiredPawns.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn5 in this.requiredPawns)
				{
					if (this.forcedRoles == null || !this.forcedRoles.ContainsValue(pawn5))
					{
						this.TryAssignSpectate(pawn5, null);
					}
				}
			}
			if (this.selectedPawn != null && this.RoleForPawn(this.selectedPawn, true) == null)
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					string text;
					if (ritualRole.defaultForSelectedColonist && ritualRole.AppliesToPawn(this.selectedPawn, out text, ritualTarget, null, this, null, false))
					{
						PsychicRitualRoleDef.Reason reason;
						this.TryAssign(this.selectedPawn, ritualRole, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null);
						break;
					}
				}
			}
			using (List<RitualRole>.Enumerator enumerator2 = this.AllRolesForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RitualRole role = enumerator2.Current;
					RitualRoleAssignments.tmpOrderedPawns.Clear();
					RitualRoleAssignments.tmpOrderedPawns.AddRange(from pawn in this.allPawns
					where filter == null || filter(pawn, !(role is RitualRoleForced), role.allowOtherIdeos)
					select pawn);
					role.OrderByDesirability(RitualRoleAssignments.tmpOrderedPawns);
					foreach (Pawn pawn2 in RitualRoleAssignments.tmpOrderedPawns)
					{
						if (this.RoleForPawn(pawn2, true) == null)
						{
							if (role.maxCount > 0 && this.AssignedPawns(role).Count<Pawn>() >= role.maxCount)
							{
								break;
							}
							string text;
							if (role.AppliesToPawn(pawn2, out text, ritualTarget, null, this, null, true))
							{
								PsychicRitualRoleDef.Reason reason;
								this.TryAssign(pawn2, role, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null);
							}
						}
					}
				}
			}
			foreach (Pawn pawn3 in this.SpectatorCandidates())
			{
				this.TryAssignSpectate(pawn3, null);
			}
			List<Pawn> pawnsToRemove = new List<Pawn>();
			foreach (Pawn pawn4 in this.allPawns)
			{
				RitualRole ritualRole2 = this.RoleForPawn(pawn4, true);
				bool flag;
				if (ritualRole2 != null && ritualRole2.required && ritualRole2.substitutable && this.PawnNotAssignableReason(pawn4, ritualRole2, out flag) != null)
				{
					this.RemoveParticipant(pawn4);
					pawnsToRemove.Add(pawn4);
				}
			}
			this.allPawns.RemoveAll((Pawn p) => pawnsToRemove.Contains(p));
		}

		// Token: 0x0600934B RID: 37707 RVA: 0x0035C7B8 File Offset: 0x0035A9B8
		public bool Required(Pawn pawn)
		{
			return this.requiredPawns.NotNullAndContains(pawn);
		}

		// Token: 0x0600934C RID: 37708 RVA: 0x0035C7C8 File Offset: 0x0035A9C8
		public bool RoleSubstituted(string roleId)
		{
			if (this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return false;
			}
			RitualRole role = this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId);
			if (role == null)
			{
				return false;
			}
			if (!role.substitutable || role.precept == null)
			{
				return false;
			}
			Precept precept = this.ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
			if (precept == null)
			{
				return false;
			}
			bool result = false;
			foreach (Pawn pawn in this.AssignedPawns(roleId))
			{
				Ideo ideo = pawn.Ideo;
				if (((ideo != null) ? ideo.GetRole(pawn) : null) != precept)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600934D RID: 37709 RVA: 0x0035C8D8 File Offset: 0x0035AAD8
		public string PawnNotAssignableReason(Pawn p, RitualRole role)
		{
			bool flag;
			return this.PawnNotAssignableReason(p, role, out flag);
		}

		// Token: 0x0600934E RID: 37710 RVA: 0x0035C8EF File Offset: 0x0035AAEF
		public string PawnNotAssignableReason(Pawn p, RitualRole role, out bool stillAddToPawnList)
		{
			return RitualRoleAssignments.PawnNotAssignableReason(p, role, this.ritual, this, this.ritualTarget, out stillAddToPawnList);
		}

		// Token: 0x0600934F RID: 37711 RVA: 0x0035C908 File Offset: 0x0035AB08
		public static string PawnNotAssignableReason(Pawn p, RitualRole role, Precept_Ritual ritual, RitualRoleAssignments assignments, TargetInfo ritualTarget, out bool stillAddToPawnList)
		{
			stillAddToPawnList = false;
			if (p.Downed && (role == null || !role.allowDowned))
			{
				return "MessageRitualPawnDowned".Translate(p);
			}
			bool flag = (role != null) ? role.ignoreBleeding : (ritual != null && ritual.behavior.def.spectatorsIgnoreBleeding);
			if (p.health.hediffSet.BleedRateTotal > 0f && !flag)
			{
				return "MessageRitualPawnInjured".Translate(p);
			}
			if (p.InMentalState && (role == null || !role.allowNonAggroMentalState || p.InAggroMentalState))
			{
				return "MessageRitualPawnMentalState".Translate(p);
			}
			if (p.IsPrisoner && role == null)
			{
				string key = "MessageRitualRoleMustNotBePrisonerToSpectate";
				string text;
				if (ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = ritual.behavior;
					text = ((behavior != null) ? behavior.def.spectatorGerund : null);
				}
				return key.Translate(text ?? "Spectate".Translate()).CapitalizeFirst();
			}
			if (ModsConfig.BiotechActive && role != null && !role.allowBaby && (p.DevelopmentalStage == DevelopmentalStage.Baby || p.DevelopmentalStage == DevelopmentalStage.Newborn))
			{
				return "MessageRitualRoleCannotBeBaby".Translate(role.LabelCap);
			}
			if (ModsConfig.AnomalyActive && p.IsSubhuman)
			{
				return "MessageRitualCannotBeMutant".Translate();
			}
			Lord lord = p.GetLord();
			LordJob lordJob = (lord != null) ? lord.LordJob : null;
			if (lordJob != null && !(lordJob is LordJob_VoluntarilyJoinable) && assignments != null)
			{
				List<Pawn> list = assignments.requiredPawns;
				bool? flag2 = (list != null) ? new bool?(list.Contains(p)) : null;
				bool flag3 = false;
				if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
				{
					stillAddToPawnList = true;
					return "MessageRitualRoleBusy".Translate(p);
				}
			}
			if (p.IsPrisoner)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (!p.guest.PrisonerIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnPrisonerNotSecured".Translate(p);
				}
			}
			else if (ritualTarget.IsValid && (ritual == null || !ritual.ignoreExtremeTemperatures) && !p.SafeTemperatureRange().IncludesEpsilon(ritualTarget.Cell.GetTemperature(ritualTarget.Map)))
			{
				return "MessageRitualWontAttendExtremeTemperature".Translate(p);
			}
			if (p.IsSlave)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (p.InMentalState && p.MentalStateDef != MentalStateDefOf.Rebellion)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnMentalState".Translate(p);
				}
				if (!p.guest.SlaveIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnSlaveNotSecured".Translate(p);
				}
			}
			if (role == null && !p.RaceProps.Humanlike)
			{
				return "MessageRitualRoleMustBeHumanlike".Translate("Spectators".Translate());
			}
			if (role == null && ritual != null && !ritual.def.allowSpectatorsFromOtherIdeos && ritual.ritualOnlyForIdeoMembers && p.Ideo != ritual.ideo)
			{
				string key2 = "MessageRitualRoleMustHaveIdeoToSpectate";
				NamedArgument arg = ritual.ideo.MemberNamePlural;
				string text2;
				if (ritual == null)
				{
					text2 = null;
				}
				else
				{
					RitualBehaviorWorker behavior2 = ritual.behavior;
					text2 = ((behavior2 != null) ? behavior2.def.spectatorGerund : null);
				}
				return key2.Translate(arg, text2 ?? "Spectate".Translate());
			}
			string result;
			if (ritual != null && !ritual.behavior.PawnCanFillRole(p, role, out result, ritualTarget))
			{
				return result;
			}
			if (role != null && !role.AppliesToPawn(p, out result, ritualTarget, null, assignments, null, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06009350 RID: 37712 RVA: 0x0035CCFC File Offset: 0x0035AEFC
		public void SetRoleChangeSelection(Precept_Role role)
		{
			this.roleChangeSelection = role;
		}

		// Token: 0x06009351 RID: 37713 RVA: 0x0035CD08 File Offset: 0x0035AF08
		public void UpdateRoleChangeTargetRole(Pawn pawn)
		{
			Precept_Role precept_Role = null;
			Ideo ideo = pawn.Ideo;
			if (((ideo != null) ? ideo.GetRole(pawn) : null) == null)
			{
				precept_Role = RitualUtility.AllRolesForPawn(pawn).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(pawn));
			}
			this.SetRoleChangeSelection(precept_Role);
		}

		// Token: 0x06009352 RID: 37714 RVA: 0x0035CD67 File Offset: 0x0035AF67
		public IEnumerable<IGrouping<string, RitualRole>> RoleGroups()
		{
			return from r in this.AllRolesForReading
			group r by r.mergeId ?? r.id;
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x0035CDA4 File Offset: 0x0035AFA4
		[CompilerGenerated]
		private bool <CandidatesForRole>g__ShouldIncludePawn|53_0(Pawn pawn, ref RitualRoleAssignments.<>c__DisplayClass53_0 A_2)
		{
			return (A_2.includeAssigned || (A_2.includeAssignedForSameRole && this.RoleForPawn(pawn, true) == A_2.role) || this.RoleForPawn(pawn, true) == null) && GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, this.ritual, A_2.role != null && A_2.role.ignoreBleeding);
		}

		// Token: 0x0400590E RID: 22798
		private List<Pawn> allPawns;

		// Token: 0x0400590F RID: 22799
		private List<Pawn> nonAssignablePawns;

		// Token: 0x04005910 RID: 22800
		private List<RitualRole> allRoles;

		// Token: 0x04005911 RID: 22801
		private Dictionary<string, Pawn> forcedRoles;

		// Token: 0x04005912 RID: 22802
		private Dictionary<string, List<Pawn>> assignedRoles = new Dictionary<string, List<Pawn>>();

		// Token: 0x04005913 RID: 22803
		private Precept_Role roleChangeSelection;

		// Token: 0x04005914 RID: 22804
		private List<Pawn> spectators = new List<Pawn>();

		// Token: 0x04005915 RID: 22805
		private List<Pawn> requiredPawns;

		// Token: 0x04005916 RID: 22806
		private Precept_Ritual ritual;

		// Token: 0x04005917 RID: 22807
		private TargetInfo ritualTarget;

		// Token: 0x04005918 RID: 22808
		private Pawn selectedPawn;

		// Token: 0x04005919 RID: 22809
		private List<Pawn> tmpParticipants = new List<Pawn>();

		// Token: 0x0400591A RID: 22810
		private List<Pawn> tmpForcedRolePawns;

		// Token: 0x0400591B RID: 22811
		private List<string> tmpForcedRoleIds;

		// Token: 0x0400591C RID: 22812
		private List<string> tmpAssignedRoleIds;

		// Token: 0x0400591D RID: 22813
		private List<List<Pawn>> tmpAssignedRolePawns;

		// Token: 0x0400591E RID: 22814
		private static List<Pawn> tmpOrderedPawns = new List<Pawn>(32);
	}
}
