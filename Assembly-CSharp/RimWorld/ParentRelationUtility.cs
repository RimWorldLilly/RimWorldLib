using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152A RID: 5418
	public static class ParentRelationUtility
	{
		// Token: 0x060084A0 RID: 33952 RVA: 0x002FCE40 File Offset: 0x002FB040
		public static TaggedString GetParentage(this Pawn pawn)
		{
			ParentRelationUtility.workingParentagePieces.Clear();
			Pawn mother = pawn.GetMother();
			if (mother != null)
			{
				ParentRelationUtility.workingParentagePieces.Add(ParentRelationUtility.<GetParentage>g__FactionDesc|1_0(mother));
			}
			Pawn father = pawn.GetFather();
			if (father != null)
			{
				ParentRelationUtility.workingParentagePieces.Add(ParentRelationUtility.<GetParentage>g__FactionDesc|1_0(father));
			}
			string value = ParentRelationUtility.workingParentagePieces.ToCommaList(true, true);
			if (string.IsNullOrEmpty(value))
			{
				value = "ParentsUnknown".Translate();
			}
			return "BornOfParents".Translate(value);
		}

		// Token: 0x060084A1 RID: 33953 RVA: 0x002FCEC0 File Offset: 0x002FB0C0
		public static Pawn GetFather(this Pawn pawn)
		{
			return pawn.GetParent(Gender.Male);
		}

		// Token: 0x060084A2 RID: 33954 RVA: 0x002FCEC9 File Offset: 0x002FB0C9
		public static Pawn GetMother(this Pawn pawn)
		{
			return pawn.GetParent(Gender.Female);
		}

		// Token: 0x060084A3 RID: 33955 RVA: 0x002FCED2 File Offset: 0x002FB0D2
		public static bool HasSameFather(this Pawn pawn, Pawn other)
		{
			return ParentRelationUtility.HasSameParent(pawn, other, Gender.Male);
		}

		// Token: 0x060084A4 RID: 33956 RVA: 0x002FCEDC File Offset: 0x002FB0DC
		public static bool HasSameMother(this Pawn pawn, Pawn other)
		{
			return ParentRelationUtility.HasSameParent(pawn, other, Gender.Female);
		}

		// Token: 0x060084A5 RID: 33957 RVA: 0x002FCEE8 File Offset: 0x002FB0E8
		private static Pawn GetParent(this Pawn pawn, Gender parentGender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.Parent && directPawnRelation.otherPawn.gender == parentGender)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x060084A6 RID: 33958 RVA: 0x002FCF58 File Offset: 0x002FB158
		private static bool HasSameParent(Pawn pawn, Pawn other, Gender parentGender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (pawn.relations == null)
			{
				return false;
			}
			Pawn parent = pawn.GetParent(parentGender);
			Pawn parent2 = other.GetParent(parentGender);
			if (parent != null && parent2 != null && parent == parent2)
			{
				return true;
			}
			VirtualPawnRelation virtualParent = ParentRelationUtility.GetVirtualParent(pawn, parentGender);
			VirtualPawnRelation virtualParent2 = ParentRelationUtility.GetVirtualParent(other, parentGender);
			return virtualParent != null && virtualParent2 != null && virtualParent.record.ID == virtualParent2.record.ID;
		}

		// Token: 0x060084A7 RID: 33959 RVA: 0x002FCFCC File Offset: 0x002FB1CC
		private static VirtualPawnRelation GetVirtualParent(Pawn pawn, Gender gender)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<VirtualPawnRelation> virtualRelations = pawn.relations.VirtualRelations;
			for (int i = 0; i < virtualRelations.Count; i++)
			{
				VirtualPawnRelation virtualPawnRelation = virtualRelations[i];
				if (virtualPawnRelation.def == PawnRelationDefOf.Parent && virtualPawnRelation.record.Gender == gender)
				{
					return virtualPawnRelation;
				}
			}
			return null;
		}

		// Token: 0x060084A8 RID: 33960 RVA: 0x002FD038 File Offset: 0x002FB238
		public static Pawn GetBirthParent(this Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			if (pawn.relations == null)
			{
				return null;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				if (directPawnRelation.def == PawnRelationDefOf.ParentBirth)
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x060084A9 RID: 33961 RVA: 0x002FD0A4 File Offset: 0x002FB2A4
		public static void SetFather(this Pawn pawn, Pawn newFather)
		{
			if (newFather != null && newFather.gender == Gender.Female)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ",
					(newFather != null) ? newFather.ToString() : null,
					" with gender ",
					newFather.gender.ToString(),
					" as ",
					(pawn != null) ? pawn.ToString() : null,
					"'s father."
				}));
				return;
			}
			Pawn father = pawn.GetFather();
			if (father != newFather)
			{
				if (father != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, father);
				}
				if (newFather != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newFather);
				}
			}
		}

		// Token: 0x060084AA RID: 33962 RVA: 0x002FD158 File Offset: 0x002FB358
		public static void SetMother(this Pawn pawn, Pawn newMother)
		{
			if (newMother != null && newMother.gender != Gender.Female)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set ",
					(newMother != null) ? newMother.ToString() : null,
					" with gender ",
					newMother.gender.ToString(),
					" as ",
					(pawn != null) ? pawn.ToString() : null,
					"'s mother."
				}));
				return;
			}
			Pawn mother = pawn.GetMother();
			if (mother != newMother)
			{
				if (mother != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Parent, mother);
				}
				if (newMother != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, newMother);
				}
			}
		}

		// Token: 0x060084AC RID: 33964 RVA: 0x002FD218 File Offset: 0x002FB418
		[CompilerGenerated]
		internal static string <GetParentage>g__FactionDesc|1_0(Pawn parent)
		{
			return parent.FactionDesc(parent.NameFullColored, false, parent.NameFullColored, parent.gender.GetLabel(parent.RaceProps.Animal)).Resolve();
		}

		// Token: 0x040051D3 RID: 20947
		private static List<string> workingParentagePieces = new List<string>();
	}
}
