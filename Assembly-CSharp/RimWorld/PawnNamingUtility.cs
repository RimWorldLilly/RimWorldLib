using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DE RID: 5342
	public static class PawnNamingUtility
	{
		// Token: 0x060082A8 RID: 33448 RVA: 0x002EE6E0 File Offset: 0x002EC8E0
		public static Dialog_NamePawn NamePawnDialog(this Pawn pawn, string initialFirstNameOverride = null)
		{
			Dictionary<NameFilter, List<string>> suggestedNames = null;
			NameFilter editableNames;
			NameFilter visibleNames;
			if (pawn.babyNamingDeadline >= Find.TickManager.TicksGame || DebugSettings.ShowDevGizmos)
			{
				editableNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last);
				visibleNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last);
				List<string> list = new List<string>();
				Pawn mother = pawn.GetMother();
				if (mother != null)
				{
					list.Add(PawnNamingUtility.GetLastName(mother));
				}
				Pawn father = pawn.GetFather();
				if (father != null)
				{
					list.Add(PawnNamingUtility.GetLastName(father));
				}
				Pawn birthParent = pawn.GetBirthParent();
				if (birthParent != null)
				{
					list.Add(PawnNamingUtility.GetLastName(birthParent));
				}
				list.RemoveDuplicates(null);
				suggestedNames = new Dictionary<NameFilter, List<string>>
				{
					{
						NameFilter.Last,
						list
					}
				};
			}
			else
			{
				visibleNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last | NameFilter.Title);
				editableNames = (NameFilter.Nick | NameFilter.Title);
			}
			return new Dialog_NamePawn(pawn, visibleNames, editableNames, suggestedNames, initialFirstNameOverride, null, null, null);
		}

		// Token: 0x060082A9 RID: 33449 RVA: 0x002EE788 File Offset: 0x002EC988
		public static string GetLastName(Pawn pawn)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null)
			{
				return nameTriple.Last;
			}
			NameSingle nameSingle = pawn.Name as NameSingle;
			if (nameSingle != null)
			{
				return nameSingle.Name;
			}
			throw new NotImplementedException(pawn.Name.GetType().ToString());
		}
	}
}
