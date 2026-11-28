using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200091E RID: 2334
	public class PsychicRitualRoleAssignments : ILordJobAssignmentsManager<PsychicRitualRoleDef>, IExposable
	{
		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x0600473A RID: 18234 RVA: 0x001BF06C File Offset: 0x001BD26C
		public IEnumerable<Pawn> AllAssignedPawns
		{
			get
			{
				return this.pawnAssignments.Keys;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x001BF079 File Offset: 0x001BD279
		public int AssignedPawnCount
		{
			get
			{
				return this.pawnAssignments.Count;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x0600473C RID: 18236 RVA: 0x001BF086 File Offset: 0x001BD286
		public List<Pawn> SpectatorsForReading
		{
			get
			{
				return this.spectators;
			}
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x001BF08E File Offset: 0x001BD28E
		public bool PawnParticipating(Pawn pawn)
		{
			return this.pawnAssignments.ContainsKey(pawn);
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x001BF09C File Offset: 0x001BD29C
		public bool Required(Pawn pawn)
		{
			return this.ForcedRole(pawn) != null;
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x001BF0A8 File Offset: 0x001BD2A8
		public bool SpectatorsAllowed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004740 RID: 18240 RVA: 0x001BF0AB File Offset: 0x001BD2AB
		public IReadOnlyDictionary<PsychicRitualRoleDef, List<Pawn>> RoleAssignments
		{
			get
			{
				return this.roleAssignments;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004741 RID: 18241 RVA: 0x001BF0B3 File Offset: 0x001BD2B3
		public virtual TargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x001BF0BB File Offset: 0x001BD2BB
		protected PsychicRitualRoleAssignments()
		{
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001BF0C4 File Offset: 0x001BD2C4
		public PsychicRitualRoleAssignments(List<PsychicRitualRoleDef> roles, TargetInfo target, Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments = null)
		{
			this.target = target;
			int num = 0;
			this.roleAssignments = new Dictionary<PsychicRitualRoleDef, List<Pawn>>(roles.Count);
			foreach (PsychicRitualRoleDef psychicRitualRoleDef in roles)
			{
				num += psychicRitualRoleDef.MaxCount;
				this.roleAssignments[psychicRitualRoleDef] = new List<Pawn>(psychicRitualRoleDef.MaxCount);
			}
			this.pawnAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(num);
			this.spectators = new List<Pawn>();
			if (forcedAssignments != null)
			{
				this.forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(forcedAssignments);
				foreach (KeyValuePair<Pawn, PsychicRitualRoleDef> keyValuePair in forcedAssignments)
				{
					Pawn pawn;
					PsychicRitualRoleDef psychicRitualRoleDef2;
					keyValuePair.Deconstruct(out pawn, out psychicRitualRoleDef2);
					Pawn pawn2 = pawn;
					PsychicRitualRoleDef psychicRitualRoleDef3 = psychicRitualRoleDef2;
					PsychicRitualRoleDef.Reason reason;
					if (!this.TryAssign(pawn2, psychicRitualRoleDef3, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Cannot assign pawn ");
						defaultInterpolatedStringHandler.AppendFormatted(pawn2.ToStringSafe<Pawn>());
						defaultInterpolatedStringHandler.AppendLiteral(" to forced role ");
						defaultInterpolatedStringHandler.AppendFormatted(psychicRitualRoleDef3.ToStringSafe<PsychicRitualRoleDef>());
						defaultInterpolatedStringHandler.AppendLiteral(". Reason: ");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(reason.ToPlayerReadable());
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x001BF23C File Offset: 0x001BD43C
		public void AddForcedRole(Pawn pawn, PsychicRitualRoleDef role, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
		{
			if (this.forcedAssignments == null)
			{
				this.forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>();
			}
			this.forcedAssignments[pawn] = role;
			PsychicRitualRoleDef.Reason reason;
			if (!this.TryAssign(pawn, role, out reason, context, null))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot force pawn ");
				defaultInterpolatedStringHandler.AppendFormatted(pawn.ToStringSafe<Pawn>());
				defaultInterpolatedStringHandler.AppendLiteral(" to have role ");
				defaultInterpolatedStringHandler.AppendFormatted(role.ToStringSafe<PsychicRitualRoleDef>());
				defaultInterpolatedStringHandler.AppendLiteral(". Reason: ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(reason.ToPlayerReadable());
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x001BF2DC File Offset: 0x001BD4DC
		public virtual bool CanParticipate(Pawn pawn, out TaggedString reason)
		{
			reason = TaggedString.Empty;
			using (Dictionary<PsychicRitualRoleDef, List<Pawn>>.KeyCollection.Enumerator enumerator = this.roleAssignments.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PsychicRitualRoleDef.Reason reason2;
					if (enumerator.Current.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.target, out reason2))
					{
						reason = TaggedString.Empty;
						return true;
					}
					if (reason.NullOrEmpty())
					{
						reason = reason2.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
					}
				}
			}
			return false;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x001BF380 File Offset: 0x001BD580
		public virtual string PawnNotAssignableReason(Pawn pawn, PsychicRitualRoleDef role)
		{
			if (role != null)
			{
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
				PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
				if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
				{
					return "RoleIsLocked".Translate(role.label);
				}
				PsychicRitualRoleDef.Reason reason;
				if (!role.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.target, out reason))
				{
					return reason.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
				}
				return null;
			}
			else
			{
				TaggedString taggedString;
				if (this.CanParticipate(pawn, out taggedString))
				{
					return null;
				}
				return taggedString;
			}
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x001BF40C File Offset: 0x001BD60C
		public int RoleAssignedCount(PsychicRitualRoleDef role)
		{
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x001BF431 File Offset: 0x001BD631
		public bool PawnSpectating(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x001BF434 File Offset: 0x001BD634
		public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
		{
			PsychicRitualRoleDef.Reason reason;
			return !this.IsChanter(pawn) && (this.TryAssign(pawn, PsychicRitualRoleDefOf.ChanterAdvanced, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore) || this.TryAssign(pawn, PsychicRitualRoleDefOf.Chanter, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore));
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x001BF470 File Offset: 0x001BD670
		public PsychicRitualRoleDef ForcedRole(Pawn pawn)
		{
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			if (dictionary == null)
			{
				return null;
			}
			return dictionary.TryGetValue(pawn, null);
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x001BF488 File Offset: 0x001BD688
		public bool IsChanter(Pawn pawn)
		{
			return (this.roleAssignments.ContainsKey(PsychicRitualRoleDefOf.ChanterAdvanced) && this.roleAssignments[PsychicRitualRoleDefOf.ChanterAdvanced].Contains(pawn)) || (this.roleAssignments.ContainsKey(PsychicRitualRoleDefOf.Chanter) && this.roleAssignments[PsychicRitualRoleDefOf.Chanter].Contains(pawn));
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x001BF4EE File Offset: 0x001BD6EE
		public IEnumerable<IGrouping<string, PsychicRitualRoleDef>> RoleGroups()
		{
			return from role in this.roleAssignments.Keys
			group role by role.Label;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x001BF520 File Offset: 0x001BD720
		public IEnumerable<Pawn> AssignedPawns(PsychicRitualRoleDef role)
		{
			IEnumerable<Pawn> enumerable = this.roleAssignments.TryGetValue(role, null);
			return enumerable ?? Enumerable.Empty<Pawn>();
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x001BF548 File Offset: 0x001BD748
		public PsychicRitualRoleDef RoleForPawn(Pawn pawn, bool includeForced = true)
		{
			PsychicRitualRoleDef result;
			if (this.pawnAssignments.TryGetValue(pawn, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x001BF568 File Offset: 0x001BD768
		public Pawn FirstAssignedPawn(PsychicRitualRoleDef role)
		{
			if (role == null)
			{
				Log.Error("Tried to get first assigned pawn for null role.");
				return null;
			}
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x001BF5A8 File Offset: 0x001BD7A8
		public bool TryUnassignAnyRole(Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef;
			if (!this.pawnAssignments.TryGetValue(pawn, out psychicRitualRoleDef))
			{
				return false;
			}
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			PsychicRitualRoleDef psychicRitualRoleDef2 = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
			if (psychicRitualRoleDef == psychicRitualRoleDef2)
			{
				return false;
			}
			this.pawnAssignments.Remove(pawn);
			this.roleAssignments[psychicRitualRoleDef].Remove(pawn);
			return true;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x001BF603 File Offset: 0x001BD803
		public void RemoveParticipant(Pawn pawn)
		{
			this.TryUnassignAnyRole(pawn);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x001BF60D File Offset: 0x001BD80D
		public void RemoveAllParticipantsAndTargets()
		{
			this.pawnAssignments.Clear();
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			this.roleAssignments.ClearValueLists<PsychicRitualRoleDef, Pawn>();
			this.spectators.Clear();
			this.target = TargetInfo.Invalid;
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x001BF64C File Offset: 0x001BD84C
		public void TryAssignAnyUnassigned(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				if (!this.PawnParticipating(pawn))
				{
					Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
					PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
					PsychicRitualRoleDef.Reason reason;
					if (psychicRitualRoleDef != null && this.TryAssign(pawn, psychicRitualRoleDef, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
					{
						break;
					}
					foreach (PsychicRitualRoleDef role in this.roleAssignments.Keys)
					{
						if (this.TryAssign(pawn, role, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x001BF720 File Offset: 0x001BD920
		public bool TryAssign(Pawn pawn, PsychicRitualRoleDef role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null)
		{
			reason = PsychicRitualRoleDef.Reason.None;
			if (role == null)
			{
				return false;
			}
			if (pawn == null)
			{
				return false;
			}
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
			if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
			{
				return false;
			}
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return false;
			}
			if (!role.PawnCanDo(context, pawn, this.target, out reason))
			{
				return false;
			}
			if (list.Count >= role.MaxCount)
			{
				return false;
			}
			this.TryUnassignAnyRole(pawn);
			if (insertBefore != null)
			{
				list.Insert(list.IndexOf(insertBefore), pawn);
			}
			else
			{
				list.Add(pawn);
			}
			this.pawnAssignments[pawn] = role;
			return true;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x001BF7C8 File Offset: 0x001BD9C8
		public void SetTarget(TargetInfo target)
		{
			this.target = target;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x001BF7D1 File Offset: 0x001BD9D1
		public IEnumerable<string> BlockingIssues()
		{
			PsychicRitualRoleAssignments.<BlockingIssues>d__39 <BlockingIssues>d__ = new PsychicRitualRoleAssignments.<BlockingIssues>d__39(-2);
			<BlockingIssues>d__.<>4__this = this;
			return <BlockingIssues>d__;
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x001BF7E4 File Offset: 0x001BD9E4
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.roleAssignments != null)
				{
					foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in this.roleAssignments)
					{
						List<Pawn> value = keyValuePair.Value;
						if (value != null)
						{
							value.RemoveAll((Pawn pawn) => pawn == null || pawn.Discarded);
						}
					}
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.pawnAssignments;
				if (dictionary != null)
				{
					dictionary.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary2 = this.forcedAssignments;
				if (dictionary2 != null)
				{
					dictionary2.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
				}
			}
			Scribe_Collections.Look<PsychicRitualRoleDef, List<Pawn>>(ref this.roleAssignments, "roleAssignments", LookMode.Def, LookMode.Reference, ref this.roleAssignmentsKeys, ref this.roleAssignmentsValues, true, false, true);
			Scribe_Collections.Look<Pawn, PsychicRitualRoleDef>(ref this.pawnAssignments, "pawnAssignments", LookMode.Reference, LookMode.Def, ref this.pawnAssignmentsKeys, ref this.pawnAssignmentsValues, false, true, false);
			Scribe_Collections.Look<Pawn, PsychicRitualRoleDef>(ref this.forcedAssignments, "forcedAssignments", LookMode.Reference, LookMode.Def, ref this.forcedAssignmentKeys, ref this.forcedAssignmentValues, false, true, false);
			Scribe_Collections.Look<Pawn>(ref this.spectators, "spectators", true, LookMode.Reference, Array.Empty<object>());
			Scribe_TargetInfo.Look(ref this.target, true, "target");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.roleAssignments != null)
				{
					foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair2 in this.roleAssignments)
					{
						List<Pawn> value2 = keyValuePair2.Value;
						if (value2 != null)
						{
							value2.RemoveAll((Pawn pawn) => pawn == null);
						}
					}
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary3 = this.pawnAssignments;
				if (dictionary3 != null)
				{
					dictionary3.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary4 = this.forcedAssignments;
				if (dictionary4 == null)
				{
					return;
				}
				dictionary4.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
			}
		}

		// Token: 0x04002E3C RID: 11836
		protected Dictionary<PsychicRitualRoleDef, List<Pawn>> roleAssignments;

		// Token: 0x04002E3D RID: 11837
		protected Dictionary<Pawn, PsychicRitualRoleDef> pawnAssignments;

		// Token: 0x04002E3E RID: 11838
		protected List<Pawn> spectators;

		// Token: 0x04002E3F RID: 11839
		protected TargetInfo target;

		// Token: 0x04002E40 RID: 11840
		protected Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments;

		// Token: 0x04002E41 RID: 11841
		private List<PsychicRitualRoleDef> roleAssignmentsKeys;

		// Token: 0x04002E42 RID: 11842
		private List<List<Pawn>> roleAssignmentsValues;

		// Token: 0x04002E43 RID: 11843
		private List<Pawn> pawnAssignmentsKeys;

		// Token: 0x04002E44 RID: 11844
		private List<PsychicRitualRoleDef> pawnAssignmentsValues;

		// Token: 0x04002E45 RID: 11845
		private List<Pawn> forcedAssignmentKeys;

		// Token: 0x04002E46 RID: 11846
		private List<PsychicRitualRoleDef> forcedAssignmentValues;
	}
}
