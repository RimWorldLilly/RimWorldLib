using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BE RID: 5566
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x060089F8 RID: 35320 RVA: 0x0031ECA0 File Offset: 0x0031CEA0
		public int PassionCount
		{
			get
			{
				int num = 0;
				foreach (SkillRecord skillRecord in this.skills)
				{
					if (!skillRecord.TotallyDisabled)
					{
						if (skillRecord.passion == Passion.Major)
						{
							num += 2;
						}
						else if (skillRecord.passion == Passion.Minor)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x0031ED14 File Offset: 0x0031CF14
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x0031ED90 File Offset: 0x0031CF90
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.skills.RemoveAll((SkillRecord x) => x == null) != 0)
				{
					Log.Error("Some skills were null after loading for " + this.pawn.ToStringSafe<Pawn>());
				}
				if (this.skills.RemoveAll((SkillRecord x) => x.def == null) != 0)
				{
					Log.Error("Some skills had null def after loading for " + this.pawn.ToStringSafe<Pawn>());
				}
				List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < this.skills.Count; j++)
					{
						if (this.skills[j].def == allDefsListForReading[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had no " + allDefsListForReading[i].ToStringSafe<SkillDef>() + " skill. Adding.");
						this.skills.Add(new SkillRecord(this.pawn, allDefsListForReading[i]));
					}
				}
			}
		}

		// Token: 0x060089FB RID: 35323 RVA: 0x0031EF04 File Offset: 0x0031D104
		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			string str = "Did not find skill of def ";
			string str2 = (skillDef != null) ? skillDef.ToString() : null;
			string str3 = ", returning ";
			SkillRecord skillRecord = this.skills[0];
			Log.Error(str + str2 + str3 + ((skillRecord != null) ? skillRecord.ToString() : null));
			return this.skills[0];
		}

		// Token: 0x060089FC RID: 35324 RVA: 0x0031EF90 File Offset: 0x0031D190
		public void SkillsTickInterval(int delta)
		{
			if (!this.pawn.IsHashIntervalTick(200, delta))
			{
				return;
			}
			if (!this.CanGainXP())
			{
				return;
			}
			if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
			{
				for (int i = 0; i < this.skills.Count; i++)
				{
					this.skills[i].xpSinceMidnight = 0f;
				}
				this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
			}
			for (int j = 0; j < this.skills.Count; j++)
			{
				this.skills[j].Interval();
			}
		}

		// Token: 0x060089FD RID: 35325 RVA: 0x0031F04B File Offset: 0x0031D24B
		private bool CanGainXP()
		{
			return !ModsConfig.AnomalyActive || !this.pawn.IsMutant || this.pawn.mutant.Def.canGainXP;
		}

		// Token: 0x060089FE RID: 35326 RVA: 0x0031F07B File Offset: 0x0031D27B
		public void Learn(SkillDef sDef, float xp, bool direct = false, bool ignoreLearnRate = false)
		{
			if (!this.CanGainXP())
			{
				return;
			}
			this.GetSkill(sDef).Learn(xp, direct, ignoreLearnRate);
		}

		// Token: 0x060089FF RID: 35327 RVA: 0x0031F098 File Offset: 0x0031D298
		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return 3f;
			}
			float num = 0f;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
			}
			return num / (float)workDef.relevantSkills.Count;
		}

		// Token: 0x06008A00 RID: 35328 RVA: 0x0031F100 File Offset: 0x0031D300
		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return Passion.None;
			}
			Passion passion = Passion.None;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
				if (passion2 > passion)
				{
					passion = passion2;
				}
			}
			return passion;
		}

		// Token: 0x06008A01 RID: 35329 RVA: 0x0031F154 File Offset: 0x0031D354
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x06008A02 RID: 35330 RVA: 0x0031F188 File Offset: 0x0031D388
		public void DirtyAptitudes()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].DirtyAptitudes();
			}
		}

		// Token: 0x040054E7 RID: 21735
		private const int MinorPassionWeight = 1;

		// Token: 0x040054E8 RID: 21736
		private const int MajorPassionWeight = 2;

		// Token: 0x040054E9 RID: 21737
		private Pawn pawn;

		// Token: 0x040054EA RID: 21738
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x040054EB RID: 21739
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
