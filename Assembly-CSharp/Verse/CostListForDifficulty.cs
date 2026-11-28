using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;

namespace Verse
{
	// Token: 0x02000122 RID: 290
	public class CostListForDifficulty
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00036299 File Offset: 0x00034499
		public bool Applies
		{
			get
			{
				if (Find.Storyteller == null)
				{
					return false;
				}
				if (this.cachedDifficulty != Find.Storyteller.difficulty)
				{
					this.RecacheApplies();
				}
				return this.cachedApplies;
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000362C4 File Offset: 0x000344C4
		public void RecacheApplies()
		{
			this.cachedDifficulty = Find.Storyteller.difficulty;
			if (this.difficultyVar.NullOrEmpty())
			{
				this.cachedApplies = false;
				return;
			}
			FieldInfo field = typeof(Difficulty).GetField(this.difficultyVar, BindingFlags.Instance | BindingFlags.Public);
			this.cachedApplies = (bool)field.GetValue(this.cachedDifficulty);
			if (this.invert)
			{
				this.cachedApplies = !this.cachedApplies;
			}
		}

		// Token: 0x040004C6 RID: 1222
		public string difficultyVar;

		// Token: 0x040004C7 RID: 1223
		public List<ThingDefCountClass> costList;

		// Token: 0x040004C8 RID: 1224
		public int costStuffCount;

		// Token: 0x040004C9 RID: 1225
		public bool invert;

		// Token: 0x040004CA RID: 1226
		private bool cachedApplies;

		// Token: 0x040004CB RID: 1227
		private Difficulty cachedDifficulty;
	}
}
