using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9B RID: 3995
	public class LearningDesireDef : Def
	{
		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060065D3 RID: 26067 RVA: 0x00243A83 File Offset: 0x00241C83
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x060065D4 RID: 26068 RVA: 0x00243AAB File Offset: 0x00241CAB
		public LearningGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LearningGiver)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x00243ADD File Offset: 0x00241CDD
		public override IEnumerable<string> ConfigErrors()
		{
			LearningDesireDef.<ConfigErrors>d__11 <ConfigErrors>d__ = new LearningDesireDef.<ConfigErrors>d__11(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003D15 RID: 15637
		public float selectionWeight = 1f;

		// Token: 0x04003D16 RID: 15638
		public float xpPerTick;

		// Token: 0x04003D17 RID: 15639
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003D18 RID: 15640
		public Type workerClass = typeof(LearningGiver);

		// Token: 0x04003D19 RID: 15641
		public JobDef jobDef;

		// Token: 0x04003D1A RID: 15642
		private Texture2D icon;

		// Token: 0x04003D1B RID: 15643
		private LearningGiver workerInt;
	}
}
