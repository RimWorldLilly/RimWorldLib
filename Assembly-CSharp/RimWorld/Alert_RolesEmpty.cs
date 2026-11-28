using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C05 RID: 7173
	public class Alert_RolesEmpty : Alert
	{
		// Token: 0x0600B73D RID: 46909 RVA: 0x0040935B File Offset: 0x0040755B
		public Alert_RolesEmpty()
		{
			this.requireIdeology = true;
		}

		// Token: 0x0600B73E RID: 46910 RVA: 0x0040938C File Offset: 0x0040758C
		private void GetTargets()
		{
			this.emptyRoles.Clear();
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					if (!precept.def.leaderRole && precept.def.createsRoleEmptyThought)
					{
						Precept_Role precept_Role = precept as Precept_Role;
						if (precept_Role != null && precept_Role.ChosenPawnSingle() == null && precept_Role.Active)
						{
							this.emptyRoles.Add(precept_Role);
						}
					}
				}
			}
			if (this.emptyRoles.Any<Precept_Role>())
			{
				using (List<Pawn>.Enumerator enumerator3 = Find.CurrentMap.mapPawns.FreeColonistsSpawned.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Pawn p = enumerator3.Current;
						if (!p.IsSlave && this.emptyRoles.Any((Precept_Role x) => x.ideo == p.Ideo))
						{
							this.targets.Add(p);
							this.targetLabels.Add(p.NameFullColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x0600B73F RID: 46911 RVA: 0x00409538 File Offset: 0x00407738
		public override string GetLabel()
		{
			if (this.emptyRoles.Count == 1)
			{
				return "IdeoRoleEmpty".Translate(this.emptyRoles[0].LabelCap);
			}
			return "IdeoRolesEmpty".Translate(this.emptyRoles.Count);
		}

		// Token: 0x0600B740 RID: 46912 RVA: 0x00409598 File Offset: 0x00407798
		public override TaggedString GetExplanation()
		{
			return "IdeoRolesEmptyDesc".Translate((from x in this.emptyRoles
			select x.LabelCap.ApplyTag(x.ideo).Resolve()).ToLineList("  - ", false)) + ":\n" + this.targetLabels.ToLineList("  - ");
		}

		// Token: 0x0600B741 RID: 46913 RVA: 0x00409608 File Offset: 0x00407808
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			if (Find.CurrentMap == null)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400691B RID: 26907
		private List<Precept_Role> emptyRoles = new List<Precept_Role>();

		// Token: 0x0400691C RID: 26908
		private List<string> targetLabels = new List<string>();

		// Token: 0x0400691D RID: 26909
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400691E RID: 26910
		public const int DayEnable = 10;
	}
}
