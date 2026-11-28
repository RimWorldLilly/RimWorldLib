using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CA RID: 5578
	public class Pawn_RelationsTracker : IExposable
	{
		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x06008A42 RID: 35394 RVA: 0x00320E4B File Offset: 0x0031F04B
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x06008A43 RID: 35395 RVA: 0x00320E53 File Offset: 0x0031F053
		public List<VirtualPawnRelation> VirtualRelations
		{
			get
			{
				return this.virtualRelations;
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x06008A44 RID: 35396 RVA: 0x00320E5B File Offset: 0x0031F05B
		public IEnumerable<Pawn> Children
		{
			get
			{
				Pawn_RelationsTracker.<get_Children>d__27 <get_Children>d__ = new Pawn_RelationsTracker.<get_Children>d__27(-2);
				<get_Children>d__.<>4__this = this;
				return <get_Children>d__;
			}
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x06008A45 RID: 35397 RVA: 0x00320E6B File Offset: 0x0031F06B
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x00320E78 File Offset: 0x0031F078
		public bool IsTryRomanceOnCooldown
		{
			get
			{
				return this.romanceEnableTick > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06008A47 RID: 35399 RVA: 0x00320E8C File Offset: 0x0031F08C
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.virtualRelations.Any<VirtualPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06008A48 RID: 35400 RVA: 0x00320EB8 File Offset: 0x0031F0B8
		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						foreach (Pawn item in this.FamilyByBlood_Internal)
						{
							this.cachedFamilyByBlood.Add(item);
						}
						this.familyByBloodIsCached = true;
					}
					return this.cachedFamilyByBlood;
				}
				return this.FamilyByBlood_Internal;
			}
		}

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x06008A49 RID: 35401 RVA: 0x00320F3C File Offset: 0x0031F13C
		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				Pawn_RelationsTracker.<get_FamilyByBlood_Internal>d__37 <get_FamilyByBlood_Internal>d__ = new Pawn_RelationsTracker.<get_FamilyByBlood_Internal>d__37(-2);
				<get_FamilyByBlood_Internal>d__.<>4__this = this;
				return <get_FamilyByBlood_Internal>d__;
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x06008A4A RID: 35402 RVA: 0x00320F4C File Offset: 0x0031F14C
		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				Pawn_RelationsTracker.<get_PotentiallyRelatedPawns>d__39 <get_PotentiallyRelatedPawns>d__ = new Pawn_RelationsTracker.<get_PotentiallyRelatedPawns>d__39(-2);
				<get_PotentiallyRelatedPawns>d__.<>4__this = this;
				return <get_PotentiallyRelatedPawns>d__;
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06008A4B RID: 35403 RVA: 0x00320F5C File Offset: 0x0031F15C
		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				Pawn_RelationsTracker.<get_RelatedPawns>d__41 <get_RelatedPawns>d__ = new Pawn_RelationsTracker.<get_RelatedPawns>d__41(-2);
				<get_RelatedPawns>d__.<>4__this = this;
				return <get_RelatedPawns>d__;
			}
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00320F6C File Offset: 0x0031F16C
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x00320FE4 File Offset: 0x0031F1E4
		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<VirtualPawnRelation>(ref this.virtualRelations, "virtualRelations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.virtualRelations == null)
				{
					this.virtualRelations = new List<VirtualPawnRelation>();
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						string[] array = new string[5];
						array[0] = "Pawn ";
						int num = 1;
						Pawn pawn = this.pawn;
						array[num] = ((pawn != null) ? pawn.ToString() : null);
						array[2] = " has relation \"";
						array[3] = this.directRelations[i].def.defName;
						array[4] = "\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships).";
						Log.Warning(string.Concat(array));
					}
				}
				this.directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					this.directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				}
				if (this.pregnancyApproaches == null)
				{
					this.pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();
				}
				this.pregnancyApproaches.RemoveAll((KeyValuePair<Pawn, PregnancyApproach> x) => x.Key == null);
				for (int k = this.virtualRelations.Count - 1; k >= 0; k--)
				{
					if (this.virtualRelations[k].record == null)
					{
						this.virtualRelations.RemoveAt(k);
					}
				}
			}
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", true, false);
			Scribe_Values.Look<bool>(ref this.canGetRescuedThought, "canGetRescuedThought", true, false);
			Scribe_Values.Look<MarriageNameChange>(ref this.nextMarriageNameChange, "nextMarriageNameChange", MarriageNameChange.NoChange, false);
			Scribe_References.Look<Pawn>(ref this.relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest", false);
			Scribe_Values.Look<bool>(ref this.hidePawnRelations, "hidePawnRelations", false, false);
			Scribe_Collections.Look<Pawn, PregnancyApproach>(ref this.pregnancyApproaches, "pregnancyApproaches", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpPregnancyApproaches, false, false, false);
			Scribe_Values.Look<int>(ref this.romanceEnableTick, "romanceEnableTick", 0, false);
		}

		// Token: 0x06008A4E RID: 35406 RVA: 0x00321234 File Offset: 0x0031F434
		public PregnancyApproach GetPregnancyApproachForPartner(Pawn partner)
		{
			if (!this.pregnancyApproaches.ContainsKey(partner))
			{
				PregnancyApproach value;
				if (partner.relations.pregnancyApproaches.TryGetValue(this.pawn, out value))
				{
					this.pregnancyApproaches.Add(partner, value);
				}
				else
				{
					this.SetPregnancyApproach(partner, PregnancyApproach.Normal);
				}
			}
			return this.pregnancyApproaches[partner];
		}

		// Token: 0x06008A4F RID: 35407 RVA: 0x0032128C File Offset: 0x0031F48C
		public void SetPregnancyApproach(Pawn partner, PregnancyApproach mode)
		{
			this.pregnancyApproaches[partner] = mode;
			partner.relations.pregnancyApproaches[this.pawn] = mode;
		}

		// Token: 0x06008A50 RID: 35408 RVA: 0x003212B2 File Offset: 0x0031F4B2
		public void RelationsTrackerTickInterval(int delta)
		{
			if (this.pawn.Dead)
			{
				return;
			}
			this.Tick_CheckStartMarriageCeremony(delta);
			this.Tick_CheckDevelopBondRelation(delta);
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x003212D0 File Offset: 0x0031F4D0
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return null;
			}
			foreach (DirectPawnRelation directPawnRelation in this.directRelations)
			{
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return directPawnRelation;
				}
			}
			return null;
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x0032135C File Offset: 0x0031F55C
		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return null;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x003213D4 File Offset: 0x0031F5D4
		public int GetDirectRelationsCount(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			int num = 0;
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return num;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008A54 RID: 35412 RVA: 0x0032144C File Offset: 0x0031F64C
		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008A55 RID: 35413 RVA: 0x003214B8 File Offset: 0x0031F6B8
		public bool VirtualRelationExists(PawnRelationDef def, RelationshipRecord record)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.virtualRelations.Count; i++)
			{
				VirtualPawnRelation virtualPawnRelation = this.virtualRelations[i];
				if (virtualPawnRelation.def == def && virtualPawnRelation.record.ID == record.ID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008A56 RID: 35414 RVA: 0x00321530 File Offset: 0x0031F730
		public void GetDirectRelations(PawnRelationDef def, ref List<Pawn> list, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(((def != null) ? def.ToString() : null) + " is not a direct relation.");
				return;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					list.Add(directPawnRelation.otherPawn);
				}
			}
		}

		// Token: 0x06008A57 RID: 35415 RVA: 0x003215AC File Offset: 0x0031F7AC
		public void ElevateToVirtualRelation(DirectPawnRelation relation)
		{
			RelationshipRecord orCreateRecord = Find.RelationshipRecords.GetOrCreateRecord(relation.otherPawn);
			this.virtualRelations.Add(new VirtualPawnRelation(relation.def, orCreateRecord, relation.startTicks));
			orCreateRecord.AddReference(this.pawn);
			this.directRelations.Remove(relation);
		}

		// Token: 0x06008A58 RID: 35416 RVA: 0x00321600 File Offset: 0x0031F800
		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				string[] array = new string[6];
				array[0] = "Tried to directly add implied pawn relation ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = ", pawn=";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				array[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array));
				return;
			}
			if (otherPawn == this.pawn)
			{
				string str = "Tried to add pawn relation ";
				string str2 = (def != null) ? def.ToString() : null;
				string str3 = " with self, pawn=";
				Pawn pawn2 = this.pawn;
				Log.Warning(str + str2 + str3 + ((pawn2 != null) ? pawn2.ToString() : null));
				return;
			}
			if (this.DirectRelationExists(def, otherPawn))
			{
				string[] array2 = new string[6];
				array2[0] = "Tried to add the same relation twice: ";
				array2[1] = ((def != null) ? def.ToString() : null);
				array2[2] = ", pawn=";
				int num2 = 3;
				Pawn pawn3 = this.pawn;
				array2[num2] = ((pawn3 != null) ? pawn3.ToString() : null);
				array2[4] = ", otherPawn=";
				array2[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array2));
				return;
			}
			int startTicks = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
			def.Worker.OnRelationCreated(this.pawn, otherPawn);
			this.directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
			otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
			if (def.reflexive)
			{
				otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, this.pawn, startTicks));
				this.pawnsWithDirectRelationsWithMe.Add(otherPawn);
			}
			this.GainedOrLostDirectRelation();
			otherPawn.relations.GainedOrLostDirectRelation();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (!this.pawn.Dead && this.pawn.health != null)
				{
					for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						this.pawn.health.hediffSet.hediffs[i].Notify_RelationAdded(otherPawn, def);
					}
				}
				if (!otherPawn.Dead && otherPawn.health != null)
				{
					for (int j = otherPawn.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
					{
						otherPawn.health.hediffSet.hediffs[j].Notify_RelationAdded(this.pawn, def);
					}
				}
			}
		}

		// Token: 0x06008A59 RID: 35417 RVA: 0x00321870 File Offset: 0x0031FA70
		public void RemoveRelation(RelationshipRecord record)
		{
			for (int i = this.virtualRelations.Count - 1; i >= 0; i--)
			{
				if (this.virtualRelations[i].record == record)
				{
					this.virtualRelations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x003218B5 File Offset: 0x0031FAB5
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x003218CC File Offset: 0x0031FACC
		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				string[] array = new string[6];
				array[0] = "Could not remove relation ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " because it's not here. pawn=";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				array[5] = ((otherPawn != null) ? otherPawn.ToString() : null);
				Log.Warning(string.Concat(array));
			}
		}

		// Token: 0x06008A5C RID: 35420 RVA: 0x00321940 File Offset: 0x0031FB40
		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				string[] array = new string[6];
				array[0] = "Tried to remove implied pawn relation ";
				int num = 1;
				PawnRelationDef def2 = def;
				array[num] = ((def2 != null) ? def2.ToString() : null);
				array[2] = ", pawn=";
				int num2 = 3;
				Pawn pawn = this.pawn;
				array[num2] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", otherPawn=";
				int num3 = 5;
				Pawn otherPawn2 = otherPawn;
				array[num3] = ((otherPawn2 != null) ? otherPawn2.ToString() : null);
				Log.Warning(string.Concat(array));
				return false;
			}
			Predicate<DirectPawnRelation> <>9__1;
			Predicate<DirectPawnRelation> <>9__2;
			Predicate<DirectPawnRelation> <>9__0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
				{
					if (def.reflexive)
					{
						List<DirectPawnRelation> list = otherPawn.relations.directRelations;
						Predicate<DirectPawnRelation> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn));
						}
						DirectPawnRelation directPawnRelation = list.Find(match);
						directPawnRelation.def.Worker.OnRelationRemoved(otherPawn, this.pawn);
						list.Remove(directPawnRelation);
						Predicate<DirectPawnRelation> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((DirectPawnRelation x) => x.otherPawn == this.pawn));
						}
						if (list.Find(match2) == null)
						{
							this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
						}
					}
					this.directRelations[i].def.Worker.OnRelationRemoved(this.pawn, otherPawn);
					this.directRelations.RemoveAt(i);
					List<DirectPawnRelation> list2 = this.directRelations;
					Predicate<DirectPawnRelation> match3;
					if ((match3 = <>9__0) == null)
					{
						match3 = (<>9__0 = ((DirectPawnRelation x) => x.otherPawn == otherPawn));
					}
					if (list2.Find(match3) == null)
					{
						otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(this.pawn);
					}
					this.GainedOrLostDirectRelation();
					otherPawn.relations.GainedOrLostDirectRelation();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008A5D RID: 35421 RVA: 0x00321B68 File Offset: 0x0031FD68
		public int OpinionOf(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return 0;
			}
			if (this.pawn.Dead)
			{
				return 0;
			}
			int num = 0;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
			{
				num += pawnRelationDef.opinionOffset;
			}
			if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				num += this.pawn.needs.mood.thoughts.TotalOpinionOffset(other);
			}
			if (num != 0)
			{
				float num2 = 1f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].CurStage != null)
					{
						num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
					}
				}
				num = Mathf.RoundToInt((float)num * num2);
			}
			if (num > 0 && this.pawn.HostileTo(other))
			{
				num = 0;
			}
			return Mathf.Clamp(num, -100, 100);
		}

		// Token: 0x06008A5E RID: 35422 RVA: 0x00321CAC File Offset: 0x0031FEAC
		public string OpinionExplanation(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("OpinionOf".Translate(other.LabelShort).Colorize(ColoredText.TipSectionTitleColor) + ": " + this.OpinionOf(other).ToStringWithSign());
			string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, this.pawn);
			if (!pawnSituationLabel.NullOrEmpty())
			{
				stringBuilder.AppendLine(" (" + pawnSituationLabel + ")");
			}
			else
			{
				stringBuilder.AppendLine();
			}
			bool flag = false;
			if (this.pawn.Dead)
			{
				stringBuilder.AppendLine(" - " + "IAmDead".Translate());
				flag = true;
			}
			else
			{
				foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
				{
					stringBuilder.AppendLine(" - " + pawnRelationDef.GetGenderSpecificLabelCap(other) + ": " + pawnRelationDef.opinionOffset.ToStringWithSign());
					flag = true;
				}
				if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
				{
					ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
					thoughts.GetDistinctSocialThoughtGroups(other, Pawn_RelationsTracker.tmpSocialThoughts);
					for (int i = 0; i < Pawn_RelationsTracker.tmpSocialThoughts.Count; i++)
					{
						ISocialThought socialThought = Pawn_RelationsTracker.tmpSocialThoughts[i];
						int num = 1;
						Thought thought = (Thought)socialThought;
						if (thought.def.IsMemory)
						{
							num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
						}
						stringBuilder.Append(" - " + thought.LabelCapSocial);
						if (num != 1)
						{
							stringBuilder.Append(" x" + num.ToString());
						}
						stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
						flag = true;
					}
				}
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int j = 0; j < hediffs.Count; j++)
				{
					HediffStage curStage = hediffs[j].CurStage;
					if (curStage != null && curStage.opinionOfOthersFactor != 1f)
					{
						stringBuilder.Append(" - " + hediffs[j].LabelBaseCap);
						if (curStage.opinionOfOthersFactor != 0f)
						{
							stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
						}
						else
						{
							stringBuilder.AppendLine();
						}
						flag = true;
					}
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine(" - " + "NoneBrackets".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06008A5F RID: 35423 RVA: 0x00321FC8 File Offset: 0x003201C8
		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			if (this.pawn.story != null && this.pawn.story.traits != null)
			{
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					return 0f;
				}
				if (!this.pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
				{
					if (this.pawn.story.traits.HasTrait(TraitDefOf.Gay))
					{
						if (otherPawn.gender != this.pawn.gender)
						{
							return 0f;
						}
					}
					else if (otherPawn.gender == this.pawn.gender)
					{
						return 0f;
					}
				}
			}
			if (this.pawn.ageTracker.AgeBiologicalYearsFloat < 16f || otherPawn.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				return 0f;
			}
			return this.LovinAgeFactor(otherPawn) * this.PrettinessFactor(otherPawn);
		}

		// Token: 0x06008A60 RID: 35424 RVA: 0x003220E8 File Offset: 0x003202E8
		public float PrettinessFactor(Pawn otherPawn)
		{
			float num = 0f;
			if (otherPawn.RaceProps.Humanlike)
			{
				num = otherPawn.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			}
			if (num < 0f)
			{
				return 0.3f;
			}
			if (num > 0f)
			{
				return 2.3f;
			}
			return 1f;
		}

		// Token: 0x06008A61 RID: 35425 RVA: 0x00322138 File Offset: 0x00320338
		public float LovinAgeFactor(Pawn otherPawn)
		{
			float num = 1f;
			float ageBiologicalYearsFloat = this.pawn.ageTracker.AgeBiologicalYearsFloat;
			float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
			if (this.pawn.gender == Gender.Male)
			{
				float min = ageBiologicalYearsFloat - 30f;
				float lower = ageBiologicalYearsFloat - 10f;
				float upper = ageBiologicalYearsFloat + 3f;
				float max = ageBiologicalYearsFloat + 10f;
				num = GenMath.FlatHill(0.2f, min, lower, upper, max, 0.2f, ageBiologicalYearsFloat2);
			}
			else if (this.pawn.gender == Gender.Female)
			{
				float min2 = ageBiologicalYearsFloat - 10f;
				float lower2 = ageBiologicalYearsFloat - 3f;
				float upper2 = ageBiologicalYearsFloat + 10f;
				float max2 = ageBiologicalYearsFloat + 30f;
				num = GenMath.FlatHill(0.2f, min2, lower2, upper2, max2, 0.2f, ageBiologicalYearsFloat2);
			}
			float num2 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat);
			float num3 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat2);
			return num * num2 * num3;
		}

		// Token: 0x06008A62 RID: 35426 RVA: 0x0032222C File Offset: 0x0032042C
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.romanceChanceFactor;
			}
			float num2 = 1f;
			HediffWithTarget hediffWithTarget = (HediffWithTarget)this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (hediffWithTarget != null && hediffWithTarget.target == otherPawn)
			{
				num2 = 10f;
			}
			float num3 = 1f;
			if (ModsConfig.BiotechActive && this.pawn.genes != null)
			{
				Pawn_StoryTracker story = otherPawn.story;
				if (((story != null) ? story.traits : null) == null || !otherPawn.story.traits.HasTrait(TraitDefOf.Kind))
				{
					List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
					for (int i = 0; i < genesListForReading.Count; i++)
					{
						if (genesListForReading[i].def.missingGeneRomanceChanceFactor != 1f && (otherPawn.genes == null || !otherPawn.genes.HasActiveGene(genesListForReading[i].def)))
						{
							num3 *= genesListForReading[i].def.missingGeneRomanceChanceFactor;
						}
					}
				}
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num * num2 * num3;
		}

		// Token: 0x06008A63 RID: 35427 RVA: 0x0032239C File Offset: 0x0032059C
		public float CompatibilityWith(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
			float num = Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x), -0.45f, 0.45f);
			float num2 = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
			return num + num2;
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x00322425 File Offset: 0x00320625
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x0032245C File Offset: 0x0032065C
		public void ClearAllRelations()
		{
			this.CleanupVirtualRelationReferences();
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x00322514 File Offset: 0x00320714
		private void CleanupVirtualRelationReferences()
		{
			foreach (VirtualPawnRelation virtualPawnRelation in this.virtualRelations)
			{
				virtualPawnRelation.record.RemoveReference(this.pawn);
			}
			this.virtualRelations.Clear();
		}

		// Token: 0x06008A67 RID: 35431 RVA: 0x0032257C File Offset: 0x0032077C
		public void ClearAllNonBloodRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.familyByBloodRelation)
				{
					this.RemoveDirectRelation(list[i]);
				}
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn && !list3[j].def.familyByBloodRelation)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x06008A68 RID: 35432 RVA: 0x00322658 File Offset: 0x00320858
		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			this.RemoveRelationsOnDeath();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn) && !this.pawn.RaceProps.Animal)
			{
				this.AffectBondedAnimalsOnMyDeath();
			}
			this.Notify_FailedRescueQuest();
		}

		// Token: 0x06008A69 RID: 35433 RVA: 0x0032270C File Offset: 0x0032090C
		public void Notify_PawnDestroyed(DestroyMode mode)
		{
			this.RemoveRelationsOnDeath();
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x00322714 File Offset: 0x00320914
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x0032272A File Offset: 0x0032092A
		public void Notify_PawnLeftBehind()
		{
			this.RemoveRelationsOnLeftBehind();
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x00322732 File Offset: 0x00320932
		public void Notify_ExitedMap()
		{
			this.CheckRescued();
		}

		// Token: 0x06008A6D RID: 35437 RVA: 0x0032273A File Offset: 0x0032093A
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.CheckRescued();
			}
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x00322754 File Offset: 0x00320954
		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThoughts != null)
					{
						if (mostImportantRelation == PawnRelationDefOf.Bond)
						{
							this.pawn.relations.RemoveDirectRelation(mostImportantRelation, pawn);
						}
						foreach (ThoughtDef def in mostImportantRelation.soldThoughts)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(def, playerNegotiator, null);
						}
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06008A6F RID: 35439 RVA: 0x00322850 File Offset: 0x00320A50
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x00322858 File Offset: 0x00320A58
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.pawn.needs.mood != null && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer, null);
				this.canGetRescuedThought = false;
			}
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x003228BC File Offset: 0x00320ABC
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06008A72 RID: 35442 RVA: 0x00322990 File Offset: 0x00320B90
		private void CheckRescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06008A73 RID: 35443 RVA: 0x00322A64 File Offset: 0x00320C64
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		// Token: 0x06008A74 RID: 35444 RVA: 0x00322A86 File Offset: 0x00320C86
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		// Token: 0x06008A75 RID: 35445 RVA: 0x00322AA8 File Offset: 0x00320CA8
		private void RemoveMySpouseMarriageRelatedThoughts()
		{
			foreach (Pawn pawn in this.pawn.GetSpouses(false))
			{
				if (pawn.needs.mood != null)
				{
					MemoryThoughtHandler memories = pawn.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
					memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
				}
			}
		}

		// Token: 0x06008A76 RID: 35446 RVA: 0x00322B34 File Offset: 0x00320D34
		private void RemoveRelationsOnDeath()
		{
			for (int i = this.directRelations.Count - 1; i >= 0; i--)
			{
				if (this.directRelations[i].def.removeOnDeath)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					Pawn otherPawn = directPawnRelation.otherPawn;
					this.RemoveDirectRelation(this.directRelations[i]);
					directPawnRelation.def.Worker.Notify_PostRemovedByDeath(this.pawn, otherPawn);
				}
			}
		}

		// Token: 0x06008A77 RID: 35447 RVA: 0x00322BAC File Offset: 0x00320DAC
		private void RemoveRelationsOnLeftBehind()
		{
			for (int i = this.directRelations.Count - 1; i >= 0; i--)
			{
				if (this.directRelations[i].def.removeOnLeftBehind)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					Pawn otherPawn = directPawnRelation.otherPawn;
					this.RemoveDirectRelation(this.directRelations[i]);
					directPawnRelation.def.Worker.Notify_PostRemovedLeftBehind(this.pawn, otherPawn);
				}
			}
		}

		// Token: 0x06008A78 RID: 35448 RVA: 0x00322C24 File Offset: 0x00320E24
		public void CheckAppendBondedAnimalDiedInfo(ref TaggedString letter, ref TaggedString label)
		{
			if (!this.pawn.RaceProps.Animal || !this.everSeenByPlayer || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				return;
			}
			Predicate<Pawn> isAffected = (Pawn x) => !x.Dead && (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath));
			int num = 0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			TaggedString t;
			if (num == 1)
			{
				Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
				t = "LetterPartBondedAnimalDied".Translate(this.pawn.LabelDefinite(), firstDirectRelationPawn.LabelShort, this.pawn.Named("ANIMAL"), firstDirectRelationPawn.Named("HUMAN")).CapitalizeFirst();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
					{
						stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
					}
				}
				t = "LetterPartBondedAnimalDiedMulti".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			label += " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
			if (!letter.NullOrEmpty())
			{
				letter += "\n\n";
			}
			letter += t;
		}

		// Token: 0x06008A79 RID: 35449 RVA: 0x00322E48 File Offset: 0x00321048
		private void AffectBondedAnimalsOnMyDeath()
		{
			int num = 0;
			Pawn pawn = null;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && this.directRelations[i].otherPawn.Spawned)
				{
					pawn = this.directRelations[i].otherPawn;
					num++;
					float value = Rand.Value;
					MentalStateDef stateDef;
					if (value < 0.25f)
					{
						stateDef = MentalStateDefOf.Wander_Sad;
					}
					else if (value < 0.5f)
					{
						stateDef = MentalStateDefOf.Wander_Psychotic;
					}
					else if (value < 0.75f)
					{
						stateDef = MentalStateDefOf.Berserk;
					}
					else
					{
						stateDef = MentalStateDefOf.Manhunter;
					}
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "MentalStateReason_BondedHumanDeath".Translate(this.pawn).Resolve(), false, true, false, null, false, false, false);
				}
			}
			if (num == 1)
			{
				TaggedString taggedString;
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					taggedString = "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabelIndefinite(), pawn.Name.ToStringShort, this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				else
				{
					taggedString = "MessageBondedAnimalMentalBreak".Translate(pawn.LabelIndefinite(), this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				Messages.Message(taggedString.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
				return;
			}
			if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort, this.pawn.Named("HUMAN")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x06008A7A RID: 35450 RVA: 0x00323064 File Offset: 0x00321264
		private void Tick_CheckStartMarriageCeremony(int delta)
		{
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(1017, delta))
			{
				int ticksGame = Find.TickManager.TicksGame;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					float num = (float)(ticksGame - this.directRelations[i].startTicks) / 60000f;
					if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn, this.directRelations[i].otherPawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn, this.pawn))
					{
						this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
					}
				}
			}
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x00323204 File Offset: 0x00321404
		private void Tick_CheckDevelopBondRelation(int delta)
		{
			if (!this.pawn.Spawned || !this.pawn.IsAnimal || this.pawn.Faction != Faction.OfPlayer || this.pawn.playerSettings.RespectedMaster == null)
			{
				return;
			}
			Pawn respectedMaster = this.pawn.playerSettings.RespectedMaster;
			if (this.pawn.IsHashIntervalTick(2500, delta) && this.pawn.Map == respectedMaster.Map && this.pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(this.pawn.Position, respectedMaster.Position, this.pawn.Map))
			{
				RelationsUtility.TryDevelopBondRelation(respectedMaster, this.pawn, 0.001f);
			}
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x003232DC File Offset: 0x003214DC
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x04005547 RID: 21831
		private Pawn pawn;

		// Token: 0x04005548 RID: 21832
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		// Token: 0x04005549 RID: 21833
		private List<VirtualPawnRelation> virtualRelations = new List<VirtualPawnRelation>();

		// Token: 0x0400554A RID: 21834
		public bool everSeenByPlayer;

		// Token: 0x0400554B RID: 21835
		public bool canGetRescuedThought = true;

		// Token: 0x0400554C RID: 21836
		public Pawn relativeInvolvedInRescueQuest;

		// Token: 0x0400554D RID: 21837
		public MarriageNameChange nextMarriageNameChange;

		// Token: 0x0400554E RID: 21838
		public bool hidePawnRelations;

		// Token: 0x0400554F RID: 21839
		[LoadAlias("rhythmModes")]
		private Dictionary<Pawn, PregnancyApproach> pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();

		// Token: 0x04005550 RID: 21840
		public int romanceEnableTick = -1;

		// Token: 0x04005551 RID: 21841
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		// Token: 0x04005552 RID: 21842
		private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

		// Token: 0x04005553 RID: 21843
		private bool familyByBloodIsCached;

		// Token: 0x04005554 RID: 21844
		private bool canCacheFamilyByBlood;

		// Token: 0x04005555 RID: 21845
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		// Token: 0x04005556 RID: 21846
		private const float MaxBondRelationCheckDist = 12f;

		// Token: 0x04005557 RID: 21847
		private const float BondRelationPerIntervalChance = 0.001f;

		// Token: 0x04005558 RID: 21848
		public const int FriendOpinionThreshold = 20;

		// Token: 0x04005559 RID: 21849
		public const int RivalOpinionThreshold = -20;

		// Token: 0x0400555A RID: 21850
		public const int MinLovinAge = 16;

		// Token: 0x0400555B RID: 21851
		private const float MinAgeFactor = 0.2f;

		// Token: 0x0400555C RID: 21852
		public const float PsychicLoveFactor = 10f;

		// Token: 0x0400555D RID: 21853
		private List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400555E RID: 21854
		private List<PregnancyApproach> tmpPregnancyApproaches = new List<PregnancyApproach>();

		// Token: 0x0400555F RID: 21855
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
