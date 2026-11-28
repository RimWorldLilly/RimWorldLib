using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006C6 RID: 1734
	public class Effecter
	{
		// Token: 0x060036F5 RID: 14069 RVA: 0x0016080C File Offset: 0x0015EA0C
		public Effecter(EffecterDef def)
		{
			this.def = def;
			this.spawnTick = Find.TickManager.TicksGame;
			for (int i = 0; i < def.children.Count; i++)
			{
				SubEffecterDef subEffecterDef = def.children[i];
				if (DebugSettings.anomalyDarkeningFX)
				{
					this.children.Add(subEffecterDef.Spawn(this));
				}
			}
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x00160890 File Offset: 0x0015EA90
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x001608C8 File Offset: 0x0015EAC8
		public Effecter Trigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B, overrideSpawnTick, false);
			}
			return this;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x00160901 File Offset: 0x0015EB01
		public void ForceEnd()
		{
			this.ticksLeft = 0;
			this.Cleanup();
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x00160910 File Offset: 0x0015EB10
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x04002421 RID: 9249
		public EffecterDef def;

		// Token: 0x04002422 RID: 9250
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x04002423 RID: 9251
		public int ticksLeft = -1;

		// Token: 0x04002424 RID: 9252
		public Vector3 offset;

		// Token: 0x04002425 RID: 9253
		public float scale = 1f;

		// Token: 0x04002426 RID: 9254
		public int spawnTick;
	}
}
