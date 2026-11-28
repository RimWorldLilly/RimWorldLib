using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152B RID: 5419
	public static class PawnRelationUtility
	{
		// Token: 0x060084AD RID: 33965 RVA: 0x002FD25B File Offset: 0x002FB45B
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			PawnRelationUtility.<GetRelations>d__0 <GetRelations>d__ = new PawnRelationUtility.<GetRelations>d__0(-2);
			<GetRelations>d__.<>3__me = me;
			<GetRelations>d__.<>3__other = other;
			return <GetRelations>d__;
		}

		// Token: 0x060084AE RID: 33966 RVA: 0x002FD274 File Offset: 0x002FB474
		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef pawnRelationDef2 in me.GetRelations(other))
			{
				if (pawnRelationDef == null || pawnRelationDef2.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = pawnRelationDef2;
				}
			}
			return pawnRelationDef;
		}

		// Token: 0x060084AF RID: 33967 RVA: 0x002FD2D4 File Offset: 0x002FB4D4
		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> list = (from x in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x).ToList<Pawn>();
			bool flag = false;
			foreach (Pawn pawn in seenPawns)
			{
				if (pawn.RaceProps.IsFlesh && (informEvenIfSeenBefore || !pawn.relations.everSeenByPlayer) && (!pawn.Spawned || !pawn.Fogged()))
				{
					pawn.relations.everSeenByPlayer = true;
					bool flag2 = false;
					foreach (Pawn pawn2 in list)
					{
						if (pawn != pawn2)
						{
							PawnRelationDef mostImportantRelation = pawn2.GetMostImportantRelation(pawn);
							if (mostImportantRelation != null)
							{
								if (!flag2)
								{
									flag2 = true;
									if (flag)
									{
										stringBuilder.AppendLine();
									}
									if (writeSeenPawnsNames)
									{
										stringBuilder.AppendLine(pawn.KindLabel.CapitalizeFirst() + " " + pawn.Name.ToStringShort.Colorize(ColoredText.NameColor) + ":");
									}
								}
								flag = true;
								stringBuilder.AppendLine("  - " + "Relationship".Translate(mostImportantRelation.GetGenderSpecificLabelCap(pawn), pawn2.KindLabel + " " + pawn2.NameShortColored.Resolve(), pawn2));
							}
						}
					}
				}
			}
			if (flag)
			{
				pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
				return;
			}
			pawnRelationsInfo = null;
		}

		// Token: 0x060084B0 RID: 33968 RVA: 0x002FD4CC File Offset: 0x002FB6CC
		public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref TaggedString letterLabel, ref TaggedString letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				if (letterLabel.NullOrEmpty())
				{
					letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
				}
				else
				{
					letterLabel += ": " + "RelationshipAppendedLetterSuffix".Translate().CapitalizeFirst();
				}
				if (!letterText.NullOrEmpty())
				{
					letterText += "\n\n";
				}
				letterText += relationsInfoHeader + "\n\n" + text;
			}
		}

		// Token: 0x060084B1 RID: 33969 RVA: 0x002FD574 File Offset: 0x002FB774
		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			TaggedString label = "";
			TaggedString text = "";
			seenPawns = seenPawns.ToList<Pawn>();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref label, ref text, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				Pawn pawn = null;
				foreach (Pawn pawn2 in seenPawns)
				{
					if (PawnRelationUtility.GetMostImportantColonyRelative(pawn2) != null)
					{
						pawn = pawn2;
						break;
					}
				}
				if (pawn == null)
				{
					pawn = seenPawns.FirstOrDefault<Pawn>();
				}
				Find.LetterStack.ReceiveLetter(label, text, letterDef, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x002FD620 File Offset: 0x002FB820
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, Pawn pawn)
		{
			TaggedString taggedString = null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref taggedString, pawn);
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x002FD640 File Offset: 0x002FB840
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, ref TaggedString title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative == null)
			{
				return false;
			}
			if (title != null)
			{
				title += " (" + "RelationshipAppendedLetterSuffix".Translate() + ")";
			}
			PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
			string text2 = (mostImportantRelation != null) ? mostImportantRelation.GetGenderSpecificLabel(pawn) : null;
			if (text2 == null)
			{
				return false;
			}
			if (mostImportantColonyRelative.IsColonist)
			{
				text += "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			else
			{
				text += "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			return true;
		}

		// Token: 0x060084B4 RID: 33972 RVA: 0x002FD770 File Offset: 0x002FB970
		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return null;
			}
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			float num = 0f;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in enumerable)
			{
				PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(pawn3);
				if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
				{
					num = mostImportantRelation.importance;
					pawn2 = pawn3;
				}
			}
			return pawn2;
		}

		// Token: 0x060084B5 RID: 33973 RVA: 0x002FD820 File Offset: 0x002FBA20
		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0f)
			{
				return -1f;
			}
			return num;
		}

		// Token: 0x060084B6 RID: 33974 RVA: 0x002FD846 File Offset: 0x002FBA46
		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
